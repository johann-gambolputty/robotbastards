using System;
using System.Net;
using System.Net.Sockets;
using Rb.Core.Utils;

namespace Rb.Network
{
	/// <summary>
	/// Listens out for client connection requests, creating new <see cref="TcpSocketConnection"/> objects for each one
	/// </summary>
	public class TcpConnectionListener
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public TcpConnectionListener( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public TcpConnectionListener( string connectionString, int port )
		{
			ConnectionString = connectionString;
			Port = port;
		}

		/// <summary>
		/// The address to listen at for connection requests
		/// </summary>
		public string	ConnectionString
		{
			get { return m_ConnectionString; }
			set { m_ConnectionString = value; }
		}

		/// <summary>
		/// The port to listen at for connection requests
		/// </summary>
		public int		Port
		{
			get { return m_Port; }
			set { m_Port = value; }
		}

		/// <summary>
		/// Listens out for connection requests from clients. If a connection is successfully made, it's added to the specified connections object
		/// </summary>
		public void Listen( IConnections connections )
		{
			m_Connections = connections;
			
			IPAddress address;

			//	Try to resolve the connection string
			try
			{
				NetworkLog.Info( "Attemping to resolve listener address \"{0}\"", ConnectionString );
				address = Dns.GetHostEntry( ConnectionString ).AddressList[ 0 ];
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to resolve connection string \"{0}\"", ConnectionString ), exception );
			}

			//	Create a TCP listener at the address
			try
			{
				NetworkLog.Info( "Attempting to listen for clients at \"{0}:{1}\"", address, Port );
				m_Listener = new TcpListener(address, Port);
				m_Listener.Start();
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to create TCP listener at \"{0}\"", m_Listener.LocalEndpoint ), exception );
			}

			m_Clock.Subscribe( Update );
		}


		#region	Private stuff

		private string			m_ConnectionString	= "localhost";
		private int				m_Port				= 11000;
		private Clock			m_Clock				= new Clock( "tcpListenerClock", 50 );
		private IConnections	m_Connections;
		private TcpListener		m_Listener;


		/// <summary>
		/// Adds a client socket
		/// </summary>
		private static void AddConnection( IConnections connections, Socket clientSocket )
		{
			NetworkLog.Info( "Accepting connection \"{0}\"", clientSocket.RemoteEndPoint );

			connections.Add( new SocketConnection( clientSocket, clientSocket.RemoteEndPoint.ToString( ) ) );
		}

		/// <summary>
		/// Thread method that listens out for new client connections
		/// </summary>
		private void Update( Clock clock )
		{
			while ( m_Listener.Pending( ) )
			{
				AddConnection( m_Connections, m_Listener.AcceptSocket( ) );
			}
		}

		#endregion
	}
}
