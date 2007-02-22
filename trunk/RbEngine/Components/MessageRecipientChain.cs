using System;
using System.Collections;

namespace RbEngine.Components
{
	/// <summary>
	/// Message recipient chain ordering
	/// </summary>
	public enum MessageRecipientOrder
	{
		First	= 0,
		Default	= 50,
		Last	= 100
	}

	/// <summary>
	/// Delegate that handles messages
	/// </summary>
	public delegate void MessageRecipientDelegate( Message msg );

	/// <summary>
	/// Stores an ordered list of recipients
	/// </summary>
	public class MessageRecipientChain
	{
		/// <summary>
		/// Adds a recipient to the recipient list. The recipient will be among the first to receive delivered messages
		/// </summary>
		/// <param name="recipient">New message recipient</param>
		/// <param name="order">Recipient order</param>
		public void				AddRecipient( MessageRecipientDelegate recipient, int order )
		{
			//	TODO: proper ordering
			if ( order == ( int )MessageRecipientOrder.First )
			{
				m_Recipients.Insert( 0, recipient );
			}
			else
			{
				m_Recipients.Add( recipient );
			}
		}

		/// <summary>
		/// Adds a recipient to the end of the recipient list. The recipient will be among the last to receive delivered messages
		/// </summary>
		/// <param name="recipient">New message recipient</param>
		public void				AddRecipientToEnd( MessageRecipientDelegate recipient )
		{
			m_Recipients.Add( recipient );
		}

		/// <summary>
		/// Attaches a message to this recipient list, and delivers it to the first recipient
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		public void				Deliver( Message msg )
		{
			if ( m_Recipients.Count > 0 )
			{
				msg.AddToRecipientChain( this );
				( ( MessageRecipientDelegate )m_Recipients[ 0 ] )( msg );
			}
		}

		/// <summary>
		/// Delivers a message to the next recipient in the list
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		public void				DeliverToNext( ref int recipientIndex, Message msg )
		{
			int numRecipients = m_Recipients.Count;
			do
			{
				++recipientIndex;
			}
			while ( ( recipientIndex < numRecipients ) && ( m_Recipients[ recipientIndex ] == null ) );

			if ( recipientIndex >= numRecipients )
			{
				msg.RemoveFromRecipientChain( );
			}
			else
			{
				( ( MessageRecipientDelegate )m_Recipients[ recipientIndex ] )( msg );
			}
		}

		private ArrayList	m_Recipients = new ArrayList( );
	}
}
