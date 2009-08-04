
using Goo.Common.Ui.Controllers.CompositeEditor;

namespace Goo.Common.Ui.Controllers
{
	/// <summary>
	/// Event, raised when a component is selected by in a view like <see cref="ICompositeEditorView"/>
	/// </summary>
	public class ComponentSelectedEvent
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="component">Selected component</param>
		/// <param name="selectedComponents">Current selection set</param>
		public ComponentSelectedEvent( object component, object[] selectedComponents )
		{
			m_Component = component;
			m_SelectedComponents = selectedComponents;
		}

		/// <summary>
		/// Gets the component that was selected
		/// </summary>
		public object Component
		{
			get { return m_Component; }
		}

		/// <summary>
		/// Gets the current selection set
		/// </summary>
		public object[] SelectedComponents
		{
			get { return m_SelectedComponents; }
		}

		#region Private Members

		private readonly object m_Component;
		private readonly object[] m_SelectedComponents;

		#endregion

	}
}
