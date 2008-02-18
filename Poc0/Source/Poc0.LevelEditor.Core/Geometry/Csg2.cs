using System.Collections.Generic;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Handles level geometry CSG operations
	/// </summary>
	internal class Csg2
	{

		//	TODO: AP: Bucket vertices, edges, and polygons into loose quadtree
		//	TODO: AP: Add vertex merging

		#region Public members

		/// <summary>
		/// Builds a BSP tree for a set of level polygons
		/// </summary>
		public static Node Build( IEnumerable< LevelPolygon > polygons )
		{
			Node rootNode = null;
			IList< Edge > allEdges = new List<Edge>( );
			IList< Edge > edges = new List<Edge>( );
			foreach ( LevelPolygon polygon in polygons )
			{
				if ( rootNode == null )
				{
					AddEdges( polygon.Edges, allEdges );
					rootNode = BuildNode( allEdges );
				}
				else
				{
					IList< Edge > tempEdges = new List< Edge >( );

					//	TODO: AP: Could cache edge list and BSP node in level polygon
					edges.Clear( );
					AddEdges( polygon.Edges, edges );
					Node polyNode = BuildNode( edges );

					ClipEdges( rootNode, edges, true, false, tempEdges );
					ClipEdges( polyNode, allEdges, true, false, tempEdges );

					rootNode = BuildNode( tempEdges );
					allEdges = tempEdges;
				}
				
			}

			return rootNode;
		}

		/// <summary>
		/// Combines two polygons together using a CSG operator. Returns a BSP tree for the result polygon
		/// </summary>
		public static Node Combine( LevelPolygon poly1, LevelPolygon poly2, Operation op )
		{
			IList< Edge > poly1Edges = BuildPolygonEdgeList( poly1 );
			IList< Edge > poly2Edges = BuildPolygonEdgeList( poly2 );

			Node poly1Root = BuildNode( poly1Edges );
			Node poly2Root = BuildNode( poly2Edges );

			List< Edge > sourceEdges = new List< Edge >( );

			switch ( op )
			{
				case Operation.Union :
				{
					ClipEdges( poly1Root, poly2Edges, true, false, sourceEdges );
					ClipEdges( poly2Root, poly1Edges, true, false, sourceEdges );
					break;
				}
				case Operation.Intersection :
				{
					ClipEdges( poly1Root, poly2Edges, false, true, sourceEdges );
					ClipEdges( poly2Root, poly1Edges, false, true, sourceEdges );
					break;
				}
				case Operation.Complement :
				{
					ClipEdges( poly1Root, poly2Edges, false, true, sourceEdges );
					ClipEdges( poly2Root, poly1Edges, false, true, sourceEdges );
					break;
				}
			}

			return BuildNode( sourceEdges );
		}


		#endregion

		#region Public types

		/// <summary>
		/// CSG operations
		/// </summary>
		public enum Operation
		{
			Union,
			Intersection,
			Complement
		}

		/// <summary>
		/// An edge
		/// </summary>
		public class Edge
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public Edge( LevelEdge srcEdge )
			{
				m_Start = srcEdge.Start.Position;
				m_End = srcEdge.End.Position;
				m_IsDoubleSided = srcEdge.IsDoubleSided;
				m_SourceEdge = srcEdge;
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="start">Start point of the edge</param>
			/// <param name="end">End point of the edge</param>
			/// <param name="isDoubleSided">true if this edge is double-sided</param>
			/// <param name="srcEdge">Source edge</param>
			public Edge( Point2 start, Point2 end, bool isDoubleSided, LevelEdge srcEdge )
			{
				m_Start = start;
				m_End = end;
				m_IsDoubleSided = isDoubleSided;
				m_SourceEdge = srcEdge;
			}

			/// <summary>
			/// Gets the source edge
			/// </summary>
			public LevelEdge SourceEdge
			{
				get { return m_SourceEdge; }
			}

			/// <summary>
			/// Get the start point of the edge
			/// </summary>
			public Point2 Start
			{
				get { return m_Start; }
			}

			/// <summary>
			/// Get the end point of the edge
			/// </summary>
			public Point2 End
			{
				get { return m_End; }
			}

			/// <summary>
			/// Returns true if this edge is double sided
			/// </summary>
			public bool IsDoubleSided
			{
				get { return m_IsDoubleSided; }
			}

			private readonly Point2		m_Start;
			private readonly Point2		m_End;
			private readonly bool		m_IsDoubleSided;
			private readonly LevelEdge	m_SourceEdge;
		}

		/// <summary>
		/// A BSP node
		/// </summary>
		public class Node
		{
			public Node( Edge edge )
			{
				m_Edge = edge;
				m_Plane = new Plane2( edge.Start, edge.End );
			}

			public bool IsDoubleSided
			{
				get { return m_Edge.IsDoubleSided; }
			}

			public Node Behind
			{
				get { return m_Behind; }
				set { m_Behind = value; }
			}

			public Node InFront
			{
				get { return m_InFront; }
				set { m_InFront = value; }
			}

			public Edge Edge
			{
				get { return m_Edge; }
			}

			public Plane2 Plane
			{
				get { return m_Plane; }
			}

			private readonly Edge	m_Edge;
			private readonly Plane2	m_Plane;
			private Node			m_Behind;
			private Node			m_InFront;
		}

		#endregion

		#region Private members

		/// <summary>
		/// Tolerance value. Points are determined to be on a plane if they are within this distance of them
		/// </summary>
		private const float OnPlaneTolerance = 0.01f;

		/// <summary>
		/// Clips a set of edges against a BSP node
		/// </summary>
		private static void ClipEdges( Node node, IEnumerable< Edge > edges, bool keepOutsideEdges, bool keepInsideEdges, IList< Edge > outputEdges )
		{
			if ( node == null )
			{
				return;
			}

			IList< Edge > outsideEdges = ( node.Behind != null ) ? new List< Edge >( ) : ( keepOutsideEdges ? outputEdges : null );
			IList< Edge > insideEdges = ( node.InFront != null ) ? new List< Edge >( ) : ( keepInsideEdges ? outputEdges : null );
			
			foreach ( Edge edge in edges )
			{
				ClassifyEdge( node.Plane, edge, insideEdges, outsideEdges );
			}
	
			ClipEdges( node.Behind, outsideEdges, keepOutsideEdges, keepInsideEdges, outputEdges );
			ClipEdges( node.InFront, insideEdges, keepOutsideEdges, keepInsideEdges, outputEdges );
		}

		/// <summary>
		/// Builds a BSP tree from a set of edges
		/// </summary>
		private static Node BuildNode( IList< Edge > edges )
		{
			if ( edges.Count == 0 )
			{
				return null;
			}

			Node node = new Node( edges[ 0 ] );
			
			List< Edge > inFrontEdges = new List< Edge >( );
			List< Edge > behindEdges = new List< Edge >( );

			for ( int edgeIndex = 1; edgeIndex < edges.Count; ++edgeIndex )
			{
				ClassifyEdge( node.Plane, edges[ edgeIndex ], inFrontEdges, behindEdges );
			}

			node.Behind = BuildNode( behindEdges );
			node.InFront = BuildNode( inFrontEdges );

			return node;
		}

		/// <summary>
		/// Classifies an edge w.r.t a plane, adding it to inFrontEdges or behindEdges. If the plane intersects the edge, it's split,
		/// and the two resulting edges are added to inFrontEdges or behindEdges instead
		/// </summary>
		private static void ClassifyEdge( Plane2 plane, Edge edge, ICollection< Edge > inFrontEdges, ICollection< Edge > behindEdges )
		{
			PlaneClassification startClass = plane.ClassifyPoint( edge.Start, OnPlaneTolerance );
			PlaneClassification endClass = plane.ClassifyPoint( edge.End, OnPlaneTolerance );

			if ( startClass == PlaneClassification.On )
			{
				startClass = endClass;
			}
			if ( endClass == PlaneClassification.On )
			{
				endClass = startClass;
			}
			if ( startClass == endClass )
			{
				//	Edge endpoints on the same side of the splitter plane - no split required
				if ( startClass == PlaneClassification.Behind )
				{
					AddEdge( behindEdges, edge );
				}
				else if ( startClass == PlaneClassification.InFront )
				{
					AddEdge( inFrontEdges, edge );
				}
				//	Don't care about edges that are on the splitter plane
				return;
			}
			Point2 midPt = Intersections2.GetLinePlaneIntersection( edge.Start, edge.End, plane ).IntersectionPosition;

			Edge startToMid = new Edge( edge.Start, midPt, edge.IsDoubleSided, edge.SourceEdge );
			Edge midToEnd = new Edge( midPt, edge.End, edge.IsDoubleSided, edge.SourceEdge );

			if ( startClass == PlaneClassification.Behind )
			{
				AddEdge( behindEdges, startToMid );
				AddEdge( inFrontEdges, midToEnd );
			}
			else
			{
				AddEdge( inFrontEdges, startToMid );
				AddEdge( behindEdges, midToEnd );
			}
		}

		/// <summary>
		/// Adds an edge to an edge collection (if the edge collection is not null)
		/// </summary>
		private static void AddEdge( ICollection< Edge > edges, Edge edge )
		{
			if ( edges != null )
			{
				edges.Add( edge );
			}
		}
		
		/// <summary>
		/// Addges edges from a LevelEdge list to an Edge list
		/// </summary>
		private static void AddEdges( IEnumerable< LevelEdge > sourceEdges, ICollection< Edge > edges )
		{
			foreach ( LevelEdge sourceEdge in sourceEdges )
			{
				edges.Add( new Edge( sourceEdge ) );
			}
		}

		/// <summary>
		/// Builds a list of Edge objects from a LevelPolygon
		/// </summary>
		private static IList< Edge > BuildPolygonEdgeList( LevelPolygon poly )
		{
			List< Edge > edges = new List< Edge >( poly.Edges.Length );

			for ( int edgeIndex = 0; edgeIndex < edges.Count; ++edgeIndex )
			{
				LevelEdge srcEdge = poly.Edges[ edgeIndex ];
				edges[ edgeIndex ] = new Edge( srcEdge );
			}

			return edges;
		}

		#endregion
	}

}
