using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Interface for classes that can handle <see cref="Message"/> objects
	/// </summary>
	public interface IMessageHandler
	{
		/// <summary>
		/// Handles messages
		/// </summary>
		/// <param name="msg">Message to handle</param>
		void HandleMessage( Message msg );

		/// <summary>
		/// Adds a recipient for messages of a given type
		/// </summary>
		/// <param name="messageType">Base class of messages that the recipient is interested in</param>
		/// <param name="recipient">Recipient call</param>
		/// <param name="order">Recipient order value</param>
		/// <seealso cref="MessageRecipientChain.AddRecipient"/>
		void AddRecipient( Type messageType, Message.RecipientDelegate recipient, int order );
	}
}
