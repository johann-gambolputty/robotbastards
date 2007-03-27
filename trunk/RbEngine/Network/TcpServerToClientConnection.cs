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
		/// Returns true (connects a server to a client)
		/// </summary>
		public override bool	ConnectionToClient
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Creates this connection from a socket
		/// </summary>
		/// <param name="socket">Active socket, created by TcpClientConnectionRequestListener</param>
		public TcpServerToClientConnection( Scene.SceneDb scene, Socket socket )
		{
			//	
			Scene.Clock networkClock = scene.GetNamedClock( "NetworkClock" );
			if ( networkClock == null )
			{
				throw new ApplicationException( "TcpServerToClientConnection requires a clock named \"NetworkClock\" in the scene" );
			}

			//	TODO: Dispense with thread - only use network clock tick?
			networkClock.Subscribe( new Scene.Clock.TickDelegate( OnTick ) );

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

				//	Read any messages that have appeared at the client socket, passing them on to any interested parties using
				//	the message recipient chain
				if ( m_Socket.Available > 0 )
				{
					lock ( m_ReceivedMessages )
					{
						while ( m_Socket.Available > 0 )
						{
							Components.Message newMessage = Components.Message.ReadMessage( m_Reader );
							if ( newMessage == null )
							{
								throw new ApplicationException( "Invalid message appeared at client socket" );
							}

							//	TODO: ...
							m_ReceivedMessages.Add( newMessage );
						}
					}
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

		#region	Private stuff

		private Socket			m_Socket;
		private Thread			m_Thread;
		private NetworkStream	m_Stream;
		private BinaryReader	m_Reader;
		private BinaryWriter	m_Writer;
		private ArrayList		m_PendingMessages	= new ArrayList( );
		private ArrayList		m_ReceivedMessages	= new ArrayList( );

		/// <summary>
		/// Network click tick
		/// </summary>
		private void			OnTick( Scene.Clock clock )
		{
			lock ( m_ReceivedMessages )
			{
				foreach ( Components.Message curMessage in m_ReceivedMessages )
				{
					OnReceivedMessage( curMessage );
				}
				m_ReceivedMessages.Clear( );
			}
		}

		#endregion
	}
}
