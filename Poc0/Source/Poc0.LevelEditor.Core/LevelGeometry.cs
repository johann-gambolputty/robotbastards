using System;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.World;

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


	/// <summary>
	/// Level geometry
	/// </summary>
	[Serializable]
	[RenderingLibraryType]
	public class LevelGeometry : IRay3Intersector, IRenderable, ISceneObject
	{
		/// <summary>
		/// Gets the level geometry singleton
		/// </summary>
		public static LevelGeometry Instance
		{
			get { return ms_Instance; }
		}

		private void RebuildGeometry( )
		{
			m_Root = Csg2.Build( m_Polygons );

			m_DisplayPolygons = new List< LevelGeometryTesselator.Polygon >( );

			LevelGeometryTesselator tess = new LevelGeometryTesselator( );
			LevelGeometryTesselator.Polygon poly = tess.CreateBoundingPolygon( -100, -100, 100, 100 );

			BuildConvexRegions( m_Root, tess, poly );

			m_DisplayPoints = tess.Points.ToArray( );
		}

		/// <summary>
		/// Builds the convex region for a node and its children
		/// </summary>
		private void BuildConvexRegions( Csg2.Node node, LevelGeometryTesselator tess, LevelGeometryTesselator.Polygon poly )
		{
			LevelGeometryTesselator.Polygon behindPoly;
			LevelGeometryTesselator.Polygon inFrontPoly;
			tess.Split( node.Plane, poly, out behindPoly, out inFrontPoly );

			if ( node.Behind != null )
			{
				BuildConvexRegions( node.Behind, tess, behindPoly );
			}
			else
			{
				m_DisplayPolygons.Add( behindPoly );
			}
			if ( node.InFront != null )
			{
				BuildConvexRegions( node.InFront, tess, inFrontPoly );
			}
			else
			{
			//	node.ConvexRegion = inFrontPoly.Indices;
			//	m_DisplayPolygons.Add( inFrontPoly );
			}
		}


		private Point2[] m_DisplayPoints;
		private List<LevelGeometryTesselator.Polygon> m_DisplayPolygons;
		private Csg2.Node m_Root;

		/// <summary>
		/// Converts a UI polygon to a <see cref="LevelPolygon"/>, adding the result to the level geometry
		/// </summary>
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

			Add( polygon );
		}

		/// <summary>
		/// Adds a <see cref="LevelPolygon"/> to the level
		/// </summary>
		public void Add( LevelPolygon polygon )
		{
			m_Polygons.Add( polygon );
			RebuildGeometry( );
		}

		/// <summary>
		/// Removes a <see cref="LevelPolygon"/> from the level
		/// </summary>
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

		#region IRenderable Members

		private IEnumerable< Point3 > PolyPoints( LevelGeometryTesselator.Polygon poly )
		{
			for ( int index = 0; index < poly.Indices.Length; ++index )
			{
				Point2 pt = m_DisplayPoints[ poly.Indices[ index ] ];
				yield return new Point3( pt.X, 0.01f, pt.Y );
			}
		}

		public void Render( IRenderContext context )
		{
			if ( m_DisplayPolygons == null )
			{
				return;
			}
			foreach ( LevelGeometryTesselator.Polygon poly in m_DisplayPolygons )
			{
				IEnumerable< Point3 > nextPoint = PolyPoints( poly );
				Graphics.Draw.Polygon( Draw.Brushes.Blue, nextPoint );
			}
		}

		#endregion

		#region ISceneObject Members

		public void AddedToScene( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		public void RemovedFromScene( Scene scene )
		{
			scene.Renderables.Remove( this );
		}

		#endregion
	}
}
