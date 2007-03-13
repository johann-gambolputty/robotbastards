using System;
using RbEngine.Components;

namespace RbEngine.Network
{
	/// <summary>
	/// Base class for connections from client to server
	/// </summary>
	public abstract class ClientToServerConnection : IMessageHandler
	{
		#region	Server message delivery

		/// <summary>
		/// Delivers a specified message to the server over the connection
		/// </summary>
		/// <remarks>
		/// HandleMessage() can also be called to achieve this - HandleMessage() will also send the message to any recipients added by the AddRecipient() method
		/// </remarks>
		public abstract void	DeliverMessageToServer( Message msg );

		#endregion

		#region	IMessageHandler Members

		/// <summary>
		/// Delivers the specified message to the client, and also to any recipients, added with the AddRecipient() method
		/// </summary>
		public virtual void		HandleMessage( Message msg )
		{
			DeliverMessageToServer( msg );

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
		/// Adds a recipient for messages of a given type
		/// </summary>
		/// <param name="messageType">Base class of messages that the recipient is interested in</param>
		/// <param name="recipient">Recipient call</param>
		/// <param name="order">Recipient order value</param>
		public virtual void AddRecipient( Type messageType, MessageRecipientDelegate recipient, int order )
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

		private System.Collections.Hashtable m_RecipientChains = new System.Collections.Hashtable( );

		#endregion
	}
}
