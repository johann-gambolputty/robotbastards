using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
	/// <summary>
	/// Handy implementation of IMessageHub
	/// </summary>
	public class MessageHub : IMessageHub
	{
		#region IMessageHub Members

		/// <summary>
		/// Adds a recipient for messages of, or derived from, a given type
		/// </summary>
		/// <param name="messageType">Message type</param>
		/// <param name="recipient">Delegate to call when a message of the designated type arrives</param>
		/// <param name="order">Recipient order <see cref="MessageRecipientOrder"/></param>
		public void AddRecipient( Type messageType, MessageRecipientDelegate recipient, int order )
		{
		}

		/// <summary>
		///	Removes a recipient from the message hub
		/// </summary>
		/// <param name="messageType">Type of message</param>
		/// <param name="obj">Object to remove</param>
		public void RemoveRecipient( Type messageType, object obj )
		{
		}

		#endregion
	}
}
