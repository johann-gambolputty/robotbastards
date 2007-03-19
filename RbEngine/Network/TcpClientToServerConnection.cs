using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// Implements ClientToServerConnection using TCP sockets
	/// </summary>
	public class TcpClientToServerConnection : ClientToServerConnection, IDisposable
	{
		/// <summary>
		/// Access to the connection string
		/// </summary>
		public string	ConnectionString
		{
			set
			{
				m_Connection = value;
			}
			get
			{
				return m_Connection;
			}
		}

		/// <summary>
		/// The port through which the connection is made
		/// </summary>
		public const int Port = 11000;


		/// <summary>
		/// Kills the connection
		/// </summary>
		~TcpClientToServerConnection( )
		{
			Dispose( );
		}

		/// <summary>
		/// Connects to the server
		/// </summary>
		public void		Run( )
		{
			Output.DebugAssert( m_Thread == null, Output.NetworkError, "Can only call Run() once" );
			m_Thread = new Thread( new ThreadStart( RunConnection ) );
			m_Thread.Start( );
		}

		/// <summary>
		/// Runs the connection
		/// </summary>
		private void	RunConnection( )
		{
			m_Socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

			IPEndPoint endPoint = null;
			try
			{
				//	Resolve connection string
				Output.WriteLineCall( Output.NetworkInfo, "Resolving connection string \"{0}\"", ConnectionString );
				IPAddress address = Dns.Resolve( ConnectionString ).AddressList[ 0 ];

				//	Create end point
				endPoint = new IPEndPoint( address, Port );

				//	Connect to end point
				Output.WriteLineCall( Output.NetworkInfo, "Attempting to connect to \"{0}\"", endPoint );
				m_Socket.Connect( endPoint );

				//	Create a network stream for the socket, and readers and writers for that stream
				m_Stream	= new NetworkStream( m_Socket );
				m_Writer	= new BinaryWriter( m_Stream );
				m_Reader	= new BinaryReader( m_Stream );
			}
			catch ( Exception exception )
			{
				//	Failed to connect
				throw new ApplicationException( string.Format( "Failed to connect client to \"{0}\"", endPoint ), exception );
			}

			Output.WriteLineCall( Output.NetworkInfo, "Connected to \"{0}\"", endPoint );
			while ( true )
			{
				lock ( m_PendingMessages )
				{
					foreach ( Components.Message msg in m_PendingMessages )
					{
						msg.Write( m_Writer );
					}
					m_PendingMessages.Clear( );
					m_Thread.Suspend( );
				}
			}
		}

		/// <summary>
		/// Writes the specified message to the server network stream
		/// </summary>
		public override void DeliverMessageToServer( Components.Message msg )
		{
			lock ( m_PendingMessages )
			{
				m_PendingMessages.Add( msg );
				if ( m_Thread != null )
				{
					m_Thread.Resume( );
				}
			}
		}

		#region IDisposable Members

		/// <summary>
		/// Kills the connection thread
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

		private string			m_Connection		= "localhost";
		private ArrayList		m_PendingMessages	= new ArrayList( );
		private Thread			m_Thread;
		private Socket			m_Socket;
		private NetworkStream	m_Stream;
		private BinaryReader	m_Reader;
		private BinaryWriter	m_Writer;

	}
}
