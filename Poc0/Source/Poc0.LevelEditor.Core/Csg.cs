using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tesselator;
using Rb.Tools.LevelEditor.Core.Selection;

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
		/// <exception cref="InvalidOperationException">Thrown by convex region builder if BSP tree is internally invalid</exception>
		public void Combine( Operation op, CsgBrush brush )
		{
			List< Edge > brushEdges;
			BspNode brushBsp = Build( brush, out brushEdges );
			BspNode newRoot = null;
			List< Edge > allEdges = null;
			if ( m_Root == null )
			{
				if ( op == Operation.Union || op == Operation.EdgeUnion )
				{
					newRoot = brushBsp;
					allEdges = brushEdges;
				}
			}
			else
			{
				newRoot = Combine( op, m_Root, brushBsp, out allEdges );
			}
			if ( newRoot != null )
			{
				BuildConvexRegions2( new List<Edge>( allEdges ) );
				FixUpDoubleSidedNodes( newRoot );
			//	BuildConvexRegions( newRoot );
				m_Root = newRoot;
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
		public class BspNode : ISelectable
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

				m_Quad[ 0 ] = new Point3( edge.P0.X, 0, edge.P0.Y );
				m_Quad[ 1 ] = new Point3( edge.P1.X, 0, edge.P1.Y );
				m_Quad[ 2 ] = new Point3( edge.P1.X, 5, edge.P1.Y );
				m_Quad[ 3 ] = new Point3( edge.P0.X, 5, edge.P0.Y );
			}

			/// <summary>
			/// The node plane
			/// </summary>
			[Browsable( false )]
			public Plane2 Plane
			{
				get { return Edge.Plane; }
			}

			/// <summary>
			/// The subtree of nodes behind this node's plane
			/// </summary>
			[Browsable( false )]
			public BspNode Behind
			{
				get { return m_Behind; }
				set { m_Behind = value; }
			}

			/// <summary>
			/// The subtree of nodes in front of this node's plane
			/// </summary>
			[Browsable( false )]
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
			[ReadOnly(true)]
			public Point2[] ConvexRegion
			{
				get { return m_Region; }
				set
				{
					m_Region = value;
					m_FloorData = StaticGeometryData.CreateDefaultFloorData( );
				}
			}

			/// <summary>
			/// Gets/sets floor data at this node
			/// </summary>
			public StaticGeometryData FloorData
			{
				get { return m_FloorData; }
				set { m_FloorData = value; }
			}

			/// <summary>
			/// Gets the parent node
			/// </summary>
			[Browsable(false)]
			public BspNode Parent
			{
				get { return m_Parent; }
			}

			/// <summary>
			/// Returns this node's wall quad
			/// </summary>
			[Browsable(false)]
			public Point3[] Quad
			{
				get { return m_Quad; }
			}

			#region ISelectable Members

			/// <summary>
			/// Highlight flag
			/// </summary>
			[Browsable(false)]
			public bool Highlighted
			{
				get { return m_Highlight; }
				set { m_Highlight = value; }
			}

			/// <summary>
			/// Selection flag
			/// </summary>
			[Browsable(false)]
			public bool Selected
			{
				get { return m_Selected; }
				set { m_Selected = value; }
			}

			#endregion

			#region Private stuff

			private StaticGeometryData m_FloorData;
			private readonly Point3[] m_Quad = new Point3[ 4 ];
			private readonly BspNode m_Parent;
			private BspNode m_Behind;
			private BspNode m_InFront;
			private readonly Edge m_Edge;
			private Point2[] m_Region;
			private bool m_Highlight;
			private bool m_Selected;

			#endregion

		}
		
		/// <summary>
		/// A line segment with a plane
		/// </summary>
		[Serializable]
		public class Edge
		{

			/// <summary>
			/// Gets data associated with the wall at the BSP node
			/// </summary>
			public StaticGeometryData WallData
			{
				get { return m_WallData; }
				set { m_WallData = value; }
			}

			/// <summary>
			/// Gets the temporary flag - temporary edges are created to allow double-sided edges
			/// </summary>
			public bool Temporary
			{
				get { return m_Temporary; }
			}

			/// <summary>
			/// Creates a flipped version of this edge, with its temporary flag set to true
			/// </summary>
			public Edge MakeTemporaryFlippedEdge( )
			{
				Edge edge = new Edge( new Point2( P1 ), new Point2( P0 ) );
				edge.m_Temporary = true;
				edge.m_WallData = m_WallData;
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
			/// Gets/sets the index of this edge in the edge list
			/// </summary>
			public int Index
			{
				get { return m_Index; }
				set { m_Index = value; }
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

			public static void LinkEdges( Edge edge0, Edge edge1 )
			{
				edge0.NextEdge = edge1;
				edge1.PreviousEdge = edge0;
			}

			public Edge PreviousEdge
			{
				get { return m_Prev; }
				set { m_Prev = value; }
			}

			public Edge NextEdge
			{
				get { return m_Next; }
				set { m_Next = value; }
			}

			public bool IsInternal
			{
				get { return m_Internal; }
				set { m_Internal = value; }
			}

			private int m_Index = -1;
			private Edge m_Next;
			private Edge m_Prev;
			private bool m_Internal;
			private bool m_Temporary;
			private bool m_DoubleSided;
			private Point2 m_P0;
			private Point2 m_P1;
			private readonly Plane2 m_Plane;
			private StaticGeometryData m_WallData = StaticGeometryData.CreateDefaultWallData( );
		}

		#endregion

		#region BSP building

		private BspNode m_Root;

		/// <summary>
		/// A point is considered to be on the plane (PlaneClassification.On) if it's within this distance of the plane
		/// </summary>
		private const float OnPlaneTolerance = 0.01f;

		/// <summary>
		/// Sets the index properties of all edges in an edge list
		/// </summary>
		private static void IndexEdges( IList< Edge > edges )
		{
			//	Set the indices of all edges
			for ( int edgeIndex = 0; edgeIndex < edges.Count; ++edgeIndex )
			{
				edges[ edgeIndex ].Index = edgeIndex;
			}
		}

		/// <summary>
		/// Combines two BSP trees using a specified CSG operation
		/// </summary>
		private static BspNode Combine( Operation op, BspNode set0, BspNode set1, out List< Edge > allEdges )
		{
			IList< Edge > set0Edges = Flatten( set0 );
			IList< Edge > set1Edges = Flatten( set1 );

			List< Edge > sourceEdges = new List<Edge>( );

			switch ( op )
			{
				case Operation.Union :
					Clip( true, false, set0, set1Edges, sourceEdges );
					Clip( true, false, set1, set0Edges, sourceEdges );
					break;

				case Operation.EdgeUnion :
					Clip( false, true, set0, set1Edges, sourceEdges );

					foreach ( Edge edge in sourceEdges )
					{
						edge.DoubleSided = true;
					}
					sourceEdges.AddRange( set0Edges );
					break;

				case Operation.Intersection :
					Clip( false, true, set0, set1Edges, sourceEdges );
					Clip( false, true, set1, set0Edges, sourceEdges );
					break;

				case Operation.Complement :
					Clip( false, true, set0, set1Edges, sourceEdges );
					foreach ( Edge edge in sourceEdges )
					{
						Point2 oldP0 = new Point2( edge.P0 );
						edge.P0 = edge.P1;
						edge.P1 = oldP0;
						edge.Plane.Invert( );
					}
					Clip( true, false, set1, set0Edges, sourceEdges );
					break;
			}

			sourceEdges = MergeEdges( sourceEdges );
			allEdges = new List< Edge >( );
			BspNode rootNode = BuildNode( null, sourceEdges, allEdges );

			IndexEdges( allEdges );

			return rootNode;
		}

		private static bool Equivalent( Point2 p0, Point2 p1 )
		{
			return p0.SqrDistanceTo( p1 ) <= 0.1f ;
		}

		/// <summary>
		/// Merges colinear edges that share points
		/// </summary>
		private static List< Edge > MergeEdges( IList< Edge > srcEdges )
		{
			int initialEdgeCount = srcEdges.Count;
			List< Edge > mergedEdges = new List< Edge >( );

			while ( srcEdges.Count > 0 )
			{
				Edge srcEdge = srcEdges[ 0 ];
				srcEdges.RemoveAt( 0 );
				
				for ( int mergeIndex = 0; mergeIndex < srcEdges.Count; )
				{
					Edge mergeEdge = srcEdges[ mergeIndex ];
					if ( ( mergeEdge.Plane.Normal.Dot( srcEdge.Plane.Normal ) >= 0.9f ) &&
					     ( Utils.CloseToZero( mergeEdge.Plane.Distance - srcEdge.Plane.Distance, 0.1f ) ) )
					{
						//	Edges are colinear - do they share a common vertex?
						if ( Equivalent( mergeEdge.P0, srcEdge.P0 ) )
						{
							srcEdge.P0 = mergeEdge.P1;
							srcEdges.RemoveAt( mergeIndex );
						}
						else if ( Equivalent( mergeEdge.P1, srcEdge.P0 ) )
						{
							srcEdge.P0 = mergeEdge.P0;
							srcEdges.RemoveAt( mergeIndex );
						}
						else if ( Equivalent( mergeEdge.P0, srcEdge.P1 ) )
						{
							srcEdge.P1 = mergeEdge.P1;
							srcEdges.RemoveAt( mergeIndex );
						}
						else if ( Equivalent( mergeEdge.P1, srcEdge.P1 ) )
						{
							srcEdge.P1 = mergeEdge.P0;
							srcEdges.RemoveAt( mergeIndex );
						}
						else
						{
							++mergeIndex;
						}
					}
					else
					{
						++mergeIndex;
					}
				}

				mergedEdges.Add( srcEdge );
			}

			GraphicsLog.Verbose( "Removed {0} edges (was {1}, is {2})", initialEdgeCount - mergedEdges.Count, initialEdgeCount, mergedEdges.Count );

			return mergedEdges;
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
			if ( !root.Edge.Temporary )
			{
				edges.Add( root.Edge );
			}
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
		private static void Clip( bool keepOutsideEdges, bool keepInsideEdges, BspNode node, IEnumerable< Edge > edges, IList< Edge > outputEdges )
		{
			IList< Edge > outsideEdges = ( node.Behind != null ) ? new List< Edge >( ) : ( keepOutsideEdges ? outputEdges : null );
			IList< Edge > insideEdges = ( node.InFront != null ) ? new List< Edge >( ) : ( keepInsideEdges ? outputEdges : null );

			foreach ( Edge edge in edges )
			{
				ClassifyEdge( node.Edge, edge, insideEdges, outsideEdges );
			}
			if ( node.Behind != null )
			{
				Clip( keepOutsideEdges, keepInsideEdges, node.Behind, outsideEdges, outputEdges );
			}
			if ( node.InFront != null )
			{
				Clip( keepOutsideEdges, keepInsideEdges, node.InFront, insideEdges, outputEdges );
			}
		}
		
		/// <summary>
		/// Builds a BSP tree from a CSG brush
		/// </summary>
		private static BspNode Build( CsgBrush brush, out List< Edge > outputEdges )
		{
			Point2[] points = brush.Points;

			//	Create edges
			List< Edge > sourceEdges = new List< Edge >( points.Length );
			for ( int ptIndex = 0; ptIndex < points.Length; ++ptIndex )
			{
				Edge edge = new Edge( points[ ptIndex ], points[ ( ptIndex + 1 ) % points.Length ] );
				sourceEdges.Add( edge );
				if ( ptIndex > 0 )
				{
					Edge.LinkEdges( sourceEdges[ ptIndex - 1 ], sourceEdges[ ptIndex ] );
				}
			}

			//	Link up the final edge to the first edge
			Edge.LinkEdges( sourceEdges[ sourceEdges.Count - 1 ], sourceEdges[ 0 ] );

			//	Build the BSP tree from the edge list
			outputEdges = new List< Edge >( );
			BspNode node = BuildNode( null, sourceEdges, outputEdges );

			//	Index all the edges in the edge list
			IndexEdges( outputEdges );

			return node;
		}

		private void FixUpDoubleSidedNodes( BspNode node )
		{
			if ( node == null )
			{
				return;
			}
			FixUpDoubleSidedNodes( node.Behind );
			FixUpDoubleSidedNodes( node.InFront );

			if ( ( node.Edge.DoubleSided ) && ( node.Behind == null ) )
			{
				node.Behind = new BspNode( node, node.Edge.MakeTemporaryFlippedEdge( ) );
			}
		}

		private void BuildConvexRegions2( IList< Edge > edges )
		{
			TesselatorInput input = new TesselatorInput( );

			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = float.MinValue;
			float maxY = float.MinValue;

			foreach ( Edge edge in edges )
			{
				minX = edge.P0.X < minX ? edge.P0.X : minX;
				minY = edge.P0.Y < minY ? edge.P0.Y : minY;
				maxX = edge.P0.X > maxX ? edge.P0.X : maxX;
				maxY = edge.P0.Y > maxY ? edge.P0.Y : maxY;
			}
			float padding = 1;
			minX -= padding;
			minY -= padding;
			maxX += padding;
			maxY += padding;

			input.Polygon = new Point2[]
				{
					new Point2( minX, minY ),
					new Point2( maxX, minY ),
					new Point2( maxX, maxY ),
					new Point2( minX, maxY ), 
				};

			bool[] isProcessed = new bool[ edges.Count ];
			int firstEdgeIndex = 0;

			while ( firstEdgeIndex < edges.Count )
			{
				//	Each iteration begins with an unprocessed edge, then runs through the edge loop,
				//	adding points to the current contour until it returns to the initial edge.

				List< Point2 > points = new List< Point2 >( );
				Edge edge = edges[ firstEdgeIndex ];
				bool internalEdges = edge.IsInternal;

				do
				{
					isProcessed[ edge.Index ] = true;
					points.Add( edge.P0 );
					edge = edge.NextEdge;
				}
				while ( edge.Index != firstEdgeIndex );

				//if ( !internalEdges )
				//{
				//    if ( input.Polygon != null )
				//    {
				//        throw new InvalidOperationException( "Tried to generate more than one boundary during tesselation" );
				//    }
				//    input.Polygon = points;
				//}
				//else
				{
					input.AddContour( points );
				}

				//	Find the first unprocessed edge for the next iteration
				for ( ; firstEdgeIndex < edges.Count; ++firstEdgeIndex )
				{
					if ( !isProcessed[ firstEdgeIndex ] )
					{
						break;
					}
				}
			}

			m_PolyLists = Tesselator.Tesselate( input );
		}

		public Tesselator.PolygonLists m_PolyLists;

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
				node.ConvexRegion = BuildConvexRegion( planes );
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
		private static BspNode BuildNode( BspNode parentNode, IList< Edge > edges, IList< Edge > outputEdges )
		{
			if ( edges.Count == 0 )
			{
				return null;
			}

			Edge splitter = edges[ 0 ]; // TODO: AP: Choose better splitter
			BspNode splitNode = new BspNode( parentNode, splitter );

			outputEdges.Add( splitter );

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

			//	Build BSP subtrees from left and right edges
			if ( leftEdges.Count > 0 )
			{
				splitNode.Behind = BuildNode( splitNode, leftEdges, outputEdges );
			}
			if ( rightEdges.Count > 0 )
			{
				splitNode.InFront = BuildNode( splitNode, rightEdges, outputEdges );
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
		private static void ClassifyEdge( Edge splitterEdge, Edge edge, ICollection< Edge > outsideEdges, ICollection< Edge > insideEdges )
		{
			Plane2 splitter = splitterEdge.Plane;

			//	TODO: AP: Expand point classification code. Results from classifications can be re-used later in intersection code
			PlaneClassification p0Class = splitter.ClassifyPoint( edge.P0, OnPlaneTolerance );
			PlaneClassification p1Class = splitter.ClassifyPoint( edge.P1, OnPlaneTolerance );

			PlaneClassification edgeClass;

			if ( GetEdgeClassification( p0Class, p1Class, out edgeClass ) )
			{
				if ( edgeClass == PlaneClassification.Behind )
				{
					if ( outsideEdges != null )
					{
						outsideEdges.Add( edge );
					}
				}
				else
				{
					if ( insideEdges != null )
					{
						insideEdges.Add( edge );
					}
				}
			}
			else
			{
				//	Calculate the intersection between edge and splitterEdge. If the intersection occurs inside
				//	splitterEdge, then we have to link splitterEdge to either the new start-to-mid or mid-to-end
				//	edge.

				Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( edge.P0, edge.P1, splitter );

				//	Create subdivided edges
				Edge startToMidEdge = new Edge( edge.P0, intersection.IntersectionPosition );
				Edge midToEndEdge = new Edge( intersection.IntersectionPosition, edge.P1 );

				//	Make sure both halves get the same wall data
				startToMidEdge.WallData = edge.WallData;
				midToEndEdge.WallData = edge.WallData;

				//	Link new edges into edge loop
				Edge.LinkEdges( edge.PreviousEdge, startToMidEdge );
				Edge.LinkEdges( startToMidEdge, midToEndEdge );
				Edge.LinkEdges( midToEndEdge, edge.NextEdge );

				//	Add new edges to the appropriate edge lists
				if ( p0Class != PlaneClassification.Behind )
				{
					Utils.Swap( ref outsideEdges, ref insideEdges );
				}
				if ( outsideEdges != null )
				{
					outsideEdges.Add( startToMidEdge );
					startToMidEdge.DoubleSided = edge.DoubleSided;
				}
				else
				{
					Edge.LinkEdges( edge.PreviousEdge, midToEndEdge );
				}

				if ( insideEdges != null )
				{
					insideEdges.Add( midToEndEdge );
					midToEndEdge.DoubleSided = edge.DoubleSided;
				}
				else
				{
					Edge.LinkEdges( startToMidEdge, edge.NextEdge );	
				}
			}
		}

		#endregion
	}
}
