using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Network
{
	/// <summary>
	/// Provides a standard implementation of IConnection (and IMessageHandler)
	/// </summary>
	public abstract class ConnectionBase : IConnection
	{
		#region IConnection Members

		/// <summary>
		/// Gets the name of this connection
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
		/// Returns true if connects to a client
		/// </summary>
		public abstract bool ConnectionToClient
		{
			get;
		}

		/// <summary>
		/// Received message event
		/// </summary>
		public event ConnectionReceivedMessageDelegate	ReceivedMessage;

		/// <summary>
		/// Delivers a message over this connection
		/// </summary>
		/// <param name="msg">Message to deliver</param>
		public abstract void DeliverMessage( Components.Message msg );

		/// <summary>
		/// Invokes the ReceivedMessage event
		/// </summary>
		protected void OnReceivedMessage( Components.Message msg )
		{
			if ( ReceivedMessage != null )
			{
				ReceivedMessage( this, msg );
			}
		}

		#endregion

		#region	Private stuff

		private string m_Name = string.Empty;

		#endregion

	}
}
