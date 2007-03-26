using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Entities
{
	/// <summary>
	/// Sends entity action messages to a client
	/// </summary>
	public class EntityActionClientUpdater : Network.Runt.IClientUpdater, Components.IChildObject, Scene.ISceneObject
	{
		#region IClientUpdater Members

		/// <summary>
		/// Sets the oldest client sequence value
		/// </summary>
		/// <remarks>
		/// Set by ClientUpdateManager. This is the sequence value of the least up-to-date client connected to the server.
		/// </remarks>
		public int OldestClientSequence
		{
			set
			{
				//	Remove all messages from the message list that are older than the oldest value
				while ( ( m_UpdateMessageSequences.Count > 0 ) && ( ( int )m_UpdateMessageSequences[ 0 ] < value ) )
				{
					m_UpdateMessageSequences.RemoveAt( 0 );
					m_UpdateMessages.RemoveAt( 0 );
				}
			}
		}

		//	TODO: This is pretty bad - instead of calculating movement deltas, I've just batched up all movement messages
		//	received, then dumped them onto the client (i.e. the movement messages themselves constitute the deltas, which
		//	is pretty wasteful)... it's just to test the system

		/// <summary>
		/// Creates a message used to update the client
		/// </summary>
		public Message[] CreateUpdateMessages( int clientSequence, int serverSequence )
		{
			if ( m_NumUpdateMessages == 0 )
			{
				return null;
			}

			int messageCount	= 0;
			int messageIndex	= 0;
			for ( ; messageCount < m_; ++messageCount )
			{
				if ( m_UpdateMessageSequences[ messageIndex ] )
				{
				}
			}

			m_NumUpdateMessages = 0;
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
				if ( m_NumUpdateMessages < MaxUpdateMessages )
				{
					//	TODO: Aggregate movements (e.g. two MovementXzRequest objects could be aggregated into a single request)
					m_UpdateMessages[ m_UpdateMessageIndex ]			= msg;
					m_UpdateMessageSequences[ m_UpdateMessageIndex ]	= m_CurrentSequence;

					m_UpdateMessageIndex = ( m_UpdateMessageIndex + 1 ) % MaxUpdateMessages;
					++m_NumUpdateMessages;
				}
				else
				{
					//	TODO: Client should be disconnected
				}
			}
			return MessageRecipientResult.DeliverToNext;
		}

		#region ISceneObject Members

		public void AddedToScene( Scene.SceneDb db )
		{
		}

		public void RemovedFromScene( Scene.SceneDb db )
		{ 
		}

		#endregion

		private const int	MaxUpdateMessages = 128;

		private Message[]	m_UpdateMessages			= new Message[ MaxUpdateMessages ];
		private int[]		m_UpdateMessageSequences	= new int[ MaxUpdateMessages ];
		private int			m_UpdateMessageIndex		= 0;
		private int			m_NumUpdateMessages			= 0;
		private int			m_CurrentSequence			= 0;
	}
}
