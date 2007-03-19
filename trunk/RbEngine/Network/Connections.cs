using System;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// A set of client-to-server and server-to-client connections
	/// </summary>
	public class Connections
	{
		/// <summary>
		/// Gets a named client to server connection
		/// </summary>
		public IConnection	GetClientToServerConnection( string connectionName )
		{
			foreach ( IConnection curConnection in m_ServerConnections )
			{
				if ( string.Compare( curConnection.Name, connectionName, true ) == 0 )
				{
					return curConnection;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets a named server to client connection
		/// </summary>
		public IConnection	GetServerToClientConnection( string connectionName )
		{
			foreach ( IConnection curConnection in m_ClientConnections )
			{
				if ( string.Compare( curConnection.Name, connectionName, true ) == 0 )
				{
					return curConnection;
				}
			}
			return null;
		}

		private ArrayList	m_ClientConnections = new ArrayList( );
		private ArrayList	m_ServerConnections	= new ArrayList( );
	}
}
