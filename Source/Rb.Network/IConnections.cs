using System.Collections.Generic;

namespace Rb.Network
{
    public delegate void ConnectionAddedDelegate(IConnection connection);
    public delegate void ConnectionRemovedDelegate(IConnection connection);

    /// <summary>
    /// Stores a list of all connections
    /// </summary>
    public interface IConnections : IList< IConnection >
    {
        /// <summary>
        /// Event, called when a connection is added to the connection list
        /// </summary>
        public event ConnectionAddedDelegate ConnectionAdded;

        /// <summary>
        /// Event, called when a connection is removed from the connection list
        /// </summary>
        public event ConnectionRemovedDelegate ConnectionRemoved;
    }
}
