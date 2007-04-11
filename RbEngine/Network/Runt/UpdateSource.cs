using System;
using System.Collections;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Keeps one or more UpdateTarget objects synchronised by sending UpdateMessageBatch messages to them
	/// </summary>
	public class UpdateSource : Scene.ISceneObject
	{
		#region	Providers

		/// <summary>
		/// Adds a provider
		/// </summary>
		public void AddProvider( IUpdateProvider provider )
		{
			m_Providers.Add( provider );
		}

		/// <summary>
		/// Removes a provider
		/// </summary>
		public void RemoveProvider( IUpdateProvider provider )
		{
			m_Providers.Remove( provider );
		}

		#endregion

		#region	TargetConnection Class

		/// <summary>
		/// A connection to a target that requires synchronisation from the stored update providers
		/// </summary>
		private class TargetConnection
		{
			/// <summary>
			/// Gets the connection to the client
			/// </summary>
			public IConnection	Connection
			{
				get
				{
					return m_Connection;
				}
			}

			/// <summary>
			/// Gets the current sequence value of the remote object
			/// </summary>
			public uint			Sequence
			{
				get
				{
					return m_Sequence;
				}
				set
				{
					m_Sequence = value;
				}
			}

			/// <summary>
			/// Sets up the client connection
			/// </summary>
			public TargetConnection( IConnection connection )
			{
				m_Connection	= connection;
				m_Sequence		= 0;
			}

			private IConnection	m_Connection;
			private uint		m_Sequence;
		}

		#endregion

		#region	Private stuff

		private uint		m_Sequence;
		private ArrayList	m_Providers = new ArrayList( );
		private ArrayList	m_Targets	= new ArrayList( );

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnNewConnection( IConnection connection )
		{
			//	Add information about the connection to the m_Targets list
			m_Targets.Add( new TargetConnection( connection ) );
		}

		private void OnTick( Scene.Clock networkClock )
		{
			if ( m_Targets.Count == 0 )
			{
				//	TODO: There are no targets, so none of the updaters should be storing any state...
				return;
			}

			uint oldestSequence = uint.MaxValue;

			//	Determine the oldest target sequence
			foreach ( TargetConnection target in m_Targets )
			{
				if ( target.Sequence < oldestSequence )
				{
					oldestSequence = target.Sequence;
				}
			}

			//	Inform all the update providers what the oldest target sequence is
			foreach ( IUpdateProvider provider in m_Providers )
			{
				provider.SetOldestTargetSequence( oldestSequence );
				provider.SetLocalSequence( m_Sequence );
			}

			ArrayList messages = new ArrayList( );

			//	Run through all the target connections
			foreach ( TargetConnection target in m_Targets )
			{
				//	Collate update messages from all the update providers in the messages array
				foreach ( IUpdateProvider provider in m_Providers )
				{
					provider.GetUpdateMessages( messages, target.Sequence );
				}

				
				//	Always send an UpdateBatchMessage, even if the message array is empty - this will inform the target
				//	what sequence number we're at
				UpdateMessageBatch msg = new UpdateMessageBatch( m_Sequence, ( UpdateMessage[] )messages.ToArray( typeof( UpdateMessage ) ) );

				//	Deliver the message to the client
				target.Connection.DeliverMessage( msg );

				messages.Clear( );
			}

			++m_Sequence;
		}

		#endregion

		#region	ISceneObject Members
		
		/// <summary>
		/// Invoked when this object is added to a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was added to</param>
		public void	AddedToScene( Scene.SceneDb db )
		{
			//	Get the Connections object from the scene systems
			Connections connections = ( Connections )db.GetSystem( typeof( Connections ) );
			if ( connections == null )
			{
				throw new ApplicationException( "RemoteUpdateManager requires that a Connections object be present in the scene systems" );
			}

			//	Subscribe to the new connection event
			foreach ( IConnection connection in connections.AllConnections )
			{
				OnNewConnection( connection );
			}
			connections.NewConnection += new NewConnectionDelegate( OnNewConnection );

			//	Subscribe to the network clock
			Scene.Clock networkClock = db.GetNamedClock( "NetworkClock" );
			if ( networkClock == null )
			{
				throw new ApplicationException( "RemoteUpdateManager requires a clock named \"NetworkClock\" to be present in the scene" );
			}

			networkClock.Subscribe( new Scene.Clock.TickDelegate( OnTick ) );
		}

		/// <summary>
		/// Invoked when this object is removed from a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was removed from</param>
		public void	RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion
	}
}
