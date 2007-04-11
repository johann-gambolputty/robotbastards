using System;
using RbEngine.Components;

namespace RbEngine.Network.Runt
{
	/// <summary>
	/// Listens for messages of a given type being sent to the object's parent. Buffers them and sends them
	/// to an update target
	/// </summary>
	public class ChildUpdateProvider : Components.IChildObject, Components.IUnique, IUpdateProvider, Scene.ISceneObject
	{
		#region	Public properties

		/// <summary>
		/// Access to the message type used by this update provider
		/// </summary>
		public Type UpdateMessageType
		{
			get
			{
				return m_UpdateMessageType;
			}
			set
			{
				RemoveFromParentRecipientChain( );
				m_UpdateMessageType = value;
				AddToParentRecipientChain( );
			}
		}

		#endregion

		#region IChildObject Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public void AddedToParent( Object parentObject )
		{
			RemoveFromParentRecipientChain( );
			m_Parent = parentObject;
			AddToParentRecipientChain( );
		}

		#endregion

		#region	IUnique Members

		/// <summary>
		/// Gets the ID of this object (steals the parent object's ID)
		/// </summary>
		public ObjectId Id
		{
			get
			{
				return ( ( Components.IUnique )m_Parent ).Id;
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
		public void GetUpdateMessages( System.Collections.ArrayList messages, uint targetSequence )
		{
			m_Buffer.GetMessages( messages, targetSequence );
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			UpdateSource source = ( UpdateSource )db.GetSystem( typeof( UpdateSource ) );
			if ( source == null )
			{
				throw new ApplicationException( "ChildUpdateProvider requires that an UpdateSource be present in the scene systems" );
			}
			source.AddProvider( this );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		/// <param name="db"></param>
		public void RemovedFromScene(RbEngine.Scene.SceneDb db)
		{
			UpdateSource source = ( UpdateSource )db.GetSystem( typeof( UpdateSource ) );
			source.RemoveProvider( this );
		}

		#endregion

		#region	Private stuff

		private object					m_Parent;
		private Type					m_UpdateMessageType;
		private MessageBufferUpdater	m_Buffer = new MessageBufferUpdater( );
		private uint					m_Sequence;

		/// <summary>
		/// Removes this object from its parent's recipient chain
		/// </summary>
		private void RemoveFromParentRecipientChain( )
		{
			if ( ( m_Parent != null ) && ( m_UpdateMessageType != null ) )
			{
				( ( IMessageHub )m_Parent ).RemoveRecipient( m_UpdateMessageType, this );
			}
		}

		/// <summary>
		/// Adds this object to its parnet's recipient chain
		/// </summary>
		private void AddToParentRecipientChain( )
		{
			if ( ( m_Parent != null ) && ( m_UpdateMessageType != null ) )
			{
				( ( IMessageHub )m_Parent ).AddRecipient( m_UpdateMessageType, new MessageRecipientDelegate( ReceivedMessage ), ( int )MessageRecipientOrder.First );
			}
		}

		/// <summary>
		/// Called when this object receives a message of UpdateMessageType
		/// </summary>
		private MessageRecipientResult ReceivedMessage( Message msg )
		{
			//	If the message came from an update handler with the same ID, then just ignore the message. Why? Because
			//	that means that the handler received a message from the source, which is also the target for this update
			//	provider's messages, meaning the message will be endlessly circulating
			//	TODO: Maybe just make this a flag (IgnoreUpdateHandlerMessages?) rather than use the ID?
			if ( msg.Sender != null )
			{
				IUpdateHandler handler = ( IUpdateHandler )msg.Sender;
				if ( ( handler != null ) && ( handler.Id == Id ) )
				{
					return MessageRecipientResult.DeliverToNext;
				}
			}

			//	Add the message to the message buffer, and don't let the rest of the chain handle the message
			m_Buffer.AddMessage( new UpdateMessage( Id, m_Sequence, msg ), m_Sequence );

			return MessageRecipientResult.RemoveFromChain;
		}

		#endregion

	}
}
