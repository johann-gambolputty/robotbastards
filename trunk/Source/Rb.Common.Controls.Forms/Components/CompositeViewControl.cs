using System;
using System.Drawing;
using System.Windows.Forms;
using Rb.Common.Controls.Components;
using Rb.Core.Components;

namespace Rb.Common.Controls.Forms.Components
{
	public partial class CompositeViewControl : UserControl, ICompositeViewControl
	{
		public CompositeViewControl( )
		{
			InitializeComponent( );
			imageList.Images.Add( ComponentImageKey, SystemIcons.Information );
		}

		/// <summary>
		/// Gets the seleced component. Returns null if nothing is selected
		/// </summary>
		public object SelectedComponent
		{
			get
			{
				return compositeView.SelectedNode == null ? null : compositeView.SelectedNode.Tag;
			}
		}

		
		#region ICompositeViewControl Members

		/// <summary>
		/// User selected a component in the view
		/// </summary>
		public event Action<object> ComponentSelected;

		/// <summary>
		/// Event raised when a template is double-clicked
		/// </summary>
		public event Action<object> ComponentAction;
		
		/// <summary>
		/// Gets/sets the displayed component
		/// </summary>
		public IComposite Composite
		{
			get { return m_Composite; }
			set
			{
				if ( m_Composite != value )
				{
					UnlinkCurrentComposite( );
					m_Composite = value;
					LinkCurrentComposite( );
				}
			}
		}
		
		/// <summary>
		/// Refreshes the view
		/// </summary>
		public void RefreshView( )
		{
			UnlinkCurrentComposite( );
			LinkCurrentComposite( );
		}

		#endregion

		#region Private Members

		private const string ComponentImageKey = "ComponentImage";
		private IComposite m_Composite;

		/// <summary>
		/// Unlinks the current composite object from the control
		/// </summary>
		private void UnlinkCurrentComposite( )
		{
			if ( m_Composite == null )
			{
				return;
			}
			compositeView.Nodes.Clear( );
		}

		/// <summary>
		/// Links the current composite object to the control
		/// </summary>
		private void LinkCurrentComposite( )
		{
			if ( m_Composite == null )
			{
				return;
			}
			TreeNode rootNode = CreateNodeFromComponent( m_Composite );
			compositeView.Nodes.Add( rootNode );

			foreach ( object component in m_Composite.Components )
			{
				rootNode.Nodes.Add( CreateNodeFromComponent( component ) );
			}
			rootNode.ExpandAll( );
		}

		/// <summary>
		/// Creates a tree node for a model component
		/// </summary>
		private static TreeNode CreateNodeFromComponent( object component )
		{
			TreeNode node = new TreeNode( component.GetType( ).Name );
			node.Tag = component;
			node.ImageKey = node.SelectedImageKey = ComponentImageKey;
			return node;
		}

		#region Event Handlers

		private void compositeView_MouseDoubleClick( object sender, MouseEventArgs e )
		{
			TreeNode node = compositeView.GetNodeAt( e.Location );
			if ( ( node != null ) && ( ComponentAction != null ) )
			{
				ComponentAction( node.Tag );
			}
		}

		private void compositeView_AfterSelect( object sender, TreeViewEventArgs e )
		{
			if ( ( compositeView.SelectedNode != null ) && ( ComponentSelected != null ) )
			{
				ComponentSelected( compositeView.SelectedNode.Tag );
			}
		}

		#endregion

		#endregion

	}
}
