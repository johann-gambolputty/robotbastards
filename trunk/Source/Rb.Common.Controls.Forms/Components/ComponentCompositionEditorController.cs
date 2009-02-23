using System;
using System.Diagnostics;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Rb.Common.Controls.Forms.Components
{
	/// <summary>
	/// Controller class for the <see cref="IComponentCompositionEditorView"/> view
	/// </summary>
	public class ComponentCompositionEditorController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="view">View to control</param>
		/// <param name="componentTypeCategories">Categories that component types can fall into</param>
		/// <param name="types">Composite types available for use</param>
		/// <param name="composite">Current composite</param>
		/// <exception cref="ArgumentNullException">Thrown if any argument is null</exception>
		public ComponentCompositionEditorController( IComponentCompositionEditorView view, ComponentTypeCategory[] componentTypeCategories, ComponentType[] types, IComposite composite )
		{
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( types, "types" );
			Arguments.CheckNotNull( composite, "composite" );
			Arguments.CheckNotNull( componentTypeCategories, "componentTypeCategories" );

			view.Categories = componentTypeCategories;
			view.Types = types;
			view.CompositeView.Composite = composite;

			view.AddComponentType += OnAddComponentType;
			view.RemoveComponent += OnRemoveComponent;

			composite.ComponentAdded += OnCompositeChanged;
			composite.ComponentRemoved += OnCompositeChanged;

			m_Composite = composite;
			m_View = view;
			m_Types = types;
		}

		#region Private Members

		private readonly ComponentType[] m_Types;
		private readonly IComponentCompositionEditorView m_View;
		private readonly IComposite m_Composite;

		/// <summary>
		/// Finds a component type that matches a specified model
		/// </summary>
		private ComponentType FindComponentTypeForComponent( object component )
		{
			Debug.Assert( component != null );
			foreach ( ComponentType type in m_Types )
			{
				if ( type.Type == component.GetType( ) )
				{
					return type;
				}
			}
			return null;
		}

		/// <summary>
		/// Removes the specified type, and all its dependencies, from the current template
		/// </summary>
		private void RemoveComponentTypesFromTemplate( ComponentType type )
		{
			CompositeUtils.RemoveAllOfType( m_Composite, type.Type );
			foreach ( ComponentType dependent in type.Dependents )
			{
				RemoveComponentTypesFromTemplate( dependent );
			}
		}

		private void OnRemoveComponent( object component )
		{
			//	Find a component type that matches the component (we need this to determine what dependent components
			//	to remove too)
			ComponentType type = FindComponentTypeForComponent( component );
			Debug.Assert( type != null, "Did not expect model template to not match any type in the component type array" );

			RemoveComponentTypesFromTemplate( type );
		}

		private void OnAddComponentType( ComponentType type )
		{
			type.Create( m_Composite );
		}

		private void OnCompositeChanged( IComposite composite, object component )
		{
			m_View.CompositeView.RefreshView( );
		}

		#endregion
	}

}
