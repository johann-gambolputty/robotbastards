using System;
using System.Runtime.Serialization;
using Poc0.LevelEditor.Core;
using Rb.Core.Assets;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Tao.OpenGl;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	/// <summary>
	/// Rendering for level geometry in the level editor
	/// </summary>
	[Serializable]
	public class OpenGlLevelGeometry : LevelGeometry
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public OpenGlLevelGeometry( EditorScene scene ) :
			base( scene )
		{
			//	Try to load the wall rendering effect
			try
			{
				IEffect wallEffect = ( IEffect )AssetManager.Instance.Load( WallEffectPath );
				m_WallTechnique = wallEffect.GetTechnique( "MainTechnique" );
			}
			catch ( Exception ex )
			{
				GraphicsLog.Exception( ex, "Failed to build wall technique from \"{0}\" - making fallback technique", WallEffectPath );
				
				//	Failed to load wall rendering effect - create a simple default
				RenderState wallState = Graphics.Factory.NewRenderState( );
				wallState.DisableLighting( );
				wallState.SetColour( System.Drawing.Color.DarkOrange );
				//wallState.EnableCap( RenderStateFlag.Texture2d | RenderStateFlag.Texture2dUnit0 );
				m_WallTechnique = new Technique( "FallbackWallTechnique", wallState );
			}

			//	Try to load the floor rendering effect
			try
			{
				IEffect floorEffect = ( IEffect )AssetManager.Instance.Load( FloorEffectPath );
				m_FloorTechnique = floorEffect.GetTechnique( "MainTechnique" );
			}
			catch ( Exception ex )
			{
				GraphicsLog.Exception( ex, "Failed to build floor technique from \"{0}\" - making fallback technique", FloorEffectPath );

				//	Failed to load floor rendering effect - create a simple default
				RenderState floorState = Graphics.Factory.NewRenderState( );
				floorState.DisableLighting( );
				//floorState.EnableCap( RenderStateFlag.Texture2d | RenderStateFlag.Texture2dUnit0 );
				floorState.SetColour( System.Drawing.Color.Blue );

				m_FloorTechnique = new Technique( "FallbackFloorTechnique", floorState );
			}

			//	Load up wall and floor textures
			m_WallTextures = Graphics.Factory.NewTexture2d( );
			m_WallTextures.Load( DefaultWallBitmap, true );

			m_FloorTextures = Graphics.Factory.NewTexture2d( );
			m_FloorTextures.Load( DefaultFloorBitmap, true );
		}

		#region Serialization

		/// <summary>
		/// Called when this object is deserialized
		/// </summary>
		/// <param name="context">Deserialization context</param>
		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			DestroyDisplayList( ref m_WallDisplayList );
			DestroyDisplayList( ref m_FloorDisplayList );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Renders the level geometry in 3D
		/// </summary>
		/// <param name="context">Rendering context</param>
		/// <param name="csg">Level geometry CSG object</param>
		protected override void Render3d( IRenderContext context, Csg csg )
		{
			if ( csg == null )
			{
				return;
			}
			if ( m_WallDisplayList == -1 )
			{
				RebuildWallDisplayList( );
			}
			
			if ( m_FloorDisplayList == -1 )
			{
				RebuildFloorDisplayList( );
			}


			//	Render walls
			Graphics.Renderer.BindTexture( m_WallTextures );
			m_WallTechnique.Apply( context, Render3dWalls );
			Graphics.Renderer.UnbindTexture( m_WallTextures );

			//	Render floors
			Graphics.Renderer.BindTexture( m_FloorTextures );
			m_FloorTechnique.Apply( context, Render3dFloors );
			Graphics.Renderer.UnbindTexture( m_FloorTextures );
		}

		/// <summary>
		/// Called when level geometry has changed
		/// </summary>
		protected override void OnGeometryChanged( object sender, EventArgs args )
		{
			RebuildWallDisplayList( );
			RebuildFloorDisplayList( );
			base.OnGeometryChanged( sender, args );
		}


		#endregion

		#region Private members

		private const string WallEffectPath = @"Editor\Effects\Walls.cgfx";
		private const string FloorEffectPath = @"Editor\Effects\Floors.cgfx";

		[NonSerialized]
		private int m_WallDisplayList = -1;

		[NonSerialized]
		private int m_FloorDisplayList = -1;

		private readonly Texture2d m_WallTextures;
		private readonly Texture2d m_FloorTextures;

		private readonly ITechnique m_WallTechnique;
		private readonly ITechnique m_FloorTechnique;
		
		/// <summary>
		/// Renders 3D wall geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void Render3dWalls( IRenderContext context )
		{
			if ( m_WallDisplayList != -1 )
			{
				Gl.glCallList( m_WallDisplayList );
			}
		}

		/// <summary>
		/// Renders 3D floor geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void Render3dFloors( IRenderContext context )
		{
			if ( m_FloorDisplayList != -1 )
			{
				Gl.glCallList( m_FloorDisplayList );
			}
		}

		/// <summary>
		/// Renders the current node's walls
		/// </summary>
		/// <param name="node">Current node</param>
		private void RenderWall( Csg.BspNode node )
		{
			if ( node == null )
			{
				return;
			}
			Point2 start = node.Edge.P0;
			Point2 end = node.Edge.P1;

			float minU = 0;
			float maxU = 4;
			float minV = 0;
			float maxV = 4;

			float height = 5;

			Gl.glNormal3f( node.Plane.Normal.X, 0, node.Plane.Normal.Y );
			Gl.glTexCoord2f( minU, minV );
			Gl.glVertex3f( start.X, 0, start.Y );
			Gl.glTexCoord2f( maxU, minV );
			Gl.glVertex3f( end.X, 0, end.Y );
			Gl.glTexCoord2f( maxU, maxV );
			Gl.glVertex3f( end.X, height, end.Y );

			Gl.glNormal3f( node.Plane.Normal.X, 0, node.Plane.Normal.Y );
			Gl.glTexCoord2f( maxU, maxV );
			Gl.glVertex3f( end.X, height, end.Y );
			Gl.glTexCoord2f( minU, maxV );
			Gl.glVertex3f( start.X, height, start.Y );
			Gl.glTexCoord2f( minU, minV );
			Gl.glVertex3f( start.X, 0, start.Y );

			RenderWall( node.InFront );
			RenderWall( node.Behind );
		}

		/// <summary>
		/// Renders floor nodes
		/// </summary>
		/// <param name="node">Current node</param>
		private void RenderFloor( Csg.BspNode node )
		{
			if ( node == null )
			{
				return;
			}

			if ( node.ConvexRegion != null )
			{
				Point2[] points = node.ConvexRegion;
				Point2 basePos = points[ 0 ];
				float height = 0;
				float uScale = 0.5f;
				float vScale = 0.5f;
				for ( int vertexIndex = 1; vertexIndex < points.Length - 1; ++vertexIndex )
				{
					Gl.glNormal3f( 0, 1, 0 );

					Gl.glTexCoord2f( basePos.X * uScale, basePos.Y * vScale );
					Gl.glVertex3f( basePos.X, height, basePos.Y );

					Point2 pt = points[ vertexIndex ];
					Gl.glTexCoord2f( pt.X * uScale, pt.Y * vScale );
					Gl.glVertex3f( pt.X, height, pt.Y );
					
					pt = points[ vertexIndex + 1 ];
					Gl.glTexCoord2f( pt.X * uScale, pt.Y * vScale );
					Gl.glVertex3f( pt.X, height, pt.Y );
				}
			}


			RenderFloor( node.InFront );
			RenderFloor( node.Behind );
		}

		/// <summary>
		/// Rebuilds the current display list
		/// </summary>
		private void RebuildWallDisplayList( )
		{
			DestroyDisplayList( ref m_WallDisplayList );

			if ( Csg != null )
			{
				m_WallDisplayList = Gl.glGenLists( 1 );
				Gl.glNewList( m_WallDisplayList, Gl.GL_COMPILE );

				Gl.glBegin( Gl.GL_TRIANGLES );
				RenderWall( Csg.Root );
				Gl.glEnd( );

				Gl.glEndList( );
			}
		}
		
		/// <summary>
		/// Rebuilds the current display list
		/// </summary>
		private void RebuildFloorDisplayList( )
		{
			DestroyDisplayList( ref m_FloorDisplayList );

			if ( Csg != null )
			{
				m_FloorDisplayList = Gl.glGenLists( 1 );
				Gl.glNewList( m_FloorDisplayList, Gl.GL_COMPILE );

				Gl.glBegin( Gl.GL_TRIANGLES );
				RenderFloor( Csg.Root );
				Gl.glEnd( );

				Gl.glEndList( );
			}
		}

		/// <summary>
		/// Destroys the current display list
		/// </summary>
		private static void DestroyDisplayList( ref int displayList )
		{
			if ( displayList != -1 )
			{
				Gl.glDeleteLists( displayList, 1 );
			}
			displayList = -1;
		}

		#endregion
	}
}
