using System.Collections.Generic;

namespace Rb.Network
{
    /// <summary>
    /// Stores a list of all connections
    /// </summary>
    public class Connections : IConnections
	{
		#region IConnections Members

		/// <summary>
		/// Event, called when a connection is added to the connection list
		/// </summary>
		public event ConnectionAddedDelegate ConnectionAdded;

		/// <summary>
		/// Event, called when a connection is removed from the connection list
		/// </summary>
		public event ConnectionRemovedDelegate ConnectionRemoved;

		/// <summary>
        /// Adds a connection to the list
        /// </summary>
        /// <param name="connection">Connection to add</param>
        public void Add( IConnection connection )
        {
			lock ( m_Connections )
			{
				m_Connections.Add( connection );
			}
			connection.Disconnected += Remove;
			if ( ConnectionAdded != null )
			{
				ConnectionAdded( connection );
			}
        }

        /// <summary>
        /// Removes a connection to the list
        /// </summary>
        /// <param name="connection">Connection to remove</param>
        public void Remove( IConnection connection )
        {
			connection.Disconnected -= Remove;
			lock ( m_Connections )
			{
				m_Connections.Remove( connection );
			}
            if ( ConnectionRemoved != null )
            {
                ConnectionRemoved( connection );
            }
        }

		/// <summary>
		/// Receives messages from all connections (calling <see cref="IConnection.ReceiveMessages"/>)
		/// </summary>
		public void ReceiveMessages( )
		{
			lock ( m_Connections )
			{
				foreach ( IConnection connection in m_Connections )
				{
					connection.ReceiveMessages( );
				}
			}
		}

		/// <summary>
		/// Closes all connections
		/// </summary>
		public void DisconnectAll( )
		{
			while ( m_Connections.Count > 0 )
			{
				m_Connections[ 0 ].Disconnect( );
				//	No need to remove them from the connection, the handling
				//	of the connection Disconnected even will do that
			}
		}


		/// <summary>
		/// Returns the number of stored connections
		/// </summary>
		public int ConnectionCount
		{
			get
			{
				lock ( m_Connections )
				{
					return m_Connections.Count;
				}
			}
		}

		/// <summary>
		/// Returns an indexed connection
		/// </summary>
		public IConnection GetConnection( int index )
		{
			lock ( m_Connections )
			{
				return m_Connections[ index ];
			}
		}

		#endregion

		#region IEnumerable<IConnection> Members

		/// <summary>
		/// Returns an enumerator that runs through all the stored connections
		/// </summary>
		public IEnumerator<IConnection> GetEnumerator( )
		{
			return m_Connections.GetEnumerator( );
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that runs through all the stored connections
		/// </summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( )
		{
			return m_Connections.GetEnumerator( );
		}

		#endregion

		#region Private stuff

		private IList<IConnection> m_Connections = new List<IConnection>( );

		#endregion
	}
}