
using System;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	/// <summary>
	/// Creates UI items for components
	/// </summary>
	public interface IComponentUiElementFactory
	{
		/// <summary>
		/// Gets a UI element representing a component type 
		/// </summary>
		ComponentTypeUiElement GetTypeElement( Type componentType );

		/// <summary>
		/// Creates a component UI element from the specified component
		/// </summary>
		/// <param name="parentUiElement">Parent UI element (null if new element is a root element)</param>
		/// <param name="component">Component requiring UI element</param>
		/// <returns>Returns a new component UI element for the component</returns>
		ComponentUiElement GetElement( ComponentUiElement parentUiElement, object component );
	}
}
