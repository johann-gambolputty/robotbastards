using System;
using System.Collections;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Manages objects that update the server
	/// </summary>
	public class ServerUpdateManager : Scene.ISceneObject
	{
		/// <summary>
		/// Gets the connection to the server
		/// </summary>
		public IConnection	ServerConnection
		{
			get
			{
				return m_ServerConnection;
			}
		}

		#region	Updaters

		/// <summary>
		/// Adds a client updater to the manager
		/// </summary>
		public void AddUpdater( IServerUpdater updater )
		{
			m_Updaters.Add( updater );
			m_UpdaterIdMap[ updater.Id ] = updater;
		}

		/// <summary>
		/// Removes an updater from the manager
		/// </summary>
		public void RemoveUpdater( IServerUpdater updater )
		{
			m_Updaters.Remove( updater );
			m_UpdaterIdMap.Remove( updater.Id );
		}

		#endregion

		#region	ISceneObject Members

		/// <summary>
		///	Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			//	Get the Connections object from the scene systems
			Connections connections = ( Connections )db.GetSystem( typeof( Connections ) );
			if ( connections == null )
			{
				throw new ApplicationException( "ServerUpdateManager requires that a Connections object be present in the scene systems" );
			}

			//	Is there a RUNT server connection already active?
			IConnection serverConnection = connections.GetConnection( "RuntServer" );
			if ( serverConnection == null )
			{
				//	Subscribe to the new server connection event
				Output.WriteLineCall( Output.NetworkInfo, "No \"RuntServer\" server connection exists yet - subscribing to new server connection event" );
				connections.NewServerConnection += new NewServerConnectionDelegate( OnNewServerConnection );
			}
			else
			{
				OnNewServerConnection( serverConnection );
			}

			//	Subscribe to the network clock
			Scene.Clock networkClock = db.GetNamedClock( "NetworkClock" );
			if ( networkClock == null )
			{
				throw new ApplicationException( "ServerUpdateManager requires a clock named \"NetworkClock\" to be present in the scene" );
			}

			networkClock.Subscribe( new Scene.Clock.TickDelegate( OnTick ) );
		}

		/// <summary>
		///	Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion

		private Hashtable	m_UpdaterIdMap	= new Hashtable( );
		private ArrayList	m_Updaters		= new ArrayList( );
		private uint		m_Sequence		= 0;
		private IConnection	m_ServerConnection;


		/// <summary>
		/// Adds a new connection
 		private void OnNewServerConnection( IConnection connection )
		{
			if ( m_ServerConnection != null )
			{
				//	TODO: ...
				throw new ApplicationException( "ServerUpdateManager can only handle one server connection" );
			}
			if ( connection.Name == "RuntServer" )
			{
				m_ServerConnection = connection;
				connection.ReceivedMessage += new ConnectionReceivedMessageDelegate( OnReceivedServerMessage );
			}
		}

		/// <summary>
		/// Called when the client receives a message from the server
		/// </summary>
		private void OnReceivedServerMessage( IConnection connection, Components.Message msg )
		{
			ClientMessage clientMsg = ( ClientMessage )msg;

			//	TODO: Handle out-of-synch messages

			foreach ( UpdateMessage curUpdateMessage in clientMsg.UpdateMessages )
			{
				IServerUpdater updater = ( IServerUpdater )m_UpdaterIdMap[ curUpdateMessage.TargetId ];
				updater.HandleServerUpdate( curUpdateMessage );
			}
		}

		/// <summary>
		/// Network update tick
		/// </summary>
		private void OnTick( Scene.Clock clock )
		{
			if ( m_ServerConnection == null )
			{
				return;
			}

			//	Run through all the server updaters
			ArrayList messages = new ArrayList( );
			foreach ( IServerUpdater updater in m_Updaters )
			{
				updater.GetUpdateMessages( messages );
			}

			if ( messages.Count > 0 )
			{
				ServerMessage msg = new ServerMessage( m_Sequence, 0 , ( UpdateMessage[] )messages.ToArray( typeof( UpdateMessage ) ) );
				m_ServerConnection.DeliverMessage( msg );
			}
		}
	}
}
