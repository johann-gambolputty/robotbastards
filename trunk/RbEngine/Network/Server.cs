using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;

namespace RbEngine.Network
{
	/// <summary>
	/// Full-on server
	/// </summary>
	public class Server : ServerBase, IDisposable
	{
		/// <summary>
		/// Gets rid of the server connection thread
		/// </summary>
		~Server( )
		{
			Dispose( );
		}

		/// <summary>
		/// Listens for connection requests
		/// </summary>
		public void SetupConnection( string connectionString )
		{
			m_ConnectionString = connectionString;
			Thread thread = new Thread( new ThreadStart( RunServerComms ) );
			thread.Start( );
		}

		public const int Port = 11000;

		/// <summary>
		/// Runs server communications
		/// </summary>
		private void RunServerComms( )
		{
			IPAddress	address		= Dns.Resolve( m_ConnectionString ).AddressList[ 0 ];
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

		/// <summary>
		/// Checks a socket
		/// </summary>
		private void CheckSocket( ClientSocket client )
		{
			if ( client.Socket.Available == 0 )
			{
				return;
			}

			Components.Message msg = Components.Message.CreateFromStream( client.Reader );
			base.HandleMessage( msg );
		}

		/// <summary>
		/// Handles a message.
		/// </summary>
		public override void HandleMessage( Components.Message msg )
		{
			base.HandleMessage( msg );
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

		#region IDisposable Members

		/// <summary>
		/// Kills the thread
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

		private Thread		m_Thread;
		private string		m_ConnectionString;
		private ArrayList	m_Clients = new ArrayList( );
	}
}
