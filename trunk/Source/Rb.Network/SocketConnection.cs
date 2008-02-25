using System;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Rb.Core.Components;

namespace Rb.Network
{
	/// <summary>
	/// Sockets implementation of IConnection
	/// </summary>
	public class SocketConnection : IDisposable, IConnection
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SocketConnection( )
		{	
		}

		/// <summary>
		/// Sets up the connection
		/// </summary>
		/// <param name="socket">Socket used by the connection</param>
		/// <param name="name">Connection name</param>
		public SocketConnection( Socket socket, string name )
		{
			Setup( socket, name );
		}

		/// <summary>
		/// Kills the connection
		/// </summary>
		~SocketConnection( )
		{
			Dispose( );
		}

		/// <summary>
		/// Sets up the connection
		/// </summary>
		/// <param name="socket">Socket</param>
		/// <param name="name">Connection name</param>
		public void Setup( Socket socket, string name )
		{
			m_Socket = socket;
			m_Name = name;
		}

		#region IDisposable Members

		/// <summary>
		/// Kills the socket
		/// </summary>
		public void Dispose()
		{
			Disconnect( );
		}

		#endregion

		#region IConnection Members

		/// <summary>
		/// The name of this connection
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Returns true if the connection is connected
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return ( m_Socket != null ) && ( m_Socket.Connected );
			}
		}

		/// <summary>
		/// Event, invoked when the connection receives a message
		/// </summary>
		public event ConnectionReceivedMessageDelegate ReceivedMessage;

		/// <summary>
		/// Connection disconnected event
		/// </summary>
		public event ConnectionDisconnected Disconnected;

		/// <summary>
		/// Adds a message to the outgoing message queue. On the next network tick, all queued messages will be sent
		/// </summary>
		public void DeliverMessage( Message msg )
		{
			if ( IsConnected )
			{
				//	TODO: AP: Use a network stream? Requires stream sockets
				//	TODO: AP: Cache messages in a memory stream?
				MemoryStream messageStore = new MemoryStream( );
				m_Formatter.Serialize( messageStore, msg );
				messageStore.Close( );

				byte[] messageBytes = messageStore.ToArray( );
				m_Socket.Send( messageBytes );
			}
		}

		/// <summary>
		/// Updates the connection, checking for any received messages
		/// </summary>
		public void ReceiveMessages( )
		{
			if ( ( !IsConnected ) || ( m_Socket.Available == 0 ) )
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
			MemoryStream messageStore = new MemoryStream( messageMem );
			while ( messageStore.Position < messageStore.Length )
			{
				Message msg = ( Message )m_Formatter.Deserialize( messageStore );
				ReceivedMessage( this, msg );
			}
		}

		/// <summary>
		/// Closes the connection
		/// </summary>
		public void Disconnect( )
		{
			if ( m_Socket != null )
			{
				NetworkLog.Info( "Closing socket connection \"{0}\"", m_Name );
				m_Socket.Close( );
				m_Socket = null;

				if ( Disconnected != null )
				{
					Disconnected( this );
				}
			}
		}

		private readonly IFormatter m_Formatter = new BinaryFormatter( );

		#endregion

		#region	Private stuff

		private Socket m_Socket;
		private string m_Name;

		#endregion

	}
}
