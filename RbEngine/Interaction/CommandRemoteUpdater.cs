using System;
using System.Collections;
using RbEngine.Components;
using RbEngine.Network.Runt;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A Runt IRemoteUpdater. Sends command messages passed from a remote object to all remote objects
	/// </summary>
	public class CommandRemoteUpdater : IChildObject, IRemoteUpdater, Scene.ISceneObject
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
			Network.Runt.RemoteUpdateManager updateManager = ( Network.Runt.RemoteUpdateManager )db.GetSystem( typeof( Network.Runt.RemoteUpdateManager ) );
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

		#region IRemoteUpdater Members

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

		/// <summary>
		/// Sets the current local sequence value
		/// </summary>
		public void SetLocalSequence( uint sequence )
		{
			m_LocalSequence = sequence;
		}

		/// <summary>
		/// Tells the updater what the sequence value of the most out-of-synch remote object is
		/// </summary>
		public void SetOldestRemoteSequence( uint sequence )
		{
			m_Buffer.SetOldestSequence( sequence );
		}

		/// <summary>
		/// Handles an update message sent from this updater's remote counterpart
		/// </summary>
		public void HandleUpdateMessage( UpdateMessage msg )
		{
			Message baseMsg = msg.Payload;
			( ( Components.IMessageHandler )m_Parent ).HandleMessage( baseMsg );

			m_Buffer.AddMessage( msg, m_LocalSequence );
		}

		/// <summary>
		/// Sets the oldest sequence of any client attached to the server
		/// </summary>
		public void SetOldestClientSequence( uint sequence )
		{
			m_Buffer.SetOldestSequence( sequence );
		}

		/// <summary>
		/// Gets update messages to send to this updater's remote counterpart, which has a sequence value of remoteSequence
		/// </summary>
		public void GetUpdateMessages( System.Collections.ArrayList messages, uint remoteSequence )
		{
			m_Buffer.GetMessages( messages, remoteSequence );
		}

		#endregion

		#region	Private stuff

		private uint					m_LocalSequence;
		private Object					m_Parent;
		private MessageBufferUpdater	m_Buffer = new Network.Runt.MessageBufferUpdater( );

		#endregion
	}
}
