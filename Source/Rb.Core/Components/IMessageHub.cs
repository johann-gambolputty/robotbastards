using System;

namespace Rb.Core.Components
{
	/// <summary>
	/// Interface for objects that act as a hub for chains of message handlers
	/// </summary>
	public interface IMessageHub
	{
		/// <summary>
		/// Adds a recipient for messages of a given type
		/// </summary>
		/// <param name="messageType">Base class of messages that the recipient is interested in</param>
		/// <param name="recipient">Recipient call</param>
		/// <param name="order">Recipient order value</param>
        /// <seealso cref="MessageRecipientChain.AddRecipient"/>
        /// <seealso cref="MessageHub.AddDispatchRecipient"/>
		void AddRecipient( Type messageType, MessageRecipientDelegate recipient, int order );

		/// <summary>
		/// Removes a recipient from the recipient chain for messages of the specified type
		/// </summary>
		void RemoveRecipient( Type messageType, object obj );

		/// <summary>
		/// Sends a message to all recipients of that message type
		/// </summary>
		void DeliverMessageToRecipients( Message msg );
	}
}
