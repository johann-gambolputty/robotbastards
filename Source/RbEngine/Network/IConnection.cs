using System;

namespace RbEngine.Network
{

	/// <summary>
	/// Delegate, used by the IConnection.ReceivedMessage event
	/// </summary>
	public delegate void ConnectionReceivedMessageDelegate( IConnection connection, Components.Message msg );

	/// <summary>
	/// Connection interface
	/// </summary>
	public interface IConnection
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
		/// Returns true if the connection is connected
		/// </summary>
		bool IsConnected
		{
			get;
		}

		/// <summary>
		/// Received message event
		/// </summary>
		event ConnectionReceivedMessageDelegate	ReceivedMessage;

		/// <summary>
		/// Delivers a message over this connection
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		void DeliverMessage( Components.Message msg );
	}
}
