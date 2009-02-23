using System;
using System.Collections.Generic;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// Listens for messages of a given type being sent to the object's parent (or another target). Buffers them and sends them
	/// to an update target
	/// </summary>
	public class ChildUpdateProvider : IComponent, IUnique, IUpdateProvider
	{
		#region Construction

		/// <summary>
		/// Default constructor
		/// </summary>
		public ChildUpdateProvider( )
		{
		}
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ChildUpdateProvider( IMessageHub source, UpdateSource target )
		{
			Source = source;
			Target = target;
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// If true, then this object ignores messages sent from an IUpdateHandler
		/// </summary>
		public bool IgnoreUpdateHandlerMessages
		{
			get { return m_IgnoreUpdateHandlerMessages; }
			set { m_IgnoreUpdateHandlerMessages = value; }
		}

		/// <summary>
		/// If true, then messages that are buffered by this object are removed from the recipient chain
		/// </summary>
		public bool RemoveBufferedMessages
		{
			get { return m_RemoveBufferedMessages; }
			set { m_RemoveBufferedMessages = value; }
		}

		/// <summary>
		/// Access to the message type used by this update provider
		/// </summary>
		public Type UpdateMessageType
		{
			get { return m_UpdateMessageType; }
			set
			{
				RemoveFromSourceRecipientChain( );
				m_UpdateMessageType = value;
				AddToSourceRecipientChain( );
			}
		}

		/// <summary>
		/// The source of update messages
		/// </summary>
		public IMessageHub Source
		{
			get { return m_Source; }
			set
			{
				RemoveFromSourceRecipientChain( );
				m_Source = value;
				AddToSourceRecipientChain( );
			}
		}
		
		/// <summary>
		/// Paradoxically, the UpdateSource (the object that sends out update messages) is the message target
		/// </summary>
		public UpdateSource Target
		{
			get { return m_Target; }
			set
			{
				if ( m_Target != value )
				{
					if ( m_Target != null )
					{
						m_Target.RemoveProvider( this );
					}
					m_Target = value;
					if ( m_Target != null )
					{
						m_Target.AddProvider( this );
					}
				}
			}
		}

		#endregion

		#region IChild Members

		/// <summary>
		/// Called when this object is added to a parent object. If the Target hasn't already been set, the parent becomes the target
		/// </summary>
		public void AddedToParent( object parentObject )
		{
			if ( ( m_Source == null ) && ( parentObject is IMessageHub ) )
			{
				Source = ( IMessageHub )parentObject;
			}
		}

		/// <summary>
		/// Called when this object is removed from a parent object
		/// </summary>
		/// <param name="parent">Parent object</param>
		public void RemovedFromParent( object parent )
		{
			if ( Source == parent )
			{
				Source = null;
			}
		}

		#endregion

		#region	IUnique Members

		/// <summary>
		/// Gets the ID of this object (steals the parent object's ID)
		/// </summary>
		public Guid Id
		{
			set
			{
				throw new ApplicationException( string.Format( "Can't set the GUID of a \"{0}\"", GetType( ).Name ) );
			}
			get
			{
				return ( ( IUnique )m_Source ).Id;
			}
		}

		#endregion

		#region IUpdateProvider Members

		/// <summary>
		/// Sets the current local sequence
		/// </summary>
		public void SetLocalSequence( uint sequence )
		{
			m_Sequence = sequence;
		}

		/// <summary>
		/// Sets the sequence value of the least up to date target
		/// </summary>
		public void SetOldestTargetSequence( uint sequence )
		{
			m_Buffer.SetOldestSequence( sequence );
		}

		/// <summary>
		/// Gets a set of update messages for a target at a given sequence point
		/// </summary>
		public void GetUpdateMessages( IList< UpdateMessage > messages, uint targetSequence )
		{
			m_Buffer.GetMessages( messages, targetSequence );
		}

		#endregion

		#region	Private stuff

		private UpdateSource			m_Target;
		private IMessageHub				m_Source;
		private Type					m_UpdateMessageType;
		private MessageBufferUpdater	m_Buffer = new MessageBufferUpdater( );
		private uint					m_Sequence;
		private bool					m_IgnoreUpdateHandlerMessages = true;
		private bool					m_RemoveBufferedMessages = true;

		/// <summary>
		/// Removes this object from its parent's recipient chain
		/// </summary>
		private void RemoveFromSourceRecipientChain( )
		{
			if ( ( m_Source != null ) && ( m_UpdateMessageType != null ) )
			{
				m_Source.RemoveRecipient( m_UpdateMessageType, this );
			}
		}

		/// <summary>
		/// Adds this object to its parnet's recipient chain
		/// </summary>
		private void AddToSourceRecipientChain( )
		{
			if ( ( m_Source != null ) && ( m_UpdateMessageType != null ) )
			{
				m_Source.AddRecipient( m_UpdateMessageType, new MessageRecipientDelegate( ReceivedMessage ), MessageRecipientOrder.First );
			}
		}

		/// <summary>
		/// Called when this object receives a message of UpdateMessageType
		/// </summary>
		private MessageRecipientResult ReceivedMessage( Message msg )
		{
			//	If the message came from an update handler (should be - with the same host ID), then just ignore the message. Why?
			//	Because that means that the handler received a message from the update source, which is also the target for this update
			//	provider's messages, meaning the message will be endlessly circulating if they aren't ignored here
			if ( ( m_IgnoreUpdateHandlerMessages ) && ( msg.Sender is IUpdateHandler ) )
			{
				return MessageRecipientResult.DeliverToNext;
			}

			//	Add the message to the message buffer, and don't let the rest of the chain handle the message
			m_Buffer.AddMessage( new UpdateMessage( Id, m_Sequence, msg ), m_Sequence );

			if ( m_RemoveBufferedMessages )
			{
				return MessageRecipientResult.RemoveFromChain;
			}

			return MessageRecipientResult.DeliverToNext;
		}

		#endregion

	}
}
