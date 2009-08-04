using Goo.Core.Mvc;
using Rb.Core.Utils;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	/// <summary>
	/// Composite editor view interface
	/// </summary>
	public interface ICompositeEditorView : IView
	{
		/// <summary>
		/// Event raised when the user wants to add a new item to the currently selected component
		/// </summary>
		event ActionDelegates.Action<ComponentUiElement, ComponentTypeUiElement> AddToComponent;

		/// <summary>
		/// Event raised when the user wants to remove the currently selected component
		/// </summary>
		event ActionDelegates.Action<ComponentUiElement> RemoveSelectedComponent;

		/// <summary>
		/// Event raised when the user selects a new component in the composite for editing
		/// </summary>
		event ActionDelegates.Action<ComponentUiElement> SelectionChanged;

		/// <summary>
		/// Event raised when the user wants to edit an item
		/// </summary>
		event ActionDelegates.Action<ComponentUiElement> EditComponent;

		/// <summary>
		/// Gets/sets the currently selected object. Must be a descendant of the root composite
		/// </summary>
		ComponentUiElement SelectedObject
		{
			get; set;
		}

		/// <summary>
		/// Adds a component to the view
		/// </summary>
		/// <param name="component">Component to add</param>
		void AddComponent( ComponentUiElement component );

		/// <summary>
		/// Adds a component to the view, adding it to the specified parent item
		/// </summary>
		/// <param name="parent">Parent item</param>
		/// <param name="component">Component item to add</param>
		void AddComponent( ComponentUiElement parent, ComponentUiElement component );

		/// <summary>
		/// Removes the specified component from the view (and all child elements)
		/// </summary>
		/// <param name="component">Component to remove</param>
		void RemoveComponent( ComponentUiElement component );

		/// <summary>
		/// Clears all components
		/// </summary>
		void Clear( );

		/// <summary>
		/// Shows options for editing an element
		/// </summary>
		void ShowEditOptions( ComponentUiElement component, ComponentTypeUiElement[] addTypes );
	}
}
