using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace RbEngine.Network
{
	/// <summary>
	/// Base class for server objects that communicate using sockets
	/// </summary>
	public abstract class SocketServer : ServerBase, IDisposable
	{
		/// <summary>
		/// Starts conversation
		/// </summary>
		public void					StartConversation( )
		{
			Socket			socket	= CreateSocket( );
			NetworkStream	stream	= new NetworkStream( socket );
			m_SocketReader			= new BinaryReader( stream );
			m_SocketWriter			= new BinaryWriter( stream );

			m_Thread = new Thread( new ThreadState( Converse ) );
			m_Thread.Start( );
		}

		/// <summary>
		/// Creates a socket
		/// </summary>
		protected abstract Socket	CreateSocket( );

		/// <summary>
		/// Talks and listens to clients
		/// </summary>
		private void				Converse( )
		{
			while ( true )
			{
				if ( socket.Available > 0 )
				{
					Components.Message msg = Components.Message.CreateFromStream( m_SocketReader );
					base.HandleMessage( msg );
				}
			}
		}

		/// <summary>
		/// Handles a message
		/// </summary>
		public override void HandleMessage( Components.Message msg )
		{
			msg.Write( m_SocketWriter );
		}


		#region IDisposable Members

		/// <summary>
		/// Cleans up the thread
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

		private BinaryReader	m_SocketReader;
		private BinaryWriter	m_SocketWriter;
		private Thread			m_Thread;

		#endregion
	}
}
