using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Network
{
	/// <summary>
	/// Provides a standard implementation of IConnection (and IMessageHandler)
	/// </summary>
	public abstract class ConnectionBase : IConnection
	{
		#region IConnection Members

		/// <summary>
		/// Gets the name of this connection
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		/// <summary>
		/// Delivers a message over this connection
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		public abstract void DeliverMessage( Components.Message msg );

		#endregion

		#region IMessageHandler Members

		/// <summary>
		/// Delivers the message over the connection, and also to any recipients added by the AddRecipient() call
		/// </summary>
		public void HandleMessage( Components.Message msg )
		{
			DeliverMessage( msg );

			//	Deliver message to anybody that's interested
			Type baseType = typeof( Object );
			for ( Type messageType = msg.GetType( ); messageType != baseType; messageType = messageType.BaseType )
			{
				MessageRecipientChain chain = ( MessageRecipientChain )m_RecipientChains[ messageType ];
				if ( chain != null )
				{
					chain.Deliver( msg );
					return;
				}
			}
		}

		/// <summary>
		/// Adds a recipient. Any messages of, or derived from, the specified type, get passed to the specified delegate
		/// </summary>
		/// <param name="messageType">Type of message to look out for</param>
		/// <param name="recipient">Delegate to call if this object receives a message of the correct type</param>
		/// <param name="order">Recipient order</param>
		public void AddRecipient( Type messageType, Components.MessageRecipientDelegate recipient, int order )
		{
			MessageRecipientChain chain = ( MessageRecipientChain )m_RecipientChains[ messageType ];
			if ( chain == null )
			{
				chain = new MessageRecipientChain( );
				m_RecipientChains[ messageType ] = chain;
			}
			chain.AddRecipient( recipient, order );
		}

		#endregion

		#region	Private stuff

		private Hashtable	m_RecipientChains	= new Hashtable( );
		private string		m_Name				= string.Empty;

		#endregion

	}
}
