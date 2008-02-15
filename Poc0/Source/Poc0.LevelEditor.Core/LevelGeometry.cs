using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Rb.World.Services;
using Graphics=Rb.Rendering.Graphics;

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
			polygon.Changed += OnPolygonChanged;
			RebuildDisplay( );
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

		/// <summary>
		/// Removes a vertex from the level
		/// </summary>
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

		#region IRay3Intersector Members

		public bool TestIntersection( Ray3 ray )
		{
			Line3Intersection intersection = Intersections3.GetRayIntersection( ray, m_GroundPlane );
			if ( intersection == null )
			{
				return false;
			}
			Point2 pt = new Point2( intersection.IntersectionPosition.X, intersection.IntersectionPosition.Z );

			return FindClosestObject( pt ) != null;
		}

		public Line3Intersection GetIntersection( Ray3 ray )
		{
			Line3Intersection intersection = Intersections3.GetRayIntersection( ray, m_GroundPlane );
			if ( intersection == null )
			{
				return null;
			}
			Point2 pt = new Point2( intersection.IntersectionPosition.X, intersection.IntersectionPosition.Z );

			intersection.IntersectedObject = FindClosestObject( pt );

			return ( intersection.IntersectedObject == null ) ? null : intersection;
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		public void Render( IRenderContext context )
		{
			if ( m_DisplayCache == null )
			{
				RebuildDisplay( );
			}

			if ( m_DisplayCache != null )
			{
				m_DisplayCache.Render( context );
			}

		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene scene )
		{
			scene.GetService< IRayCastService >( ).AddIntersector( this );
			scene.Renderables.Add( this );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene scene )
		{
			scene.GetService< IRayCastService >( ).RemoveIntersector( this );
			scene.Renderables.Remove( this );
		}

		#endregion
		
		#region Private members
		
		private const float 							SelectionRadius		= 0.25f;
		private const float 							SelectionRadiusSqr	= SelectionRadius * SelectionRadius;

		private IRenderable								m_DisplayCache;

		private readonly Plane3							m_GroundPlane		= new Plane3( Vector3.YAxis, 0 );
		private readonly List< LevelVertex >			m_Vertices			= new List< LevelVertex >( );
		private readonly List< LevelEdge >				m_Edges				= new List< LevelEdge >( );
		private readonly List< LevelPolygon >			m_Polygons			= new List< LevelPolygon >( );
		private Point2[]								m_DisplayPoints;
		private List< LevelGeometryTesselator.Polygon >	m_FloorDisplayPolygons;
		private List< LevelGeometryTesselator.Polygon >	m_ObstacleDisplayPolygons;
		private Csg2.Node								m_Root;
		
		private readonly static Draw.IPen 				ms_EdgePen;
		private readonly static Draw.IPen 				ms_SelectedEdgePen;
		private readonly static Draw.IPen 				ms_HighlightedEdgePen;

		private readonly static Draw.IBrush 			ms_VertexBrush;
		private readonly static Draw.IBrush 			ms_SelectedVertexBrush;
		private readonly static Draw.IBrush 			ms_HighlightedVertexBrush;

		private readonly static Draw.IBrush 			ms_PolyBrush;
		private readonly static Draw.IBrush 			ms_SelectedPolyBrush;
		private readonly static Draw.IBrush 			ms_HighlightedPolyBrush;

		private static Draw.IPen SelectPen( ISelectable sel, Draw.IPen selPen, Draw.IPen highlightPen, Draw.IPen defaultPen )
		{
			return ( sel.Selected ? selPen : ( sel.Highlighted ? highlightPen : defaultPen ) );
		}
		
		private static Draw.IBrush SelectBrush( ISelectable sel, Draw.IBrush selBrush, Draw.IBrush highlightBrush, Draw.IBrush defaultBrush )
		{
			return ( sel.Selected ? selBrush : ( sel.Highlighted ? highlightBrush : defaultBrush ) );
		}

		static LevelGeometry( )
		{
			ms_EdgePen					= Graphics.Draw.NewPen( Color.Black, 1.5f );
			ms_HighlightedEdgePen		= Graphics.Draw.NewPen( Color.Orange, 1.5f );
			ms_SelectedEdgePen			= Graphics.Draw.NewPen( Color.Yellow, 1.5f );

			ms_VertexBrush				= Graphics.Draw.NewBrush( Color.Red, Color.Black );
			ms_SelectedVertexBrush		= Graphics.Draw.NewBrush( Color.Yellow, Color.Black );
			ms_HighlightedVertexBrush	= Graphics.Draw.NewBrush( Color.Orange, Color.Black );
			
			ms_PolyBrush				= Graphics.Draw.NewBrush( Color.WhiteSmoke );
			ms_SelectedPolyBrush		= Graphics.Draw.NewBrush( Color.Yellow );
			ms_HighlightedPolyBrush		= Graphics.Draw.NewBrush( Color.Orange );
		}

		/// <summary>
		/// Bodgy hack job to retrieve Point3s from a polygon
		/// </summary>
		private IEnumerable< Point3 > PolyPoints( LevelGeometryTesselator.Polygon poly )
		{
			for ( int index = 0; index < poly.Indices.Length; ++index )
			{
				Point2 pt = m_DisplayPoints[ poly.Indices[ index ] ];
				yield return new Point3( pt.X, 0.01f, pt.Y );
			}
		}

		private LevelVertex AddVertex( Point2 pt )
		{
			LevelVertex vertex = new LevelVertex( pt );
			m_Vertices.Add( vertex );

			vertex.Changed += OnVertexChanged;
			return vertex;
		}
		
		private void OnPolygonChanged( LevelPolygon poly )
		{
			//	Force rebuild of display cache
			m_DisplayCache = null;
		}

		private void OnEdgeChanged( LevelEdge edge )
		{
			//	Force rebuild of display cache
			m_DisplayCache = null;
		}

		private void OnVertexChanged( LevelVertex vertex )
		{
			//	Force rebuild of display cache
			m_DisplayCache = null;
		}

		private void AddEdge( LevelEdge edge )
		{
			m_Edges.Add( edge );
			edge.Changed += OnEdgeChanged;
		}

		private object FindClosestObject( Point2 pt )
		{
			foreach ( LevelVertex vertex in m_Vertices )
			{
				if ( vertex.Position.SqrDistanceTo( pt ) < SelectionRadiusSqr )
				{
					return vertex;
				}
			}

			foreach ( LevelEdge edge in m_Edges )
			{
				if ( edge.SqrDistanceTo( pt ) < SelectionRadiusSqr )
				{
					return edge;
				}
			}

			return ( m_Root == null ) ? null : FindClosestObject( pt, m_Root );
		}

		private object FindClosestObject( Point2 pt, Csg2.Node node )
		{
			PlaneClassification ptClass = node.Plane.ClassifyPoint( pt, 0.01f );
			LevelEdge edge = node.Edge.SourceEdge;
			if ( ptClass == PlaneClassification.Behind )
			{
				return ( node.Behind == null ) ? null : FindClosestObject( pt, node.Behind );
			}

			return ( node.InFront == null ) ? edge.Polygon : FindClosestObject( pt, node.InFront );
		}

		/// <summary>
		/// Rebuilds display data
		/// </summary>
		private void RebuildDisplay( )
		{
			if ( m_Polygons.Count == 0 )
			{
				return;
			}

			m_Root = Csg2.Build( m_Polygons );

			m_FloorDisplayPolygons = new List< LevelGeometryTesselator.Polygon >( );
			m_ObstacleDisplayPolygons = new List<LevelGeometryTesselator.Polygon>( );

			LevelGeometryTesselator tess = new LevelGeometryTesselator( );
			LevelGeometryTesselator.Polygon poly = tess.CreateBoundingPolygon( -100, -100, 100, 100 );

			BuildConvexRegions( m_Root, tess, poly );

			m_DisplayPoints = tess.Points.ToArray( );

			Graphics.Draw.StartCache( );
			RenderGeometry( );
			m_DisplayCache = Graphics.Draw.StopCache( );
		}

		private void RenderGeometry( )
		{
			if ( m_FloorDisplayPolygons == null )
			{
				return;
			}
			foreach ( LevelGeometryTesselator.Polygon poly in m_FloorDisplayPolygons )
			{
				IEnumerable< Point3 > nextPoint = PolyPoints( poly );
				Graphics.Draw.Polygon( Draw.Brushes.Blue, nextPoint );
			}
			foreach ( LevelGeometryTesselator.Polygon poly in m_ObstacleDisplayPolygons )
			{
				IEnumerable< Point3 > nextPoint = PolyPoints( poly );

				LevelPolygon levelPoly = poly.LevelPolygon;

				Draw.IBrush brush = Draw.Brushes.White;
				if ( levelPoly != null )
				{
					brush = SelectBrush( levelPoly, ms_SelectedPolyBrush, ms_HighlightedPolyBrush, ms_PolyBrush );
				}
				Graphics.Draw.Polygon( brush, nextPoint );
			}

			foreach ( LevelEdge edge in m_Edges )
			{
				Draw.IPen pen = SelectPen( edge, ms_SelectedEdgePen, ms_HighlightedEdgePen, ms_EdgePen );
				Graphics.Draw.Line( pen, edge.Start.Position.X, 0.01f, edge.Start.Position.Y, edge.End.Position.X, 0.01f, edge.End.Position.Y );
			}

			foreach ( LevelVertex vertex in m_Vertices )
			{
				Draw.IBrush brush = SelectBrush( vertex, ms_SelectedVertexBrush, ms_HighlightedVertexBrush, ms_VertexBrush );
				Graphics.Draw.Circle( brush, vertex.Position.X, 0.01f, vertex.Position.Y, SelectionRadius );
			}
		}
		
		/// <summary>
		/// Adds a floor display polygon
		/// </summary>
		private void AddFloorPolygon( LevelGeometryTesselator.Polygon poly )
		{
			m_FloorDisplayPolygons.Add( poly );
		}

		/// <summary>
		/// Adds an obstacle display polygon
		/// </summary>
		private void AddObstaclePolygon( LevelGeometryTesselator.Polygon poly )
		{
			m_ObstacleDisplayPolygons.Add( poly );
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
				AddFloorPolygon( behindPoly );
			}
			if ( node.InFront != null )
			{
				BuildConvexRegions( node.InFront, tess, inFrontPoly );
			}
			else
			{
				AddObstaclePolygon( inFrontPoly );
				inFrontPoly.LevelPolygon = node.Edge.SourceEdge.Polygon;
			}
		}

		#endregion

	}
}
