using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RbEngine.Network
{
	/// <summary>
	/// Full-on server
	/// </summary>
	public class Server : ServerBase
	{
		/// <summary>
		/// Listens for connection requests
		/// </summary>
		public void SetupConnection( string connectionString )
		{
			m_ConnectionString = connectionString;
			Thread thread = new Thread( new ThreadStart( RunServerComms ) );
			thread.Start( );
		}


		private void RunServerComms( )
		{
			IPAddress	address		= Dns.Resolve( m_ConnectionString ).AddressList[ 0 ];
			TcpListener	listener	= new TcpListener( address, Port );

			while ( true )
			{
				if ( listener.Pending )
				{
					Socket clientSocket = listener.AcceptSocket( );
					Output.WriteLineCall( Output.NetworkInfo, "Accepted client \"{0}\"", );
					m_Sockets.Add( clientSocket );
				}
			}
		}

		/// <summary>
		/// Creates a socket
		/// </summary>
		protected override Socket	CreateSocket( )
		{
			IPAddress address = Dns.Resolve( m_ConnectionString ).AddressList[ 0 ];

			TcpListener listener = new TcpListener( address, Port );

			Socket socket = null;
			try
			{
				listener.Start( );
				while ( !listener.Pending( ) )
				{
					Thread.Sleep( 10 );
				}
				socket = listener.AcceptSocket( );
			}
			catch//( Exception exception )
			{
				
			}

			return socket;
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

		private string	m_ConnectionString;
	}
}
