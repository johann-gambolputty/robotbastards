using System.Collections.Generic;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// Keeps one or more UpdateTarget objects synchronised by sending UpdateMessageBatch messages to them
	/// </summary>
	public class UpdateSource
	{
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
				m_Connections.ConnectionsUpdated += OnTick;
				m_Connections.ConnectionAdded += OnConnectionAdded;
				m_Connections.ConnectionRemoved += OnConnectionRemoved;
			}
		}

		#endregion

		#region	Providers

		/// <summary>
		/// Adds a provider
		/// </summary>
		public void AddProvider( IUpdateProvider provider )
		{
			m_Providers.Add( provider );
		}

		/// <summary>
		/// Removes a provider
		/// </summary>
		public void RemoveProvider( IUpdateProvider provider )
		{
			m_Providers.Remove( provider );
		}

		#endregion

		#region	TargetConnection Class

		/// <summary>
		/// A connection to a target that requires synchronisation from the stored update providers
		/// </summary>
		private class TargetConnection
		{
			/// <summary>
			/// Gets the connection to the client
			/// </summary>
			public IConnection	Connection
			{
				get { return m_Connection; }
			}

			/// <summary>
			/// Gets the current sequence value of the remote object
			/// </summary>
			public uint			Sequence
			{
				get { return m_Sequence; }
				set { m_Sequence = value; }
			}

			/// <summary>
			/// Sets up the client connection
			/// </summary>
			public TargetConnection( IConnection connection )
			{
				m_Connection	= connection;
				m_Sequence		= 0;
			}

			private IConnection	m_Connection;
			private uint		m_Sequence;
		}

		#endregion

		#region	Private stuff

		private uint						m_Sequence;
		private List< IUpdateProvider >		m_Providers = new List< IUpdateProvider >( );
		private List< TargetConnection >	m_Targets	= new List< TargetConnection >( );
		private IConnections				m_Connections;

		/// <summary>
		/// Adds a new connection
		/// </summary>
		private void OnConnectionAdded( IConnection connection )
		{
			//	Add information about the connection to the m_Targets list
			m_Targets.Add( new TargetConnection( connection ) );

			//	Listen out for new messages coming over the target connection
			connection.ReceivedMessage += new ConnectionReceivedMessageDelegate( OnReceivedMessage );
		}

		/// <summary>
		/// Removes an existing connection
		/// </summary>
		private void OnConnectionRemoved( IConnection connection )
		{
			for ( int targetIndex = 0; targetIndex < m_Targets.Count; ++targetIndex )
			{
				if ( m_Targets[ targetIndex ].Connection == connection )
				{
					connection.ReceivedMessage -= OnReceivedMessage;
					m_Targets.RemoveAt( targetIndex );
					return;
				}
			}
		}

		/// <summary>
		/// Called when the specified connection receives a message
		/// </summary>
		private void OnReceivedMessage( IConnection connection, Message msg )
		{
			if ( msg is TargetSequenceMessage )
			{
				foreach ( TargetConnection target in m_Targets )
				{
					if ( target.Connection == connection )
					{
						target.Sequence = ( ( TargetSequenceMessage )msg ).Sequence;
						return;
					}
				}
			}
		}

		/// <summary>
		/// Called when the connections object that this source is attached to updates
		/// </summary>
		private void OnTick( )
		{
			if ( m_Targets.Count == 0 )
			{
				//	TODO: There are no targets, so none of the updaters should be storing any state...
				return;
			}

			uint oldestSequence = uint.MaxValue;

			//	Determine the oldest target sequence
			foreach ( TargetConnection target in m_Targets )
			{
				if ( target.Sequence < oldestSequence )
				{
					oldestSequence = target.Sequence;
				}
			}

			//	Inform all the update providers what the oldest target sequence is
			foreach ( IUpdateProvider provider in m_Providers )
			{
				provider.SetOldestTargetSequence( oldestSequence );
				provider.SetLocalSequence( m_Sequence );
			}

			List< UpdateMessage > messages = new List< UpdateMessage >( );

			//	Run through all the target connections
			foreach ( TargetConnection target in m_Targets )
			{
				//	Collate update messages from all the update providers in the messages array
				foreach ( IUpdateProvider provider in m_Providers )
				{
					provider.GetUpdateMessages( messages, target.Sequence );
				}

				//	Always send an UpdateBatchMessage, even if the message array is empty - this will inform the target
				//	what sequence number we're at
				UpdateMessageBatch msg = new UpdateMessageBatch( m_Sequence, messages.ToArray( ) );

				//	Deliver the message to the client
				target.Connection.DeliverMessage( msg );

				messages.Clear( );
			}

			++m_Sequence;
		}

		#endregion

	}
}
