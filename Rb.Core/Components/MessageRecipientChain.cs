using System;
using System.Collections;

namespace Rb.Core.Components
{
	/// <summary>
	/// Message recipient chain ordering
	/// </summary>
	public enum MessageRecipientOrder
	{
		/// <summary>
		/// Denotes that the recipient will be the among the first recipients in a recipient chain to process messages
		/// </summary>
		First	= 0,
		
		/// <summary>
		/// Denotes that the recipient will be occupy a default position in the recipient chain
		/// </summary>
		Default	= 50,
		
		/// <summary>
		/// Denotes that the recipient will be among the last recipients in a recipient chain to process messages
		/// </summary>
		Last	= 100
	}

	/// <summary>
	/// MessageRecipientDelegate return values
	/// </summary>
	public enum MessageRecipientResult
	{
		/// <summary>
		/// Delivers the message to the next recipient in the chain (calls <see cref="Message.DeliverToNextRecipient"/>)
		/// </summary>
		DeliverToNext,

		/// <summary>
		/// Removes the message from the recipient chain (calls <see cref="Message.RemoveFromRecipientChain"/>)
		/// </summary>
		RemoveFromChain,

		/// <summary>
		/// Message has been held by the recipient, and it'll be passed on at some later time
		/// </summary>
		/// <remarks>
		/// The recipient becomes responsible for calling <see cref="Message.DeliverToNextRecipient"/>, or <see cref="Message.RemoveFromRecipientChain"/>
		/// at some later point
		/// </remarks>
		Deferred
	}

	/// <summary>
	/// Delegate that handles messages for MessageRecipientChain.Deliver()
	/// </summary>
	public delegate MessageRecipientResult MessageRecipientDelegate( Message msg );

	/// <summary>
	/// Stores an ordered list of recipients
	/// </summary>
	public class MessageRecipientChain
	{
		/// <summary>
		/// Setup constructor. Sets the type of message this chain handles
		/// </summary>
		public MessageRecipientChain( Type messageType )
		{
			m_MessageType = messageType;
		}

		/// <summary>
		/// Gets the type of message this chain handles
		/// </summary>
		public Type MessageType
		{
			get { return m_MessageType; }
		}
	
		/// <summary>
		/// Adds a recipient to the recipient list. The recipient will be among the first to receive delivered messages
		/// </summary>
		/// <param name="recipient">New message recipient</param>
		/// <param name="order">Recipient order</param>
		public void AddRecipient( MessageRecipientDelegate recipient, int order )
		{
			m_Recipients.Add( new Recipient( recipient, order ) );
			m_Recipients.Sort( );
		}

		/// <summary>
		/// Adds a recipient to the end of the recipient list. The recipient will be among the last to receive delivered messages
		/// </summary>
		/// <param name="recipient">New message recipient</param>
		public void AddRecipientToEnd( MessageRecipientDelegate recipient )
		{
			m_Recipients.Add( recipient );
		}

		/// <summary>
		/// Removes a recipient from this chain
		/// </summary>
		public void RemoveRecipient( Object recipient )
		{
			for ( int index = 0; index < m_Recipients.Count; ++index )
			{
				object curRecipient = ( ( Recipient )m_Recipients[ index ] ).RecipientDelegate.Target;
				if ( curRecipient == recipient )
				{
					m_Recipients.RemoveAt( index );
					return;
				}
			}

			throw new ApplicationException( "Could not find recipient in message recipient chain" );
		}

		/// <summary>
		/// Attaches a message to this recipient list, and delivers it to the first recipient
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		public void Deliver( Message msg )
		{
			if ( m_Recipients.Count > 0 )
			{
				msg.AddToRecipientChain( this );
				switch ( ( ( Recipient )m_Recipients[ 0 ] ).RecipientDelegate( msg ) )
				{
					case MessageRecipientResult.DeliverToNext	: msg.DeliverToNextRecipient( );	break;
					case MessageRecipientResult.RemoveFromChain	: msg.RemoveFromRecipientChain( );	break;
					case MessageRecipientResult.Deferred		: break;
				}
			}
		}

		/// <summary>
		/// Delivers a message to the next recipient in the list
		/// </summary>
		/// <param name="recipientIndex">A reference to the index of the recipient in the chain. Set to the index of the next valid recipient</param>
		/// <param name="msg">Message to deliver</param>
		public void DeliverToNext( ref int recipientIndex, Message msg )
		{
			int numRecipients = m_Recipients.Count;
			do
			{
				++recipientIndex;
			}
			while ( ( recipientIndex < numRecipients ) && ( m_Recipients[ recipientIndex ] == null ) );

			if ( recipientIndex >= numRecipients )
			{
				//	Message reached the end of the chain
				msg.RemoveFromRecipientChain( );
			}
			else
			{
				//	Let the current recipient handle the message
				switch ( ( ( Recipient )m_Recipients[ recipientIndex ] ).RecipientDelegate( msg ) )
				{
					case MessageRecipientResult.DeliverToNext	: msg.DeliverToNextRecipient( );	break;
					case MessageRecipientResult.RemoveFromChain	: msg.RemoveFromRecipientChain( );	break;
					case MessageRecipientResult.Deferred		: break;
				}
			}
		}

		/// <summary>
		/// Ordered recipient information
		/// </summary>
		private struct Recipient : IComparable< Recipient >
		{
			public int						Order;
			public MessageRecipientDelegate	RecipientDelegate;

			/// <summary>
			/// Sets up this recipient
			/// </summary>
			public Recipient( MessageRecipientDelegate recipientDelegate, int order )
			{
				RecipientDelegate	= recipientDelegate;
				Order				= order;
			}

			#region IComparable Members

			/// <summary>
			/// Compares this recipient with another
			/// </summary>
			public int CompareTo( Recipient obj )
			{
				int objOrder = obj.Order;
				return ( Order < objOrder ) ? -1 : ( Order > objOrder ? 1 : 0 );
			}

			#endregion
		}

	    private Type        m_MessageType;
		private ArrayList	m_Recipients = new ArrayList( );
	}
}
