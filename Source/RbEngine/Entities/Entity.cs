using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// Entity base class
	/// </summary>
	/// <remarks>
	/// Entities are components that can be moved and rotated, so they get explicit recipient chains for movement (MovementRequests) and rotation (RotationRequests) messages.
	/// </remarks>
	public class Entity : Components.Component
	{

		#region	Public properties

		/// <summary>
		/// Gets the movement request chain associated with this entity
		/// </summary>
		public Components.MessageRecipientChain	MovementRequest
		{
			get { return m_MovementRequests; }
		}

		/// <summary>
		/// Gets the rotation request chain associated with this entity
		/// </summary>
		public Components.MessageRecipientChain	RotationRequests
		{
			get { return m_RotationRequests; }
		}

		#endregion

		#region	Entity message handling

		/// <summary>
		/// Adds a recipient for messages of a given type
		/// </summary>
		/// <param name="messageType">Base class of messages that the recipient is interested in</param>
		/// <param name="recipient">Recipient call</param>
		/// <param name="order">Recipient order value</param>
		public override void AddRecipient( Type messageType, Components.MessageRecipientDelegate recipient, int order )
		{
			if ( ( messageType == typeof( MovementRequest ) ) || ( messageType.IsSubclassOf( typeof( MovementRequest ) ) ) )
			{
				m_MovementRequests.AddRecipient( recipient, order );
			}
			else if ( ( messageType == typeof( RotationRequest ) ) || ( messageType.IsSubclassOf( typeof( RotationRequest ) ) ) )
			{
				m_RotationRequests.AddRecipient( recipient, order );
			}
			else
			{
				base.AddRecipient( messageType, recipient, order );
			}
		}

		/// <summary>
		/// Handles a movement request message
		/// </summary>
		/// <param name="movement">Movement request message</param>
		[ Components.MessageHandler( ) ]
		public virtual void				HandleMovement( MovementRequest movement )
		{
			m_MovementRequests.Deliver( movement );
		}

		/// <summary>
		/// Handles a rotation request message
		/// </summary>
		/// <param name="rotation">Rotation request message</param>
		[ Components.MessageHandler( ) ]
		public virtual void				HandleRotation( RotationRequest rotation )
		{
			m_RotationRequests.Deliver( rotation );
		}

		/// <summary>
		/// Handles movement and rotation messages
		/// </summary>
		/// <param name="msg">Message to handle</param>
		public override void			HandleMessage( Components.Message msg )
		{
			if ( msg is MovementRequest )
			{
				HandleMovement( ( MovementRequest )msg );
			}
			else if ( msg is RotationRequest )
			{
				HandleRotation( ( RotationRequest )msg );
			}
			else
			{
				//	Default recipient chain handling
				base.HandleMessage( msg );
			}
		}

		#endregion

		private Components.MessageRecipientChain	m_MovementRequests	= new Components.MessageRecipientChain( );
		private Components.MessageRecipientChain	m_RotationRequests	= new Components.MessageRecipientChain( );
	}
}
