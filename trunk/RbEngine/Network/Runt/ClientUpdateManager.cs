using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Network.Runt
{
	/*
	 * TcpClientConnectionListener listens out for client connections. If it finds one, it creates
	 * a connection, and a ClientUpdateManager based on that connection. The manager updates on
	 * a fixed time-step. Entity client update components subscribe to the manager, which requests
	 * update messages, which it collates into a single large packet that is sent over the connection
	 */

	/// <summary>
	/// Sends clients update messages using the Q3 unreliable network model
	/// </summary>
	/// <remarks>
	/// <note>
	/// A great description of the Q3 network model can be found on <see cref="http://bookofhook.com"/>.
	/// </note>
	/// The ClientUpdateManager keeps track of a number of IClientUpdater objects. These track the state of objects on a server, and 
	/// send updates to clients, through the ClientUpdateManager, on state changes. Given a reliable connection, this would be all that
	/// was required, but the ClientUpdateManager and IClientUpdater are designed to work with unreliable protocols. (... TODO: ...)
	/// 
	/// <note>
	/// The ClientUpdateManager can work with any IConnection type - reliable or unreliable (although it's obviously
	/// designed for an unreliable context).
	/// </note>
	/// 
	/// <note>
	/// A Connections object must exist in the scene systems for ClientUpdateManager to function.
	/// </note>
	/// 
	/// </remarks>
	public class ClientUpdateManager : Scene.ISceneObject
	{
		#region	Updaters

		/// <summary>
		/// Adds a client updater to the manager
		/// </summary>
		public void AddUpdater( IClientUpdater updater )
		{
			m_Updaters.Add( updater );
		}

		/// <summary>
		/// Removes an updater from the manager
		/// </summary>
		public void RemoveUpdater( IClientUpdater updater )
		{
			m_Updaters.Remove( updater );
		}

		#endregion


		#region	ClientConnection

		/// <summary>
		/// A connection to a client
		/// </summary>
		private class ClientConnection
		{
			/// <summary>
			/// Gets the connection to the client
			/// </summary>
			public IConnection Connection
			{
				get
				{
					return m_Connection;
				}
			}

			/// <summary>
			/// Gets the index of the client
			/// </summary>
			public int			ClientIndex
			{
				get
				{
					return m_ClientIndex;
				}
			}

			/// <summary>
			/// Gets the current sequence value of the client
			/// </summary>
			public int			CurrentSequence
			{
				get
				{
					return m_CurrentSequence;
				}
				set
				{
					m_CurrentSequence = value;
				}
			}

			/// <summary>
			/// Sets up the client connection
			/// </summary>
			public ClientConnection( IConnection connection, int clientIndex )
			{
				m_Connection		= connection;
				m_ClientIndex		= clientIndex;
				m_CurrentSequence	= -1;
			}

			private IConnection	m_Connection;
			private int			m_ClientIndex;
			private int			m_CurrentSequence;
		}

		#endregion

		#region	Private stuff

		private ArrayList	m_Clients	= new ArrayList( );
		private ArrayList	m_Updaters	= new ArrayList( );
		private int			m_Sequence	= 0;

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnNewClientConnection( IConnection connection )
		{
			//	Add information about the connection to the m_Clients list
			int clientIndex = m_Clients.Count;
			m_Clients.Add( new ClientConnection( connection, clientIndex ) );

			//	Listen for new messages from the client
			connection.AddRecipient( typeof( ServerMessage ), new MessageRecipientDelegate( OnReceivedServerMessage ), ( int )MessageRecipientOrder.First );

			//	Tell all updaters about the new client
			foreach ( IClientUpdater updater in m_Updaters )
			{
				updater.AddNewClient( clientIndex );
			}
		}

		/// <summary>
		/// Called when a new ServerMessage is received
		/// </summary>
		/// <param name="serverMsg">ServerMessage object</param>
		private MessageRecipientResult OnReceivedServerMessage( Message msg )
		{
			//	Find the client
			ServerMessage serverMsg = ( ServerMessage )msg;
		//	m_Clients[ serverMsg.ClientIndex ];

			return MessageRecipientResult.DeliverToNext;
		}

		/// <summary>
		/// Network update tick
		/// </summary>
		private void OnTick( Scene.Clock clock )
		{
			//	Run through all the client connections
			for ( int clientIndex = 0; clientIndex < m_Clients.Count; ++clientIndex )
			{
				//	Skip past removed connections
				if ( m_Clients[ clientIndex ] == null )
				{
					continue;
				}

				//	Get the current connection
				ClientConnection curConnection = ( ClientConnection )m_Clients[ clientIndex ];

				//	Run through all the updaters
				foreach ( IClientUpdater curUpdater in m_Updaters )
				{
					//	Get the current updater to generate update messages for the current client
					Message[] updateMessages = curUpdater.CreateUpdateMessages( curConnection.ClientIndex, curConnection.CurrentSequence, m_Sequence );
					if ( updateMessages != null )
					{
						//	Run through all the messages generated by the updater
						for ( int messageIndex = 0; messageIndex < updateMessages.Length; ++messageIndex )
						{
							//	Deliver the message to the client
							curConnection.Connection.DeliverMessage( updateMessages[ messageIndex ] );
						}
					}
				}
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		///	Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			//	Get the Connections object from the scene systems
			Connections connections = ( Connections )db.GetSystem( typeof( Connections ) );
			if ( connections == null )
			{
				throw new ApplicationException( "ClientUpdateManager requires that a Connections object be present in the scene systems" );
			}

			//	Subscribe to the new client connection event
            connections.NewClientConnection += new NewClientConnectionDelegate( OnNewClientConnection );
		}

		/// <summary>
		///	Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion
	}
}
