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
					m_Contours = new Edge[] { brushBsp.Edge };
					m_Root = brushBsp;
				}
			}
			else
			{
				m_Root = Combine( op, m_Root, brushBsp );
			}
			BuildConvexRegions( m_Root );
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

		/// <summary>
		/// Gets the contours
		/// </summary>
		public Edge[] Contours
		{
			get { return m_Contours; }
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
			//private readonly int m_Index = EdgeCounter++;
			//private static int EdgeCounter = 0;

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
			/// The previous edge in the contour
			/// </summary>
			public Edge PreviousEdge
			{
				get { return m_Prev; }
				set { m_Prev = value; }
			}

			/// <summary>
			/// The next edge in the contour
			/// </summary>
			public Edge NextEdge
			{
				get { return m_Next; }
				set { m_Next = value; }
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

			/// <summary>
			/// Unlinks this edge from its next and previous edges
			/// </summary>
			public void Unlink( )
			{
				if ( m_Prev != null )
				{
					m_Prev.NextEdge = null;
				}
				if ( m_Next != null )
				{
					m_Next.PreviousEdge = null;
				}
			}


			private Edge m_Prev;
			private Edge m_Next;
			private Point2 m_P0;
			private Point2 m_P1;
			private readonly Plane2 m_Plane;
		}

		#endregion

		#region BSP building

		private BspNode m_Root;
		private Edge[] m_Contours;

		/// <summary>
		/// A point is considered to be on the plane (PlaneClassification.On) if it's within this distance of the plane
		/// </summary>
		private const float OnPlaneTolerance = 0.001f;

		/// <summary>
		/// Edges are considered coincident if the dot of their plane normals are less than this tolerance
		/// </summary>
		private const float CoincidentEdgeTolerance = 0.2f;

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
					Clip( false, set0, set1Edges, allEdges );
					Clip( false, set1, set0Edges, allEdges );
					break;

				case Operation.EdgeUnion :
					Clip( true, set0, set1Edges, allEdges );
					allEdges.AddRange( set0Edges );
					break;

				case Operation.Intersection :
					Clip( true, set0, set1Edges, allEdges );
					Clip( true, set1, set0Edges, allEdges );
					break;

				case Operation.Complement :
					Clip( true, set0, set1Edges, allEdges );
					foreach ( Edge edge in allEdges )
					{
						Point2 oldP0 = new Point2( edge.P0 );
						edge.P0 = edge.P1;
						edge.P1 = oldP0;
						edge.Plane.Invert( );
					}
					Clip( false, set1, set0Edges, allEdges );
					break;
			}

			//m_Contours = BuildContours( allEdges );

			return Build( null, allEdges );
		}

		/// <summary>
		/// Returns true if two edges are colinear
		/// </summary>
		private static bool AreEdgesColinear( Edge edge1, Edge edge2 )
		{
			return edge1.Plane.Normal.Dot( edge2.Plane.Normal ) > ( 1.0f - CoincidentEdgeTolerance );
		}

		/// <summary>
		/// Builds a list of contours from an edge list, and also combines an colinear edges
		/// </summary>
		private static Edge[] BuildContours( ICollection< Edge > edges )
		{
			List< Edge > processSet = new List< Edge >( edges );
			List< Edge > contours = new List< Edge >( );

			edges.Clear( );

			while ( processSet.Count > 0 )
			{
				Edge firstEdge = processSet[ 0 ];
				Edge curEdge = firstEdge;
				Edge addEdge = firstEdge;
				do
				{
					if ( AreEdgesColinear( addEdge, curEdge ) )
					{
					    addEdge.P1 = curEdge.P1;
					}
					else
					{
						curEdge.PreviousEdge = addEdge;
						addEdge.NextEdge = curEdge;

					    edges.Add( addEdge );
					    addEdge = curEdge;
					}

					processSet.Remove(curEdge);
					curEdge = curEdge.NextEdge;

				} while ( ( curEdge != null ) && ( curEdge != firstEdge ) );

				if ( addEdge != curEdge )
				{
					if ( curEdge != null )
					{
						curEdge.PreviousEdge = addEdge;
						addEdge.NextEdge = curEdge.NextEdge;
					}
					else
					{
						addEdge.NextEdge = null;
					}
				    edges.Add( addEdge );
				}

				contours.Add( firstEdge );
			}

			return contours.ToArray( );
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
		private static void Clip( bool keepInsideEdges, BspNode node, IEnumerable< Edge > edges, IList< Edge > outputEdges )
		{
			IList< Edge > leftEdges = ( node.Behind != null ) ? new List< Edge >( ) : ( keepInsideEdges ? null : outputEdges );
			IList< Edge > rightEdges = ( node.InFront != null ) ? new List< Edge >( ) : ( keepInsideEdges ? outputEdges : null );

			foreach ( Edge edge in edges )
			{
				ClassifyEdge( node.Plane, edge, leftEdges, rightEdges );
			}
			if ( node.Behind != null )
			{
				Clip( keepInsideEdges, node.Behind, leftEdges, outputEdges );
			}
			if ( node.InFront != null )
			{
				Clip( keepInsideEdges, node.InFront, rightEdges, outputEdges );
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

			//	Set up edge links
			int lastEdgeIndex = edges.Count - 1;
			int nextEdgeIndex = 1;
			for ( int edgeIndex = 0; edgeIndex < edges.Count; ++edgeIndex )
			{
				edges[ edgeIndex ].PreviousEdge = edges[ lastEdgeIndex ];
				edges[ edgeIndex ].NextEdge = edges[ nextEdgeIndex ];

				lastEdgeIndex = edgeIndex;
				nextEdgeIndex = ( nextEdgeIndex + 1 ) % edges.Count;
			}

			return Build( null, edges );
		}

		private static void BuildConvexRegions( BspNode node )
		{
			List< Plane2 > planes = new List< Plane2 >( );
			BuildConvexRegions( node, planes );
		}

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
				node.ConvexRegion = BuildConvexRegion( planes );
			}
		}

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
				//	Leaf node in the tree. Path from leaf to root is a convex hull
				//splitNode.ConvexRegion = BuildConvexRegion( splitNode );

				return splitNode;
			}

			//	Classify edges as left or right edges
			List< Edge > leftEdges = new List< Edge >( );
			List< Edge > rightEdges = new List< Edge >( );
			for ( int edgeIndex = 1; edgeIndex < edges.Count; ++edgeIndex )
			{
				ClassifyEdge( splitter.Plane, edges[ edgeIndex ], leftEdges, rightEdges );
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
		private static void ClassifyEdge( Plane2 splitter, Edge edge, ICollection< Edge > leftEdges, ICollection< Edge > rightEdges )
		{
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
					else
					{
						//	Edge is being removed - unlink from other edges
						edge.Unlink( );
					}
				}
				else
				{
					if ( rightEdges != null )
					{
						rightEdges.Add( edge );
					}
					else
					{
						//	Edge is being removed - unlink from other edges
						edge.Unlink( );
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
					if ( edge.PreviousEdge != null )
					{
						edge.PreviousEdge.NextEdge = startToMidEdge;
					}
					startToMidEdge.PreviousEdge = edge.PreviousEdge;
					midToEndEdge.PreviousEdge = startToMidEdge;
				}
				else if ( edge.PreviousEdge != null )
				{
					edge.PreviousEdge.NextEdge = null;
				}

				if ( rightEdges != null )
				{
					rightEdges.Add( midToEndEdge );
					if ( edge.NextEdge != null )
					{
						edge.NextEdge.PreviousEdge = midToEndEdge;
					}
					midToEndEdge.NextEdge = edge.NextEdge;
					startToMidEdge.NextEdge = midToEndEdge;
				}
				else if ( edge.NextEdge != null )
				{
					edge.NextEdge.PreviousEdge = null;
				}
			}
		}

		#endregion
	}
}
