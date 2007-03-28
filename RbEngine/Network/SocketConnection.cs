using System;
using System.Net.Sockets;
using System.Collections;
using System.IO;

namespace RbEngine.Network
{
	/// <summary>
	/// Summary description for SocketConnection.
	/// </summary>
	public class SocketConnection : IDisposable, IConnection
	{
		/// <summary>
		/// Creates this connection
		/// </summary>
		public SocketConnection( Scene.SceneDb scene, Socket socket, bool connectionToClient )
		{
			m_Socket = socket;
			m_Client = connectionToClient;

			//	
			Scene.Clock networkClock = scene.GetNamedClock( "NetworkClock" );
			if ( networkClock == null )
			{
				throw new ApplicationException( "TcpServerToClientConnection requires a clock named \"NetworkClock\" in the scene" );
			}

			//	TODO: Dispense with thread - only use network clock tick?
			networkClock.Subscribe( new Scene.Clock.TickDelegate( OnNetworkTick ) );

			//	Listen out for the scene dying - the connection will get closed at that point
			scene.Disposing += new RbEngine.Scene.SceneDb.DisposingDelegate( Dispose );
		}

		/// <summary>
		/// Kills the connection
		/// </summary>
		~SocketConnection( )
		{
			Dispose( );
		}

		/// <summary>
		/// Closes this connection
		/// </summary>
		public void Close( )
		{
			if ( m_Socket != null )
			{
				m_Socket.Close( );
				m_Socket = null;
			}
		}

		/// <summary>
		/// Called when the network clock ticks
		/// </summary>
		public void OnNetworkTick( Scene.Clock networkClock )
		{
			ReceiveIncomingMessages( );
			DeliverOutgoingMessages( );
		}

		/// <summary>
		/// Reads any messages from the socket
		/// </summary>
		private void ReceiveIncomingMessages( )
		{
			if ( ( m_Socket == null ) || ( m_Socket.Available == 0 ) )
			{
				return;
			}

			//	Receive bytes from the socket
			byte[] messageMem = new byte[ m_Socket.Available ];
			m_Socket.Receive( messageMem );

			//	If there's nothing listening out for new messages, then just discard the data
			if ( ReceivedMessage == null )
			{
				return;
			}

			//	Setup a stream and a binary stream reader to pick out the messages from the binary data
			MemoryStream messageStore		= new MemoryStream( messageMem );
			BinaryReader messageStoreReader	= new BinaryReader( messageStore );
			while ( messageStore.Position < messageStore.Length )
			{
				Components.Message msg = Components.Message.ReadMessage( messageStoreReader );
				ReceivedMessage( this, msg );
			}
		}

		/// <summary>
		/// Writes any outgoing messages to the socket
		/// </summary>
		private void DeliverOutgoingMessages( )
		{
			//	Early out if there's no socket, or no outgoing messages
			if ( ( m_Socket == null ) || ( m_OutgoingMessages.Count == 0 ) )
			{
				return;
			}

			MemoryStream messageStore		= new MemoryStream( );
			BinaryWriter messageStoreWriter = new BinaryWriter( messageStore );

			foreach ( Components.Message msg in m_OutgoingMessages )
			{
				msg.Write( messageStoreWriter );
			}

			m_OutgoingMessages.Clear( );

			m_Socket.Send( messageStore.ToArray( ) );
		}

		#region IDisposable Members

		/// <summary>
		/// Kills the socket
		/// </summary>
		public void Dispose()
		{
			Close( );
		}

		#endregion

		#region	Private stuff

		private Socket		m_Socket;
		private string		m_Name;
		private ArrayList	m_OutgoingMessages = new ArrayList( );
		private bool		m_Client;

		#endregion

		#region IConnection Members

		/// <summary>
		/// The name of this connection
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		/// <summary>
		/// Returns true if this is a connection to a client
		/// </summary>
		public bool ConnectionToClient
		{
			get
			{
				return m_Client;
			}
		}

		/// <summary>
		/// Event, invoked when the connection receives a message
		/// </summary>
		public event RbEngine.Network.ConnectionReceivedMessageDelegate ReceivedMessage;

		/// <summary>
		/// Adds a message to the outgoing message queue. On the next network tick, all queued messages will be sent
		/// </summary>
		public void DeliverMessage(  Components.Message msg )
		{
			m_OutgoingMessages.Add( msg );
		}

		#endregion
	}
}
