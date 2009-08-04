
using Goo.Core.Environment;
using Goo.Core.Events;
using Goo.Core.Mvc;
using Goo.Core.Services.Events;

namespace Goo.Common.Ui.Controllers.PropertyEditor
{
	/// <summary>
	/// Property editor controller
	/// </summary>
	public class PropertyEditorController : ControllerBase, IEventSubscriber<ComponentSelectedEvent>
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="env">Environment</param>
		/// <param name="view">Property editor view</param>
		public PropertyEditorController( IEnvironment env, IPropertyEditorView view ) :
			base( view )
		{
			m_View = view;
			IEventService events = env.EnsureGetService<IEventService>( );
			events.Subscribe( this );
		}

		#region IEventSubscriber<ComponentSelectedEvent> Members

		/// <summary>
		/// Handles a raised event
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="args">Event object</param>
		public void OnEvent( object sender, ComponentSelectedEvent args )
		{
			m_View.SelectedObjects = args.SelectedComponents;
		}

		#endregion

		#region Private Members

		private readonly IPropertyEditorView m_View;

		#endregion
	}
}
