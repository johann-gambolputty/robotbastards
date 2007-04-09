using System;
using System.Collections;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Keeps one or more UpdateTarget objects synchronised by sending UpdateMessageBatch messages to them
	/// </summary>
	public class UpdateSource : Scene.ISceneObject, Components.INamedObject
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

		private string		m_Name;
		private uint		m _Sequence;
		private ArrayList	m_Providers = new ArrayList( );
		private ArrayList	m_Targets	= new ArrayList( );

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnNewConnection( IConnection connection )
		{
			//	Add information about the connection to the m_Clients list
			m_Targets.Add( new TargetConnection( connection ) );
			connection.ReceivedMessage += new ConnectionReceivedMessageDelegate( OnReceivedMessage );
		}

		private void OnReceivedMessage( IConnection connection, Components.Message msg )
		{
		}

		private void OnTick( Scene.Clock networkClock )
		{
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

		#region INamedObject Members

		/// <summary>
		/// Gets the name of this source
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		#endregion
	}
}
