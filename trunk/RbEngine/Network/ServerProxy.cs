using System;
using System.Net;
using System.Net.Sockets;


namespace RbEngine.Network
{
	/// <summary>
	/// Proxy server
	/// </summary>
	public class ServerProxy : ServerBase
	{
		/// <summary>
		/// Sets up a connection to a server. Any messages that get sent to this proxy, get marshalled and forwarded to this server
		/// </summary>
		public void SetupConnection( string connectionString )
		{
			Socket socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

			IPAddress address = Dns.Resolve( connectionString ).AddressList[ 0 ];

			socket.Connect( new IPEndPoint( address, Server.Port ) );

		}


		/// <summary>
		/// Adds a client to the server
		/// </summary>
		/// <param name="client">Client to add</param>
		public override void AddClient( Client client )
		{
		}

		/// <summary>
		/// Removes a client from the server
		/// </summary>
		/// <param name="client">Client to remove</param>
		public override void RemoveClient( Client client )
		{
		}

		#region	Private stuff

		#endregion
	}
}
