using System;
using System.Collections;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Target for update messages sent by an UpdateSource
	/// </summary>
	public class UpdateTarget : Scene.ISceneObject
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
				throw new ApplicationException( "UpdateTarget requires that a Connections object be present in the scene systems" );
			}

			//	Subscribe to the new connection event
			foreach ( IConnection connection in connections.AllConnections )
			{
				OnNewConnection( connection );
			}
			connections.NewConnection += new NewConnectionDelegate( OnNewConnection );
		}

		/// <summary>
		/// Invoked when this object is removed from a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was removed from</param>
		public void	RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion

		#region	Handlers

		/// <summary>
		/// Adds an update handler
		/// </summary>
		public void AddHandler( IUpdateHandler handler )
		{
			m_Handlers.Add( handler );
			m_HandlerMap[ handler.Id ] = handler;
		}

		/// <summary>
		/// Removes an update handler
		/// </summary>
		public void RemoveHandler( IUpdateHandler handler )
		{
			m_Handlers.Remove( handler );
			m_HandlerMap.Remove( handler.Id );
		}

		#endregion

		#region	Private stuff

		private ArrayList	m_Handlers		= new ArrayList( );
		private Hashtable	m_HandlerMap	= new Hashtable( );
		private uint		m_Sequence;

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnNewConnection( IConnection connection )
		{
			connection.ReceivedMessage += new ConnectionReceivedMessageDelegate( OnReceivedMessage );
		}

		/// <summary>
		/// Handles messages sent over a specified connection
		/// </summary>
		private void OnReceivedMessage( IConnection connection, Components.Message msg )
		{
			if ( !( msg is UpdateMessageBatch ) )
			{
				//	Not interested
				return;
			}

			UpdateMessageBatch batchMsg = ( UpdateMessageBatch )msg;
			if ( batchMsg.Sequence < m_Sequence )	//	TODO: Should be <=. Just ordering issues between update message creation and sequence increment
			{
				return;
			}

			//	Message away
			if ( batchMsg.Messages != null )
			{
				foreach ( UpdateMessage updateMsg in batchMsg.Messages )
				{
					if ( updateMsg.Sequence >= m_Sequence )	//	TODO: Should be >. Just ordering issues between update message creation and sequence increment
					{
						IUpdateHandler handler = ( IUpdateHandler )m_HandlerMap[ updateMsg.TargetId ];
						handler.Handle( updateMsg );
					}
				}
			}
			m_Sequence = batchMsg.Sequence;

			//	Let's let the source know that we got an update! yay!
			//	TODO: Should this be sent at every frame?
			//	TODO: If there's an update source, then the information about the target sequence can be piggy-backed in an UpdateMessageBatch
			connection.DeliverMessage( new TargetSequenceMessage( m_Sequence ) );
		}

		#endregion
	}
}
