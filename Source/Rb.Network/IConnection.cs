using Rb.Core.Components;

namespace Rb.Network
{

	/// <summary>
	/// Delegate, used by the <see cref="IConnection.ReceivedMessage"/> event
	/// </summary>
	public delegate void ConnectionReceivedMessageDelegate( IConnection connection, Message msg );

	/// <summary>
	/// Delegate, used by <see cref="IConnection.Disconnected"/> event
	/// </summary>
	public delegate void ConnectionDisconnected( IConnection connection );

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
		/// Connection disconnected event
		/// </summary>
		event ConnectionDisconnected Disconnected;

        /// <summary>
        /// Sends a message over the connection
        /// </summary>
        /// <param name="msg">Message to send</param>
        void DeliverMessage( Message msg );

		/// <summary>
		/// Updates the connection, checking for any received messages
		/// </summary>
		void ReceiveMessages( );

		/// <summary>
		/// Closes the connection
		/// </summary>
		void Disconnect( );
	}
}
