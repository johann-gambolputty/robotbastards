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
		/// Creates this connection from a socket
		/// </summary>
		/// <param name="socket">Active socket, created by TcpClientConnectionRequestListener</param>
		public TcpServerToClientConnection( Socket socket )
		{
			m_Socket	= socket;
			m_Stream	= new NetworkStream( socket );
			m_Writer	= new BinaryWriter( m_Stream );
			m_Reader	= new BinaryReader( m_Stream );

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

				//	TODO: Should block the thread until more messages are pending
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
