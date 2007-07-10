using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Rb.Network
{
	/// <summary>
	/// A TCP SocketConnection, created from a TCP socket and various setup bits and pieces
	/// </summary>
	public class TcpSocketConnection : SocketConnection
	{
		/// <summary>
		/// Access to the connection string
		/// </summary>
		public string	ConnectionString
		{
			set { m_ConnectionString = value; }
			get { return m_ConnectionString; }
		}

		/// <summary>
		/// The port through which the connection is made
		/// </summary>
		public int	Port
		{
			set { m_Port = value; }
			get { return m_Port; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public TcpSocketConnection( )
		{
		}
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public TcpSocketConnection( string connectionString, int port )
		{
			ConnectionString = connectionString;
			Port = port;
		}


		/// <summary>
		/// Runs the connection
		/// </summary>
		public void OpenConnection( )
		{
			Socket socket;
			try
			{
				//	Create a socket
				socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
			}
			catch ( SocketException ex )
			{
				throw new ApplicationException( "Failed to create TCP connection socket", ex );
			}

			IPEndPoint endPoint = null;
			try
			{
				//	Resolve connection string
				NetworkLog.Info( "Resolving connection string \"{0}\"", ConnectionString );
				IPAddress address = Dns.GetHostEntry( ConnectionString ).AddressList[ 0 ];

				//	Create end point
				endPoint = new IPEndPoint( address, Port );

				//	Connect to the end point
				socket.Connect( endPoint );

				//	Create a SocketConnection from the freshly connected socket
				Setup( socket, endPoint.ToString( ) );
				NetworkLog.Info( "Connected to \"{0}\"", endPoint );
			}
			catch ( Exception ex )
			{
				throw new ApplicationException( string.Format( "Failed to initialise TCP socket connection \"{0}\" ({1})", endPoint, ConnectionString ), ex );
			}
		}

		#region	Private stuff

		private string	m_ConnectionString	= "localhost";
		private int		m_Port				= 11000;

		#endregion
	}
}
