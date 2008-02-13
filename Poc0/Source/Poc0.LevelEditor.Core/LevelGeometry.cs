using System;
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/*
	 * Floor region identification:
	 *		- Edges can be used to define unique floor regions (for different graphics, properties, etc.)
	 *		- When building floor regions, after BSP process, builder does as flood fill (need to keep neighbourhood information)
	 *			- Barrier edges stop flood fill recursion
	 * 
	 * BSP structure:
	 *	Convex leaf nodes 
	 * 
	 */

	public class LevelPolygon
	{
		public LevelPolygon( LevelVertex[] vertices, LevelEdge[] edges )
		{
			m_Vertices = vertices;
			m_Edges = edges;
		}

		public LevelVertex[] Vertices
		{
			get { return m_Vertices; }
			set { m_Vertices = value; }
		}

		public LevelEdge[] Edges
		{
			get { return m_Edges; }
			set { m_Edges = value; }
		}

		private LevelVertex[] m_Vertices;
		private LevelEdge[] m_Edges;
	}

	public class LevelEdge
	{
		public LevelEdge( LevelVertex start, LevelVertex end, LevelPolygon owner, bool doubleSided )
		{
			m_Start = start;
			m_End = end;
			m_Owner = owner;
			m_DoubleSided = doubleSided;

			m_Start.StartEdge = this;
			m_End.EndEdge = this;
		}

		public bool IsDoubleSided
		{
			get { return m_DoubleSided; }
		}

		public LevelPolygon Polygon
		{
			get { return m_Owner; }
		}

		public LevelVertex Start
		{
			get { return m_Start; }
			set { m_Start = value; }
		}

		public LevelVertex End
		{
			get { return m_End; }
			set { m_End = value; }
		}

		public LevelEdge PreviousEdge
		{
			get { return m_PrevEdge; }
			set { m_PrevEdge = value; }
		}

		public LevelEdge NextEdge
		{
			get { return m_NextEdge; }
			set { m_NextEdge = value; }
		}

		public static void LinkEdges( LevelEdge edge0, LevelEdge edge1 )
		{
			edge0.NextEdge = edge1;
			edge1.PreviousEdge = edge0;
		}

		public float SqrDistanceTo( Point2 pt )
		{
			Vector2 lineVec = m_End.Position - m_Start.Position;
			float sqrLength = lineVec.SqrLength;
			float t = 0.0f;

			if ( sqrLength != 0.0f )
			{
				t = Utils.Clamp( ( pt - m_Start.Position ).Dot( lineVec ) / sqrLength, 0.0f, 1.0f );
			}

			Point2 closestPt = m_Start.Position + ( lineVec * t );
			return closestPt.SqrDistanceTo( pt );
		}

		private LevelEdge m_PrevEdge;
		private LevelEdge m_NextEdge;
		private LevelVertex m_Start;
		private LevelVertex m_End;
		private readonly bool m_DoubleSided;
		private readonly LevelPolygon m_Owner;
	}

	public class LevelVertex
	{
		public LevelVertex( Point2 pt )
		{
			m_Position = pt;
			m_StartEdge = null;
			m_EndEdge = null;
		}

		public LevelEdge StartEdge
		{
			get { return m_StartEdge; }
			set { m_StartEdge = value; }
		}

		public LevelEdge EndEdge
		{
			get { return m_EndEdge; }
			set { m_EndEdge = value; }
		}

		public Point2 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		private Point2 m_Position;
		private LevelEdge m_StartEdge;
		private LevelEdge m_EndEdge;
	}

	internal class Csg2
	{
		public enum Operation
		{
			Union,
			Intersection,
			Complement
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

	/// <summary>
	/// Level geometry
	/// </summary>
	public class LevelGeometry : IRay3Intersector
	{
		public static LevelGeometry Instance
		{
			get { return ms_Instance; }
		}

		private void OnChanged( )
		{
		}

		public void Add( UiPolygon brush, bool doubleSided )
		{
			LevelVertex[] vertices = new LevelVertex[ brush.Points.Length ];
			LevelEdge[] edges = new LevelEdge[ brush.Points.Length ];

			LevelPolygon polygon = new LevelPolygon( vertices, edges );

			for ( int vertexIndex = 0; vertexIndex < vertices.Length; ++vertexIndex )
			{
				vertices[ vertexIndex ] = AddVertex( brush.Points[ vertexIndex ] );
			}

			for ( int edgeIndex = 0; edgeIndex < edges.Length; ++edgeIndex )
			{
				LevelVertex start	= vertices[ edgeIndex ];
				LevelVertex end		= vertices[ ( edgeIndex + 1 ) % vertices.Length ];

				LevelEdge newEdge = new LevelEdge( start, end, polygon, doubleSided );
				edges[ edgeIndex ] = newEdge;
				AddEdge( newEdge );

				if ( edgeIndex > 0 )
				{
					LevelEdge.LinkEdges( edges[ edgeIndex - 1 ], newEdge );
				}
			}
			
			LevelEdge.LinkEdges( edges[ edges.Length - 1 ], edges[ 0 ] );

			m_Polygons.Add( polygon );
		}

		public void Remove( LevelPolygon polygon )
		{
			foreach ( LevelVertex vertex in polygon.Vertices )
			{
				m_Vertices.Remove( vertex );
			}
			foreach ( LevelEdge edge in polygon.Edges )
			{
				m_Edges.Remove( edge );
			}
			m_Polygons.Remove( polygon );
		}

		public void Remove( LevelVertex vertex )
		{
			LevelEdge prevEdge = vertex.EndEdge;
			LevelEdge nextEdge = vertex.StartEdge;
			LevelEdge firstEdge = ( prevEdge == null ) ? null : prevEdge.PreviousEdge;
			LevelEdge lastEdge = ( nextEdge == null ) ? null : nextEdge.NextEdge;

			if ( prevEdge == null )
			{
				//	Case 0 : 0----x....x
				throw new NotImplementedException( );			
			}
			else if ( nextEdge == null )
			{
				//	Case 1 : x....x----0
				throw new NotImplementedException( );				
			}
			else if ( firstEdge == null )
			{
				//	Case 2 : x----0----x----x
				throw new NotImplementedException( );
			}
			else if ( lastEdge == null )
			{
				//	Case 3 : x----x----0----x
				throw new NotImplementedException( );
			}
			else
			{
				//	Case 4 : x----x----0----x----x
				throw new NotImplementedException( );
			}

			/*
			LevelEdge prevEdge = ( vertex.EndEdge == null ) ? null : vertex.EndEdge.PreviousEdge;
			LevelEdge nextEdge = ( vertex.StartEdge == null ) ? null : vertex.StartEdge.NextEdge;

			//	TODO: AP: Create temporary edge
			LevelEdge tempEdge = null;
			if ( ( prevEdge != null ) && ( nextEdge != null ) )
			{
			//	tempEdge = new LevelEdge( );
			}
			if ( prevEdge != null )
			{
				prevEdge.NextEdge = tempEdge;
				prevEdge.End.StartEdge = tempEdge;
			}
			if ( nextEdge != null )
			{
				nextEdge.PreviousEdge = prevEdge;
			}

			RemoveEdge( vertex.StartEdge );
			RemoveEdge( vertex.EndEdge );
			RemoveVertex( vertex );
			*/
		}

		private void RemoveVertex( LevelVertex vertex )
		{
			m_Vertices.Remove( vertex );
		}

		private void RemoveEdge( LevelEdge edge )
		{
			m_Edges.Remove( edge );
		}
		
		private LevelVertex AddVertex( Point2 pt )
		{
			LevelVertex vertex = new LevelVertex( pt );
			m_Vertices.Add( vertex );
			return vertex;
		}
		
		private void AddEdge( LevelEdge edge )
		{
			m_Edges.Add( edge );
		}

		private LevelPolygon FindClosestPolygon( Point2 pt )
		{
			//	TODO: AP: Traverse BSP tree, find polygon that contains pt
			return null;
		}

		private LevelVertex FindClosestVertex( Point2 pt, float distance )
		{
			float sqrDistance = distance * distance;
			foreach ( LevelVertex vertex in m_Vertices )
			{
				if ( pt.SqrDistanceTo( vertex.Position ) < sqrDistance )
				{
					return vertex;
				}
			}
			return null;
		}
		
		private LevelEdge FindClosestEdge( Point2 pt, float distance )
		{
			float sqrDistance = distance * distance;
			foreach ( LevelEdge edge in m_Edges )
			{
				if ( edge.SqrDistanceTo( pt ) < sqrDistance )
				{
					return edge;
				}
			}
			return null;
		}

		private readonly static LevelGeometry ms_Instance = new LevelGeometry( );

		private const float VertexSelectionRadius = 0.1f;
		private const float EdgeSelectionRadius = 0.1f;

		private readonly Plane3					m_GroundPlane	= new Plane3( Vector3.YAxis, 0 );
		private readonly List< LevelVertex >	m_Vertices		= new List< LevelVertex >( );
		private readonly List< LevelEdge >		m_Edges			= new List< LevelEdge >( );
		private readonly List< LevelPolygon >	m_Polygons		= new List< LevelPolygon >( );

		#region IRay3Intersector Members

		public bool TestIntersection( Ray3 ray )
		{
			Line3Intersection intersection = Intersections3.GetRayIntersection( ray, m_GroundPlane );
			if ( intersection == null )
			{
				return false;
			}
			Point2 pt = new Point2( intersection.IntersectionPosition.X, intersection.IntersectionPosition.Z );
			LevelVertex vertex = FindClosestVertex( pt, VertexSelectionRadius );
			if ( vertex != null )
			{
				return true;
			}

			LevelEdge edge = FindClosestEdge( pt, EdgeSelectionRadius );
			if ( edge != null )
			{
				return true;
			}
			LevelPolygon polygon = FindClosestPolygon( pt );
			if ( polygon != null )
			{
				return true;
			}
			return false;
		}

		public Line3Intersection GetIntersection( Ray3 ray )
		{
			Line3Intersection intersection = Intersections3.GetRayIntersection( ray, m_GroundPlane );
			if ( intersection == null )
			{
				return null;
			}
			Point2 pt = new Point2( intersection.IntersectionPosition.X, intersection.IntersectionPosition.Z );
			LevelVertex vertex = FindClosestVertex( pt, VertexSelectionRadius );
			if ( vertex != null )
			{
				intersection.IntersectedObject = vertex;
				return intersection;
			}

			LevelEdge edge = FindClosestEdge( pt, EdgeSelectionRadius );
			if ( edge != null )
			{
				intersection.IntersectedObject = edge;
				return intersection;
			}
			LevelPolygon polygon = FindClosestPolygon( pt );
			if ( polygon != null )
			{
				intersection.IntersectedObject = polygon;
				return intersection;
			}

			return null;
		}

		#endregion
	}
}
