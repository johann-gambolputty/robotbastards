using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
	/// <summary>
	/// Component is a handy implementation of a bunch of component interfaces
	/// </summary>
	class Component : Node, IUnique, IMessageHandler, IMessageHub
	{
		#region IUnique Members

		/// <summary>
		/// Unique component identifier
		/// </summary>
		public Guid Id
		{
			get { return m_Id;  }
			set { m_Id = value;  }
		}

		#endregion


		#region IMessageHandler Members

		/// <summary>
		/// Handles a message
		/// </summary>
		/// <param name="msg">Message to handle</param>
		public void HandleMessage( Message msg )
		{
			if ( m_MessageMap == null )
			{
				m_MessageMap = DispatchMap.SafeGet( GetType( ) );
			}
			m_MessageMap.Dispatch( this, msg );
		}

		#endregion

		#region IMessageHub Members

		/// <summary>
		/// Adds a recipient for messages of, or derived from, a given type
		/// </summary>
		/// <param name="messageType">Message type</param>
		/// <param name="recipient">Delegate to call when a message of the designated type arrives</param>
		/// <param name="order">Recipient order <see cref="MessageRecipientOrder"/></param>
		public void AddRecipient( Type messageType, MessageRecipientDelegate recipient, int order )
		{
			if ( m_MessageHub == null )
			{
				m_MessageHub = new MessageHub( );
			}
			m_MessageHub.AddRecipient( messageType, recipient, order );
		}

		/// <summary>
		///	Removes a recipient from the message hub
		/// </summary>
		/// <param name="messageType">Type of message</param>
		/// <param name="obj">Object to remove</param>
		public void RemoveRecipient( Type messageType, object obj )
		{
			if ( m_MessageHub == null )
			{
				m_MessageHub = new MessageHub( );
			}
			m_MessageHub.RemoveRecipient( messageType, obj );
		}

		#endregion

		#region Private stuff

		private Guid m_Id = Guid.Empty;
		private Utils.DispatchMap m_MessageMap;
		private MessageHub m_MessageHub;

		#endregion
	}
}
