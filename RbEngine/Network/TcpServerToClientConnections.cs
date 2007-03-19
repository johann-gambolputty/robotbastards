using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// Implements ServerToClientConnections using TCP sockets
	/// </summary>
	public class TcpServerToClientConnections : ServerToClientConnections, IDisposable
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
		~TcpServerToClientConnections( )
		{
			Dispose( );
		}

		#region	Client message delivery

		/// <summary>
		/// Delivers a specified message to the client over the connection
		/// </summary>
		/// <remarks>
		/// HandleMessage() can also be called to achieve this - HandleMessage() will also send the message to any recipients added by the AddRecipient() method
		/// </remarks>
		public override void	DeliverMessageToClient( Components.Message msg )
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

		#endregion

		#region	Connection thread

		public void Run( )
		{
			Output.DebugAssert( m_Thread == null, Output.NetworkError, "Can only call Run() once" );
			m_Thread = new Thread( new ThreadStart( RunConnections ) );
			m_Thread.Start( );
		}

		public void RunConnections( )
		{
			IPAddress	address		= Dns.Resolve( ConnectionString ).AddressList[ 0 ];
			TcpListener	listener	= new TcpListener( address, Port );

			Output.WriteLineCall( Output.NetworkInfo, "Listening for clients at \"{0}\"", listener.LocalEndpoint );

			while ( true )
			{
				if ( listener.Pending( ) )
				{
					Socket clientSocket = listener.AcceptSocket( );
					Output.WriteLineCall( Output.NetworkInfo, "Accepted client \"{0}\"", clientSocket.RemoteEndPoint );
					m_Clients.Add( clientSocket );
				}

				foreach( ClientSocket client in m_Clients )
				{
					CheckSocket( client );
				}
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Kills the connection thread
		/// </summary>
		public void Dispose()
		{
			if ( m_Thread != null )
			{
				m_Thread.Abort( );
				m_Thread.Join( );
				m_Thread = null;
			}
		}

		#endregion

		/// <summary>
		/// Stores a socket associated with a client, and objects to read and writer to it
		/// </summary>
		private class ClientSocket
		{
			/// <summary>
			/// Gets the reader that reads from the socket
			/// </summary>
			public BinaryReader		Reader
			{
				get
				{
					return m_Reader;
				}
			}

			/// <summary>
			/// Gets the writer that writes to the socket
			/// </summary>
			public BinaryWriter		Writer
			{
				get
				{
					return m_Writer;
				}
			}

			/// <summary>
			/// Gets the socket
			/// </summary>
			public Socket			Socket
			{
				get
				{
					return m_Socket;
				}
			}

			/// <summary>
			/// Sets up the socket
			/// </summary>
			public ClientSocket( Socket socket )
			{
				m_Socket	= socket;
				m_Stream	= new NetworkStream( socket );
				m_Reader	= new BinaryReader( m_Stream );
				m_Writer	= new BinaryWriter( m_Stream );
			}

			private Socket			m_Socket;
			private NetworkStream	m_Stream;
			private BinaryReader	m_Reader;
			private BinaryWriter	m_Writer;
		}

		#region	Private stuff

		private string			m_Connection		= "localhost";
		private ArrayList		m_PendingMessages	= new ArrayList( );
		private ArrayList		m_Clients			= new ArrayList( );

		#endregion
	}
}
