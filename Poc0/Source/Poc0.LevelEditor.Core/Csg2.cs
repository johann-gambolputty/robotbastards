using System.Collections.Generic;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Handles level geometry CSG operations
	/// </summary>
	internal class Csg2
	{
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
		/// Builds a BSP tree for a set of level polygons
		/// </summary>
		public static Node Build( IEnumerable< LevelPolygon > polygons )
		{
			IList< Edge > allEdges = new List< Edge>( );
			foreach ( LevelPolygon poly in polygons )
			{
				AddReversedEdges( poly.Edges, allEdges );
			}

			return BuildNode( allEdges );
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
				ClassifyEdge( node.Plane, edge, outsideEdges, insideEdges );
			}
	
			ClipEdges( node.Behind, outsideEdges, keepOutsideEdges, keepInsideEdges, outputEdges );
			ClipEdges( node.InFront, insideEdges, keepOutsideEdges, keepInsideEdges, outputEdges );
		}

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

		private const float OnPlaneTolerance = 0.01f;

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
				if ( startClass == PlaneClassification.Behind )
				{
					behindEdges.Add( edge );
				}
				else
				{
					inFrontEdges.Add( edge );
				}
				return;
			}
			Point2 midPt = Intersections2.GetLinePlaneIntersection( edge.Start, edge.End, plane ).IntersectionPosition;

			Edge startToMid = new Edge( edge.Start, midPt, edge.IsDoubleSided );
			Edge midToEnd = new Edge( midPt, edge.End, edge.IsDoubleSided );

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

		private static void AddEdge( ICollection< Edge > edges, Edge edge )
		{
			if ( edges != null )
			{
				edges.Add( edge );
			}
		}

		private static void AddReversedEdges( IEnumerable< LevelEdge > sourceEdges, ICollection< Edge > edges )
		{
			foreach ( LevelEdge sourceEdge in sourceEdges )
			{
			//	edges.Add( new Edge( sourceEdge.End.Position, sourceEdge.Start.Position, sourceEdge.IsDoubleSided ) );
				edges.Add( new Edge( sourceEdge.Start.Position, sourceEdge.End.Position, sourceEdge.IsDoubleSided ) );
			}
		}

		private static IList< Edge > BuildPolygonEdgeList( LevelPolygon poly )
		{
			List< Edge > edges = new List< Edge >( poly.Edges.Length );

			for ( int edgeIndex = 0; edgeIndex < edges.Count; ++edgeIndex )
			{
				LevelEdge srcEdge = poly.Edges[ edgeIndex ];
				edges[ edgeIndex ] = new Edge( srcEdge.Start.Position, srcEdge.End.Position, srcEdge.IsDoubleSided );
			}

			return edges;
		}

		public class Edge
		{
			public Edge( Point2 start, Point2 end, bool isDoubleSided )
			{
				m_Start = start;
				m_End = end;
				m_IsDoubleSided = isDoubleSided;
			}

			public Point2 Start
			{
				get { return m_Start; }
			}

			public Point2 End
			{
				get { return m_End; }
			}

			public bool IsDoubleSided
			{
				get { return m_IsDoubleSided; }
			}

			private readonly Point2	m_Start;
			private readonly Point2	m_End;
			private readonly bool	m_IsDoubleSided;
		}

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
	}

}
