using System;

namespace Rb.Core.Components
{
	/// <summary>
	/// Base class for messages
	/// </summary>
	[Serializable]
	public abstract class Message
	{
		#region	Message recipient chain support

		/// <summary>
		/// Adds this message to a recipient list
		/// </summary>
		/// <param name="recipients">Recipient chain</param>
		public void AddToRecipientChain( MessageRecipientChain recipients )
		{
			m_Recipients		= recipients;
			m_RecipientIndex	= 0;
		}

		/// <summary>
		/// Delivers this message to the next recipient in the recipient chain that it is attached to
		/// </summary>
		public void DeliverToNextRecipient( )
		{
			if ( m_Recipients != null )
			{
				m_Recipients.DeliverToNext( ref m_RecipientIndex, this );
			}
		}

		/// <summary>
		/// Removes this message from a recipient list
		/// </summary>
		public void RemoveFromRecipientChain( )
		{
			m_Recipients = null;
		}

		#endregion

		#region	Message sender

		/// <summary>
		/// Access to the (optional) sender of this message
		/// </summary>
		public object Sender
		{
			get { return m_Sender; }
			set { m_Sender = value; }
		}

		#endregion

		#region	Private stuff

		[NonSerialized]
		private MessageRecipientChain	m_Recipients;

		[NonSerialized]
		private int						m_RecipientIndex;

		[NonSerialized]
		private Object					m_Sender;

		#endregion
	}
}
