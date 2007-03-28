using System;
using System.Collections;
using RbEngine.Components;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A Runt IClientUpdater. Sends command messages passed from a client to all other clients
	/// </summary>
	public class CommandClientUpdater : Components.IChildObject, Network.Runt.IClientUpdater, Scene.ISceneObject
	{
		#region IChildObject Members

		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		public void AddedToParent( Object parentObject )
		{
			m_Parent = parentObject;
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			//	Add this updater to the server updater
			Network.Runt.ClientUpdateManager updateManager = ( Network.Runt.ClientUpdateManager )db.GetSystem( typeof( Network.Runt.ClientUpdateManager ) );
			if ( updateManager == null )
			{
				throw new ApplicationException( "CommandClientUpdater requires a ClientUpdateManager to present in the scene systems" );
			}

			updateManager.AddUpdater( this );
		}

		/// <summary>
		/// Called when this object is removed from the scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
		}

		#endregion

		#region IClientUpdater Members

		/// <summary>
		/// Gets the ID of this updater
		/// </summary>
		public Components.ObjectId Id
		{
			get
			{
				return ( ( Components.IUnique )m_Parent ).Id;
			}
		}

		private ArrayList m_Messages = new ArrayList( );

		/// <summary>
		/// Handles an update message sent from a client
		/// </summary>
		public void HandleClientUpdate( Network.Runt.UpdateMessage msg )
		{
			Message baseMsg = msg.Payload;
			( ( Components.IMessageHandler )m_Parent ).HandleMessage( baseMsg );

			/*
			//	NOTE: This crushes messages in order from oldest to youngest if the number of stored messages reached
			//	a maximum (because some lousy client hasn't been keeping up-to-date)
			m_UpdateMessages[ m_CurUpdateMessageIndex ] = msg;

			m_CurUpdateMessageIndex = ( m_CurUpdateMessageIndex + 1 ) % MaxUpdateMessages;
			m_NumUpdateMessages		= Maths.Utils.Min( m_NumUpdateMessages + 1, MaxUpdateMessages );	
			*/
			m_Messages.Add( msg );
		}

		/// <summary>
		/// Sets the oldest sequence of any client attached to the server
		/// </summary>
		public void SetOldestClientSequence( uint sequence )
		{
			/*
			if ( m_NumUpdateMessages == 0 )
			{
				return;
			}

			//	Find the first message that 
			int messageIndex	= m_FirstUpdateMessageIndex;
			int messageCount	= 0;
			for ( ; messageCount < m_NumUpdateMessages; ++messageCount )
			{
				if ( m_UpdateMessageSequences[ messageIndex ] >= sequence )
				{
					break;
				}

				m_UpdateMessages[ messageIndex ]			= null;
				m_UpdateMessageSequences[ messageIndex ]	= 0;

				messageIndex = ( messageIndex + 1 ) % MaxUpdateMessages;
			}

			m_FirstUpdateMessageIndex = messageIndex;
			m_NumUpdateMessages -= messageCount;
			*/
		}

		/// <summary>
		/// Gets update messages from this updater
		/// </summary>
		public void GetUpdateMessages( ArrayList messages, uint clientSequence, uint serverSequence )
		{
			messages.AddRange( m_Messages );
			m_Messages.Clear( );
			/*
			if ( m_NumUpdateMessages == 0 )
			{
				return;
			}

			//	Determine the index of the first update message that the client should receive
			int messageCount = 0;
			int messageIndex = m_FirstUpdateMessageIndex;
			for ( ; messageCount < m_NumUpdateMessages; ++messageCount )
			{
				if ( m_UpdateMessageSequences[ messageIndex ] >= clientSequence )
				{
					//	Reached the first message that starts at or after the current client sequence
					break;
				}
				messageIndex = ( messageIndex + 1 ) % MaxUpdateMessages;
			}

			//	If there were no update messages generated since the client sequence, then exit
			int updateMessageCount = m_NumUpdateMessages - messageCount;
			if ( updateMessageCount == 0 )
			{
				return;
			}

			//	Create a message array and fill it with the remaining messages
			for ( messageCount = 0; messageCount < updateMessageCount; ++messageCount )
			{
				//	NOTE: Creating the UpdateMessage can be skipped because that's what's being stored in m_UpdateMessages anyway
				messages.Add( m_UpdateMessages[ messageIndex ] );
				messageIndex = ( messageIndex + 1 ) % MaxUpdateMessages;
			}
			*/
		}

		#endregion

		#region	Private stuff

		private const int	MaxUpdateMessages			= 128;

		private Message[]	m_UpdateMessages			= new Message[ MaxUpdateMessages ];
		private uint[]		m_UpdateMessageSequences	= new uint[ MaxUpdateMessages ];
		private int			m_FirstUpdateMessageIndex	= 0;
		private int			m_CurUpdateMessageIndex		= 0;
		private int			m_NumUpdateMessages			= 0;
		private Object		m_Parent;

		#endregion
	}
}
