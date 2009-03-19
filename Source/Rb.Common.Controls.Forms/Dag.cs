using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace SchemaTest
{
	/// <summary>
	/// Node placer
	/// </summary>
	public interface IDaGraphViewNodePlacer
	{
		/// <summary>
		/// Binds the placer to a control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		void BindToControl( Control control );

		/// <summary>
		/// Unbinds the placer from a control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		void UnbindFromControl( Control control );

		/// <summary>
		/// Places a single node
		/// </summary>
		/// <param name="node">Node to place</param>
		void PlaceNode( IDaGraphNode node );

		/// <summary>
		/// Renders any placement information
		/// </summary>
		//void Render( Graphics graphics );
	}

	/// <summary>
	/// Wraps up an existing placer. Allows user to manually place nodes
	/// </summary>
	public class DaGraphViewManualNodePlacer : IDaGraphViewNodePlacer
	{
	    /// <summary>
	    /// Setup constructor
	    /// </summary>
	    /// <param name="inner">Inner placer</param>
	    public DaGraphViewManualNodePlacer( IDaGraphViewNodePlacer inner )
	    {
			Arguments.CheckNotNull( inner, "inner" );
	    	m_Inner = inner;
	    }

		#region IDaGraphViewNodePlacer Members

		public void BindToControl( Control control )
		{
			m_Inner.BindToControl( control );
			control.MouseLeave += OnMouseLeave;
			control.MouseMove += OnMouseMove;
			control.MouseDown += OnMouseDown;
			control.MouseUp += OnMouseUp;
			m_Control = control;
		}

		public void UnbindFromControl( Control control )
		{
			Debug.Assert( m_Control == control );
			m_Inner.UnbindFromControl( control );
			control.MouseLeave -= OnMouseLeave;
			control.MouseMove -= OnMouseMove;
			control.MouseDown -= OnMouseDown;
			control.MouseUp -= OnMouseUp;
		}

		public void PlaceNode( IDaGraphNode node )
		{
			m_Inner.PlaceNode( node );
			m_Nodes.Add( node );
		}

		#endregion

		#region Private Members

		private Control m_Control;
		private int m_MoveIndex = -1;
		private readonly List<IDaGraphNode> m_Nodes = new List<IDaGraphNode>( );
		private readonly IDaGraphViewNodePlacer m_Inner;
		private Point m_LastMousePos;

		/// <summary>
		/// Gets the index of a node that contains a specified point
		/// </summary>
		private int GetNodeAt( Point pt )
		{
			for ( int nodeIndex = 0; nodeIndex < m_Nodes.Count; ++nodeIndex )
			{
				if ( m_Nodes[ nodeIndex ].DisplayRectangle.Contains( pt ) )
				{
					return nodeIndex;
				}
			}
			return -1;
		}
		
		private void OnMouseMove( object sender, MouseEventArgs e )
		{
			if ( m_MoveIndex != -1 )
			{
				Point pt = m_Nodes[ m_MoveIndex ].DisplayPosition;
				pt.X += e.X - m_LastMousePos.X;
				pt.Y += e.Y - m_LastMousePos.Y;
				m_Nodes[ m_MoveIndex ].DisplayPosition = pt;
				m_Control.Invalidate( );
			}
			m_LastMousePos = e.Location;
		}
		
		private void OnMouseDown( object sender, MouseEventArgs e )
		{
			m_MoveIndex = GetNodeAt( e.Location );
		}

		private void OnMouseLeave( object sender, EventArgs e )
		{
			m_MoveIndex = -1;
		}

		private void OnMouseUp( object sender, MouseEventArgs e )
		{
			m_MoveIndex = -1;
		}

		#endregion
	}

	// http://www.cs.ubc.ca/local/reading/proceedings/spe91-95/spe/vol21/issue11/spe060tf.pdf
	public class ForceBasedNodePlacer : IDaGraphViewNodePlacer
	{
		public ForceBasedNodePlacer( )
		{
			m_SynchronizationContext = AsyncOperationManager.SynchronizationContext;
		}

		#region IDaGraphViewNodePlacer Members

		public void BindToControl( Control control )
		{
			m_Control = control;
			control.Resize += OnControlResize;
			BeginSimulation( );
		}

		public void UnbindFromControl( Control control )
		{
			Debug.Assert( control == m_Control );
			control.Resize -= OnControlResize;
			m_Control = null;
			EndSimulation( );
		}

		public void PlaceNode( IDaGraphNode node )
		{
			int rank = DaGraphUtils.GetLongestPathLengthToRoot( node );
			int rankSize = m_Ranks.ContainsKey( rank ) ? m_Ranks[ rank ] : 0;

			float x = rank * 32;
			//float y = rankSize * 32 + ( rank % 2 == 0 ? 16 : 0 );
			float y;
			if ( rank == 0 )
			{
				y = rankSize * ( node.DisplaySize.Height + 4 );
			}
			else
			{
				y = ( float )( m_Random.NextDouble( ) * m_Control.Height );
			}
			ForceNode forceNode = new ForceNode( node, new PointF( x, y ), rank == 0 );
			m_Ranks[ rank ] = rankSize + 1;

			node.DisplayPosition = new Point( ( int )forceNode.Pos.X, ( int )forceNode.Pos.Y );

			lock ( m_Nodes )
			{
				m_Nodes.Add( forceNode );
				m_NodeMap.Add( node, forceNode );
			}

			m_RefreshEvent.Set( );
		}

		private readonly Random m_Random = new Random( );
		private Dictionary<int, int> m_Ranks = new Dictionary<int, int>( );

		#endregion

		#region Private Members

		#region ForceNode Class

		private class ForceNode
		{
			public ForceNode( IDaGraphNode node, PointF pos, bool pinned )
			{
				m_Node = node;
				m_Pos = pos;
				m_Pinned = pinned;
			}

			public PointF Pos
			{
				get { return m_Pos; }
				set { m_Pos = value; }
			}

			public PointF Vel
			{
				get { return m_Vel; }
				set { m_Vel = value; }
			}

			public IDaGraphNode Node
			{
				get { return m_Node; }
			}

			public bool Pinned
			{
				get { return m_Pinned; }
			}

			private readonly bool m_Pinned;
			private PointF m_Pos;
			private PointF m_Vel;
			private readonly IDaGraphNode m_Node;
		}

		#endregion

		private readonly Dictionary<IDaGraphNode, ForceNode> m_NodeMap = new Dictionary<IDaGraphNode, ForceNode>( );
		private readonly List<ForceNode> m_Nodes = new List<ForceNode>( );
		private Control m_Control;
		private AutoResetEvent m_RefreshEvent = new AutoResetEvent( true );
		private AutoResetEvent m_ExitEvent = new AutoResetEvent( false );
		private Thread m_SimThread;
		private readonly SynchronizationContext m_SynchronizationContext;

		private void BeginSimulation( )
		{
			EndSimulation( );

			m_SimThread = new Thread( RunSimulation );
			m_SimThread.Name = "DA graph node force simulation";
			m_SimThread.IsBackground = true;
			m_SimThread.Start( );
		}

		private void RunSimulation( )
		{
			while ( !m_ExitEvent.WaitOne( 1, false ) )
			{
				m_RefreshEvent.WaitOne( );

				float t = 1;
				lock ( m_Nodes )
				{
					foreach ( ForceNode node in m_Nodes )
					{
						node.Pos = new PointF( node.Node.DisplayPosition.X, node.Node.DisplayPosition.Y );
					}
				}
				for ( int iteration = 0; iteration < 100; ++iteration )
				{
					lock ( m_Nodes )
					{
						Update( t );
					}
					t *= 0.95f;
				//	m_SynchronizationContext.Post( new SendOrPostCallback( OnIterationComplete ), null );
				//	Thread.Sleep( 30 );
				}
				m_SynchronizationContext.Post( new SendOrPostCallback( OnIterationComplete ), null );
			}
		}
		
		private void EndSimulation( )
		{
			if ( m_SimThread == null )
			{
				return;
			}

			m_ExitEvent.Set( );
			m_SimThread.Join( );
			m_SimThread = null;
		}

		private void OnIterationComplete( object state )
		{
			lock ( m_Nodes )
			{
				foreach ( ForceNode node in m_Nodes )
				{
					node.Node.DisplayPosition = new Point( ( int )node.Pos.X, ( int )node.Pos.Y );
				}
			}
			m_Control.Invalidate( );
		}

		private void OnControlResize( object sender, EventArgs args )
		{
			m_RefreshEvent.Set( );
			m_Control.Invalidate( );
		}

		private static float Length( float x, float y )
		{
			return ( float )Math.Sqrt( x * x + y * y );
		}

		private void Update( float t )
		{
			if ( m_Nodes.Count == 0 )
			{
				return;
			}
			float kSqr = ( m_Control.Width * m_Control.Height ) / m_Nodes.Count;
			float k = ( float )Math.Sqrt( kSqr );

			//	Calculate repulsive forces
			for ( int nodeIndex = 0; nodeIndex < m_Nodes.Count; ++nodeIndex )
			{
				ForceNode node = m_Nodes[ nodeIndex ];
				if ( node.Pinned )
				{
					continue;
				}
				PointF vel = new PointF( );
				for ( int repIndex = 0; repIndex < m_Nodes.Count; ++repIndex )
				{
					if ( repIndex == nodeIndex )
					{
						continue;
					}
					ForceNode repNode = m_Nodes[ repIndex ];
					float diffX = node.Pos.X - repNode.Pos.X;
					float diffY = node.Pos.Y - repNode.Pos.Y;
					float radius = node.Node.DisplaySize.Width / 2;

					//	force = D/|D| * k^2/|D| == D.k^2/|D|^2
					float len = Length( diffX, diffY ) - ( radius * 2 );
					float mag = Math.Max( 0.001f, len );
					float force = kSqr / ( mag * mag );
					vel.X += diffX * force;
					vel.Y += diffY * force;
				}
				node.Vel = vel;
			}

			//	Calculate attractive forces
			for ( int nodeIndex = 0; nodeIndex < m_Nodes.Count; ++nodeIndex )
			{
				ForceNode node = m_Nodes[ nodeIndex ];
				if ( node.Pinned )
				{
					continue;
				}
				IDaGraphNode graphNode = node.Node;
				PointF vel = node.Vel;
				foreach ( IDaGraphNode inputNode in graphNode.Inputs )
				{
					ForceNode attNode;
					if ( !m_NodeMap.TryGetValue( inputNode, out attNode ) )
					{
						continue;
					}

					float diffX = node.Pos.X - attNode.Pos.X;
					float diffY = node.Pos.Y - attNode.Pos.Y;
					//float radius = node.Node.DisplaySize.Width;
					float len = Length( diffX, diffY );
					if ( len < 0.001f )
					{
						continue;
					}
					//	force = D / |D| * |D|^2 / k = D|D|/k
					//	( D/|  D |) * fa (| D |)
					float force = len / k;

					if ( !attNode.Pinned )
					{
						attNode.Vel = new PointF( attNode.Vel.X + diffX * force, attNode.Vel.Y + diffY * force );
					}
					vel.X -= diffX * force;
					vel.Y -= diffY * force;
				}
				node.Vel = vel;
			}

			foreach ( ForceNode node in m_Nodes )
			{
				float len = Length( node.Vel.X, node.Vel.Y );
				if ( len < 0.00001f )
				{
					continue;
				}
				float m = Math.Min( len, t ) / len;
				float x = Math.Max( 0, Math.Min( node.Pos.X + node.Vel.X * m, m_Control.Width - node.Node.DisplaySize.Width ) );
				float y = Math.Max( 0, Math.Min( node.Pos.Y + node.Vel.Y * m, m_Control.Height - node.Node.DisplaySize.Height ) );
				node.Pos = new PointF( x, y );
			}
		}


		#endregion
	}


	/// <summary>
	/// Places nodes in ranks
	/// </summary>
	public class DaGraphViewNodeRankPlacer : IDaGraphViewNodePlacer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="verticalRanks">True if nodes are to be placed in vertical ranks. False is nodes are to be placed in horizontal ranks</param>
		public DaGraphViewNodeRankPlacer( bool verticalRanks )
		{
			m_VerticalRanks = verticalRanks;
		}

		#region IDaGraphViewNodePlacer Members
		
		/// <summary>
		/// Binds the placer to a control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		public virtual void BindToControl( Control control )
		{
		}

		/// <summary>
		/// Unbinds the placer from a control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		public virtual void UnbindFromControl( Control control )
		{
		}

		/// <summary>
		/// Places a single node
		/// </summary>
		/// <param name="node">Node to place</param>
		public void PlaceNode( IDaGraphNode node )
		{
			int rank = DaGraphUtils.GetLongestPathLengthToRoot( node );
			AddNodeToRank( rank, node );
			CalculateNodePositions( );
		}

		#endregion

		#region Protected Members

		#region Rank Class

		/// <summary>
		/// Rank information
		/// </summary>
		protected class Rank
		{
			/// <summary>
			/// Gets the list of all the nodes in this rank
			/// </summary>
			public List<IDaGraphNode> Nodes
			{
				get { return m_Nodes; }
			}

			/// <summary>
			/// Gets/sets the location(top left corner) of this rank 
			/// </summary>
			public Rectangle Rectangle
			{
				get { return m_Rectangle; }
				set { m_Rectangle = value; }
			}

			#region Private Members

			private Rectangle m_Rectangle;
			private readonly List<IDaGraphNode> m_Nodes = new List<IDaGraphNode>();

			#endregion

		}

		#endregion

		/// <summary>
		/// Returns true if nodes are being placed in vertical ranks
		/// </summary>
		protected bool VerticalRanks
		{
			get { return m_VerticalRanks; }
		}

		/// <summary>
		/// Returns a read-only list of all ranks
		/// </summary>
		protected ReadOnlyCollection<Rank> Ranks
		{
			get { return m_Ranks.AsReadOnly( ); }
		}
		
		/// <summary>
		/// Resizes the first rank, then offsets all subsequent ranks
		/// </summary>
		protected void OffsetRanks( int startIndex, Point offset )
		{
			Rectangle inflateRect = m_Ranks[ startIndex ].Rectangle;
			inflateRect.Width += offset.X;
			inflateRect.Height += offset.Y;
			m_Ranks[ startIndex ].Rectangle = inflateRect;

			for ( int rankIndex = startIndex + 1; rankIndex < m_Ranks.Count; ++rankIndex )
			{
				Rectangle moveRect = m_Ranks[ rankIndex ].Rectangle;
				moveRect.X += offset.X;
				moveRect.Y += offset.Y;
				m_Ranks[ rankIndex ].Rectangle = moveRect;
			}
		}

		#endregion

		#region Private Members

		private readonly static int s_NodePadding = 6;
		private readonly static int s_RankPadding = 6;

		private readonly bool m_VerticalRanks;
		private List<Rank> m_Ranks = new List<Rank>( );

		private void CalculateNodePositions( )
		{
			if ( !m_VerticalRanks )
			{
				throw new NotImplementedException( );
			}
			if ( m_Ranks.Count == 0 )
			{
				return;
			}
			Dictionary<IDaGraphNode, int> visited = new Dictionary<IDaGraphNode, int>( );
			int y = 0;
			foreach ( IDaGraphNode node in m_Ranks[ 0 ].Nodes )
			{
				y += CalculateNodePositions( node, y, 0, visited );
			}
		}

		private int CalculateNodePositions( IDaGraphNode node, int startY, int rankIndex, Dictionary<IDaGraphNode, int> visited )
		{
			if ( visited.ContainsKey( node ) )
			{
				return 0;
			}
			node.DisplayPosition = new Point( node.DisplayPosition.X, startY );
			int heightOfOutputNodes = node.DisplaySize.Height + s_NodePadding;
			for ( ++rankIndex; rankIndex < m_Ranks.Count; ++rankIndex )
			{
				visited[ node ] = heightOfOutputNodes;
				foreach ( IDaGraphNode outputNode in m_Ranks[ rankIndex ].Nodes )
				{
					if ( !DaGraphUtils.IsInputNodeOf( outputNode, node ) )
					{
						continue;
					}
					heightOfOutputNodes += CalculateNodePositions( outputNode, startY + heightOfOutputNodes, rankIndex, visited );
				}
			}
			visited[ node ] = heightOfOutputNodes;
			return heightOfOutputNodes;
		}

		private void AddNodeToRank( int index, IDaGraphNode node )
		{
			Rank rank = GetRank( index );
			rank.Nodes.Add( node );
			Rectangle rankRect = rank.Rectangle;
			Point offset = new Point( 0, 0 );
			if ( m_VerticalRanks )
			{
				node.DisplayPosition = new Point( rankRect.Left, rankRect.Bottom + s_NodePadding );
				rankRect.Height += node.DisplaySize.Height + s_NodePadding;
				int diff = ( node.DisplaySize.Width + s_RankPadding ) - rankRect.Width;
				if ( diff > 0 )
				{
					offset = new Point( diff, 0 );
				}
			}
			else
			{
				node.DisplayPosition = new Point( rankRect.Right + s_NodePadding, rankRect.Top );
				rankRect.Width += node.DisplaySize.Width + s_NodePadding;
				int diff = ( node.DisplaySize.Height + s_RankPadding ) - rankRect.Height;
				if ( diff > 0 )
				{
					offset = new Point( 0, diff );
				}
			}
			rank.Rectangle = rankRect;
			if ( offset.X != 0 || offset.Y != 0 )
			{
				OffsetRanks( index, offset );
			}
		}

		/// <summary>
		/// Gets a rank at a specified index
		/// </summary>
		private Rank GetRank( int index )
		{
			while ( m_Ranks.Count <= index )
			{
				Rank rank = new Rank( );
				if ( index > 0 )
				{
					Rectangle lastRect = m_Ranks[ index - 1 ].Rectangle;
					if ( m_VerticalRanks )
					{
						rank.Rectangle = new Rectangle( lastRect.Right, lastRect.Y, 0, 0 );
					}
					else
					{
						rank.Rectangle = new Rectangle( lastRect.X, lastRect.Bottom, 0, 0 );
					}
				}
				m_Ranks.Add( rank );
			}
			return m_Ranks[ index ];
		}

		#endregion
	}

	public class DaGraphViewNodeResizableRankPlacer : DaGraphViewNodeRankPlacer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="verticalRanks">True if nodes are to be placed in vertical ranks. False is nodes are to be placed in horizontal ranks</param>
		public DaGraphViewNodeResizableRankPlacer( bool verticalRanks ) : base( verticalRanks )
		{
		}

		/// <summary>
		/// Binds the placer to a control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		public override void BindToControl( Control control )
		{
			m_Control = control;
			control.MouseLeave += OnMouseLeave;
			control.MouseMove += OnMouseMove;
			control.MouseDown += OnMouseDown;
			control.MouseUp += OnMouseUp;
		}

		/// <summary>
		/// Unbinds the placer from a control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		public override void UnbindFromControl( Control control )
		{
			Debug.Assert( m_Control == control );
			m_Control = null;
		}

		#region Private Members

		private int m_ResizeIndex = -1;
		private bool m_Resizing;
		private Point m_LastMousePos;
		private Control m_Control;

		/// <summary>
		/// Returns the rank whose splitter the specified position is over
		/// </summary>
		private int GetRankIndexAt( Point pos )
		{
			for ( int rankIndex = 0; rankIndex < Ranks.Count; ++rankIndex )
			{
			    Rank rank = Ranks[ rankIndex ];
			    if ( rank.Rectangle.Contains( pos ) )
			    {
			        return rankIndex;
			    }
			}
			return -1;
		}
		
		/// <summary>
		/// Sets the resize index, and updates the cursor accordinly
		/// </summary>
		private int ResizeIndex
		{
			set
			{
				if ( m_ResizeIndex != value )
				{
					m_ResizeIndex = value;
					m_Control.Cursor = ( m_ResizeIndex != -1 ) ? ( VerticalRanks ? Cursors.VSplit : Cursors.HSplit ) : Cursors.Arrow;
					if ( m_ResizeIndex == -1 )
					{
						m_Resizing = false;
					}
				}
			}
		}
		
		private void OnMouseMove( object sender, MouseEventArgs e )
		{
			if ( m_Resizing && m_ResizeIndex != -1 )
			{
				if ( VerticalRanks )
				{
					OffsetRanks( m_ResizeIndex, new Point( e.Location.X - m_LastMousePos.X, 0 ) );
				}
				else
				{
					OffsetRanks( m_ResizeIndex, new Point( 0, e.Location.Y - m_LastMousePos.Y ) );
				}

				m_Control.Invalidate( true );
			}
			else
			{
				ResizeIndex = GetRankIndexAt( e.Location );
			}
			m_LastMousePos = e.Location;
		}
		
		private void OnMouseDown( object sender, MouseEventArgs e )
		{
			if ( m_ResizeIndex != -1 )
			{
				m_Resizing = true;
			}
		}

		private void OnMouseLeave( object sender, EventArgs e )
		{
			ResizeIndex = -1;
		}

		private void OnMouseUp( object sender, MouseEventArgs e )
		{
			ResizeIndex = -1;
		}

		#endregion
	}

	/// <summary>
	/// Interface for rendering connections using GDI+
	/// </summary>
	public interface IDaGraphViewConnectionRenderer
	{
		/// <summary>
		/// Renders the connections between a node and its inputs
		/// </summary>
		void RenderConnections( Graphics graphics, Rectangle bounds, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes );
	}

	/// <summary>
	/// Interface for rendering a node using GDI+
	/// </summary>
	public interface IDaGraphViewNodeRenderer
	{
		/// <summary>
		/// Gets the display size of a node
		/// </summary>
		Size CalculateNodeDisplaySize( IDaGraphNode node, Font font );

		/// <summary>
		/// Renders the specified node
		/// </summary>
		void Render( Graphics graphics, Font defaultFont, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes );

		/// <summary>
		/// Renders a connection between two nodes
		/// </summary>
		//void RenderConnection( Graphics graphics, Font font,  );
	}

	/// <summary>
	/// Default connection renderer
	/// </summary>
	public class DaGraphViewDefaultConnectionRenderer : IDaGraphViewConnectionRenderer
	{
		/// <summary>
		/// Renders the connections between a node and its inputs
		/// </summary>
		public void RenderConnections( Graphics graphics, Rectangle bounds, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes )
		{
			using
			(
				Pen connectionPen	= new Pen( style.ConnectionColour ),
					selInputPen		= new Pen( style.InputConnectionColour, 2 ),
					selOutputPen	= new Pen( style.OutputConnectionColour, 2 )
			)
			{
				foreach ( IDaGraphNode node in nodes )
				{
					float x = node.DisplayPosition.X + node.DisplaySize.Width / 2;
					float y = node.DisplayPosition.Y + node.DisplaySize.Height / 2;
					foreach ( IDaGraphNode inputNode in node.Inputs )
					{
						float eX = inputNode.DisplayPosition.X + inputNode.DisplaySize.Width / 2;
						float eY = inputNode.DisplayPosition.Y + inputNode.DisplaySize.Height / 2;
						
						Pen pen = node.Selected ? selInputPen : ( inputNode.Selected ? selOutputPen : connectionPen );
						graphics.DrawLine( pen, x, y, eX, eY );
					}
				}
			}
		}

	}

	/// <summary>
	/// Default connection renderer
	/// </summary>
	public class DaGraphViewConnectionPathRenderer : IDaGraphViewConnectionRenderer
	{
		/// <summary>
		/// Renders the connections between a node and its inputs
		/// </summary>
		public void RenderConnections( Graphics graphics, Rectangle bounds, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes )
		{
			int width = bounds.Width;
			int height = bounds.Height;
			if ( ( m_Grid == null ) || ( m_Grid.Width != width ) || ( m_Grid.Height != height ) )
			{
				m_Grid = new ObstacleGrid( width, height );
				m_Pathfinder = new ConnectionPathfinder( m_Grid );
			}
			else
			{
				Debug.Assert( m_Pathfinder != null );
				m_Grid.Clear( );
			}
		
			foreach ( IDaGraphNode node in nodes )
			{
				m_Grid.AddObstacle( node.DisplayRectangle );
			}

			using
			(
				Pen connectionPen	= new Pen( style.ConnectionColour ),
					selInputPen		= new Pen( style.InputConnectionColour, 2 ),
					selOutputPen	= new Pen( style.OutputConnectionColour, 2 )
			)
			{
				foreach ( IDaGraphNode node in nodes )
				{
					int x = node.DisplayPosition.X + node.DisplaySize.Width / 2;
					int y = node.DisplayPosition.Y + node.DisplaySize.Height / 2;
					foreach ( IDaGraphNode inputNode in node.Inputs )
					{
						int eX = inputNode.DisplayPosition.X + inputNode.DisplaySize.Width / 2;
						int eY = inputNode.DisplayPosition.Y + inputNode.DisplaySize.Height / 2;
						
						Pen pen = node.Selected ? selInputPen : ( inputNode.Selected ? selOutputPen : connectionPen );
						Point[] path = m_Pathfinder.GetPathPoints( new Point( x, y ), new Point( eX, eY ) );

						DrawConnectionPath( graphics, path, pen, 4.0f, 6.0f );
					}
				}
			}
		}

		#region Private Members

		private ConnectionPathfinder m_Pathfinder;
		private ObstacleGrid m_Grid;

		/// <summary>
		/// Draws a connection path
		/// </summary>
		private static void DrawConnectionPath( Graphics graphics, Point[] path, Pen pen, float arcRadius, float arrowSize )
		{
			List<Point> points = new List<Point>( );
			int lastIndex = path.Length - 1;
			for ( int pathIndex = 1; pathIndex < path.Length; ++pathIndex )
			{
				Point v0 = path[ pathIndex - 1 ];
				Point v1 = path[ pathIndex ];
				float vX = v1.X - v0.X;
				float vY = v1.Y - v0.Y;
				float len = ( float )Math.Sqrt( ( vX * vX ) + ( vY * vY ) );
				if ( len < 0.00001f )
				{
					continue;
				}
				vX /= len;
				vY /= len;

				Point start	= ( pathIndex == 1 ) ? v0 : new Point( ( int )( v0.X + vX * arcRadius ), ( int )( v0.Y + vY * arcRadius ) );
				Point end	= ( pathIndex == lastIndex ) ? v1 : new Point( ( int )( v1.X - vX * arcRadius ), ( int )( v1.Y - vY * arcRadius ) );
				
				points.Add( start );
				points.Add( end );

				//	Draw an arrow midway along the line
				float midX = ( v0.X + v1.X ) / 2.0f;
				float midY = ( v0.Y + v1.Y ) / 2.0f;

				float sz = arrowSize;
				float hSz = arrowSize / 2;
				float cpVx = vY;
				float cpVy = -vX;

				float bX = midX - vX * sz;
				float bY = midY - vY * sz;

				float lVx = bX + cpVx * hSz;
				float lVy = bY + cpVy * hSz;
				float rVx = bX - cpVx * hSz;
				float rVy = bY - cpVy * hSz;

				graphics.FillPolygon( pen.Brush, new PointF[] { new PointF( midX, midY ), new PointF( rVx, rVy ), new PointF( lVx, lVy ) } );
			}
			
			graphics.DrawLines( pen, points.ToArray( ) );
		}

		#endregion
	}
	
	/// <summary>
	/// Node renderer abstract base
	/// </summary>
	public abstract class AbstractDaGraphViewNodeRenderer : IDaGraphViewNodeRenderer
	{
		/// <summary>
		/// Gets/sets the top padding of the node within the node rectangle
		/// </summary>
		public int NodeRectTopPadding
		{
			get { return m_NodeRectTopPadding; }
			set { m_NodeRectTopPadding = value; }
		}

		/// <summary>
		/// Gets/sets the bottom padding of the node within the node rectangle
		/// </summary>
		public int NodeRectBottomPadding
		{
			get { return m_NodeRectBottomPadding; }
			set { m_NodeRectBottomPadding = value; }
		}

		/// <summary>
		/// Gets/sets the right padding of the node within the node rectangle
		/// </summary>
		public int NodeRectRightPadding
		{
			get { return m_NodeRectRightPadding; }
			set { m_NodeRectRightPadding = value; }
		}

		/// <summary>
		/// Gets/sets the left padding of the node within the node rectangle
		/// </summary>
		public int NodeRectLeftPadding
		{
			get { return m_NodeRectLeftPadding; }
			set { m_NodeRectLeftPadding = value; }
		}
		
		#region IDaGraphViewNodeRenderer Members

		/// <summary>
		/// Gets the display size of a node
		/// </summary>
		public virtual Size CalculateNodeDisplaySize( IDaGraphNode node, Font font )
		{
			Size size = TextRenderer.MeasureText( node.Text, font );
			size.Width += ( NodeRectLeftPadding + NodeRectRightPadding );
			size.Height += ( NodeRectTopPadding + NodeRectBottomPadding );
			return size;
		}
		
		/// <summary>
		/// Renders the specified node
		/// </summary>
		public abstract void Render( Graphics graphics, Font defaultFont, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes );

		#endregion

		#region Private Members

		private int m_NodeRectTopPadding = 2;
		private int m_NodeRectBottomPadding = 2;
		private int m_NodeRectRightPadding = 2;
		private int m_NodeRectLeftPadding = 2;

		#endregion
	}


	/// <summary>
	/// Default node renderer
	/// </summary>
	public class DaGraphViewDefaultNodeRenderer : AbstractDaGraphViewNodeRenderer
	{
		#region IDaGraphViewNodeRenderer Members

		/// <summary>
		/// Renders the specified node
		/// </summary>
		public override void Render( Graphics graphics, Font defaultFont, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes )
		{
			using ( Pen nodePen = new Pen( Color.Black ), selNodePen = new Pen( Color.Black, 2.0f ) )
			{
				foreach ( IDaGraphNode node in nodes )
				{
					Rectangle nodeRect = new Rectangle( node.DisplayPosition, node.DisplaySize );
					using ( Brush brush = new SolidBrush( style.NodeBackgroundColour ) )
					{
						graphics.FillRectangle( brush, nodeRect );
					}
					graphics.DrawRectangle( node.Selected ? selNodePen : nodePen, nodeRect );

					TextRenderer.DrawText( graphics, node.Text, defaultFont, nodeRect, Color.Black, TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine );
				}
			}
		}

		#endregion
	}

	public class DaGraphViewCurvyNodeRenderer : AbstractDaGraphViewNodeRenderer
	{
		/// <summary>
		/// Renders the specified node
		/// </summary>
		public override void Render( Graphics graphics, Font defaultFont, DaGraphViewStyle style, IEnumerable<IDaGraphNode> nodes )
		{
			Color back = style.NodeBackgroundColour;
			Color lightBack = GraphUtils.Scale( back, 1.3f );
			ColorBlend blend = GraphUtils.CreateColourBlend( back, 0, back, 0.8f, lightBack, 0.85f, lightBack, 1.0f );

			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			using ( SolidBrush shadowBrush = new SolidBrush( GraphUtils.Scale( style.BackgroundColour,  0.9f ) ) )
			{
				foreach ( IDaGraphNode node in nodes )
				{
					Rectangle nodeRect = new Rectangle( node.DisplayPosition, node.DisplaySize );
					using ( GraphicsPath path = GraphUtils.CreateRoundedRectanglePath( nodeRect.X - 2, nodeRect.Y - 2, nodeRect.Width, nodeRect.Height, 4 ) )
					{
						graphics.FillPath( shadowBrush, path );
					}
				}
			}

			using ( Pen nodePen = new Pen( Color.Black ), selNodePen = new Pen( Color.Black, 2.0f ) )
			{
				foreach ( IDaGraphNode node in nodes )
				{
					Rectangle nodeRect = new Rectangle( node.DisplayPosition, node.DisplaySize );

					using ( GraphicsPath path = GraphUtils.CreateRoundedRectanglePath( nodeRect.X, nodeRect.Y, nodeRect.Width, nodeRect.Height, 4 ) )
					{
						using ( LinearGradientBrush brush = new LinearGradientBrush( nodeRect, Color.Black, Color.Black, 90.0f ) )
						{
							brush.InterpolationColors = blend;
							graphics.FillPath( brush, path );
						}
						graphics.DrawPath( node.Selected ? selNodePen : nodePen, path );
					}
					TextRenderer.DrawText( graphics, node.Text, defaultFont, nodeRect, Color.Black, TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine );
				}
			}
		}
	}

	public class ObstacleGrid
	{
		/// <summary>
		/// Sets up the pathfinder
		/// </summary>
		public ObstacleGrid( int width, int height )
		{
			if ( width <= 0 )
			{
				throw new ArgumentException( "Invalid width", "width" );
			}
			if ( height <= 0 )
			{
				throw new ArgumentException( "Invalid height", "height" );
			}
			int cellWidth = ( int )( width / ( float )m_GridCellSize ) + 1;
			int cellHeight = ( int )( height / ( float )m_GridCellSize ) + 1;
			m_Cells = new int[ cellWidth, cellHeight ];
		}

		/// <summary>
		/// Clears the grid of all obstacles
		/// </summary>
		public void Clear( )
		{
			for ( int y = 0; y < Height; ++y )
			{
				for ( int x = 0; x < Width; ++x )
				{
					m_Cells[ x, y ] = 0;
				}
			}
		}

		/// <summary>
		/// Adds an axis-aligned rectangular obstacle to the pathfinder
		/// </summary>
		public void AddObstacle( Rectangle rect )
		{
			Point tl = ClampToGrid( ToGrid( rect.Location ) );
			Point br = ClampToGrid( ToGrid( new Point( rect.Right, rect.Bottom ) ) );
			++m_ObstacleCount;
			for ( int y = tl.Y; y <= br.Y; ++y )
			{
				for ( int x = tl.X; x <= br.X; ++x )
				{
					m_Cells[ x, y ] = m_ObstacleCount;
				}
			}
		}

		public int GetGridId( Point pt )
		{
			return m_Cells[ pt.X, pt.Y ];
		}
		
		public Point ClampToGrid( Point pt )
		{
			int x = pt.X < 0 ? 0 : ( pt.X >= Width ? Width - 1 : pt.X );
			int y = pt.Y < 0 ? 0 : ( pt.Y >= Height ? Height - 1 : pt.Y );
			return new Point( x, y );
		}

		public Point ToGrid( Point pt )
		{
			float x = pt.X / ( float )m_GridCellSize;
			float y = pt.Y / ( float )m_GridCellSize;
			return new Point( ( int )Math.Round( x ), ( int )Math.Round( y ) );
		}
		
		public Point FromGrid( Point pt )
		{
			float x = pt.X * ( float )m_GridCellSize;
			float y = pt.Y * ( float )m_GridCellSize;
			return new Point( ( int )x, ( int )y );
		}

		public int Width
		{
			get { return m_Cells.GetLength( 0 ); }
		}

		public int Height
		{
			get { return m_Cells.GetLength( 1 ); }
		}

		private int m_GridCellSize = 8;
		private int m_ObstacleCount;
		private int[,] m_Cells;
	}

	public class ConnectionPathfinder
	{
		public ConnectionPathfinder( ObstacleGrid grid )
		{
			Arguments.CheckNotNull( grid, "grid" );
			m_Grid		= grid;
			m_FScores	= new float[ grid.Width, grid.Height ];
			m_GScores	= new float[ grid.Width, grid.Height ];
			m_HScores	= new float[ grid.Width, grid.Height ];
			m_Flags		= new byte[ grid.Width, grid.Height ];
		}

		public Point[] GetPathPoints( Point start, Point end )
		{
			start = m_Grid.ClampToGrid( m_Grid.ToGrid( start ) );
			end = m_Grid.ClampToGrid( m_Grid.ToGrid( end ) );
			if ( start == end )
			{
				return new Point[] { start, end };
			}
			for ( int y = 0; y < m_Grid.Height; ++y )
			{
				for ( int x = 0; x < m_Grid.Width; ++x )
				{
					m_GScores[ x, y ] = 0;
					m_FScores[ x, y ] = 0;
					m_HScores[ x, y ] = 0;
					m_Flags[ x, y ] = 0;
				}
			}
			m_OpenSet.Clear( );

			int ignore0 = m_Grid.GetGridId( start );
			int ignore1 = m_Grid.GetGridId( end );

			float firstGuess = EstimatePathDistance( start, end );

			m_GScores[ start.X, start.Y ] = 0;
			m_HScores[ start.X, start.Y ] = firstGuess;
			m_FScores[ start.X, start.Y ] = firstGuess;

			AddOpen( start, firstGuess );

			while ( m_OpenSet.Count > 0 )
			{
				Point p = PopOpen( );
				if ( p == end )
				{
					return BuildPath( p, start );
				}
				float neighbourG = m_GScores[ p.X, p.Y ] + 1;
				for ( int neighbourIndex = 0; neighbourIndex < m_NeighbourOffsets.Length; ++neighbourIndex )
				{
					Point nO = m_NeighbourOffsets[ neighbourIndex ];
					Point nP = new Point( p.X + nO.X, p.Y + nO.Y );
					if ( nP.X < 0 || nP.X >= m_Grid.Width || nP.Y < 0 || nP.Y >= m_Grid.Height )
					{
						continue;
					}
					if ( ( m_Flags[ nP.X, nP.Y ] & InCloseSet ) != 0 )
					{
						continue;
					}
					int gridId = m_Grid.GetGridId( nP );
					if ( ( gridId != 0 ) && ( gridId != ignore0 ) && ( gridId != ignore1 ) )
					{
						m_Flags[ nP.X, nP.Y ] |= InCloseSet;
						continue;
					}

					bool isBetter = false;
					if ( ( m_Flags[ nP.X, nP.Y ] & InOpenSet ) == 0 )
					{
						//	Not in open set
						float hScore = EstimatePathDistance( nP, end );
						m_HScores[ nP.X, nP.Y ] = hScore;
						float nFScore = neighbourG + hScore;
						AddOpen( nP, nFScore );
						isBetter = true;
					}
					else if ( neighbourG < m_GScores[ nP.X, nP.Y ] )
					{
						m_FScores[ nP.X, nP.Y ] = neighbourG + m_HScores[ nP.X, nP.Y ];
						isBetter = true;
					}
					if ( isBetter )
					{
						m_GScores[ nP.X, nP.Y ] = neighbourG;
						m_Flags[ nP.X, nP.Y ] |= ( byte )( neighbourIndex + 1 );
					}
				}
			}

			return new Point[ 0 ];
		}
		
		private Point[] BuildPath( Point start, Point goal )
		{
			List<Point> points = new List<Point>( );
			points.Add( m_Grid.FromGrid( start ) );
			Point pathPt = start;
			int lastOffsetIndex = -1;
			do
			{
				int offsetIndex = ( m_Flags[ pathPt.X, pathPt.Y ] & CameFromMask ) - 1;
				if ( offsetIndex < 0 )
				{
					break;
				}
				if ( offsetIndex != lastOffsetIndex )
				{
					//	Path changed direction - add the current path point to the path
					points.Add( m_Grid.FromGrid( pathPt ) );
				}
				Point n0 = m_NeighbourOffsets[ offsetIndex ];
				pathPt.X -= n0.X;
				pathPt.Y -= n0.Y;
				lastOffsetIndex = offsetIndex;
			} while ( pathPt != goal );
			
			points.Add( m_Grid.FromGrid( pathPt ) );
			return points.ToArray( );
		}

		private void AddOpen( Point value, float score )
		{
			m_Flags[ value.X, value.Y ] |= InOpenSet;
			m_FScores[ value.X, value.Y ] = score;
			for ( int nodeIndex = 0; nodeIndex < m_OpenSet.Count; ++nodeIndex )
			{
				if ( score < m_OpenSet[ nodeIndex ].FScore )
				{
					m_OpenSet.Insert( nodeIndex, new Node( value, score ) );
					return;
				}
			}
			m_OpenSet.Add( new Node( value, score ) );
		}

		private Point PopOpen( )
		{
			Node value = m_OpenSet[ 0 ];
			m_OpenSet.RemoveAt( 0 );

			Point p = value.Pos;
			m_Flags[ p.X, p.Y ] &= InvInOpenSet;
			m_Flags[ p.X, p.Y ] |= InCloseSet;
			return p;
		}

		private static int EstimatePathDistance( Point start, Point end )
		{
			return Math.Abs( end.X - start.X ) + Math.Abs( end.Y - start.Y );
		}

		private struct Node
		{
			public Node( Point pos, float fScore )
			{
				m_Pos = pos;
				m_FScore = fScore;
			}

			public Point Pos
			{
				get { return m_Pos; }
			}

			public float FScore
			{
				get { return m_FScore; }
			}
			
			private readonly Point m_Pos;
			private readonly float m_FScore;
		}

		private readonly ObstacleGrid m_Grid;
		private readonly float[,] m_FScores;
		private readonly float[,] m_GScores;
		private readonly float[,] m_HScores;
		private readonly byte[,] m_Flags;
		private List<Node> m_OpenSet = new List<Node>( );

		private readonly Point[] m_NeighbourOffsets = new Point[]
				{
					new Point( -1, 0 ), 
					new Point( +1, 0 ),
					new Point( 0, -1 ), 
					new Point( 0, +1 ),
				};
	
		private const byte CameFromMask = 0x7;
		private const byte InOpenSet = 0x8;
		private const byte InCloseSet = 0x10;
		private const byte InvInOpenSet = unchecked( ( byte )~InOpenSet );

	}

	/// <summary>
	/// Directed acyclic graph node
	/// </summary>
	public interface IDaGraphNode
	{
		/// <summary>
		/// Event raised when the node is moved
		/// </summary>
		event Action<IDaGraphNode> Moved;

		/// <summary>
		/// Event raised when an input node is added to this node
		/// </summary>
		event Action<IDaGraphNode> InputNodeAdded;

		/// <summary>
		/// Event raised when an input node is removed from this node
		/// </summary>
		event Action<IDaGraphNode> InputNodeRemoved;

		/// <summary>
		/// Event raised when an input node is selected/deselected
		/// </summary>
		event EventHandler SelectionChanged;

		/// <summary>
		/// Adds an input node
		/// </summary>
		/// <param name="node">Node to add</param>
		void AddInput( IDaGraphNode node );

		/// <summary>
		/// Removes an input node
		/// </summary>
		/// <param name="node">Node to remove</param>
		void RemoveInput( IDaGraphNode node );

		/// <summary>
		/// Gets the inputs to this node
		/// </summary>
		IDaGraphNode[] Inputs
		{
			get;
		}

		/// <summary>
		/// Gets/sets the expanded flag for this node
		/// </summary>
		bool Expanded
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets node text
		/// </summary>
		string Text
		{
			get; set;
		}

		/// <summary>
		/// User data tag
		/// </summary>
		object Tag
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the selected flag of this node
		/// </summary>
		bool Selected
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the display size of this node
		/// </summary>
		Size DisplaySize
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the display position of this node
		/// </summary>
		Point DisplayPosition
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the display rectangle of this node
		/// </summary>
		Rectangle DisplayRectangle
		{
			get; set;
		}
	}

	/// <summary>
	/// Directed acyclic graph view interface
	/// </summary>
	public interface IDaGraphView
	{
		/// <summary>
		/// Adds a node, and all its inputs and outputs, to the graph view
		/// </summary>
		/// <param name="node">Node to add</param>
		void Add( IDaGraphNode node );

		/// <summary>
		/// Removes a node, and all its inputs and outputs, from the graph view
		/// </summary>
		/// <param name="node">Node to remove</param>
		void Remove( IDaGraphNode node );
	}

	public static class GraphUtils
	{
        /// <summary>
        /// Creates a path representing a rounded rectangle
        /// </summary>
        public static GraphicsPath CreateRoundedRectanglePath( float x, float y, float w, float h, float radius )
        {
                GraphicsPath path = new GraphicsPath( );
                float diameter = radius * 2;
                float endX = x + w - diameter;
                float endY = y + h - diameter;
                path.AddArc( x, y, diameter, diameter, 180, 90 );
                path.AddArc( endX, y, diameter, diameter, 270, 90 );
                path.AddArc( endX, endY, diameter, diameter, 0, 90 );
                path.AddArc( x, endY, diameter, diameter, 90, 90 );
                path.CloseFigure( );
                return path;
        }


        /// <summary>
        /// Creates a 2-colour colour blend
        /// </summary>
        public static ColorBlend CreateColourBlend( Color c0, float t0, Color c1, float t1 )
        {
                ColorBlend blend = new ColorBlend( 2 );
                blend.Colors[ 0 ] = c0;
                blend.Colors[ 1 ] = c1;
                blend.Positions[ 0 ] = t0;
                blend.Positions[ 1 ] = t1;
                return blend;
        }

        /// <summary>
        /// Creates a 3-colour colour blend
        /// </summary>
        public static ColorBlend CreateColourBlend( Color c0, float t0, Color c1, float t1, Color c2, float t2 )
        {
                ColorBlend blend = new ColorBlend( 3 );
                blend.Colors[ 0 ] = c0;
                blend.Colors[ 1 ] = c1;
                blend.Colors[ 2 ] = c2;
                blend.Positions[ 0 ] = t0;
                blend.Positions[ 1 ] = t1;
                blend.Positions[ 2 ] = t2;
                return blend;
        }

        /// <summary>
        /// Creates a 4-colour colour blend
        /// </summary>
        public static ColorBlend CreateColourBlend( Color c0, float t0, Color c1, float t1, Color c2, float t2, Color c3, float t3 )
        {
                ColorBlend blend = new ColorBlend( 4 );
                blend.Colors[ 0 ] = c0;
                blend.Colors[ 1 ] = c1;
                blend.Colors[ 2 ] = c2;
                blend.Colors[ 3 ] = c3;
                blend.Positions[ 0 ] = t0;
                blend.Positions[ 1 ] = t1;
                blend.Positions[ 2 ] = t2;
                blend.Positions[ 3 ] = t3;
                return blend;
        }


		public static Color Scale( Color c, float scale )
		{
			int r = Clamp( ( int )( c.R * scale ) );
			int g = Clamp( ( int )( c.G * scale ) );
			int b = Clamp( ( int )( c.B * scale ) );
			int a = Clamp( ( int )( c.A * scale ) );
			return Color.FromArgb( a, r, g, b );
		}

		public static int Clamp( int c )
		{
			return ( c < 0 ) ? 0 : ( c > 255 ? 255 : c );
		}
	}

	/// <summary>
	/// View style for a DaGraphViewControl
	/// </summary>
	public class DaGraphViewStyle
	{
		/// <summary>
		/// Gets the background colour of a graph node
		/// </summary>
		public Color NodeBackgroundColour
		{
			get { return m_NodeBackgroundColour; }
			set { m_NodeBackgroundColour = value; }
		}

		/// <summary>
		/// Gets/sets the colour used to render node connections
		/// </summary>
		public Color ConnectionColour
		{
			get { return m_ConnectionColour; }
			set { m_ConnectionColour = value; }
		}

		/// <summary>
		/// Gets/sets the colour used to render highlighted node input connections
		/// </summary>
		public Color InputConnectionColour
		{
			get { return m_InputConnectionColour; }
			set { m_InputConnectionColour = value; }
		}

		/// <summary>
		/// Gets/sets the colour used to render highlighted node output connections
		/// </summary>
		public Color OutputConnectionColour
		{
			get { return m_OutputConnectionColour; }
			set { m_OutputConnectionColour = value; }
		}

		/// <summary>
		/// Gets/sets the background colour
		/// </summary>
		public Color BackgroundColour
		{
			get { return m_BackgroundColour; }
			set { m_BackgroundColour = value; }
		}

		#region Private Members

		private Color m_BackgroundColour = Color.Gray;
		private Color m_ConnectionColour = Color.Black;
		private Color m_InputConnectionColour = Color.DarkRed;
		private Color m_OutputConnectionColour = Color.DarkGreen;
		private Color m_NodeBackgroundColour = Color.LightSteelBlue;

		#endregion

	}

	/// <summary>
	/// Typed graph node
	/// </summary>
	/// <typeparam name="T">Value type to store in the node</typeparam>
	public class DaGraphNode<T> : IDaGraphNode
	{
		/// <summary>
		/// Gets/sets the node value
		/// </summary>
		public T Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		#region IDaGraphNode Members

		/// <summary>
		/// Event raised when the node is moved
		/// </summary>
		public event Action<IDaGraphNode> Moved;

		/// <summary>
		/// Event raised when an input node is selected/deselected
		/// </summary>
		public event EventHandler SelectionChanged;

		/// <summary>
		/// Event raised when an input node is added to this node
		/// </summary>
		public event Action<IDaGraphNode> InputNodeAdded;

		/// <summary>
		/// Event raised when an input node is removed from this node
		/// </summary>
		public event Action<IDaGraphNode> InputNodeRemoved;

		/// <summary>
		/// Gets/sets the expanded flag for this node
		/// </summary>
		public bool Expanded
		{
			get { return m_Expanded; }
			set { m_Expanded = value; }
		}

		/// <summary>
		/// Gets/sets node text
		/// </summary>
		public string Text
		{
			get { return m_Text; }
			set { m_Text = value; }
		}

		/// <summary>
		/// Adds an input node
		/// </summary>
		/// <param name="node">Node to add</param>
		public void AddInput( IDaGraphNode node )
		{
			Arguments.CheckNotNull( node, "node" );
			m_Inputs.Add( node );
			if ( InputNodeAdded != null )
			{
				InputNodeAdded( node );
			}
		}

		/// <summary>
		/// Removes an input node
		/// </summary>
		/// <param name="node">Node to remove</param>
		public void RemoveInput( IDaGraphNode node )
		{
			Arguments.CheckNotNull( node, "node" );
			if ( m_Inputs.Remove( node ) )
			{
				if ( InputNodeRemoved != null )
				{
					InputNodeRemoved( node );
				}
			}
		}

		/// <summary>
		/// Gets the inputs to this node
		/// </summary>
		public IDaGraphNode[] Inputs
		{
			get { return m_Inputs.ToArray( ); }
		}

		/// <summary>
		/// User data tag
		/// </summary>
		public object Tag
		{
			get { return m_Tag; }
			set { m_Tag = value; }
		}

		/// <summary>
		/// Gets/sets the selected flag of this node
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set
			{
				if ( m_Selected != value )
				{
					m_Selected = value;
					if ( SelectionChanged != null )
					{
						SelectionChanged( this, EventArgs.Empty );
					}
				}
			}
		}

		/// <summary>
		/// Gets/sets the display size of this node
		/// </summary>
		public Size DisplaySize
		{
			get { return m_DisplaySize; }
			set { m_DisplaySize = value; }
		}

		/// <summary>
		/// Gets/sets the display position of this node
		/// </summary>
		public Point DisplayPosition
		{
			get { return m_DisplayPosition; }
			set
			{
				if ( m_DisplayPosition != value )
				{
					m_DisplayPosition = value;
					if ( Moved != null )
					{
						Moved( this );
					}
				}
			}
		}

		/// <summary>
		/// Gets/sets the display rectangle of this node
		/// </summary>
		public Rectangle DisplayRectangle
		{
			get { return new Rectangle( DisplayPosition, DisplaySize );}
			set
			{
				DisplayPosition = value.Location;
				DisplaySize = value.Size;
			}
		}

		#endregion

		/// <summary>
		/// Returns the node text
		/// </summary>
		public override string ToString( )
		{
			return m_Text;
		}

		#region Private Members

		private bool m_Expanded;
		private string m_Text;
		private Size m_DisplaySize;
		private Point m_DisplayPosition;
		private bool m_Selected;
		private object m_Tag;
		private T m_Value;
		private readonly List<IDaGraphNode> m_Inputs = new List<IDaGraphNode>( );

		#endregion
	}

	/// <summary>
	/// Directed acyclic graph node builder. Adapter for an existing DAG representation
	/// </summary>
	public class DaGraphNodeBuilder<T>
	{
		/// <summary>
		/// Delegate used to retrieve the connections between objects of type T
		/// </summary>
		public delegate IEnumerable<T> GetInputsDelegate( T value );

		/// <summary>
		/// Creates a list of DA graph nodes from a set of values of type T
		/// </summary>
		/// <param name="values">Values to transform</param>
		/// <param name="getInputs">Delegate for retrieving inputs between elements in the values array</param>
		/// <returns>Returns an array of graph node representing the connections between elements in the values array</returns>
		public static IDaGraphNode[] CreateNodes( T[] values, GetInputsDelegate getInputs )
		{
			Dictionary<T, IDaGraphNode> nodeMap = new Dictionary<T, IDaGraphNode>();
			foreach ( T value in values )
			{
				CreateNode( nodeMap, value, getInputs );
			}
			return new List<IDaGraphNode>( nodeMap.Values ).ToArray( );
		}

		/// <summary>
		/// Creates a node from a value of type T
		/// </summary>
		private static IDaGraphNode CreateNode( Dictionary<T, IDaGraphNode> nodeMap, T value, GetInputsDelegate getInputs )
		{
			if ( nodeMap.ContainsKey( value ) )
			{
				return nodeMap[ value ];
			}
			DaGraphNode<T> node = new DaGraphNode<T>( );
			node.Value = value;
			node.Tag = value;
			node.Text = value.ToString( );
			nodeMap[ value ] = node;

			foreach ( T dependency in getInputs( value ) )
			{
				node.AddInput( CreateNode( nodeMap, dependency, getInputs ) );
			}
			return node;
		}
	}

	/// <summary>
	/// Directed acyclic graph utilities
	/// </summary>
	public static class DaGraphUtils
	{
		/// <summary>
		/// Returns true if rootNode is an input node of node
		/// </summary>
		public static bool IsInputNodeOf( IDaGraphNode node, IDaGraphNode rootNode )
		{
			if ( node == rootNode )
			{
				return true;
			}
			foreach ( IDaGraphNode inputNode in node.Inputs )
			{
				if ( IsInputNodeOf( inputNode, rootNode ) )
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the length of the longest path from a node to a root node
		/// </summary>
		/// <param name="node">Node to measure</param>
		/// <returns>Returns the length of the longest path to a root node (node with no inputs)</returns>
		public static int GetLongestPathLengthToRoot( IDaGraphNode node )
		{
			Arguments.CheckNotNull( node, "node" );
			if ( node.Inputs.Length == 0 )
			{
				return 0;
			}
			int length = 0;
			foreach ( IDaGraphNode input in node.Inputs )
			{
				length = Math.Max( length, GetLongestPathLengthToRoot( input ) + 1 );
			}
			return length;
		}
	}
}
