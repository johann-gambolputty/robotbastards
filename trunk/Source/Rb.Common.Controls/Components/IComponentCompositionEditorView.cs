using System;
using Rb.Core.Components;

namespace Rb.Common.Controls.Components
{
	/// <summary>
	/// Composite composition editor interface
	/// </summary>
	public interface IComponentCompositionEditorView
	{
		/// <summary>
		/// Event raised when the user wants to add a component of the specified type
		/// </summary>
		event Action<ComponentType> AddComponentType;

		/// <summary>
		/// Event raised when the user wants to remove the specified component instance
		/// </summary>
		event Action<object> RemoveComponent;

		/// <summary>
		/// Gets/sets the component type categories used to organize component types added to this view
		/// </summary>
		ComponentTypeCategory[] Categories
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the component types displayed by this view
		/// </summary>
		ComponentType[] Types
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the view for composite objects attached to this editor
		/// </summary>
		ICompositeViewControl CompositeView
		{
			get;
		}

	}
}
