using System;
using System.Collections.Generic;
using System.Drawing;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tesselator;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Rb.World.Services;
using Graphics=Rb.Rendering.Graphics;
using Environment=Poc0.Core.Environment.Environment;

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
		public LevelEdge( LevelVertex start, LevelVertex end, LevelPolygon owner )
		{
			m_Start = start;
			m_End = end;
			m_Owner = owner;

			m_Start.StartEdge = this;
			m_End.EndEdge = this;
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

		public void Add( UiPolygon brush )
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

				LevelEdge newEdge = new LevelEdge( start, end, polygon );
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
				//	Case 0 : 0----x
			}
			else if ( nextEdge == null )
			{
				//	Case 1 : x----0
			}
			else if ( firstEdge == null )
			{
				//	Case 2 : x----0----x----x
			}
			else if ( lastEdge == null )
			{
				//	Case 3 : x----x----0----x
			}
			else
			{
				//	Case 4 : x----x----0----x----x
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
