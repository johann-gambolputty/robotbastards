using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Entities
{
	/// <summary>
	/// Sends entity action messages to a server
	/// </summary>
	public class EntityActionServerUpdater : Components.IMessageHandler, Scene.ISceneObject, Components.IChildObject
	{
		#region IMessageHandler Members

		/// <summary>
		/// Sends a message on to the 
		/// </summary>
		/// <param name="msg"></param>
		public void HandleMessage( Components.Message msg )
		{

		}

		/// <summary>
		/// Throws an exception - can't add recipients
		/// </summary>
		public void AddRecipient(Type messageType, RbEngine.Components.MessageRecipientDelegate recipient, int order)
		{
			//	TODO: AddRecipient() should be in a different interface
			throw new ApplicationException( "Can't add recipients" );
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			Network.Connections connections = ( Network.Connections )db.GetSystem( typeof( Network.Connections ) );
			connections.NewServerConnection += new Network.NewServerConnectionDelegate( OnNewServerConnection );
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
			( ( IMessageHandler )parentObject ).AddRecipient( typeof( MovementRequest ), new MessageRecipientDelegate( OnActionMessage ), ( int )MessageRecipientOrder.Last );
		}

		#endregion

		private Network.IConnection m_ServerConnection;
		private ArrayList			m_Messages = new ArrayList( );

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
			m_Messages.Add( msg );

			//	Removes the message from the chain - don't want any more objects processing this
			return MessageRecipientResult.RemoveFromChain;
		}


	}
}
