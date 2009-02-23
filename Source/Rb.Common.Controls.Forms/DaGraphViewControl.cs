using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SchemaTest
{
	public partial class DaGraphViewControl : UserControl, IDaGraphView
	{
		public DaGraphViewControl( )
		{
			m_SelectedNodes.ClearingItems += SelectedNodes_ClearingItems;
			m_SelectedNodes.RemovingItem += SelectedNodes_RemovingItem;
			m_SelectedNodes.ListChanged += SelectedNodes_ListChanged;
			InitializeComponent( );
			DoubleBuffered = true;
		
			m_NodePlacer.BindToControl( graphPanel );
		}

		/// <summary>
		/// Gets/sets the vertical ranks flag
		/// </summary>
		/// <remarks>
		/// If true, ranks are displayed in vertical stripes. If false, ranks are displayed
		/// in horizontal stripes
		/// </remarks>
		public bool VerticalRanks
		{
			get { return m_VerticalRanks; }
			set { m_VerticalRanks = value; }
		}

		#region IDagView Members

		/// <summary>
		/// Gets/sets the multi-select flag. A multi-select enabled view allows the 
		/// user to select more than one node
		/// </summary>
		public bool MultiSelect
		{
			get { return m_MultiSelect; }
			set { m_MultiSelect = value; }
		}

		/// <summary>
		/// Gets the currently selected node (first selected node if multi-select is enabled)
		/// </summary>
		public IDaGraphNode SelectedNode
		{
			get { return m_SelectedNodes.Count > 0 ? m_SelectedNodes[ 0 ] : null; }
			set
			{
				if ( ( value != null ) && ( value.Selected ) )
				{
					return;
				}
				SelectedNodes.Clear( );
				if ( value != null )
				{
					m_SelectedNodes.Add( value );
				}
			}
		}
		
		/// <summary>
		/// Gets the currently selected node list
		/// </summary>
		public IList<IDaGraphNode> SelectedNodes
		{
			get { return m_SelectedNodes; }
		}

		/// <summary>
		/// Gets a node at a given position
		/// </summary>
		/// <param name="pos">Position to check.</param>
		/// <returns>Returns the node whose area pos is inside. Returns null if no nodes are under pos</returns>
		public IDaGraphNode GetNodeAt( Point pos )
		{
			foreach ( IDaGraphNode node in m_Nodes )
			{
				if ( node.DisplayRectangle.Contains( pos ) )
				{
					return node;
				}
			}
			return null;
		}

		/// <summary>
		/// Adds a range of nodes to the view
		/// </summary>
		/// <param name="nodes">Nodes to add</param>
		public void AddRange( IEnumerable<IDaGraphNode> nodes )
		{
			Arguments.CheckNotNull( nodes, "nodes" );
			foreach ( IDaGraphNode node in nodes )
			{
				Add( node );
			}
		}

		/// <summary>
		/// Adds a node, and all its inputs and outputs, to the graph view
		/// </summary>
		/// <param name="node">Node to add</param>
		public void Add( IDaGraphNode node )
		{
			Arguments.CheckNotNull( node, "node" );
			if ( m_Nodes.Contains( node ) )
			{
				//	Node already displayed by control - no need to continue
				return;
			}
			//node.InputNodeAdded += OnInputNodeAdded;
			//node.InputNodeRemoved += OnInputNodeRemoved;
			node.SelectionChanged += OnNodeSelectionChanged;
			node.Moved += OnNodeMoved;

			m_Nodes.Add( node );
			node.DisplaySize = m_NodeRenderer.CalculateNodeDisplaySize( node, Font );
			m_NodePlacer.PlaceNode( node );

			Rectangle nodeRect = node.DisplayRectangle;
			graphPanel.Width = Math.Max( nodeRect.Right, graphPanel.Width );
			graphPanel.Height = Math.Max( nodeRect.Bottom, graphPanel.Height );

			Invalidate( );
		}

		/// <summary>
		/// Removes a node, and all its inputs and outputs, from the graph view
		/// </summary>
		/// <param name="node">Node to remove</param>
		public void Remove( IDaGraphNode node )
		{
			Arguments.CheckNotNull( node, "node" );
			node.SelectionChanged -= OnNodeSelectionChanged;
			throw new NotImplementedException( );
		}
		
		/// <summary>
		/// Clears all nodes from the view
		/// </summary>
		public void Clear( )
		{
			//throw new NotImplementedException( );
		}

		#endregion

		#region Private Members

		private bool m_VerticalRanks;
		private bool m_MultiSelect;

		//private IDaGraphViewNodePlacer m_NodePlacer = new DaGraphViewManualNodePlacer( new ForceBasedNodePlacer( ) );

		//private IDaGraphViewConnectionRenderer m_ConnectionRenderer = new DaGraphViewDefaultConnectionRenderer( );
		private IDaGraphViewConnectionRenderer m_ConnectionRenderer = new DaGraphViewConnectionPathRenderer( );
		private IDaGraphViewNodePlacer m_NodePlacer = new DaGraphViewManualNodePlacer(new DaGraphViewNodeRankPlacer(true));
		private IDaGraphViewNodeRenderer m_NodeRenderer = new DaGraphViewCurvyNodeRenderer( );

		private readonly List<IDaGraphNode> m_Nodes = new List<IDaGraphNode>( );
		private readonly BindingListEx<IDaGraphNode> m_SelectedNodes = new BindingListEx<IDaGraphNode>( );

		private DaGraphViewStyle m_ViewStyle = new DaGraphViewStyle( );

		private class BindingListEx<T> : BindingList<T>
		{
			public event EventHandler ClearingItems;
			public event EventHandler<ListChangedEventArgs> RemovingItem;

			protected override void ClearItems( )
			{
				if ( ClearingItems != null )
				{
					ClearingItems( this, EventArgs.Empty );
				}
				base.ClearItems( );
			}

			protected override void RemoveItem( int index )
			{
				if ( RemovingItem != null )
				{
					RemovingItem( this, new ListChangedEventArgs( ListChangedType.ItemDeleted, index ) );
				}
				base.RemoveItem( index );
			}
		}

		#region Event Handlers
		
		private void SelectedNodes_ClearingItems( object sender, EventArgs args )
		{
			//	This pre-clears the list (because node.Selected = false will remove the node from the list)
			foreach ( IDaGraphNode node in new List<IDaGraphNode>( m_SelectedNodes ) )
			{
				node.Selected = false;
			}
		}

		private void SelectedNodes_RemovingItem( object sender, ListChangedEventArgs args )
		{
			m_SelectedNodes[ args.NewIndex ].Selected = false;
		}

		private void SelectedNodes_ListChanged( object sender, ListChangedEventArgs args )
		{
			switch ( args.ListChangedType )
			{
				case ListChangedType.ItemAdded :
					{
						m_SelectedNodes[ args.NewIndex ].Selected = true;
						break;
					}
			}
		}

		/// <summary>
		/// Called when a node is moved
		/// </summary>
		private void OnNodeMoved( IDaGraphNode node )
		{
			Rectangle nodeRect = node.DisplayRectangle;
			graphPanel.Width = Math.Max( nodeRect.Right, graphPanel.Width );
			graphPanel.Height = Math.Max( nodeRect.Bottom, graphPanel.Height );
		}

		/// <summary>
		/// Called when selection value of a node changes
		/// </summary>
		private void OnNodeSelectionChanged( object sender, EventArgs args )
		{
			IDaGraphNode node = ( IDaGraphNode )sender;
			if ( !m_MultiSelect )
			{
				//	Deselect current node
				if ( ( node.Selected ) && ( node != SelectedNode ) && ( SelectedNode != null ) )
				{
					SelectedNode.Selected = false;
				}
			}
			if ( node.Selected )
			{
				if ( !m_SelectedNodes.Contains( node ) )
				{
					m_SelectedNodes.Add( node );
				}
			}
			else
			{
				m_SelectedNodes.Remove( node );
			}

			Invalidate( true );
		}
		
		private void graphPanel_MouseDown( object sender, MouseEventArgs e )
		{
			Point pt = e.Location;
			IDaGraphNode nodeUnderCursor = GetNodeAt( pt );
			if ( nodeUnderCursor != null )
			{
				nodeUnderCursor.Selected = true;
			}
			else if ( SelectedNode != null )
			{
				SelectedNode = null;
			}
		}

		private void graphPanel_Paint( object sender, PaintEventArgs e )
		{
			m_ViewStyle.BackgroundColour = BackColor;
			m_ConnectionRenderer.RenderConnections( e.Graphics, graphPanel.DisplayRectangle, m_ViewStyle, m_Nodes );
			m_NodeRenderer.Render( e.Graphics, Font, m_ViewStyle, m_Nodes );
		}

		#endregion

		#endregion

	}
}
