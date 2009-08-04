using System;
using Goo.Core.Environment;
using Goo.Core.Mvc;
using Goo.Core.Services.Events;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	/// <summary>
	/// Composite controller
	/// </summary>
	public class CompositeEditorController : ControllerBase
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public CompositeEditorController
			(
				IEnvironment env,
				ICompositeEditorView view,
				object composite,
				IComponentUiElementFactory uiFactory,
				IComponentDependencies dependencies,
				IComponentAggregator aggregator
			) :
			base( view )
		{
			Arguments.CheckNotNull( env, "env" );
			Arguments.CheckNotNull( uiFactory, "uiFactory" );
			Arguments.CheckNotNull( dependencies, "dependencies" );
			Arguments.CheckNotNull( aggregator, "aggregator" );

			m_View = view;
			m_Environment = env;
			m_UiFactory = uiFactory;
			m_Dependencies = dependencies;
			m_Aggregator = aggregator;

			m_View.SelectionChanged += OnSelectionChanged;
			m_View.EditComponent += OnEditComponent;
			m_View.RemoveSelectedComponent += OnRemoveComponent;
			m_View.AddToComponent += OnAddComponent;

			ResetView( composite );
		}

		#region Private Members

		private readonly ICompositeEditorView m_View;
		private readonly IEnvironment m_Environment;
		private readonly IComponentUiElementFactory m_UiFactory;
		private readonly IComponentDependencies m_Dependencies;
		private readonly IComponentAggregator m_Aggregator;

		/// <summary>
		/// Raises a <see cref="ComponentSelectedEvent"/> in the envionment events service
		/// </summary>
		private void OnSelectionChanged( ComponentUiElement element )
		{
			IEventService events = m_Environment.EnsureGetService<IEventService>( );
			events.Raise( this, new ComponentSelectedEvent( element.Component, new object[] { element.Component } ) );
		}

		/// <summary>
		/// Clears the view
		/// </summary>
		private void ResetView( object composite )
		{
			m_View.Clear( );
			if ( composite == null )
			{
				return;
			}
			ComponentUiElement uiElement = m_UiFactory.GetElement( null, composite );
			m_View.AddComponent( uiElement );
			AddComponents( uiElement );
		}

		/// <summary>
		/// Adds all child components of the parent composite in a UI element, to the view
		/// </summary>
		private void AddComponents( ComponentUiElement parentElement )
		{
			foreach ( object component in m_Aggregator.GetChildComponents( parentElement.Component ) )
			{
				ComponentUiElement uiElement = m_UiFactory.GetElement( parentElement, component );
				m_View.AddComponent( parentElement, uiElement );
				AddComponents( uiElement );
			}
		}

		private void OnAddComponent( ComponentUiElement element, ComponentTypeUiElement addTypeElement )
		{
			//	TODO: AP: Switch to factory pattern
			object component = Activator.CreateInstance( addTypeElement.ComponentType );
			m_Aggregator.Add( element.Component, component );
			m_View.AddComponent( element, m_UiFactory.GetElement( element, component ) );
		}

		private void OnRemoveComponent( ComponentUiElement element )
		{
			m_Aggregator.Remove( element.ParentUiElement.Component, element.Component );
			m_View.RemoveComponent( element );
		}

		private void OnEditComponent( ComponentUiElement element )
		{
			Type[] allowedComponentTypes = m_Dependencies.GetAllowedComponentTypes( element.Component.GetType( ) );
			ComponentTypeUiElement[] allowedComponentTypeUiElements = new ComponentTypeUiElement[ allowedComponentTypes.Length ];
			for ( int i = 0; i < allowedComponentTypes.Length; ++i )
			{
				allowedComponentTypeUiElements[ i ] = m_UiFactory.GetTypeElement( allowedComponentTypes[ i ] );
			}
			m_View.ShowEditOptions( element, allowedComponentTypeUiElements );
		}

		#endregion
	}
}
