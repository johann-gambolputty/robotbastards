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
	public class TcpServerToClientConnection : ConnectionBase, IDisposable
	{
		/// <summary>
		/// Returns false (this is a server connection)
		/// </summary>
		public override bool	ConnectionToClient
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Creates this connection from a socket
		/// </summary>
		/// <param name="socket">Active socket, created by TcpClientConnectionRequestListener</param>
		public TcpServerToClientConnection( Scene.SceneDb scene, Socket socket )
		{
			//	Listen out for the scene dying - the connection will get closed at that point
			scene.Disposing += new RbEngine.Scene.SceneDb.DisposingDelegate( Dispose );

			//	Create a network stream for the socket, and objects to read and write to the stream
			m_Socket	= socket;
			m_Stream	= new NetworkStream( socket );
			m_Writer	= new BinaryWriter( m_Stream );
			m_Reader	= new BinaryReader( m_Stream );

  			//	Kick off a thread that sends pending messages to the client
			m_Thread = new Thread( new ThreadStart( RunConnection ) );
			m_Thread.Start( );
		}

		/// <summary>
		/// Kills the connection
		/// </summary>
		~TcpServerToClientConnection( )
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
		public override void	DeliverMessage( Components.Message msg )
		{
			lock ( m_PendingMessages )
			{
				m_PendingMessages.Add( msg );
			}
		}

		#endregion

		#region	Connection thread

		/// <summary>
		/// Connection thread method. Any pending messages queued up by DeliverMessage() get sent over the connection
		/// </summary>
		private void RunConnection( )
		{
			while ( true )
			{
				lock ( m_PendingMessages )
				{
					foreach ( Components.Message msg in m_PendingMessages )
					{
						msg.Write( m_Writer );
					}
					m_PendingMessages.Clear( );
				}

				//	TODO: Should block the thread until more messages are pending, or there's data available on the connection
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

		#region	Private stuff

		private Socket			m_Socket;
		private Thread			m_Thread;
		private NetworkStream	m_Stream;
		private BinaryReader	m_Reader;
		private BinaryWriter	m_Writer;
		private ArrayList		m_PendingMessages = new ArrayList( );

		#endregion
	}
}
