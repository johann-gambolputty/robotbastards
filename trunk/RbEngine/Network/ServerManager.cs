using System;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// Stores any registered servers
	/// </summary>
	public class ServerManager
	{

		/// <summary>
		/// Server manager instance
		/// </summary>
		public static ServerManager	Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		/// <summary>
		/// Adds a new server to the server manager
		/// </summary>
		/// <param name="server">Server to register</param>
		/// <exception cref="ApplicationException">Throws an ApplicationException if a server with the same name already exists in the manager</exception>
		public void			RegisterServer( ServerBase server )
		{
			if ( FindServer( server.Name ) != null )
			{
				throw new ApplicationException( string.Format( "Unable to add server \"{0}\" - a server with that name already exists", server.Name ) );
			}
			m_Servers.Add( server );
		}

		/// <summary>
		/// Finds a named server in the manager
		/// </summary>
		/// <param name="name">Server name</param>
		/// <returns>Returns the named server, or null if no server with that name could be found</returns>
		public ServerBase	FindServer( string name )
		{
			foreach ( ServerBase curServer in m_Servers )
			{
				if ( string.Compare( curServer.Name, name, true ) == 0 )
				{
					return curServer;
				}
			}
			return null;
		}

		private ArrayList				m_Servers		= new ArrayList( );
		private static ServerManager	ms_Singleton	= new ServerManager( );
	}
}
