using System;
using System.Drawing;
using Poc0.Core.Environment;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;
using Environment=Poc0.Core.Environment.Environment;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Level geometry. Combines brushes to create the walkable geometry of a level
	/// </summary>
	[Serializable]
	public class LevelGeometry : IRenderable
	{
		#region Public members

		/// <summary>
		/// Default constructor
		/// </summary>
		public LevelGeometry( EditorScene scene )
		{
			scene.Renderables.Add( this );

			Scene runtimeScene = scene.RuntimeScene;
			m_Environment = Builder.CreateInstance< Environment >( runtimeScene.Builder );

			m_Csg.GeometryChanged += OnGeometryChanged;
		}

		/// <summary>
		/// Gets the CSG handler for this level
		/// </summary>
		public Csg Csg
		{
			get { return m_Csg; }
		}

		private readonly Matrix44 m_YZSwap = new Matrix44
			(
				1, 0, 0, 0,
				0, 0, 1, 0,
				0, 1, 0, 0,
				0, 0, 0, 1
			);

		/// <summary>
		/// Renders the level geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Renderable == null )
			{
				BuildRenderable( );
			}

			//	The graphics are all 2D, but we're watching it in 3D, so flip the Y and Z axis before we render
			Graphics.Renderer.PushTransform( Transform.LocalToWorld, m_YZSwap );

			m_Renderable.Render( context );

			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
		}

		#endregion

		#region Private members

		private readonly Environment m_Environment;
		private readonly Csg m_Csg = new Csg( );

		[NonSerialized]
		private IRenderable m_Renderable;

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
			IDisposable dispose = m_Renderable as IDisposable;
			if ( dispose != null )
			{
				dispose.Dispose( );
			}
			m_Renderable = null;
		}

		/// <summary>
		/// Builds a renderable representation of the level geometry
		/// </summary>
		private void BuildRenderable( )
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

			Render( m_Csg.Root );

			m_Renderable = Graphics.Draw.StopCache( );
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
		private void Render( Csg.BspNode node )
		{
			if ( node == null )
			{
				return;
			}

			if ( node.ConvexRegion != null )
			{
				Graphics.Draw.Polygon( m_FillPolygon, node.ConvexRegion );
			}

			Render( node.Behind );
			Render( node.InFront );

			if ( node.ConvexRegion != null )
			{
				Graphics.Draw.Polygon( m_DrawPolygon, node.ConvexRegion );
			}

			DrawEdge( node.Edge );

		}

		/// <summary>
		/// Called when level geometry has changed
		/// </summary>
		private void OnGeometryChanged( object sender, EventArgs args )
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

			WallNode newNode = new WallNode( srcNode.Edge.P0, srcNode.Edge.P1, 10.0f, floor );
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

	}
}
