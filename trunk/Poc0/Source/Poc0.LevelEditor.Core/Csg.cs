using System;
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Handles CSG operations
	/// </summary>
	[Serializable]
	public class Csg
	{
		#region Public members

		/// <summary>
		/// Raised after a CSG operation has occurred
		/// </summary>
		public event EventHandler GeometryChanged;

		/// <summary>
		/// CSG set operations
		/// </summary>
		public enum Operation
		{
			/// <summary>
			/// The set of set A edges outside set B, and set B edges outside set A
			/// </summary>
			Union,

			/// <summary>
			///	The set of all edges in set A edges inside or outside set B, and set B edges inside or outside set A
			/// </summary>
			EdgeUnion,


			/// <summary>
			/// The set of set A edges inside set B, and set B edges inside set A
			/// </summary>
			Intersection,

			/// <summary>
			/// The set of set edges outside set B, and set B edges inside set A
			/// </summary>
			Complement
		}


		/// <summary>
		/// Combines a brush with the current geometry set
		/// </summary>
		/// <param name="op">CSG operation</param>
		/// <param name="brush">Brush to add to the level geometry</param>
		public void Combine( Operation op, CsgBrush brush )
		{
			BspNode brushBsp = Build( brush );
			if ( m_Root == null )
			{
				if ( op == Operation.Union || op == Operation.EdgeUnion )
				{
					m_Root = brushBsp;
				}
			}
			else
			{
				m_Root = Combine( op, m_Root, brushBsp );
			}
			if ( m_Root != null )
			{
				BuildConvexRegions( m_Root );
			}
			if ( GeometryChanged != null )
			{
				GeometryChanged( this, null );
			}
		}

		/// <summary>
		/// Returns the root node of the current BSP tree
		/// </summary>
		public BspNode Root
		{
			get { return m_Root; }
		}

		#endregion

		#region BSP types
		
		/// <summary>
		/// Currently using a BSP tree for our CSG needs
		/// </summary>
		[Serializable]
		public class BspNode
		{
			/// <summary>
			/// Sets up the node
			/// </summary>
			/// <param name="parent">Node parent</param>
			/// <param name="edge">Node edge</param>
			public BspNode( BspNode parent, Edge edge )
			{
				m_Parent = parent;
				m_Edge = edge;
			}

			/// <summary>
			/// The node plane
			/// </summary>
			public Plane2 Plane
			{
				get { return Edge.Plane; }
			}

			/// <summary>
			/// The subtree of nodes behind this node's plane
			/// </summary>
			public BspNode Behind
			{
				get { return m_Behind; }
				set { m_Behind = value; }
			}

			/// <summary>
			/// THe subtree of nodes in front of this node's plane
			/// </summary>
			public BspNode InFront
			{
				get { return m_InFront; }
				set { m_InFront = value; }
			}

			/// <summary>
			/// This node's edge
			/// </summary>
			public Edge Edge
			{
				get { return m_Edge; }
			}

			/// <summary>
			/// Gets the convex region associated with this node
			/// </summary>
			public Point2[] ConvexRegion
			{
				get { return m_Region; }
				set { m_Region = value; }
			}

			/// <summary>
			/// Gets the parent node
			/// </summary>
			public BspNode Parent
			{
				get { return m_Parent; }
			}

			private readonly BspNode m_Parent;
			private BspNode m_Behind;
			private BspNode m_InFront;
			private readonly Edge m_Edge;
			private Point2[] m_Region;
		}
		
		/// <summary>
		/// A line segment with a plane
		/// </summary>
		[Serializable]
		public class Edge
		{
			// Helper stuff for debugging
			private readonly int m_Index = EdgeCounter++;
			private static int EdgeCounter = 0;

			public int EdgeIndex
			{
				get { return m_Index; }
			}

			public Edge MakeFlippedEdge( )
			{
				Edge edge = new Edge( new Point2( P1 ), new Point2( P0 ) );
				return edge;
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="p0">Start point of the edge</param>
			/// <param name="p1">End point of the edge</param>
			public Edge( Point2 p0, Point2 p1 )
			{
				m_P0 = p0;
				m_P1 = p1;
				m_Plane = new Plane2( p0, p1 );
			}

			/// <summary>
			/// Gets/sets the double sided flag
			/// </summary>
			public bool DoubleSided
			{
				get { return m_DoubleSided; }
				set { m_DoubleSided = value; }
			}

			/// <summary>
			/// Start of the edge
			/// </summary>
			public Point2 P0
			{
				get { return m_P0; }
				set { m_P0 = value; }
			}

			/// <summary>
			/// End of the edge
			/// </summary>
			public Point2 P1
			{
				get { return m_P1; }
				set { m_P1 = value; }
			}

			/// <summary>
			/// Edge plane
			/// </summary>
			public Plane2 Plane
			{
				get { return m_Plane; }
			}

			private bool m_DoubleSided;
			private Point2 m_P0;
			private Point2 m_P1;
			private readonly Plane2 m_Plane;
		}

		#endregion

		#region BSP building

		private BspNode m_Root;

		/// <summary>
		/// A point is considered to be on the plane (PlaneClassification.On) if it's within this distance of the plane
		/// </summary>
		private const float OnPlaneTolerance = 0.001f;

		/// <summary>
		/// Combines two BSP trees using a specified CSG operation
		/// </summary>
		private static BspNode Combine( Operation op, BspNode set0, BspNode set1 )
		{
			IList< Edge > set0Edges = Flatten( set0 );
			IList< Edge > set1Edges = Flatten( set1 );
			List< Edge > allEdges = new List< Edge >( );

			switch ( op )
			{
				case Operation.Union :
					Clip( true, false, set0, set1Edges, allEdges );
					Clip( true, false, set1, set0Edges, allEdges );
					break;

				case Operation.EdgeUnion :
					Clip( false, true, set0, set1Edges, allEdges );

					foreach ( Edge edge in allEdges )
					{
						edge.DoubleSided = true;
					}
					allEdges.AddRange( set0Edges );
					break;

				case Operation.Intersection :
					Clip( false, true, set0, set1Edges, allEdges );
					Clip( false, true, set1, set0Edges, allEdges );
					break;

				case Operation.Complement :
					Clip( false, true, set0, set1Edges, allEdges );
					foreach ( Edge edge in allEdges )
					{
						Point2 oldP0 = new Point2( edge.P0 );
						edge.P0 = edge.P1;
						edge.P1 = oldP0;
						edge.Plane.Invert( );
					}
					Clip( true, false, set1, set0Edges, allEdges );
					break;
			}

			return Build( null, allEdges );
		}
		
		/// <summary>
		/// Flattens a BSP tree into a list of edges
		/// </summary>
		private static IList< Edge > Flatten( BspNode root )
		{
			List< Edge > edges = new List< Edge >( );
			Flatten( root, edges );
			return edges;
		}

		/// <summary>
		/// Flattens a BSP tree into a list of edges
		/// </summary>
		private static void Flatten( BspNode root, IList< Edge > edges )
		{
			edges.Add( root.Edge );
			if ( root.Behind != null )
			{
				Flatten( root.Behind, edges );
			}
			if ( root.InFront != null )
			{
				Flatten( root.InFront, edges );
			}
		}
		
		/// <summary>
		/// Clips a set of edges against a BSP tree
		/// </summary>
		private static void Clip( bool keepLeftEdges, bool keepRightEdges, BspNode node, IEnumerable< Edge > edges, IList< Edge > outputEdges )
		{
			IList< Edge > leftEdges = ( node.Behind != null ) ? new List< Edge >( ) : ( keepLeftEdges ? outputEdges : null );
			IList< Edge > rightEdges = ( node.InFront != null ) ? new List< Edge >( ) : ( keepRightEdges ? outputEdges : null );

			foreach ( Edge edge in edges )
			{
				ClassifyEdge( node.Edge, edge, leftEdges, rightEdges );
			}
			if ( node.Behind != null )
			{
				Clip( keepLeftEdges, keepRightEdges, node.Behind, leftEdges, outputEdges );
			}
			if ( node.InFront != null )
			{
				Clip( keepLeftEdges, keepRightEdges, node.InFront, rightEdges, outputEdges );
			}
		}
		
		/// <summary>
		/// Builds a BSP tree from a CSG brush
		/// </summary>
		/// <param name="brush">CSG brush</param>
		/// <returns>Returns the root node of the BSP tree that represents brush</returns>
		private static BspNode Build( CsgBrush brush )
		{
			Point2[] points = brush.Points;

			List< Edge > edges = new List< Edge >( points.Length );

			//	Create edges
			for ( int ptIndex = 0; ptIndex < points.Length; ++ptIndex )
			{
				edges.Add( new Edge( points[ ptIndex ], points[ ( ptIndex + 1 ) % points.Length ] ) );
			}

			return Build( null, edges );
		}

		/// <summary>
		/// Builds convex regions from a given root node
		/// </summary>
		/// <param name="node">Root node</param>
		private static void BuildConvexRegions( BspNode node )
		{
			List< Plane2 > planes = new List< Plane2 >( );
			BuildConvexRegions( node, planes );
		}
		
		/// <summary>
		/// Builds convex regions from a given node
		/// </summary>
		/// <param name="node">Current node</param>
		/// <param name="planes">Current plane set</param>
		private static void BuildConvexRegions( BspNode node, List< Plane2 > planes )
		{
			if ( node.Behind != null )
			{
				List< Plane2 > behindPlanes = new List< Plane2 >( planes );

				behindPlanes.Add( node.Plane.MakeInversePlane( ) );
				BuildConvexRegions( node.Behind, behindPlanes );
			}
			planes.Add( node.Plane );
			if ( node.InFront != null )
			{
				BuildConvexRegions( node.InFront, planes );
			}
			else
			{
				try
				{
					node.ConvexRegion = BuildConvexRegion( planes );
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Builds a convex region from a set of planes
		/// </summary>
		/// <param name="allPlanes">Plane set</param>
		/// <returns>Returns the points of the convex polygon made up from the intersection of the planes in allPlanes</returns>
		private static Point2[] BuildConvexRegion( IList< Plane2 > allPlanes )
		{
			List< Point2 > points = new List< Point2 >( );
			IList< Plane2 > planes = new List< Plane2 >( allPlanes );
			Plane2 curPlane = allPlanes[ allPlanes.Count - 1 ];
			while ( planes.Remove( curPlane ) )
			{
				Vector2 vec = curPlane.Normal.MakePerp( );
				Plane2 nextPlane = null;
				Point2 nextPt = Point2.Origin;
				for ( int planeIndex = 0; planeIndex < allPlanes.Count; ++planeIndex )
				{
					Plane2 testPlane = allPlanes[ planeIndex ];
					if ( vec.Dot( testPlane.Normal ) > -0.01f )
					{
						continue;
					}
					Point2 intersection = Intersections2.GetPlanePlaneIntersection( curPlane, testPlane ).Value;
					if ( ( nextPlane == null ) || ( nextPlane.ClassifyPoint( intersection, 0.0001f ) == PlaneClassification.InFront ) )
					{
						nextPlane = testPlane;
						nextPt = intersection;
					}
				}
				points.Add( nextPt );
				curPlane = nextPlane;

#if DEBUG
				if ( curPlane == null )
				{
					throw new InvalidOperationException( "Current plane should never be null" );
				}
#endif
			}

			return points.ToArray( );
		}

		/// <summary>
		/// Builds a BSP node from a list of edges
		/// </summary>
		private static BspNode Build( BspNode parentNode, IList< Edge > edges )
		{
			if ( edges.Count == 0 )
			{
				return null;
			}

			Edge splitter = edges[ 0 ]; // TODO: AP: Choose better splitter
			BspNode splitNode = new BspNode( parentNode, splitter );
			if ( edges.Count == 1 )
			{
				return splitNode;
			}

			//	Classify edges as left or right edges
			List< Edge > leftEdges = new List< Edge >( );
			List< Edge > rightEdges = new List< Edge >( );
			for ( int edgeIndex = 1; edgeIndex < edges.Count; ++edgeIndex )
			{
				ClassifyEdge( splitter, edges[ edgeIndex ], leftEdges, rightEdges );
			}

			if ( splitter.DoubleSided )
			{
				leftEdges.Add( splitter.MakeFlippedEdge( ) );
			}

			//	Build BSP subtrees from left and right edges
			if ( leftEdges.Count > 0 )
			{
				splitNode.Behind = Build( splitNode, leftEdges );
			}
			if ( rightEdges.Count > 0 )
			{
				splitNode.InFront = Build( splitNode, rightEdges );
			}

			return splitNode;
		}
		
		/// <summary>
		/// Determines the classification of an edge with respect to a plane, given the classification of the edge's endpoints
		/// </summary>
		private static bool GetEdgeClassification( PlaneClassification p0Class, PlaneClassification p1Class, out PlaneClassification edgeClass )
		{
			if ( p0Class == p1Class )
			{
				edgeClass = p0Class;
				return true;
			}
			if ( p0Class == PlaneClassification.On )
			{
				edgeClass = p1Class;
				return true;
			}
			if ( p1Class == PlaneClassification.On )
			{
				edgeClass = p0Class;
				return true;
			}
			edgeClass = PlaneClassification.On;
			return false;
		}

		/// <summary>
		/// Classifies an edge to a splitter plane. If edge is totally behind the plane, it's added to leftEdges. If it's totally
		/// infront of the plane, it's added to rightEdges. Otherwise, the edge is split on the edge/plane intersection, and the
		/// 2 resulting lines are added to leftEdges and rightEdges
		/// </summary>
		private static void ClassifyEdge( Edge splitterEdge, Edge edge, ICollection< Edge > leftEdges, ICollection< Edge > rightEdges )
		{
			Plane2 splitter = splitterEdge.Plane;

			PlaneClassification p0Class = splitter.ClassifyPoint( edge.P0, OnPlaneTolerance );
			PlaneClassification p1Class = splitter.ClassifyPoint( edge.P1, OnPlaneTolerance );

			PlaneClassification edgeClass;

			if ( GetEdgeClassification( p0Class, p1Class, out edgeClass ) )
			{
				if ( edgeClass == PlaneClassification.Behind )
				{
					if ( leftEdges != null )
					{
						leftEdges.Add( edge );
					}
				}
				else
				{
					if ( rightEdges != null )
					{
						rightEdges.Add( edge );
					}
				}
			}
			else
			{
				Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( edge.P0, edge.P1, splitter );

				//	Create subdivided edges
				Edge startToMidEdge = new Edge( edge.P0, intersection.IntersectionPosition );
				Edge midToEndEdge = new Edge( intersection.IntersectionPosition, edge.P1 );

				//	Add new edges to the appropriate edge lists
				if ( p0Class != PlaneClassification.Behind )
				{
					Utils.Swap( ref leftEdges, ref rightEdges );
				}
				if ( leftEdges != null )
				{
					leftEdges.Add( startToMidEdge );
					startToMidEdge.DoubleSided = edge.DoubleSided;
				}

				if ( rightEdges != null )
				{
					rightEdges.Add( midToEndEdge );
					midToEndEdge.DoubleSided = edge.DoubleSided;
				}
			}
		}

		#endregion
	}
}
