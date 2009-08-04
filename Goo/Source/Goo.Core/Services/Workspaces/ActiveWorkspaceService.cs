using Goo.Core.Services.Events;
using Goo.Core.Workspaces;
using log4net;
using Rb.Core.Utils;

namespace Goo.Core.Services.Workspaces
{
	/// <summary>
	/// Implements the simple <see cref="IActiveWorkspaceService"/>
	/// </summary>
	public class ActiveWorkspaceService : IActiveWorkspaceService
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="eventService">Events service</param>
		public ActiveWorkspaceService( IEventService eventService )
		{
			Arguments.CheckNotNull( eventService, "eventService" );
			m_Logger = LogManager.GetLogger( GetType( ) );
			m_EventService = eventService;
		}

		#region IActiveWorkspaceService Members

		/// <summary>
		/// Gets/sets the currently active environment
		/// </summary>
		/// <remarks>
		/// Setting the current environment to a new value will trigger an <see cref="ActiveWorkspaceChangedEvent"/>
		/// event in the event service.
		/// </remarks>
		public IWorkspace CurrentWorkspace
		{
			get { return m_CurrentWorkspace; }
			set
			{
				if ( m_CurrentWorkspace == value )
				{
					return;
				}
				Arguments.CheckNotNull( value, "value" );
				m_Logger.InfoFormat( "Changing environment from \"{0}\" to \"{1}\"", m_CurrentWorkspace, value );
				ActiveWorkspaceChangedEvent evt = new ActiveWorkspaceChangedEvent( m_CurrentWorkspace, value );
				m_CurrentWorkspace = value;
				m_EventService.Raise( this, evt );
			}
		}

		#endregion

		#region Private Members

		private readonly ILog m_Logger;
		private readonly IEventService m_EventService;
		private IWorkspace m_CurrentWorkspace;

		#endregion
	}
}
