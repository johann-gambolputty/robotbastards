using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Entities
{
	/// <summary>
	/// Sends entity action messages to a client
	/// </summary>
	public class EntityActionClientUpdater : Network.Runt.IClientUpdater, Components.IChildObject
	{
		#region IClientUpdater Members

		/// <summary>
		/// Informs this updater that it must be able to create update messages for a new client with the specified index
		/// </summary>
		/// <param name="clientIndex">New client's index</param>
		public void AddNewClient( int clientIndex )
		{
			//	Don't care about new clients - this updater just maintains state to the oldest ack-ed client state
		}

		//	TODO: This is pretty bad - instead of calculating movement deltas, I've just batched up all movement messages
		//	received, then dumped them onto the client (i.e. the movement messages themselves constitute the deltas, which
		//	is pretty wasteful)... it's just to test the system

		/// <summary>
		/// Creates a message used to update the client
		/// </summary>
		public Message[] CreateUpdateMessages( int clientIndex, int clientSequence, int serverSequence )
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
				m_UpdateMessageSequences.Add( m_CurrentSequence );
			}
			return MessageRecipientResult.DeliverToNext;
		}

		private ArrayList	m_UpdateMessages			= new ArrayList( );
		private ArrayList	m_UpdateMessageSequences	= new ArrayList( );
		private int			m_CurrentSequence			= 0;
	}
}
