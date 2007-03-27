using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Entities
{
	/// <summary>
	/// Sends entity action messages to a server
	/// </summary>
	public class EntityActionServerUpdater : Scene.ISceneObject, Components.IChildObject, Network.Runt.IServerUpdater
	{
		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			//	Add this updater to the server updater
			Network.Runt.ServerUpdateManager updateManager = ( Network.Runt.ServerUpdateManager )db.GetSystem( typeof( Network.Runt.ServerUpdateManager ) );
			if ( updateManager == null )
			{
				throw new ApplicationException( "EntityActionServerUpdater requires a ServerUpdateManager to present in the scene systems" );
			}

			updateManager.AddUpdater( this );
		}

		/// <summary>
		/// Called when this object is removed from the scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion

		#region	IChildObject Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		/// <param name="parentObject">Parent object</param>
		public void AddedToParent( Object parentObject )
		{
			m_Parent = parentObject;
			( ( IMessageHandler )parentObject ).AddRecipient( typeof( MovementRequest ), new MessageRecipientDelegate( OnActionMessage ), ( int )MessageRecipientOrder.Last );
		}

		#endregion

		private Network.IConnection m_ServerConnection;
		private ArrayList			m_Messages = new ArrayList( );
		private object				m_Parent;

		/// <summary>
		/// Called when a server connection is created
		/// </summary>
		private void OnNewServerConnection( Network.IConnection connection )
		{
			if ( m_ServerConnection != null )
			{
				//	TODO: ...
				throw new ApplicationException( "EntityActionServerUpdater can only handle one server connection" );
			}

			m_ServerConnection = connection;
		}

		/// <summary>
		/// Handles action messages
		/// </summary>
		private MessageRecipientResult OnActionMessage( Message msg )
		{
			//	TODO: Limit the number of stored messages
			m_Messages.Add( msg );

			//	Removes the message from the chain - don't want any more objects processing this
			return MessageRecipientResult.RemoveFromChain;
		}

		#region IServerUpdater Members

		/// <summary>
		/// Handles an update message sent from a server
		/// </summary>
		public void HandleServerUpdate( Network.Runt.UpdateMessage msg )
		{
		}

		/// <summary>
		/// Adds all stored messages to the specified messages list
		/// </summary>
		public void GetUpdateMessages( ArrayList messages )
		{
			Components.ObjectId id = ( ( Components.IUnique )m_Parent ).Id;
			foreach ( Message msg in m_Messages )
			{
				messages.Add( new Network.Runt.WrapUpdateMessage( id, msg ) );
			}
			m_Messages.Clear( );
		}

		#endregion
	}
}
