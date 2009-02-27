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
	public class ChildUpdateHandler : Component, IUpdateHandler
	{
		#region Public properties

		/// <summary>
		/// Paradoxically, the UpdateTarget (the object that receives update messages and dispatches them to update handlers like this one)
		/// is the message source
		/// </summary>
		public UpdateTarget Source
		{
			get { return m_Source;  }
			set
			{
				if ( m_Source != value )
				{
					//	Remove this handler from the existing source
					if ( m_Source != null )
					{
						m_Source.RemoveHandler( this );
					}
					m_Source = value;
					//	Add this handler to the new source
					if ( m_Source != null )
					{
						m_Source.AddHandler( this );
					}
				}
			}
		}

		/// <summary>
		/// Sets the target for update messages sent to this object
		/// </summary>
		public IMessageHandler Target
		{
			get { return m_Target; }
			set
			{
				m_Target = value;
				NetworkLog.RuntLog.Assert( m_Target is IUnique, "Targets for \"{0}\" must implement the IUnique interface", GetType( ).Name );
			}
		}

		#endregion

		/// <summary>
		/// Gets/sets the owner of this component
		/// </summary>
		public override IComposite Owner
		{
			get { return base.Owner; }
			set
			{
				base.Owner = value;
				Target = ( value is IMessageHandler ) ? ( IMessageHandler )value : null;
			}
		}

		#region IUpdateHandler Members

		/// <summary>
		/// Handles an update message. Passes the message payload up to the parent object
		/// </summary>
		public void Handle( UpdateMessage msg )
		{
			if ( m_Target == null )
			{
				if ( m_NotifyNullTarget )
				{
					NetworkLog.RuntLog.Warning( "{0} somehow received an UpdateMessage without a valid" );
					m_NotifyNullTarget = false;
				}
				
			}
			else
			{
				Message payload = msg.Payload;

				//	Mark the payload message with this object as the source, so it can be ignored
				//	when it comes back along the message recipient chain
				payload.Sender = this;

				m_Target.HandleMessage( payload );
			}
		}

		#endregion

		#region IUnique Members

		/// <summary>
		/// Gets the ID of this updater (returns the ID of the parent object)
		/// </summary>
		public Guid Id
		{
			set
			{
				throw new ApplicationException( string.Format( "Can't set the GUID of a \"{0}\"", GetType( ).Name ) );
			}
			get
			{
				return ( m_Target == null ) ? Guid.Empty : ( ( IUnique )m_Target ).Id;
			}
		}

		#endregion

		#region	Private stuff

		private IMessageHandler	m_Target;
		private bool			m_NotifyNullTarget = true;
		private UpdateTarget	m_Source;

		#endregion
	}
}
