using System.Collections.Generic;

namespace Rb.Network
{
    /// <summary>
    /// Stores a list of all connections
    /// </summary>
    public class Connections : Core.Utils.PolyList< IConnection >, IConnections
    {
        /// <summary>
        /// Adds a connection to the list
        /// </summary>
        /// <param name="connection">Connection to add</param>
        public override void Add( IConnection connection )
        {
            base.Add( connection );
            if ( ConnectionAdded != null )
            {
                ConnectionAdded( connection );
            }
        }

        /// <summary>
        /// Removes a connection to the list
        /// </summary>
        /// <param name="connection">Connection to remove</param>
        public override void Remove( IConnection connection )
        {
            base.Remove( connection );
            if ( ConnectionRemoved != null )
            {
                ConnectionRemoved( connection );
            }
        }

        //  TODO: AP: Add overrides for other remove/add methods
    }
}