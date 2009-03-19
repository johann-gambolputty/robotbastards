using System;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Components;
using Rb.Common.Controls.Components;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Classes.Components
{
	/// <summary>
	/// Composite object view controller
	/// </summary>
	public class EditableCompositeViewController
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="viewFactory">View factory used by the controller to create dependent views</param>
		/// <param name="view">View to control</param>
		/// <param name="composite">Composite object to show</param>
		/// <exception cref="ArgumentNullException">Thrown if view or template is null</exception>
		public EditableCompositeViewController( IViewFactory viewFactory, IEditableCompositeView view, IComposite composite )
		{
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			Arguments.CheckNotNull( view, "view" );
			Arguments.CheckNotNull( composite, "composite" );
			view.EditComposition += OnEditComposition;
			view.Composite = composite;
			m_View = view;
			m_Composite = composite;
			m_ViewFactory = viewFactory;
		}

		#region Protected Members

		/// <summary>
		/// Returns an array of component type categories supported by this controller
		/// </summary>
		protected virtual ComponentTypeCategory[] ComponentTypeCategories
		{
			get
			{
				return new ComponentTypeCategory[] { new ComponentTypeCategory( typeof( object ), "All Components", "All available components" ) };
			}
		}

		/// <summary>
		/// Returns an array of component types supported by this controller
		/// </summary>
		protected virtual ComponentType[] ComponentTypes
		{
			get { return new ComponentType[ 0 ];}
		}

		/// <summary>
		/// Gets the composite controlled by this controller
		/// </summary>
		protected IComposite Composite
		{
			get { return m_Composite; }
		}

		#endregion

		#region Private Members

		private readonly IViewFactory m_ViewFactory;
		private readonly IComposite m_Composite;
		private readonly IEditableCompositeView m_View;

		/// <summary>
		/// Handles the EditComposition event on the view
		/// </summary>
		private void OnEditComposition( object sender, EventArgs e )
		{
			m_ViewFactory.ShowEditCompositeView( Composite, ComponentTypeCategories, ComponentTypes );
			m_View.RefreshView( );
		}

		#endregion
	}
}
