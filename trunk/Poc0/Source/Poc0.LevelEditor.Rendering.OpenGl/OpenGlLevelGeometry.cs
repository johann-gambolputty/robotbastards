using System;
using System.Drawing;
using System.Runtime.Serialization;
using Poc0.LevelEditor.Core;
using Rb.Core.Assets;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Tao.OpenGl;
using Rb.Core.Maths;
using Graphics=Rb.Rendering.Graphics;

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
				wallState.SetColour( Color.DarkOrange );
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
				floorState.SetColour( Color.Blue );

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

			//	Render selected walls
			Graphics.Renderer.PushRenderState( ms_SelState );

			Gl.glBegin( Gl.GL_QUADS );
			RenderSelectedWalls( csg.Root );
			Gl.glEnd( );
			Graphics.Renderer.PopRenderState( );

		}

		/// <summary>
		/// Renders semi-transparent coloured quads over selected/highlit walls
		/// </summary>
		private void RenderSelectedWalls( Csg.BspNode node )
		{
			if ( node == null )
			{
				return;
			}
			if ( node.Highlighted || node.Selected )
			{
				if ( node.Selected )
				{
					Gl.glColor4f( 1.0f, 0.0f, 0.0f, 0.5f );
				}
				else
				{
					Gl.glColor4f( 0.0f, 0.2f, 0.0f, 0.5f );
				}

				Point3[] pts = node.Quad;
				Gl.glVertex3f( pts[ 0 ].X, pts[ 0 ].Y, pts[ 0 ].Z );
				Gl.glVertex3f( pts[ 1 ].X, pts[ 1 ].Y, pts[ 1 ].Z );
				Gl.glVertex3f( pts[ 2 ].X, pts[ 2 ].Y, pts[ 2 ].Z );
				Gl.glVertex3f( pts[ 3 ].X, pts[ 3 ].Y, pts[ 3 ].Z );
			}
			RenderSelectedWalls( node.InFront );
			RenderSelectedWalls( node.Behind );
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

		private readonly static RenderState ms_SelState = SelectionState( );

		[NonSerialized]
		private int m_WallDisplayList = -1;

		[NonSerialized]
		private int m_FloorDisplayList = -1;

		//	TODO: AP: Update serialization of these fields
		private readonly Texture2d m_WallTextures;
		private readonly Texture2d m_FloorTextures;
		private readonly ITechnique m_WallTechnique;
		private readonly ITechnique m_FloorTechnique;

		/// <summary>
		/// Creates the render state used for rendering selected walls
		/// </summary>
		private static RenderState SelectionState( )
		{
			RenderState state = Graphics.Factory.NewRenderState( );
			state.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.One );
			state.DisableCap( RenderStateFlag.DepthTest );
			state.SetDepthOffset( -1.0f );
			return state;
		}

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

			Point3 pt0 = node.Quad[ 0 ];
			Point3 pt1 = node.Quad[ 1 ];
			Point3 pt2 = node.Quad[ 2 ];
			Point3 pt3 = node.Quad[ 3 ];

			float minU = 0;
			float maxU = 4;
			float minV = 0;
			float maxV = 4;

			Gl.glNormal3f( node.Plane.Normal.X, 0, node.Plane.Normal.Y );
			Gl.glTexCoord2f( minU, minV );
			Gl.glVertex3f( pt0.X, pt0.Y, pt0.Z );
			Gl.glTexCoord2f( maxU, minV );
			Gl.glVertex3f( pt1.X, pt1.Y, pt1.Z );
			Gl.glTexCoord2f( maxU, maxV );
			Gl.glVertex3f( pt2.X, pt2.Y, pt2.Z );

			Gl.glNormal3f( node.Plane.Normal.X, 0, node.Plane.Normal.Y );
			Gl.glTexCoord2f( maxU, maxV );
			Gl.glVertex3f( pt2.X, pt2.Y, pt2.Z );
			Gl.glTexCoord2f( minU, maxV );
			Gl.glVertex3f( pt3.X, pt3.Y, pt3.Z );
			Gl.glTexCoord2f( minU, minV );
			Gl.glVertex3f( pt0.X, pt0.Y, pt0.Z );

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