using System;
using System.Collections.Generic;
using System.Drawing;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Rb.World.Services;
using Environment=Poc0.Core.Environment.Environment;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core.Geometry
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
	public class LevelGeometry : IRay3Intersector, IRenderable, ISceneObject, IObjectEditor
	{
		/// <summary>
		/// Gets a LevelGeometry object from the scene
		/// </summary>
		public static LevelGeometry FromScene( Scene scene )
		{
			return scene.Objects.GetFirstOfType< LevelGeometry >( );
		}
		
		/// <summary>
		/// Gets a LevelGeometry object from the current editor scene
		/// </summary>
		public static LevelGeometry FromCurrentScene( )
		{
			return FromScene( EditorState.Instance.CurrentScene );
		}

		/// <summary>
		/// Gets the list of obstacle polygons
		/// </summary>
		public IEnumerable< LevelPolygon > ObstaclePolygons
		{
			get { return m_Polygons; }
		}

		/// <summary>
		/// Sets up the initial bounding polygon
		/// </summary>
		public LevelGeometry( )
		{
			float minX = -20;
			float minY = -20;
			float maxX = 20;
			float maxY = 20;

			UiPolygon bounds = new UiPolygon
				(
					"",
					new Point2[]
						{
							new Point2( minX, minY ),
							new Point2( maxX, minY ),
							new Point2( maxX, maxY ),
							new Point2( minX, maxY ),
						},
					false
				);

			Add( bounds, false, true );
		}

		/// <summary>
		/// Converts a UI polygon to a <see cref="LevelPolygon"/>, adding the result to the level geometry
		/// </summary>
		public void Add( UiPolygon brush, bool doubleSided, bool reversed )
		{
			LevelVertex[] vertices = new LevelVertex[ brush.Points.Length ];
			LevelEdge[] edges = new LevelEdge[ brush.Points.Length ];

			LevelPolygon polygon = new LevelPolygon( vertices, edges, reversed );

			for ( int vertexIndex = 0; vertexIndex < vertices.Length; ++vertexIndex )
			{
				vertices[ vertexIndex ] = new LevelVertex( brush.Points[ vertexIndex ] );
			}

			for ( int edgeIndex = 0; edgeIndex < edges.Length; ++edgeIndex )
			{
				LevelVertex start	= vertices[ edgeIndex ];
				LevelVertex end		= vertices[ ( edgeIndex + 1 ) % vertices.Length ];

				LevelEdge newEdge = new LevelEdge( start, end, polygon, doubleSided );
				edges[ edgeIndex ] = newEdge;

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
			foreach ( LevelVertex vertex in polygon.Vertices )
			{
				AddVertex( vertex );
			}
			foreach ( LevelEdge edge in polygon.Edges )
			{
				AddEdge( edge );
			}

			m_Polygons.Add( polygon );
			polygon.Changed += OnPolygonChanged;

			//	Force rebuild of display cache
			DestroyDisplayCache( );
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

			//	Force rebuild of display cache
			DestroyDisplayCache( );
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

			//	Force rebuild of display cache
		//	DestroyDisplayCache( );
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

		private const float VertexSelectionRadius		= 0.75f;
		private const float VertexSelectionRadiusSqr	= VertexSelectionRadius * VertexSelectionRadius;

		private const float EdgeSelectionRadius			= 0.4f;
		private const float EdgeSelectionRadiusSqr		= EdgeSelectionRadius * EdgeSelectionRadius;
		
		private readonly Plane3							m_GroundPlane		= new Plane3( Vector3.YAxis, 0 );
		private readonly List< LevelVertex >			m_Vertices			= new List< LevelVertex >( );
		private readonly List< LevelEdge >				m_Edges				= new List< LevelEdge >( );
		private readonly List< LevelPolygon >			m_Polygons			= new List< LevelPolygon >( );

		[NonSerialized]
		private IRenderable								m_DisplayCache;

		[NonSerialized]
		private Point2[]								m_DisplayPoints;

		[NonSerialized]
		private List< LevelGeometryTesselator.Polygon >	m_FloorDisplayPolygons;

		[NonSerialized]
		private List< LevelGeometryTesselator.Polygon >	m_ObstacleDisplayPolygons;

		[NonSerialized]
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

		/// <summary>
		/// Selects an object, given the state of an ISelectable object
		/// </summary>
		private static T Select< T >( ISelectable sel, T selectedResult, T highlightResult, T defaultResult )
		{
			return ( sel.Selected ? selectedResult : ( sel.Highlighted ? highlightResult : defaultResult ) );
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
		private IEnumerable< Point3 > PolyPoints( LevelGeometryTesselator.Polygon poly, float y )
		{
			for ( int index = 0; index < poly.Edges.Length; ++index )
			{
				Point2 pt = m_DisplayPoints[ poly.Edges[ index ].StartIndex ];
				yield return new Point3( pt.X, y, pt.Y );
			}
		}

		/// <summary>
		/// Adds a vertex to the level geometry set
		/// </summary>
		private void AddVertex( LevelVertex vertex )
		{
			m_Vertices.Add( vertex );
			vertex.Changed += OnVertexChanged;
		}
		
		/// <summary>
		/// Adds an edge to the level geometry set
		/// </summary>
		private void AddEdge( LevelEdge edge )
		{
			m_Edges.Add( edge );
			edge.Changed += OnEdgeChanged;
		}

		private void OnChanged( )
		{
			DestroyDisplayCache( );
			if ( ObjectChanged != null )
			{
				ObjectChanged(this, null);
			}
		}

		/// <summary>
		/// Invoked when a polygons's properties are changed
		/// </summary>
		private void OnPolygonChanged( LevelPolygon poly )
		{
			OnChanged( );
		}

		/// <summary>
		/// Invoked when a edge's properties are changed
		/// </summary>
		private void OnEdgeChanged( LevelEdge edge )
		{
			OnChanged( );
		}

		/// <summary>
		/// Invoked when a vertex's properties are changed
		/// </summary>
		private void OnVertexChanged( LevelVertex vertex )
		{
			OnChanged( );
		}

		/// <summary>
		/// Finds the closest object in the level geometry set
		/// </summary>
		private object FindClosestObject( Point2 pt )
		{
			foreach ( LevelVertex vertex in m_Vertices )
			{
				if ( vertex.Position.SqrDistanceTo( pt ) < VertexSelectionRadiusSqr )
				{
					return vertex;
				}
			}

			foreach ( LevelEdge edge in m_Edges )
			{
				if ( edge.SqrDistanceTo( pt ) < EdgeSelectionRadiusSqr )
				{
					return edge;
				}
			}

			return ( m_Root == null ) ? null : FindClosestObject( pt, m_Root );
		}

		/// <summary>
		/// Finds the closest object in a given CSG node
		/// </summary>
		private object FindClosestObject( Point2 pt, Csg2.Node node )
		{
			PlaneClassification ptClass = node.Plane.ClassifyPoint( pt, 0.01f );
			LevelEdge edge = node.Edge.SourceEdge;
			if ( ptClass == PlaneClassification.Behind )
			{
				return ( node.Behind == null ) ? FloorProperties.Default : FindClosestObject( pt, node.Behind );
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

			m_Root = Csg2.BuildExpansion( m_Polygons, 1.0f );

			m_FloorDisplayPolygons = new List< LevelGeometryTesselator.Polygon >( );
			m_ObstacleDisplayPolygons = new List< LevelGeometryTesselator.Polygon >( );

			LevelGeometryTesselator tess = new LevelGeometryTesselator( );
			LevelGeometryTesselator.Polygon poly = tess.CreateBoundingPolygon( -100, -100, 100, 100 );

			tess.BuildConvexRegions( m_Root, poly, AddFloorPolygon, AddObstaclePolygon, false );

			m_DisplayPoints = tess.Points.ToArray( );

			Graphics.Draw.StartCache( );
			RenderGeometry( );
			m_DisplayCache = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Destroys the current display cache
		/// </summary>
		private void DestroyDisplayCache( )
		{
			if ( m_DisplayCache != null )
			{
				if ( m_DisplayCache is IDisposable )
				{
					( ( IDisposable )m_DisplayCache ).Dispose( );
				}
				m_DisplayCache = null;
			}
		}

		/// <summary>
		/// Renders geometry
		/// </summary>
		private void RenderGeometry( )
		{
			if ( m_FloorDisplayPolygons == null )
			{
				return;
			}

			const float height = 0.01f;

			foreach ( LevelGeometryTesselator.Polygon poly in m_FloorDisplayPolygons )
			{
				IEnumerable< Point3 > nextPoint = PolyPoints( poly, height );
				Graphics.Draw.Polygon( Draw.Brushes.Blue, nextPoint );
			}
			foreach ( LevelGeometryTesselator.Polygon poly in m_ObstacleDisplayPolygons )
			{
				IEnumerable< Point3 > nextPoint = PolyPoints( poly, height );

				LevelPolygon levelPoly = poly.LevelPolygon;

				Draw.IBrush brush = Draw.Brushes.White;
				if ( levelPoly != null )
				{
					brush = Select( levelPoly, ms_SelectedPolyBrush, ms_HighlightedPolyBrush, ms_PolyBrush );
				}
				Graphics.Draw.Polygon( brush, nextPoint );
			}

			foreach ( LevelEdge edge in m_Edges )
			{
				Draw.IPen pen = Select( edge, ms_SelectedEdgePen, ms_HighlightedEdgePen, ms_EdgePen );
				Graphics.Draw.Line( pen, edge.Start.Position.X, height, edge.Start.Position.Y, edge.End.Position.X, 0.01f, edge.End.Position.Y );
			}

			foreach ( LevelVertex vertex in m_Vertices )
			{
				Draw.IBrush brush = Select( vertex, ms_SelectedVertexBrush, ms_HighlightedVertexBrush, ms_VertexBrush );
				Graphics.Draw.Circle( brush, vertex.Position.X, height, vertex.Position.Y, VertexSelectionRadius );
			}
		}
		
		/// <summary>
		/// Adds a floor display polygon
		/// </summary>
		private void AddFloorPolygon( LevelGeometryTesselator.Polygon poly, Csg2.Node node )
		{
			m_FloorDisplayPolygons.Add( poly );
		}

		/// <summary>
		/// Adds an obstacle display polygon
		/// </summary>
		private void AddObstaclePolygon( LevelGeometryTesselator.Polygon poly, Csg2.Node node )
		{
			m_ObstacleDisplayPolygons.Add( poly );
			poly.LevelPolygon = node.Edge.SourceEdge.Polygon;
		}

		#endregion

		#region IObjectEditor Members

		/// <summary>
		/// Raised when this object changes
		/// </summary>
		public event EventHandler ObjectChanged;

		/// <summary>
		/// Builds runtime environment geometry and adds it to the scene
		/// </summary>
		public void Build( Scene scene )
		{
			//	TODO: AP: Make independent of rendering API (this creates an API-specific graphics object, that is serialized to the scene file)
			//	Create the environment
			Environment env = new Environment( );
			IEnvironmentGraphics graphics = Graphics.Factory.Create< IEnvironmentGraphics >( );
			env.Graphics = graphics;

			scene.Objects.Add( env );
			
			//	Build environment graphics
			EnvironmentGraphicsBuilder builder = new EnvironmentGraphicsBuilder( 10 );
			builder.Build( graphics, this );

			//	Add collisions
			env.Collisions = EnvironmentCollisionsBuilder.Build( this );
		}

		#endregion
	}
}
