using System;
using System.Collections;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Manages IRemoteUpater objects that are responsible for the scene synchronisation over an IConnection
	/// </summary>
	public class RemoteUpdateManager : Scene.ISceneObject
	{
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

		#region	Updaters

		/// <summary>
		/// Adds an updater to this manager
		/// </summary>
		public void	AddUpdater( IRemoteUpdater updater )
		{
			m_Updaters.Add( updater );
			m_UpdaterIdMap[ updater.Id ] = updater;
		}

		/// <summary>
		/// Removes an updater from this manager
		/// </summary>
		public void RemoveUpdater( IRemoteUpdater updater )
		{
			m_Updaters.Remove( updater );
			m_UpdaterIdMap.Remove( updater.Id );
		}

		#endregion

		#region	UpdateConnection

		/// <summary>
		/// A connection to a remote object that requires synchronisation from the stored updaters
		/// </summary>
		private class UpdateConnection
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
			public uint			CurrentSequence
			{
				get
				{
					return m_CurrentSequence;
				}
				set
				{
					m_CurrentSequence = value;
				}
			}

			/// <summary>
			/// Sets up the client connection
			/// </summary>
			public UpdateConnection( IConnection connection )
			{
				m_Connection		= connection;
				m_CurrentSequence	= 0;
			}

			private IConnection	m_Connection;
			private uint		m_CurrentSequence;
		}

		#endregion

		#region	Private stuff

		private uint		m_LocalSequence;
		private uint		m_SynchSequence;
		private ArrayList	m_Updaters		= new ArrayList( );
		private Hashtable	m_UpdaterIdMap	= new Hashtable( );
		private ArrayList	m_Connections	= new ArrayList( );

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnNewConnection( IConnection connection )
		{
			//	Add information about the connection to the m_Clients list
			m_Connections.Add( new UpdateConnection( connection ) );

			connection.ReceivedMessage += new ConnectionReceivedMessageDelegate( OnReceivedMessage );
		}

		/// <summary>
		/// Called when a new UpdateMessageBatch is received
		/// </summary>
		private void OnReceivedMessage( IConnection connection, Components.Message msg )
		{
			UpdateMessageBatch batch = ( UpdateMessageBatch )msg;

			if ( batch.Sequence <= m_SynchSequence )
			{
				return;
			}

			//	Find the matching UpdateConnection
			UpdateConnection updateConnection = FindUpdateConnection( connection );

			updateConnection.CurrentSequence = batch.Sequence;

			//	Run through all the update messages stored in the update message batch
			int numMessages = ( batch.Messages == null ) ? 0 : batch.Messages.Length;
			for ( int messageIndex = 0; messageIndex < numMessages; ++messageIndex )
			{
				UpdateMessage curMessage = batch.Messages[ messageIndex ];
				if ( curMessage.Sequence < m_SynchSequence )
				{
					continue;
				}

				//	Find the updater for the message
				IRemoteUpdater updater = ( IRemoteUpdater )m_UpdaterIdMap[ curMessage.TargetId ];
				if ( updater == null )
				{
					throw new ApplicationException( string.Format( "Could not find updater for target id \"{0}\"", curMessage.TargetId ) );
				}
				updater.HandleUpdateMessage( curMessage );
			}
			m_SynchSequence = batch.Sequence;
		}

		/// <summary>
		/// Gets the UpdateConnection associated with a given IConnection
		/// </summary>
		private UpdateConnection FindUpdateConnection( IConnection connection )
		{
			foreach ( UpdateConnection curConnection in m_Connections )
			{
				if ( curConnection.Connection == connection )
				{
					return curConnection;
				}
			}
			throw new ApplicationException( string.Format( "Could not find update connection \"{0}\"", connection.Name ) );
		}


		/// <summary>
		/// Network clock tick handler
		/// </summary>
		private void OnTick( Scene.Clock networkClock )
		{
			if ( m_Connections.Count == 0 )
			{
				//	TODO: There are no connections, so none of the updaters should be storing any state...
				return;
			}

			uint oldestSequence = uint.MaxValue;

			//	Determine the oldest client sequence
			foreach ( UpdateConnection connection in m_Connections )
			{
				if ( connection.CurrentSequence < oldestSequence )
				{
					oldestSequence = connection.CurrentSequence;
				}
			}

			//	Inform all the updaters what the oldest remote sequence is
			foreach ( IRemoteUpdater updater in m_Updaters )
			{
				updater.SetOldestRemoteSequence( oldestSequence );
				updater.SetLocalSequence( m_LocalSequence );
			}

			ArrayList messages = new ArrayList( );

			//	Run through all the client connections
			for ( int connectionIndex = 0; connectionIndex < m_Connections.Count; ++connectionIndex )
			{
				//	Skip past removed connections
				if ( m_Connections[ connectionIndex ] == null )
				{
					continue;
				}

				//	Get the current connection
				UpdateConnection curConnection = ( UpdateConnection )m_Connections[ connectionIndex ];

				//	Run through all the updaters
				foreach ( IRemoteUpdater curUpdater in m_Updaters )
				{
					//	Get the current updater to generate update messages for the current client
					curUpdater.GetUpdateMessages( messages, curConnection.CurrentSequence );
				}

				//	Always send a message, even if the message array is empty - this will inform the remote object what sequence number
				//	we're at
				UpdateMessageBatch msg = new UpdateMessageBatch( m_LocalSequence, ( UpdateMessage[] )messages.ToArray( typeof( UpdateMessage ) ) );

				//	Deliver the message to the client
				curConnection.Connection.DeliverMessage( msg );

				messages.Clear( );
			}

			++m_LocalSequence;
		}

		#endregion
	}
}
