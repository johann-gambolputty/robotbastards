using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Entities
{
	/// <summary>
	/// Sends entity action messages to a client
	/// </summary>
	public class EntityActionClientUpdater : Network.IClientUpdater, Components.IChildObject
	{
		#region IClientUpdater Members

		/// <summary>
		/// Creates a message used to update the client
		/// </summary>
		public Message[] CreateUpdateMessages( )
		{
			Message[] messages = ( Message[] )m_UpdateMessages.ToArray( typeof( Message[] ) );
			m_UpdateMessages.Clear( );
			return messages;
		}

		#endregion

		#region IChildObject Members

		/// <summary>
		/// Listens out for action messages in the parent object
		/// </summary>
		public void AddedToParent( Object parentObject )
		{
			IMessageHandler parentMessages = ( IMessageHandler )parentObject;
			parentMessages.AddRecipient( typeof( MovementRequest ), new MessageRecipientDelegate( HandleMovementRequest ), ( int )MessageRecipientOrder.Last );
		}

		#endregion

		/// <summary>
		/// Handles movement requests, adding them to a list that will be batched up and passed out of CreateUpdateMessage()
		/// </summary>
		private Components.MessageRecipientResult HandleMovementRequest( Message msg )
		{
			if ( msg is MovementRequest )
			{
				//	TODO: Aggregate movements (e.g. two MovementXzRequest objects could be aggregated into a single request)
				m_UpdateMessages.Add( msg );
			}
			return MessageRecipientResult.DeliverToNext;
		}

		private ArrayList m_UpdateMessages = new ArrayList( );
	}
}
