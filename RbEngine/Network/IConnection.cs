using System;

namespace RbEngine.Network
{
	/// <summary>
	/// Connection interface
	/// </summary>
	public interface IConnection : Components.IMessageHandler
	{
		/// <summary>
		/// Gets the name of this connection
		/// </summary>
		string	Name
		{
			get;
		}

		/// <summary>
		/// Returns true if this connection is a connection to a client
		/// </summary>
		bool ConnectionToClient
		{
			get;
		}

		/// <summary>
		/// Delivers a message over this connection
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		/// <remarks>
		/// This should be invoked by any implementation of IMessageHandler.HandleMessage()
		/// </remarks>
		void DeliverMessage( Components.Message msg );
	}
}
