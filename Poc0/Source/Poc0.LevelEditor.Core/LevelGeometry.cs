using System;
using System.Drawing;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.World.Services;
using Graphics=Rb.Rendering.Graphics;
using Environment=Poc0.Core.Environment.Environment;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Level geometry. Combines brushes to create the walkable geometry of a level
	/// </summary>
	[Serializable, RenderingLibraryType]
	public abstract class LevelGeometry : IRenderable, IRay3Intersector
	{
		#region Public members

		/// <summary>
		/// Setup constructor
		/// </summary>
		public LevelGeometry( EditorScene scene )
		{
			scene.Renderables.Add( this );

			m_Environment = new Environment( );
			scene.RuntimeScene.Objects.Add( m_Environment );

			m_Csg.GeometryChanged += OnGeometryChanged;

			scene.GetService< IRayCastService >( ).AddIntersector( RayCastLayers.StaticGeometry, this );
		}

		/// <summary>
		/// Gets/sets the show flat flag
		/// </summary>
		public bool ShowFlat
		{
			get { return m_ShowFlat; }
			set { m_ShowFlat = value; }
		}

		/// <summary>
		/// Gets the CSG handler for this level
		/// </summary>
		public Csg Csg
		{
			get { return m_Csg; }
		}

		/// <summary>
		/// Renders the level geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( ShowFlat )
			{
				if ( m_FlatRenderer == null )
				{
					BuildFlatRenderer( );
				}

				//	The graphics are all 2D, but we're watching it in 3D, so flip the Y and Z axis before we render
				Graphics.Renderer.PushTransform( Transform.LocalToWorld, m_YZSwap );

				m_FlatRenderer.Render( context );

				Graphics.Renderer.PopTransform( Transform.LocalToWorld );
			}
			else
			{
				Render3d( context, m_Csg );
			}
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Renders the level geometry in 3D
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="csg">Level geometry CSG object</param>
		protected abstract void Render3d( IRenderContext context, Csg csg );
		
		/// <summary>
		/// The default bitmap used for wall textures
		/// </summary>
		protected static Bitmap DefaultWallBitmap
		{
			get { return Properties.Resources.GridSquare; }
		}

		/// <summary>
		/// The default bitmap used for floor textures
		/// </summary>
		protected static Bitmap DefaultFloorBitmap
		{
			get { return Properties.Resources.GridSquare; }
		}

		#endregion

		#region Private members

		private readonly Environment m_Environment;
		private readonly Csg m_Csg = new Csg( );

		private readonly static Matrix44 m_YZSwap = new Matrix44
			(
				1, 0, 0, 0,
				0, 0, 1, 0,
				0, 1, 0, 0,
				0, 0, 0, 1
			);

		[NonSerialized]
		private bool m_ShowFlat = true;

		[NonSerialized]
		private IRenderable m_FlatRenderer;

		[NonSerialized]
		private Draw.IPen m_DrawEdge;

		[NonSerialized]
		private Draw.IBrush m_FillVertex;

		[NonSerialized]
		private Draw.IBrush m_FillPolygon;

		[NonSerialized]
		private Draw.IPen m_DrawPolygon;


		/// <summary>
		/// Destroys the current renderable representation of the level geometry
		/// </summary>
		private void DestroyRenderable( )
		{
			IDisposable dispose = m_FlatRenderer as IDisposable;
			if ( dispose != null )
			{
				dispose.Dispose( );
			}
			m_FlatRenderer = null;
		}

		/// <summary>
		/// Builds a renderable representation of the level geometry
		/// </summary>
		private void BuildFlatRenderer( )
		{
			if ( m_DrawEdge == null )
			{
				m_DrawEdge = Graphics.Draw.NewPen( Color.Red, 2.0f );
			}

			if ( m_FillVertex == null )
			{
				m_FillVertex = Graphics.Draw.NewBrush( Color.Wheat, Color.Black );
			}

			if ( m_FillPolygon == null )
			{
				m_FillPolygon = Graphics.Draw.NewBrush( Color.DeepSkyBlue );
			}

			if ( m_DrawPolygon == null )
			{
				m_DrawPolygon = Graphics.Draw.NewPen( Color.Wheat );
			}

			Graphics.Draw.StartCache( );

			RenderFlat( m_Csg.Root );

			m_FlatRenderer = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Draws a CSG edge
		/// </summary>
		private void DrawEdge( Csg.Edge edge )
		{
			Graphics.Draw.Line( m_DrawEdge, edge.P0, edge.P1 );

			if ( !edge.DoubleSided )
			{
				Vector2 vec = ( edge.P1 - edge.P0 );
				Point2 mid = edge.P0 + vec / 2;
				Graphics.Draw.Line( m_DrawEdge, mid, mid + vec.MakePerpNormal( ) );
			}

			Graphics.Draw.Circle( m_FillVertex, edge.P0.X, edge.P0.Y, 0.3f );
			Graphics.Draw.Circle( m_FillVertex, edge.P1.X, edge.P1.Y, 0.3f );
		}

		/// <summary>
		/// Renders the specified BSP node
		/// </summary>
		private void RenderFlat( Csg.BspNode node )
		{
			if ( node == null )
			{
				return;
			}

			if ( node.ConvexRegion != null )
			{
				Graphics.Draw.Polygon( m_FillPolygon, node.ConvexRegion );
			}

			RenderFlat( node.Behind );
			RenderFlat( node.InFront );

			if ( node.ConvexRegion != null )
			{
				Graphics.Draw.Polygon( m_DrawPolygon, node.ConvexRegion );
			}

			DrawEdge( node.Edge );

		}

		/// <summary>
		/// Called when level geometry has changed
		/// </summary>
		protected virtual void OnGeometryChanged( object sender, EventArgs args )
		{
			//	Rubbish renderable representation - (next) Render() recreates renderable representation
			DestroyRenderable( );

			//	Update environment
			m_Environment.Walls = BuildWalls( m_Csg.Root );
		}

		/// <summary>
		/// Recursively creates the main game environment representation (BSP tree representing walls)
		/// </summary>
		/// <param name="srcNode">Level geometry CSG BSP node</param>
		/// <returns>Returns a game wall node built from the CSG source node</returns>
		private WallNode BuildWalls( Csg.BspNode srcNode )
		{
			if ( srcNode == null )
			{
				return null;
			}

			Floor floor = null;
			if ( srcNode.ConvexRegion != null )
			{
				floor = new Floor( srcNode.ConvexRegion, 0.0f );
			}

			WallNode newNode = new WallNode( srcNode.Edge.P0, srcNode.Edge.P1, 5, floor );
			if ( srcNode.InFront != null )
			{
				newNode.InFront = BuildWalls( srcNode.InFront );
			}
			if ( srcNode.Behind != null )
			{
				newNode.Behind = BuildWalls( srcNode.Behind );
			}
			return newNode;
		}

		#endregion

		#region IRay3Intersector Members

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public bool TestIntersection( Ray3 ray )
		{
			return ( m_Csg == null ) ? false : TestIntersection( ray, m_Csg.Root );
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public Line3Intersection GetIntersection( Ray3 ray )
		{
			return ( m_Csg == null ) ? null : GetIntersection( ray, m_Csg.Root );
		}

		#endregion

		/// <summary>
		/// Tests the intersection between a ray and a csg node
		/// </summary>
		/// <param name="ray">Ray to test</param>
		/// <param name="node">CSG node</param>
		/// <returns>true if there is an intersection between ray and node, or a subnode</returns>
		private static bool TestIntersection( Ray3 ray, Csg.BspNode node )
		{
			return GetIntersection( ray, node ) != null;
		}

		/// <summary>
		/// Gets the intersection between a ray and a csg node
		/// </summary>
		/// <param name="ray">Ray to test</param>
		/// <param name="node">CSG node</param>
		/// <returns>Intersection detail if there is an intersection between ray and node, or a subnode</returns>
		private static Line3Intersection GetIntersection( Ray3 ray, Csg.BspNode node )
		{
			if ( node == null )
			{
				return null;
			}

			//	TODO: AP: Use BSP properties... this is lazy
			Line3Intersection childIntersection = GetIntersection( ray, node.InFront ) ?? GetIntersection( ray, node.Behind );
			
			Point3 pt0 = node.Quad[ 0 ];
			Point3 pt1 = node.Quad[ 1 ];
			Point3 pt2 = node.Quad[ 2 ];
			Point3 pt3 = node.Quad[ 3 ];

			Line3Intersection intersection = Intersections3.GetRayQuadIntersection( ray, pt0, pt1, pt2, pt3 );
			if ( intersection != null )
			{
				if ( ( childIntersection == null ) || ( intersection.Distance < childIntersection.Distance ) )
				{
					intersection.IntersectedObject = node;
				}
				else if ( childIntersection.Distance < intersection.Distance )
				{
					intersection = childIntersection;
				}
			}
			else
			{
				intersection = childIntersection;
			}

			return intersection;
		}
	}
}
