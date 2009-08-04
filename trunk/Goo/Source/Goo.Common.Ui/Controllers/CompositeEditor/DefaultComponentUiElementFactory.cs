
using System;
using Rb.Core.Utils;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	/// <summary>
	/// Simple implementation of <see cref="IComponentUiElementFactory"/>
	/// </summary>
	public class DefaultComponentUiElementFactory : IComponentUiElementFactory
	{
		#region IComponentUiElementFactory Members

		/// <summary>
		/// Gets a UI element representing a component type 
		/// </summary>
		public ComponentTypeUiElement GetTypeElement( Type componentType )
		{
			return new ComponentTypeUiElement( componentType, componentType.Name, componentType.Name );
		}

		/// <summary>
		/// Creates a component UI element from the specified component
		/// </summary>
		/// <param name="parentUiElement">Parent UI element (null if new element is a root element)</param>
		/// <param name="component">Component requiring UI element</param>
		/// <returns>Returns a new component UI element for the component</returns>
		public ComponentUiElement GetElement( ComponentUiElement parentUiElement, object component )
		{
			Arguments.CheckNotNull( component, "component" );
			return new ComponentUiElement( component, component.ToString( ), parentUiElement, GetTypeElement( component.GetType( ) ) );
		}

		#endregion
	}
}
