using System;
using System.Collections;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Summary description for ServerUpdateManager.
	/// </summary>
	public class ServerUpdateManager : Scene.ISceneObject
	{
		#region	Updaters

		/// <summary>
		/// Adds a client updater to the manager
		/// </summary>
		public void AddUpdater( IServerUpdater updater )
		{
			m_Updaters.Add( updater );
		}

		/// <summary>
		/// Removes an updater from the manager
		/// </summary>
		public void RemoveUpdater( IServerUpdater updater )
		{
			m_Updaters.Remove( updater );
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

			//	Subscribe to the new server connection event
			connections.NewServerConnection += new NewServerConnectionDelegate( OnNewServerConnection );

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
		
		private ArrayList	m_Updaters = new ArrayList( );
		private int			m_Sequence = 0;
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
			m_ServerConnection = connection;
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

			//	TODO: Missing proper client identifier
			m_ServerConnection.DeliverMessage( new ServerMessage( m_Sequence, 0 , ( Message[] )messages.ToArray( typeof( Message[] ) ) ) );
		}
	}
}
