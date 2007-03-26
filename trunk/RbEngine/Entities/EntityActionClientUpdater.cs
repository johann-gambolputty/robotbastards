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
		/// Gets the ID of this updater
		/// </summary>
		public ObjectId Id
		{
			get
			{
				return ( ( Components.IUnique )m_Parent ).Id;
			}
		}

		/// <summary>
		/// Handles an update message sent from a client
		/// </summary>
		void HandleClientUpdate( UpdateMessage msg )
		{

		}


		/// <summary>
		/// Sets the oldest client sequence value
		/// </summary>
		/// <remarks>
		/// Set by ClientUpdateManager. This is the sequence value of the least up-to-date client connected to the server.
		/// </remarks>
		public void SetOldestClientSequence( int sequence )
		{
			if ( m_NumUpdateMessages == 0 )
			{
				return;
			}

			//	Find the first message that 
			int messageIndex	= m_FirstUpdateMessageIndex;
			int messageCount	= 0;
			for ( ; messageCount < m_NumUpdateMessages; ++messageCount )
			{
				if ( m_UpdateMessageSequences[ messageIndex ] >= sequence )
				{
					break;
				}

				m_UpdateMessages[ messageIndex ]			= null;
				m_UpdateMessageSequences[ messageIndex ]	= -1;

				messageIndex = ( messageIndex + 1 ) % MaxUpdateMessages;
			}

			m_FirstUpdateMessageIndex = messageIndex;
			m_NumUpdateMessages -= messageCount;
		}

		//	TODO: This is pretty bad - instead of calculating movement deltas, I've just batched up all movement messages
		//	received, then dumped them onto the client (i.e. the movement messages themselves constitute the deltas, which
		//	is pretty wasteful)... it's just to test the system

		/// <summary>
		/// Creates messages used to update the client
		/// </summary>
		public Network.Runt.UpdateMessage[] CreateUpdateMessages( int clientSequence, int serverSequence )
		{
			m_CurrentSequence = serverSequence;

			if ( m_NumUpdateMessages == 0 )
			{
				return null;
			}

			//	Determine the index of the first update message that the client should receive
			int messageCount = 0;
			int messageIndex = m_FirstUpdateMessageIndex;
			for ( ; messageCount < m_NumUpdateMessages; ++messageCount )
			{
				if ( m_UpdateMessageSequences[ messageIndex ] >= clientSequence )
				{
					//	Reached the first message that starts at or after the current client sequence
					break;
				}
				messageIndex = ( messageIndex + 1 ) % MaxUpdateMessages;
			}

			//	If there were no update messages generated since the client sequence, then exit
			int updateMessageCount = m_NumUpdateMessages - messageCount;
			if ( updateMessageCount == 0 )
			{
				return null;
			}

			//	Create a message array and fill it with the remaining messages
			Components.ObjectId				id			= ( ( Components.IUnique )m_Parent ).Id;
			Network.Runt.UpdateMessage[]	messages	= new Network.Runt.UpdateMessage[ updateMessageCount ];
			for ( messageCount = 0; messageCount < updateMessageCount; ++messageCount )
			{
				messages[ messageCount ] = new Network.Runt.WrapUpdateMessage( id, m_UpdateMessages[ messageIndex ] );
				messageIndex = ( messageIndex + 1 ) % MaxUpdateMessages;
			}

			return messages;
		}

		#endregion

		#region IChildObject Members

		/// <summary>
		/// Listens out for action messages in the parent object
		/// </summary>
		public void AddedToParent( Object parentObject )
		{
			m_Parent = parentObject;
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
				if ( m_NumUpdateMessages < MaxUpdateMessages )
				{
					//	TODO: Aggregate movements (e.g. two MovementXzRequest objects could be aggregated into a single request)
					m_UpdateMessages[ m_CurUpdateMessageIndex ]			= msg;
					m_UpdateMessageSequences[ m_CurUpdateMessageIndex ]	= m_CurrentSequence;

					m_CurUpdateMessageIndex = ( m_CurUpdateMessageIndex + 1 ) % MaxUpdateMessages;
					++m_NumUpdateMessages;
				}
				else
				{
					//	TODO: Client should be disconnected, or sent the absolute state
				}
			}
			return MessageRecipientResult.DeliverToNext;
		}

		private const int	MaxUpdateMessages = 128;

		private Message[]	m_UpdateMessages			= new Message[ MaxUpdateMessages ];
		private int[]		m_UpdateMessageSequences	= new int[ MaxUpdateMessages ];
		private int			m_FirstUpdateMessageIndex	= 0;
		private int			m_CurUpdateMessageIndex		= 0;
		private int			m_NumUpdateMessages			= 0;
		private int			m_CurrentSequence			= 0;
		private Object		m_Parent;
	}
}
