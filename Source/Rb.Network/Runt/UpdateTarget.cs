using System;
using System.Collections.Generic;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// Target for update messages sent by an UpdateSource
	/// </summary>
	public class UpdateTarget
	{
		#region Construction

		/// <summary>
		/// Default constructor. Connections must be set before this UpdateSource will function
		/// </summary>
		public UpdateTarget( )
		{
		}
		
		/// <summary>
		/// Connections setup constructor
		/// </summary>
		public UpdateTarget( IConnections connections )
		{
			Connections = connections;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The connections set that this update source is assigned to
		/// </summary>
		public IConnections Connections
		{
			get { return m_Connections; }
			set
			{
				NetworkLog.RuntLog.Assert( m_Connections == null, "I'm just lazy and not handling re-assignment of Connections. Sorry" );
				m_Connections = value;
				foreach ( IConnection connection in m_Connections )
				{
					OnConnectionAdded( connection );
				}
				m_Connections.ConnectionAdded += OnConnectionAdded;
				m_Connections.ConnectionRemoved += OnConnectionRemoved;
			}
		}

		#endregion

		#region	Handlers

		/// <summary>
		/// Adds an update handler
		/// </summary>
		public void AddHandler( IUpdateHandler handler )
		{
			m_Handlers.Add( handler );
			m_HandlerMap[ handler.Id ] = handler;
		}

		/// <summary>
		/// Removes an update handler
		/// </summary>
		public void RemoveHandler( IUpdateHandler handler )
		{
			m_Handlers.Remove( handler );
			m_HandlerMap.Remove( handler.Id );
		}

		#endregion

		#region	Private stuff

		private IConnections						m_Connections;
		private List< IUpdateHandler >				m_Handlers		= new List< IUpdateHandler >( );
		private Dictionary< Guid, IUpdateHandler >	m_HandlerMap	= new Dictionary< Guid, IUpdateHandler >( );
		private uint								m_Sequence;

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnConnectionAdded( IConnection connection )
		{
			connection.ReceivedMessage += OnReceivedMessage;
		}
		
		/// <summary>
		/// Removes a new connection
		/// </summary>
		private void OnConnectionRemoved( IConnection connection )
		{
			connection.ReceivedMessage -= OnReceivedMessage;
		}

		/// <summary>
		/// Handles messages sent over a specified connection
		/// </summary>
		private void OnReceivedMessage( IConnection connection, Message msg )
		{
			if ( !( msg is UpdateMessageBatch ) )
			{
				//	Not interested
				return;
			}

			UpdateMessageBatch batchMsg = ( UpdateMessageBatch )msg;
			if ( batchMsg.Sequence < m_Sequence )	//	TODO: Should be <=. Just ordering issues between update message creation and sequence increment
			{
				return;
			}

			//	Message away
			if ( batchMsg.Messages != null )
			{
				foreach ( UpdateMessage updateMsg in batchMsg.Messages )
				{
					if ( updateMsg.Sequence >= m_Sequence )	//	TODO: Should be >. Just ordering issues between update message creation and sequence increment
					{
						IUpdateHandler handler = m_HandlerMap[ updateMsg.TargetId ];
						handler.Handle( updateMsg );
					}
				}
			}
			m_Sequence = batchMsg.Sequence;

			//	Let's let the source know that we got an update! yay!
			//	TODO: Should this be sent at every frame?
			//	TODO: If there's an update source, then the information about the target sequence can be piggy-backed in an UpdateMessageBatch
			connection.DeliverMessage( new TargetSequenceMessage( m_Sequence ) );
		}

		#endregion
	}
}
