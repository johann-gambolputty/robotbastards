using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
		public string ConnectionString
		{
			get { return m_ConnectionString; }
			set { m_ConnectionString = value; }
		}

		/// <summary>
		/// The port to listen at for connection requests
		/// </summary>
		public int Port
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
				m_Listener = new TcpListener( address, Port );
				m_Listener.Start( );
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to create TCP listener at \"{0}\"", m_Listener.LocalEndpoint ), exception );
			}

			ExitListenerThread( );
			m_ListenerThread = new Thread( ListenerThread );
			m_ListenerThread.Start( );
		}


		#region	Private stuff

		private string						m_ConnectionString	= "localhost";
		private int							m_Port				= 11000;
		private IConnections				m_Connections;
		private TcpListener					m_Listener;
		private readonly ManualResetEvent	m_ExitEvent = new ManualResetEvent( false );
		private Thread						m_ListenerThread;

		private void ExitListenerThread( )
		{
			if ( m_ListenerThread != null )
			{
				m_ExitEvent.Set( );
				m_ListenerThread.Join( );
				m_ExitEvent.Reset( );
			}
		}

		private void ListenerThread( )
		{
			//	A bit dubious... wait 10ms for the wait signal, so the thread isn't cycling like crazy
			while ( !m_ExitEvent.WaitOne( 10, false ) )	
			{
				if ( m_Listener.Pending( ) )
				{
					Socket socket = m_Listener.AcceptSocket( );
					AddConnection( socket );
				}
			}
		}

		/// <summary>
		/// Adds a client socket
		/// </summary>
		private void AddConnection( Socket clientSocket )
		{
			NetworkLog.Info( "Accepting connection \"{0}\"", clientSocket.RemoteEndPoint );
			m_Connections.Add( new SocketConnection( clientSocket, clientSocket.RemoteEndPoint.ToString( ) ) );
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Quits the listener thread
		/// </summary>
		public void Dispose( )
		{
			ExitListenerThread( );
		}

		#endregion
	}
}
