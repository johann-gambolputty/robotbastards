using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace RbEngine.Network
{
	/// <summary>
	/// Implements IConnection by decorating a SocketConnection, created from a TCP socket and various setup bits and pieces
	/// </summary>
	public class TcpClientToServerConnection : IDisposable, IConnection, Scene.ISceneObject
	{
		/// <summary>
		/// Access to the connection string
		/// </summary>
		public string	ConnectionString
		{
			set
			{
				m_ConnectionString = value;
			}
			get
			{
				return m_ConnectionString;
			}
		}

		/// <summary>
		/// The port through which the connection is made
		/// </summary>
		public int	Port
		{
			set
			{
				m_Port = value;
			}
			get
			{
				return m_Port;
			}
		}

		/// <summary>
		/// Kills the connection
		/// </summary>
		~TcpClientToServerConnection( )
		{
			Dispose( );
		}

		/// <summary>
		/// Runs the connection
		/// </summary>
		private void	RunConnection( Scene.SceneDb scene )
		{
			Socket socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

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
				socket.Connect( endPoint );

				m_BaseConnection = new SocketConnection( scene, socket, false );
				m_BaseConnection.ReceivedMessage += new ConnectionReceivedMessageDelegate( BaseConnectionReceivedMessage );
			}
			catch ( Exception exception )
			{
				//	Failed to connect
				throw new ApplicationException( string.Format( "Failed to connect client to \"{0}\"", endPoint ), exception );
			}

			Output.WriteLineCall( Output.NetworkInfo, "Connected to \"{0}\"", endPoint );
		}

		/// <summary>
		/// Called when the base connection object receives a message
		/// </summary>
		private void BaseConnectionReceivedMessage( IConnection connection, Components.Message msg )
		{
			if ( ReceivedMessage != null )
			{
				ReceivedMessage( this, msg );
			}
		}

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			RunConnection( db );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes the base connection
		/// </summary>
		public void Dispose( )
		{
			if ( m_BaseConnection is IDisposable )
			{
				( ( IDisposable )m_BaseConnection ).Dispose( );
			}
		}

		#endregion

		#region	Private stuff

		private string		m_ConnectionString	= "localhost";
		private int			m_Port				= 11000;
		private IConnection	m_BaseConnection;
		private string		m_Name;

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
		/// This is a connection to a server - returns false
		/// </summary>
		public bool ConnectionToClient
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Invoked when this connection receives a message
		/// </summary>
		public event RbEngine.Network.ConnectionReceivedMessageDelegate ReceivedMessage;

		/// <summary>
		/// Delivers a message over this connection
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		public void DeliverMessage( Components.Message msg )
		{
			if ( m_BaseConnection != null )
			{
				m_BaseConnection.DeliverMessage( msg );
			}
		}

		#endregion
	}
}
