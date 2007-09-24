using System;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Rendering;
using Tao.OpenGl;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	/// <summary>
	/// Renders a tile grid and tile selection using 2d opengl graphics. BORING.
	/// </summary>
	[Serializable]
	public sealed class OpenGlTileBlock2dRenderer : TileBlock2dRenderer
	{
		#region Public constructor

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		/// <param name="grid">Grid to render</param>
		/// <param name="editContext">Tile grid edit state to render on top of the grid</param>
		public OpenGlTileBlock2dRenderer( TileGrid grid, EditModeContext editContext ) :
			base( grid, editContext )
		{
			SetupEditStateGraphics( );
		}

		#endregion

		protected override TileRenderer NewTileRenderer( TileTypeSet set )
		{
			return new OpenGlTileRenderer( set );
		}

		protected override TileBlock NewTileBlock( int x, int y, int width, int height )
		{
			return new OpenGlTileBlock( x, y, width, height );
		}

		/// <summary>
		/// Helper for rendering individual tiles by layering together contributions from adjacent tiles
		/// </summary>
		[Serializable] // TODO: AP: Get rid of this...
		private class OpenGlTileRenderer : TileRenderer
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public OpenGlTileRenderer( TileTypeSet set ) :
				base( set )
			{
			}

			public void PreRender( int screenX, int screenY, bool selected )
			{
				m_ScreenX = screenX;
				m_ScreenY = screenY;
				m_Selected = selected;
			}

			private int m_ScreenX;
			private int m_ScreenY;
			private bool m_Selected;

			/// <summary>
			/// Renders the tiles added by <see cref="TileRenderer.AddTile"/>
			/// </summary>
			public override void Render( )
			{
				if ( m_Selected )
				{
					Gl.glColor3ub( 0xff, 0x00, 0x00 );
				}
				else
				{
					Gl.glColor3ub( 0xff, 0xff, 0xff );
				}

				base.Render( );
			}
			
			/// <summary>
			/// Renders a tile type at a given screen position
			/// </summary>
			protected override void RenderType( TileType type, byte code )
			{
				if ( code == 0 )
				{
					return;
				}
				TileTexture.Rect rect = type.GetTextureRectangle( code );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MinU, rect.MinV );
				Gl.glVertex2i( m_ScreenX, m_ScreenY );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MaxU, rect.MinV );
				Gl.glVertex2i( m_ScreenX + TileScreenWidth, m_ScreenY );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MaxU, rect.MaxV );
				Gl.glVertex2i( m_ScreenX + TileScreenWidth, m_ScreenY + TileScreenHeight );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MinU, rect.MaxV );
				Gl.glVertex2i( m_ScreenX, m_ScreenY + TileScreenHeight );	
			}
		}

		private class OpenGlTileBlock : TileBlock
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public OpenGlTileBlock( int x, int y, int width, int height ) :
				base( x, y, width, height )
			{
			}

			/// <summary>
			/// Returns true if the tile block is valid and can be rendered
			/// </summary>
			public override bool Valid
			{
				get { return m_DisplayList != InvalidDisplayList;  }
			}

			/// <summary>
			/// Destroys tile block rendering resources
			/// </summary>
			public override void Destroy( )
			{
				if ( m_DisplayList != InvalidDisplayList )
				{
					Gl.glDeleteLists( m_DisplayList, 1 );
					m_DisplayList = InvalidDisplayList;
				}
			}

			/// <summary>
			/// Updates the tile block
			/// </summary>
			public override void Update( TileGrid grid, TileRenderer renderer )
			{
				if ( m_DisplayList != InvalidDisplayList )
				{
					return;
				}

				int displayList = Gl.glGenLists( 1 );
				Gl.glNewList( displayList, Gl.GL_COMPILE );

				Gl.glBegin( Gl.GL_QUADS );

				int screenY = m_Y * TileScreenHeight;

				// TODO: AP: Can be created once for all tile blocks in the grid
				OpenGlTileRenderer glRenderer = ( OpenGlTileRenderer )renderer;

				Gl.glColor3ub( 0xff, 0xff, 0xff );
				for ( int tileY = m_Y; tileY < m_MaxY; ++tileY, screenY += TileScreenHeight )
				{
					int screenX = m_X * TileScreenWidth;
					for ( int tileX = m_X; tileX < m_MaxX; ++tileX, screenX += TileScreenWidth )
					{
						renderer.AddTile( grid, tileX - 1, tileY - 1, TransitionCodes.TopLeftCorner );
						renderer.AddTile( grid, tileX + 0, tileY - 1, TransitionCodes.TopEdge );
						renderer.AddTile( grid, tileX + 1, tileY - 1, TransitionCodes.TopRightCorner );

						renderer.AddTile( grid, tileX - 1, tileY + 0, TransitionCodes.LeftEdge );
						renderer.AddTile( grid, tileX + 0, tileY + 0, TransitionCodes.All );
						renderer.AddTile( grid, tileX + 1, tileY + 0, TransitionCodes.RightEdge );

						renderer.AddTile( grid, tileX - 1, tileY + 1, TransitionCodes.BottomLeftCorner );
						renderer.AddTile( grid, tileX + 0, tileY + 1, TransitionCodes.BottomEdge );
						renderer.AddTile( grid, tileX + 1, tileY + 1, TransitionCodes.BottomRightCorner );

						glRenderer.PreRender( screenX, screenY, grid[ tileX, tileY ].Selected );
						renderer.Render( );
					}
				}
				Gl.glEnd( );
				Gl.glEndList( );

				m_DisplayList = displayList;
			}

			public override void Render( )
			{
				Gl.glCallList( m_DisplayList );
			}

			private int m_DisplayList = InvalidDisplayList;
		}

		#region Private members

		/// <summary>
		/// The constant that represents an invalid display list in opengl
		/// </summary>
		private const int InvalidDisplayList = -1;

		/// <summary>
		/// The width of a single tile on the screen
		/// </summary>
		private const int TileScreenWidth	= TileCamera2d.TileScreenWidth;

		/// <summary>
		/// The height of a single tile on the screen
		/// </summary>
		private const int TileScreenHeight = TileCamera2d.TileScreenHeight;


		/// <summary>
		/// Renders the specified tile grid
		/// </summary>
		public override void Render( IRenderContext context )
		{
			base.Render( context );
			RenderTileUnderCursor( );
		}

		/// <summary>
		/// Renders a transparent overlay over the tile under the edit cursor
		/// </summary>
		private void RenderTileUnderCursor( )
		{
			if ( EditContext == null || EditContext.TileUnderCursor == null )
			{
				return;
			}
			Tile tile = EditContext.TileUnderCursor;

			Graphics.Renderer.PushRenderState( m_EditRenderState );

			int minX = tile.GridX * TileScreenWidth;
			int minY = tile.GridY * TileScreenHeight;
			int maxX = minX + TileScreenWidth;
			int maxY = minY + TileScreenHeight;

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glColor4ub( 0x00, 0xff, 0x00, 0x80 );

			Gl.glVertex2i( minX, minY );
			Gl.glVertex2i( maxX, minY );
			Gl.glVertex2i( maxX, maxY );
			Gl.glVertex2i( minX, maxY );

			Gl.glEnd( );

			Graphics.Renderer.PopRenderState( );
		}

		#endregion

		#region Edit state rendering

		private RenderState m_EditRenderState;

		/// <summary>
		/// Sets up edit state graphics
		/// </summary>
		private void SetupEditStateGraphics( )
		{
			m_EditRenderState = Graphics.Factory.NewRenderState( );

			m_EditRenderState
				.DisableCap( RenderStateFlag.DepthTest )
				.EnableCap( RenderStateFlag.Blend )
				.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha )
				.SetPolygonRenderingMode( PolygonRenderMode.Fill )
				.DisableLighting( );
		}

		#endregion

	}
}
