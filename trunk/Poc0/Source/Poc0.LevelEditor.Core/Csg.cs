using System;
using System.Collections.Generic;
using System.ComponentModel;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Poc0.LevelEditor.Core
{
	/*
	 * New user interface:
	 *	- User adds collision geometry to level
	 *	- Geometry can be moved, rotated, and scaled
	 *	- Geometry vertices can be moved
	 *	- Geometry pieces can be merged together, and unmerged (prio 2)
	 *	- At build time, builder uses CSG ops to merge all geometry objects, to create AI map, graphics, etc.
	 */

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
		public void Combine( Operation op, UiPolygon brush )
		{
			BspNode brushBsp = Build( brush, op == Operation.Complement );
			BspNode newRoot = null;
			if ( m_Root == null )
			{
				if ( op == Operation.Union || op == Operation.EdgeUnion )
				{
					newRoot = brushBsp;
				}
			}
			else
			{
				newRoot = Combine( op, m_Root, brushBsp );
			}
			if ( newRoot != null )
			{
				FixUpDoubleSidedNodes( newRoot );
				BuildConvexRegions( newRoot );
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

		/// <summary>
		/// Gets the points indexed by the convex regions of BSP nodes
		/// </summary>
		public Point2[] Points
		{
			get { return m_Points; }
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
			public int[] ConvexRegion
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
			private int[] m_Region;
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
		private Point2[] m_Points;

		/// <summary>
		/// A point is considered to be on the plane (PlaneClassification.On) if it's within this distance of the plane
		/// </summary>
		private const float OnPlaneTolerance = 0.01f;

		/// <summary>
		/// Combines two BSP trees using a specified CSG operation
		/// </summary>
		private static BspNode Combine( Operation op, BspNode set0, BspNode set1 )
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
					Clip( false, true, set1, set0Edges, sourceEdges );
					break;
			}

			sourceEdges = MergeEdges( sourceEdges );
			BspNode rootNode = BuildNode( null, sourceEdges );

			return rootNode;
		}

		private static bool Equivalent( Point2 p0, Point2 p1 )
		{
			return p0.SqrDistanceTo( p1 ) <= 0.01f ;
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
					if ( ( mergeEdge.Plane.Normal.Dot( srcEdge.Plane.Normal ) >= 0.99f ) &&
					     ( Utils.CloseToZero( mergeEdge.Plane.Distance - srcEdge.Plane.Distance, 0.01f ) ) )
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
				ClassifyEdge( node.Edge, edge, outsideEdges, insideEdges );
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
		private static BspNode Build( UiPolygon brush, bool reverse )
		{
			Point2[] points = brush.Points;
			if ( reverse )
			{
				Array.Reverse( points );
			}

			//	Create edges
			List< Edge > sourceEdges = new List< Edge >( points.Length );
			for ( int ptIndex = 0; ptIndex < points.Length; ++ptIndex )
			{
				Edge edge = new Edge( points[ ptIndex ], points[ ( ptIndex + 1 ) % points.Length ] );
				sourceEdges.Add( edge );
			}

			//	Build the BSP tree from the edge list
			BspNode node = BuildNode( null, sourceEdges );

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

		/// <summary>
		/// Tesselator class. Builds convex regions for nodes
		/// </summary>
		private class Tesselator
		{
			/// <summary>
			/// Tesselator output polygon type
			/// </summary>
			public class Polygon
			{
				public Polygon( int[] indices )
				{
					m_Indices = indices;
				}

				public int[] Indices
				{
					get { return m_Indices; }
				}

				private readonly int[] m_Indices;
			}

			/// <summary>
			/// Creates an initial bounding polygon
			/// </summary>
			public Polygon CreateBoundingPolygon( float minX, float minY, float maxX, float maxY )
			{
				return new Polygon
				(
					new int[]
					{
						AddPoint( new Point2( minX, minY ) ),
						AddPoint( new Point2( maxX, minY ) ),
						AddPoint( new Point2( maxX, maxY ) ),
						AddPoint( new Point2( minX, maxY ) )
					}
				);
			}

			public List<Point2> Points
			{
				get { return m_Points; }
			}

			/// <summary>
			/// Splits a source polygon along a plane, returning the sub-poly behind the plane in "behind"
			/// and the sub-poly in-front of the plane in "inFront"
			/// </summary>
			public void Split( Plane2 plane, Polygon source, out Polygon behind, out Polygon inFront )
			{
				float tolerance = 0.001f;
				int firstDefiniteClassIndex = -1;
				PlaneClassification[] classifications = new PlaneClassification[ source.Indices.Length ];
				for ( int index = 0; index < source.Indices.Length; ++index )
				{
					classifications[ index ] = plane.ClassifyPoint( m_Points[ source.Indices[ index ] ], tolerance );
					if ( ( firstDefiniteClassIndex == -1 ) && ( classifications[ index ] != PlaneClassification.On ) )
					{
						firstDefiniteClassIndex = index;
					}
				}
				if ( firstDefiniteClassIndex > 1 )
				{
					//	The first two points are on the split plane. This means that we
					//	can easily classify the polygon as either behind or in front depending on the first
					//	definite class
					if ( classifications[ firstDefiniteClassIndex ] == PlaneClassification.Behind )
					{
						behind = new Polygon( source.Indices );
						inFront = null;
					}
					else
					{
						behind = null;
						inFront = new Polygon( source.Indices );
					}
					return;
				}

				int lastPtIndex = firstDefiniteClassIndex;
				int curPtIndex = ( lastPtIndex + 1 ) % source.Indices.Length;
				Point2 lastPt = m_Points[ source.Indices[ lastPtIndex ] ];
				PlaneClassification lastClass = classifications[ lastPtIndex ];

				List< int > behindPoints = new List< int >( source.Indices.Length + 2 );
				List< int > inFrontPoints = new List< int >( source.Indices.Length + 2 );

				for ( int count = 0; count < source.Indices.Length; ++count )
				{
					Point2 curPt = m_Points[ source.Indices[ curPtIndex ] ];
					PlaneClassification curClass = classifications[ curPtIndex ];
					if ( curClass == PlaneClassification.On )
					{
						curClass = lastClass;
					}

					if ( curClass != lastClass )
					{
						//	Split line on plane
						Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( lastPt, curPt, plane );
						int newPtIndex = AddPoint( intersection.IntersectionPosition );
						behindPoints.Add( newPtIndex );
						inFrontPoints.Add( newPtIndex );
					}

					if ( curClass == PlaneClassification.Behind )
					{
						behindPoints.Add( source.Indices[ curPtIndex ] );
					}
					else
					{
						inFrontPoints.Add( source.Indices[ curPtIndex ] );
					}

					lastClass = curClass;
					lastPt = curPt;

					curPtIndex = ( curPtIndex + 1 ) % source.Indices.Length;
				}

				behind = ( behindPoints.Count == 0 ) ? null : new Polygon( behindPoints.ToArray( ) );
				inFront = ( inFrontPoints.Count == 0 ) ? null : new Polygon( inFrontPoints.ToArray( ) );
			}

			#region Private members

			private readonly List< Point2 > m_Points = new List< Point2 >( );

			/// <summary>
			/// Adds a point to the point list and returns its index
			/// </summary>
			private int AddPoint( Point2 pt )
			{
				int index = m_Points.Count;
				m_Points.Add( pt );
				return index;
			}

			#endregion
		}

		
		/// <summary>
		/// Builds the convex region for a node and its children
		/// </summary>
		private static void BuildConvexRegions( BspNode node, Tesselator tess, Tesselator.Polygon poly )
		{
			Tesselator.Polygon behindPoly;
			Tesselator.Polygon inFrontPoly;
			tess.Split( node.Plane, poly, out behindPoly, out inFrontPoly );

			if ( node.Behind != null )
			{
				BuildConvexRegions( node.Behind, tess, behindPoly );
			}
			if ( node.InFront != null )
			{
				BuildConvexRegions( node.InFront, tess, inFrontPoly );
			}
			else
			{
				node.ConvexRegion = inFrontPoly.Indices;
			}
		}

		/// <summary>
		/// Builds the convex regions for a root node and all its children
		/// </summary>
		private void BuildConvexRegions( BspNode node )
		{
			//	Find the bounding box for the tree
			float[] bounds = new float[ 4 ] { float.MaxValue, float.MaxValue, float.MinValue, float.MinValue };
			GetNodeBoundingRectangle( node, bounds );

			//	Expand bounds a bit
			float boundary = 1.0f;
			bounds[ 0 ] -= boundary;
			bounds[ 1 ] -= boundary;
			bounds[ 2 ] += boundary;
			bounds[ 3 ] += boundary;
			
			float width = bounds[ 2 ] - bounds[ 0 ];
			float height = bounds[ 3 ] - bounds[ 1 ];

			Tesselator tess = new Tesselator( );
			Tesselator.Polygon boundingPoly = tess.CreateBoundingPolygon( bounds[ 0 ], bounds[ 1 ], width, height );

			BuildConvexRegions( node, tess, boundingPoly );

			m_Points = tess.Points.ToArray( );
		}

		/// <summary>
		/// Gets a bounding rectangle that encompasses a BSP node and its children
		/// </summary>
		private static void GetNodeBoundingRectangle( BspNode node, float[] rect )
		{
			if ( node == null )
			{
				return;
			}

			rect[ 0 ] 	= Utils.Min( rect[ 0 ], node.Edge.P0.X, node.Edge.P1.X );
			rect[ 1 ] 	= Utils.Min( rect[ 1 ], node.Edge.P0.Y, node.Edge.P1.Y );
			rect[ 2 ] 	= Utils.Max( rect[ 2 ], node.Edge.P0.X, node.Edge.P1.X );
			rect[ 3 ] 	= Utils.Max( rect[ 3 ], node.Edge.P0.Y, node.Edge.P1.Y );

			GetNodeBoundingRectangle( node.Behind, rect );
			GetNodeBoundingRectangle( node.InFront, rect );
		}

		/// <summary>
		/// Builds a BSP node from a list of edges
		/// </summary>
		private static BspNode BuildNode( BspNode parentNode, IList< Edge > edges )
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
			List< Edge > outsideEdges = new List< Edge >( );
			List< Edge > insideEdges = new List< Edge >( );
			for ( int edgeIndex = 1; edgeIndex < edges.Count; ++edgeIndex )
			{
				ClassifyEdge( splitter, edges[ edgeIndex ], outsideEdges, insideEdges );
			}

			//	Build BSP subtrees from left and right edges
			if ( outsideEdges.Count > 0 )
			{
				splitNode.Behind = BuildNode( splitNode, outsideEdges );
			}
			if ( insideEdges.Count > 0 )
			{
				splitNode.InFront = BuildNode( splitNode, insideEdges );
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
				if ( insideEdges != null )
				{
					insideEdges.Add( midToEndEdge );
					midToEndEdge.DoubleSided = edge.DoubleSided;
				}
			}
		}

		#endregion

	}
}
