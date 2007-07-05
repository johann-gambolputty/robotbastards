using System.Collections.Generic;

namespace Rb.Network
{
	/// <summary>
	/// Delegate used by <see cref="IConnections.ConnectionAdded"/>
	/// </summary>
	/// <param name="connection">Connection that was added</param>
    public delegate void ConnectionAddedDelegate( IConnection connection );

	/// <summary>
	/// Delegate used by <see cref="IConnections.ConnectionRemoved"/>
	/// </summary>
	/// <param name="connection">Connection that was removed</param>
    public delegate void ConnectionRemovedDelegate( IConnection connection );

	/// <summary>
	/// Delegate used by <see cref="IConnections.ConnectionsUpdated"/>
	/// </summary>
	public delegate void ConnectionsUpdatedDelegate( );

    /// <summary>
    /// Stores a list of all connections
    /// </summary>
    /// <remarks>
	/// It's up to the implementor to call <see cref="IConnection.ReceiveMessages"/> at time intervals determined by <see cref="ReadUpdateTime"/>
    /// </remarks>
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
		/// Event, called when the connections are updated
		/// </summary>
    	event ConnectionsUpdatedDelegate ConnectionsUpdated;

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
		/// Access to the time between read updates
		/// </summary>
		int ReadUpdateTime
		{
			get; set;
		}

		/// <summary>
		/// Returns an indexed connection
		/// </summary>
		IConnection GetConnection( int index );
    }
}
