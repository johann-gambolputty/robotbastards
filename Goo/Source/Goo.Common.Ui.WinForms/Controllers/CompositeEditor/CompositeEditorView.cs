using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Goo.Common.Ui.Controllers.CompositeEditor;
using Rb.Core.Utils;

namespace Goo.Common.Ui.WinForms.Controllers.CompositeEditor
{
	public partial class CompositeEditorView : UserControl, ICompositeEditorView
	{
		public CompositeEditorView( )
		{
			InitializeComponent( );
		}

		#region ICompositeEditorView Members

		/// <summary>
		/// Event raised when the user wants to add a new item to the currently selected component
		/// </summary>
		public event ActionDelegates.Action<ComponentUiElement, ComponentTypeUiElement> AddToComponent;

		/// <summary>
		/// Event raised when the user wants to remove the currently selected component
		/// </summary>
		public event ActionDelegates.Action<ComponentUiElement> RemoveSelectedComponent;

		/// <summary>
		/// Event raised when the user selects a new component in the composite for editing
		/// </summary>
		public event ActionDelegates.Action<ComponentUiElement> SelectionChanged;

		/// <summary>
		/// Event raised when the user wants to edit an item
		/// </summary>
		public event ActionDelegates.Action<ComponentUiElement> EditComponent;

		/// <summary>
		/// Gets/sets the currently selected object. Must be a descendant of the root composite
		/// </summary>
		public ComponentUiElement SelectedObject
		{
			get
			{
				return compositeTreeView.SelectedNode == null ? null : ( ComponentUiElement )compositeTreeView.SelectedNode.Tag;
			}
			set
			{
			}
		}

		/// <summary>
		/// Adds a component to the view
		/// </summary>
		/// <param name="component">Component to add</param>
		public void AddComponent( ComponentUiElement component )
		{
			TreeNode node = CreateTreeNode( component );
			compositeTreeView.Nodes.Add( node );
			m_NodeMap.Add( component, node );
		}

		/// <summary>
		/// Adds a component to the view, adding it to the specified parent item
		/// </summary>
		/// <param name="parent">Parent item</param>
		/// <param name="component">Component item to add</param>
		public void AddComponent( ComponentUiElement parent, ComponentUiElement component )
		{
			TreeNode parentNode = m_NodeMap[ parent ];
			TreeNode node = CreateTreeNode( component );
			parentNode.Nodes.Add( node );
			m_NodeMap.Add( component, node );
		}

		/// <summary>
		/// Removes the specified component from the view (and all child elements)
		/// </summary>
		/// <param name="component">Component to remove</param>
		public void RemoveComponent( ComponentUiElement component )
		{
			TreeNode node = m_NodeMap[ component ];
			node.Remove( );
			m_NodeMap.Remove( component );
		}

		/// <summary>
		/// Clears all components
		/// </summary>
		public void Clear( )
		{
			m_NodeMap.Clear( );
			compositeTreeView.Nodes.Clear( );
		}

		/// <summary>
		/// Shows options for editing an element
		/// </summary>
		public void ShowEditOptions( ComponentUiElement component, ComponentTypeUiElement[] addTypes )
		{
			ContextMenuStrip strip = new ContextMenuStrip( );
			ToolStripMenuItem addSubMenu = ( ToolStripMenuItem )strip.Items.Add( "Add" );
			foreach ( ComponentTypeUiElement typeElement in addTypes )
			{
				ComponentTypeUiElement curTypeElement = typeElement;
				ToolStripItem addItem = addSubMenu.DropDownItems.Add( typeElement.Name, typeElement.Image );
				addItem.Click += delegate { AddToComponent( component, curTypeElement ); };
			}
			ToolStripItem removeItem = strip.Items.Add( "Remove" );
			removeItem.Click += delegate { RemoveSelectedComponent( component ); };
			strip.Show( this, m_LastClickLocation );
		}

		#endregion

		#region Private Members

		private Point m_LastClickLocation;
		private readonly Dictionary<ComponentUiElement, TreeNode> m_NodeMap = new Dictionary<ComponentUiElement, TreeNode>( );

		private static TreeNode CreateTreeNode( ComponentUiElement component )
		{
			TreeNode node = new TreeNode( component.Text );
			node.Tag = component;
			return node;
		}

		private void compositeTreeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			if ( SelectionChanged != null )
			{
				SelectionChanged( SelectedObject );
			}
		}

		private void compositeTreeView_MouseClick( object sender, MouseEventArgs e )
		{
			m_LastClickLocation = e.Location;
			if ( e.Button != MouseButtons.Right )
			{
				return;
			}
			TreeNode node = compositeTreeView.GetNodeAt( e.Location );
			if ( node == null )
			{
				return;
			}
			ComponentUiElement element = ( ComponentUiElement )node.Tag;
			
			if ( EditComponent != null )
			{
				EditComponent( element );
			}
		}

		#endregion

	}
}
