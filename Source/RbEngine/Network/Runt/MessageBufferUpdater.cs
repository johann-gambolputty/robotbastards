using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Handy base class for classes implementing the IClientUpdater or IServerUpdater interfaces
	/// </summary>
	public class MessageBufferUpdater
	{
		/// <summary>
		/// Adds a message to the buffer with a given sequence value
		/// </summary>
		public void		AddMessage( UpdateMessage msg, uint localSequence )
		{
			//	TODO: These messages will not be arriving at the server in sequence - they need to be sorted
			msg.Sequence = localSequence;
			m_Messages[ m_CurMessageIndex ] = msg;

			//	NOTE: This crushes messages in order from oldest to youngest if the number of stored messages reached
			//	a maximum (because some lousy client hasn't been keeping up-to-date)
			m_CurMessageIndex	= ( m_CurMessageIndex + 1 ) % m_Messages.Length;
			m_NumMessages		= Maths.Utils.Min( m_NumMessages + 1, m_Messages.Length );
		}

		/// <summary>
		/// Gets messages from the buffer that have later or equal sequence values
		/// </summary>
		public void		GetMessages( ArrayList messages, uint sequence )
		{
			if ( m_NumMessages == 0 )
			{
				return;
			}

			//	Get the index of the first message with a later or equal sequence value
			int messageIndex	= m_FirstMessageIndex;
			int messageCount	= 0;
			for ( ; messageCount < m_NumMessages; ++messageCount )
			{
				if ( m_Messages[ messageIndex ].Sequence > sequence )
				{
					break;
				}
				messageIndex = ( messageIndex + 1 ) % m_Messages.Length;
			}

			//	Add all remaining messages to the messages array
			for ( ; messageCount < m_NumMessages; ++messageCount )
			{
				messages.Add( m_Messages[ messageIndex ] );
				messageIndex = ( messageIndex + 1 ) % m_Messages.Length;
			}
		}

		/// <summary>
		/// Sets the oldest sequence value, removing any messages that have older sequences
		/// </summary>
		public void		SetOldestSequence( uint sequence )
		{
			//	Find the first message that has an older or equal sequence value
			int messageIndex	= m_FirstMessageIndex;
			int messageCount	= 0;
			for ( ; messageCount < m_NumMessages; ++messageCount )
			{
				if ( m_Messages[ messageIndex ].Sequence > sequence )
				{
					break;
				}

				m_Messages[ messageIndex ]	= null;
				messageIndex = ( messageIndex + 1 ) % m_Messages.Length;
			}

			m_FirstMessageIndex = messageIndex;
			m_NumMessages -= messageCount;
		}

		private const int		DefaultMaxMessages = 128;

		private int				m_FirstMessageIndex;
		private int				m_CurMessageIndex;
		private int				m_NumMessages;
		private UpdateMessage[]	m_Messages = new UpdateMessage[ DefaultMaxMessages ];
	}
}
