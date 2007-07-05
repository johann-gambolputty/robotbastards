using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Rb.Network
{
	/// <summary>
	/// Listens out for client connection requests, creating new <see cref="TcpSocketConnection"/> objects for each one
	/// </summary>
	public class TcpConnectionListener : IDisposable
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
		/// Kills the listener thread
		/// </summary>
		~TcpConnectionListener( )
		{
			Dispose( );
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
			Dispose( );
			m_Thread = new Thread( ListenThread );
			m_Thread.Name = "TcpListenerThread";
			m_Thread.Start( connections );
		}


		#region IDisposable Members

		/// <summary>
		/// Kills the listener thread
		/// </summary>
		public void Dispose( )
		{
			if ( m_Thread != null )
			{
				m_Thread.Abort( );
				m_Thread.Join( );
				m_Thread = null;
			}
		}

		#endregion

		#region	Private stuff

		private Thread			m_Thread;
		private string			m_ConnectionString	= "localhost";
		private int				m_Port				= 11000;


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
		private void ListenThread( object param )
		{
			IConnections connections = ( IConnections )param;

			IPAddress address;

			//	Try to resolve the connection string
			try
			{
				NetworkLog.Info( "Attemping to resolve listener address \"{0}\"", ConnectionString );
				address = Dns.GetHostEntry( ConnectionString ).AddressList[ 0 ];
			}
			catch ( ThreadAbortException )
			{
				NetworkLog.Warning( "Listener thread was aborted during address resolution" );
				return;
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to resolve connection string \"{0}\"", ConnectionString ), exception );
			}


			//	Create a TCP listener at the address
			TcpListener listener = null;
			try
			{
				NetworkLog.Info( "Attempting to listen for clients at \"{0}:{1}\"", address, Port );
				listener = new TcpListener( address, Port );
				listener.Start( );
			}
			catch ( ThreadAbortException )
			{
				NetworkLog.Warning( "Listener thread was aborted during listener setup" );
				return;
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to create TCP listener at \"{0}\"", listener.LocalEndpoint ), exception );
			}

			//	Loop forever, listening out for new clients
			while ( true )
			{
				if ( listener.Pending( ) )
				{
					AddConnection( connections, listener.AcceptSocket( ) );
				}

				//	Sleep for a bit (listening for connections doesn't have to be a full-time thing)
				Thread.Sleep( 10 );
			}
		}

		#endregion
	}
}
