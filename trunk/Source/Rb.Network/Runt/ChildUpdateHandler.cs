using System;
using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// A RUNT update handler. Passes any received messages to the object's parent
	/// </summary>
	/// <remarks>
	/// The parent object that this handler is added to must support the Components.IUnique 
	/// and Components.IMessageHandler interfaces
	/// </remarks>
	public class ChildUpdateHandler : IChild, IUpdateHandler, Scene.ISceneObject
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

		#region IUpdateHandler Members

		/// <summary>
		/// Handles an update message. Passes the message payload up to the parent object
		/// </summary>
		public void Handle( UpdateMessage msg )
		{
			Components.Message payload = msg.Payload;

			//	Mark the payload message with this object as the source, so it can be ignored
			//	when it comes back along the message recipient chain
			payload.Sender = this;

			( ( Components.IMessageHandler )m_Parent ).HandleMessage( payload );
		}

		#endregion

		#region IUnique Members

		/// <summary>
		/// Gets the ID of this updater (returns the ID of the parent object
		/// </summary>
		public Components.ObjectId Id
		{
			get
			{
				return ( ( Components.IUnique )m_Parent ).Id;
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			UpdateTarget target = ( UpdateTarget )db.GetSystem( typeof( UpdateTarget ) );
			if ( target == null )
			{
				throw new ApplicationException( "ChildUpdateHandler requires that an UpdateTarget be present in the scene systems" );
			}
			target.AddHandler( this );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
			UpdateTarget target = ( UpdateTarget )db.GetSystem( typeof( UpdateTarget ) );
			target.RemoveHandler( this );
		}

		#endregion

		#region	Private stuff

		private object	m_Parent;

		#endregion
	}
}
