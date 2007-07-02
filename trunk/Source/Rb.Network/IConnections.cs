using System.Collections.Generic;

namespace Rb.Network
{
    public delegate void ConnectionAddedDelegate( IConnection connection );
    public delegate void ConnectionRemovedDelegate( IConnection connection );

    /// <summary>
    /// Stores a list of all connections
    /// </summary>
    public interface IConnections : IEnumerable< IConnection >
    {
        /// <summary>
        /// Event, called when a connection is added to the connection list
        /// </summary>
        event ConnectionAddedDelegate ConnectionAdded;

        /// <summary>
        /// Event, called when a connection is removed from the connection list
        /// </summary>
        event ConnectionRemovedDelegate ConnectionRemoved;

		/// <summary>
		/// Adds an IConnection
		/// </summary>
		/// <param name="connection">Connection to add</param>
		void Add( IConnection connection );

		/// <summary>
		/// Removes an IConnection
		/// </summary>
		/// <param name="connection">Connection to remove</param>
		void Remove( IConnection connection );

		/// <summary>
		/// Receives messages from all connections (calling <see cref="IConnection.ReceiveMessages"/>)
		/// </summary>
		void ReceiveMessages( );

		/// <summary>
		/// Closes all connections
		/// </summary>
		void DisconnectAll( );

		/// <summary>
		/// Returns the number of stored connections
		/// </summary>
		int ConnectionCount
		{
			get;
		}

		/// <summary>
		/// Returns an indexed connection
		/// </summary>
		IConnection GetConnection( int index );
    }
}
