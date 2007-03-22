using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace RbEngine.Network
{
	/// <summary>
	/// Listens out for client connection requests, creating new TcpServerToClientConnection objects for each one
	/// </summary>
	/// <remarks>
	/// Must be added as a child to a Connections object.
	/// Only starts its listening thread when it is attached to a scene
	/// </remarks>
	public class TcpClientConnectionListener : IDisposable, Scene.ISceneObject
	{

		/// <summary>
		/// Kills the listener thread
		/// </summary>
		~TcpClientConnectionListener( )
		{
			Dispose( );
		}

		/// <summary>
		/// The address to listen at for connection requests
		/// </summary>
		public string	ConnectionString
		{
			get
			{
				return m_ConnectionString;
			}
			set
			{
				m_ConnectionString = value;
			}
		}

		/// <summary>
		/// The port to listen at for connection requests
		/// </summary>
		public int		Port
		{
			get
			{
				return m_Port;
			}
			set
			{
				m_Port = value;
			}
		}

		/// <summary>
		/// Listens out for connection requests from clients. If a connection is successfully made, a TcpServerToClientConnection is added to the parent object
		/// </summary>
		private void Listen( )
		{
			IPAddress address = null;

			//	Try to resolve the connection string
			try
			{
				Output.WriteLineCall( Output.NetworkInfo, "Attemping to resolve listener address \"{0}\"", ConnectionString );
				address = Dns.Resolve( ConnectionString ).AddressList[ 0 ];
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to resolve connection string \"{0}\"", ConnectionString ), exception );
			}


			//	Create a TCP listener at the address
			TcpListener	listener = null;
			try
			{
				Output.WriteLineCall( Output.NetworkInfo, "Attempting to listen for clients at \"{0}:{1}\"", address, Port );
				listener = new TcpListener( address, Port );
				listener.Start( );
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( string.Format( "Failed to create TCP listener at \"{0}\"", listener.LocalEndpoint ), exception );
			}

			//	Loop forever, listening out for new clients
			while ( true )
			{
				if ( listener.Pending( ) )
				{
					AddClient( listener.AcceptSocket( ) );
				}

				//	Sleep for a bit (listening for connections doesn't have to be a full-time thing)
				Thread.Sleep( 10 );
			}
		}


		#region IDisposable Members

		/// <summary>
		/// Kills the listener thread
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

		#region	Private stuff

		private Thread			m_Thread;
		private string			m_ConnectionString	= "localhost";
		private int				m_Port				= 11000;
		private Scene.SceneDb	m_Scene;


		/// <summary>
		/// Adds a client socket
		/// </summary>
		private void	AddClient( Socket clientSocket )
		{
			Output.WriteLineCall( Output.NetworkInfo, "Accepting client \"{0}\"", clientSocket.RemoteEndPoint );

			Connections connections = ( Connections )m_Scene.GetSystem( typeof( Connections ) );
			if ( connections == null )
			{
				throw new ApplicationException( "Cannot create client connection without a Connections object in the scene systems" );
			}

			connections.AddChild( new TcpServerToClientConnection( m_Scene, clientSocket ) );
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Adds this object to the scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			m_Scene = db;
			db.Disposing += new Scene.SceneDb.DisposingDelegate( Dispose );
			
			m_Thread = new Thread( new ThreadStart( Listen ) );
			m_Thread.Name = "Client Connection Listener";
			m_Thread.Start( );
		}

		/// <summary>
		/// Removes this object from the scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
			m_Scene = null;
		}

		#endregion
	}
}
