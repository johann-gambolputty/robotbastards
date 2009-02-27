using System;
using System.Drawing;
using System.Windows.Forms;
using Rb.Common.Controls.Components;
using Rb.Core.Components;

namespace Rb.Common.Controls.Forms.Components
{
	public partial class CompositeEditorForm : Form, IComponentCompositionEditorView
	{
		public CompositeEditorForm( )
		{
			InitializeComponent( );
		}

		#region IComponentCompositionEditorView Members

		/// <summary>
		/// Event raised when the user wants to add a component of the specified type
		/// </summary>
		public event Action<ComponentType> AddComponentType;

		/// <summary>
		/// Event raised when the user wants to remove the specified component instance
		/// </summary>
		public event Action<object> RemoveComponent;

		/// <summary>
		/// Gets/sets the component type categories used to organize component types added to this view
		/// </summary>
		public ComponentTypeCategory[] Categories
		{
			get { return availableComponentTypesView.Categories; }
			set { availableComponentTypesView.Categories = value; }
		}

		/// <summary>
		/// Gets/sets the component types displayed by this view
		/// </summary>
		public ComponentType[] Types
		{
			get { return availableComponentTypesView.Types; }
			set { availableComponentTypesView.Types = value; }
		}

		/// <summary>
		/// Gets the view for composite objects attached to this editor
		/// </summary>
		public ICompositeViewControl CompositeView
		{
			get { return currentCompositeView; }
		}


		#endregion

		#region Private Members

		private bool m_MoveSplitter;
		private int m_LastMouseX;

		#region Event Handlers

		private void addButton_Click( object sender, EventArgs e )
		{
			if ( availableComponentTypesView.SelectedType == null )
			{
				return;
			}
			if ( AddComponentType != null )
			{
				AddComponentType( availableComponentTypesView.SelectedType );
			}
		}

		private void removeButton_Click( object sender, EventArgs e )
		{
			if ( currentCompositeView.SelectedComponent == null )
			{
				return;
			}
			if ( RemoveComponent != null )
			{
				RemoveComponent( currentCompositeView.SelectedComponent );
			}
		}

		private void splitterPanel_MouseEnter( object sender, EventArgs e )
		{
			Cursor = Cursors.VSplit;
		}

		private void splitterPanel_MouseLeave(object sender, EventArgs e)
		{
			Cursor = Cursors.Arrow;
			m_MoveSplitter = false;
		}

		private void splitterPanel_MouseDown(object sender, MouseEventArgs e)
		{
			m_MoveSplitter = true;
		}

		private void splitterPanel_MouseMove( object sender, MouseEventArgs e )
		{
			Point pt = PointToScreen( e.Location );
			if ( m_MoveSplitter )
			{
				float diffX = ( pt.X - m_LastMouseX ) * ( 100.0f / tableLayoutPanel1.Width );
				tableLayoutPanel1.ColumnStyles[ 0 ].Width += diffX;
				tableLayoutPanel1.ColumnStyles[ 2 ].Width -= diffX;
			}
			m_LastMouseX = pt.X;
		}

		private void splitterPanel_MouseUp(object sender, MouseEventArgs e)
		{
			m_MoveSplitter = false;
		}

		#endregion


		#endregion

	}
}