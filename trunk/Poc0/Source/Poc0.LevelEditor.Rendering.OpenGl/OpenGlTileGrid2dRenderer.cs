using System.Drawing;
using Poc0.LevelEditor.Core;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.OpenGl;
using Tao.OpenGl;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	/// <summary>
	/// Renders a tile grid and tile selection using 2d opengl graphics. BORING.
	/// </summary>
	public sealed class OpenGlTileGrid2dRenderer : TileGrid2dRenderer
	{
		#region Public constructor

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		/// <param name="grid">Grid to render</param>
		/// <param name="editState">Tile grid edit state to render on top of the grid</param>
		public OpenGlTileGrid2dRenderer( TileGrid grid, TileGridEditState editState, TileTransitionMasks masks )
		{
			m_Masks = masks;

			Grid = grid;
			EditState = editState;

			SetupGridGraphics( );
			SetupEditStateGraphics( );
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Access to the tile edit state
		/// </summary>
		public override TileGridEditState EditState
		{
			set
			{
				if ( EditState != null )
				{
					EditState.TileSelected -= OnTileChanged;
					EditState.TileDeselected -= OnTileChanged;
				}
				base.EditState = value;
				if ( EditState != null )
				{
					EditState.TileSelected += OnTileChanged;
					EditState.TileDeselected += OnTileChanged;
				}
			}
		}

		/// <summary>
		/// Access to the tile grid
		/// </summary>
		public override TileGrid Grid
		{
			set
			{
				if ( Grid != null )
				{
					Grid.TileChanged -= OnTileChanged;
				}
				base.Grid = value;
				if ( Grid != null )
				{
					CreateTileBlocks( Grid );
					Grid.TileChanged += OnTileChanged;
				}
			}
		}

		#endregion

		private void CreateTileBlocks( TileGrid grid )
		{
			int blocksWidthRem = grid.Width % TileBlockSize;
			int blocksHeightRem = ( grid.Height % TileBlockSize );
			int blocksWidth = ( grid.Width / TileBlockSize ) + ( blocksWidthRem > 0 ? 1 : 0 );
			int blocksHeight = ( grid.Height / TileBlockSize ) + ( blocksHeightRem > 0 ? 1 : 0 );

			m_Blocks = new TileBlock[ blocksWidth, blocksHeight ];

			for ( int blockY = 0; blockY < blocksHeight; ++blockY )
			{
				int y = blockY * TileBlockSize;
				int height = ( blockY == 0 ) && ( blocksHeightRem > 0 ) ? blocksHeightRem : TileBlockSize;

				for ( int blockX = 0; blockX < blocksWidth; ++blockX )
				{
					int x = blockX * TileBlockSize;
					int width = ( blockX == 0 ) && ( blocksWidthRem > 0 ) ? blocksWidthRem : TileBlockSize;

					TileBlock block = new TileBlock( x, y, width, height );
					m_Blocks[ blockX, blockY ] = block;
				}
			}
			
		}

		#region Edit state rendering

		private RenderState m_EditRenderState;

		/// <summary>
		/// Sets up edit state graphics
		/// </summary>
		private void SetupEditStateGraphics( )
		{
			m_EditRenderState = RenderFactory.Instance.NewRenderState( );

			m_EditRenderState
				.DisableCap( RenderStateFlag.DepthTest )
				.EnableCap( RenderStateFlag.Blend )
				.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha )
				.SetPolygonRenderingMode( PolygonRenderMode.Fill )
				.DisableLighting( );
		}

		#endregion

		#region Grid rendering

		private RenderState m_TileRenderState;
		private OpenGlTextureSampler2d m_TileTextureSampler;
		private OpenGlTextureSampler2d m_MaskTextureSampler;
		private TileBlock[,] m_Blocks;

		/// <summary>
		/// Called when a tile in the grid changes
		/// </summary>
		private void OnTileChanged( Tile tile )
		{
			//	Invalidate grid graphics - they'll get regenerated before the next render
			m_Blocks[ tile.GridX / TileBlockSize, tile.GridY / TileBlockSize ].Destroy( );
		}

		/// <summary>
		/// Sets up prerequisites for rendering grid graphics. Called by constructor
		/// </summary>
		private void SetupGridGraphics( )
		{
			m_TileRenderState = RenderFactory.Instance.NewRenderState( );
			m_TileRenderState
				.DisableCap( RenderStateFlag.DepthTest )
				.SetPolygonRenderingMode( PolygonRenderMode.Fill )
				.EnableCap( RenderStateFlag.Texture2dUnit0 )
				.EnableCap( RenderStateFlag.Texture2dUnit1 )
				.DisableLighting( );

			m_TileTextureSampler = new OpenGlTextureSampler2d( );
			m_TileTextureSampler.Mode = TextureMode.Modulate;
			m_TileTextureSampler.MinFilter = TextureFilter.LinearTexel;
			m_TileTextureSampler.MagFilter = TextureFilter.LinearTexel;

			m_TileTextureSampler.Texture = ( OpenGlTexture2d )Grid.Set.DisplayTexture;

			m_MaskTextureSampler = new OpenGlTextureSampler2d( );
			m_MaskTextureSampler.Mode = TextureMode.Modulate;
			m_MaskTextureSampler.MinFilter = TextureFilter.NearestTexel;
			m_MaskTextureSampler.MagFilter = TextureFilter.NearestTexel;

			//m_MaskTextureSampler.Texture = ( OpenGlTexture2d )TileTypeTransitions.TransitionsTexture;
			m_MaskTextureSampler.Texture = ( OpenGlTexture2d )m_Masks.Texture;
		}

		/// <summary>
		/// Display the current grid graphics (regenerating them if not valid)
		/// </summary>
		private void RenderGridGraphics( )
		{
			if ( Grid == null )
			{
				return;
			}

			Renderer.Instance.PushRenderState( m_TileRenderState );
			m_TileTextureSampler.Begin( );

			//glTexEnvi( GL_TEXTURE_ENV, GL_COMBINE_RGB, GL_DOT3_RGB );
			//glTexEnvi( GL_TEXTURE_ENV, GL_SOURCE0_RGB, GL_TEXTURE0 );
			//glTexEnvi( GL_TEXTURE_ENV, GL_OPERAND0_RGB, GL_SRC_COLOR );
			//glTexEnvi( GL_TEXTURE_ENV, GL_SOURCE1_RGB, GL_PRIMARY_COLOR );
			//glTexEnvi( GL_TEXTURE_ENV, GL_OPERAND1_RGB, GL_SRC_COLOR );

			//glTexEnvi( GL_TEXTURE_ENV, GL_COMBINE_ALPHA, GL_MODULATE );
			//glTexEnvi( GL_TEXTURE_ENV, GL_SOURCE0_ALPHA, GL_TEXTURE0 );
			//glTexEnvi( GL_TEXTURE_ENV, GL_OPERAND0_ALPHA, GL_SRC_ALPHA );
			//glTexEnvi( GL_TEXTURE_ENV, GL_SOURCE1_ALPHA, GL_PRIMARY_COLOR );
			//glTexEnvi( GL_TEXTURE_ENV, GL_OPERAND1_ALPHA, GL_SRC_ALPHA );

			m_MaskTextureSampler.Begin( );

			foreach ( TileBlock block in m_Blocks )
			{
				if ( !block.Valid )
				{
					block.Update( Grid, m_Masks );
				}
				block.Render( );
			}

			m_MaskTextureSampler.End( );
			m_TileTextureSampler.End( );
			Renderer.Instance.PopRenderState( );
		}

		#endregion

		#region Private types

		private class TileBlock
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public TileBlock( int x, int y, int width, int height )
			{
				m_X = x;
				m_Y = y;
				m_MaxX = x + width;
				m_MaxY = y + height;
			}

			/// <summary>
			/// Returns true if the tile block is valid and can be rendered
			/// </summary>
			public bool Valid
			{
				get { return m_DisplayList != InvalidDisplayList;  }
			}

			/// <summary>
			/// Destroys tile block rendering resources
			/// </summary>
			public void Destroy( )
			{
				if ( m_DisplayList != InvalidDisplayList )
				{
					Gl.glDeleteLists( m_DisplayList, 1 );
					m_DisplayList = InvalidDisplayList;
				}
			}

			private static byte GetCode( TileGrid grid, int bitPos, int x, int y, int offX, int offY )
			{
				TileType type = grid[ x, y ].TileType;

				int checkX = Utils.Clamp( x + offX, 0, grid.Width - 1 );
				int checkY = Utils.Clamp( y + offY, 0, grid.Height - 1 );

				TileType otherType = grid[ checkX, checkY ].TileType;

				return ( byte )( type == otherType ? ( 1 << bitPos ) : 0 );
			}

			/// <summary>
			/// Updates the tile block
			/// </summary>
			public void Update( TileGrid grid, TileTransitionMasks masks )
			{
				if ( m_DisplayList != InvalidDisplayList )
				{
					return;
				}

				int displayList = Gl.glGenLists( 1 );
				Gl.glNewList( displayList, Gl.GL_COMPILE );

				Gl.glBegin( Gl.GL_QUADS );

				float invTexWidth = 1.0f / grid.Set.DisplayTexture.Width;
				float invTexHeight = 1.0f / grid.Set.DisplayTexture.Height;
				int screenY = m_Y * TileScreenHeight;

				for ( int tileY = m_Y; tileY < m_MaxY; ++tileY, screenY += TileScreenHeight )
				{
					int screenX = m_X * TileScreenWidth;
					for ( int tileX = m_X; tileX < m_MaxX; ++tileX, screenX += TileScreenWidth )
					{
						Tile tile = grid[ tileX, tileY ];
						Rectangle rect = tile.TileType.TextureRectangle;

						//	TODO: AP: Should store normalised texture rectangle in tile type
						float minU = rect.Left * invTexWidth;
						float maxU = rect.Right * invTexWidth;
						float minV = rect.Top * invTexHeight;
						float maxV = rect.Bottom * invTexHeight;

						if ( tile.Selected )
						{
							Gl.glColor3ub( 0xff, 0x00, 0x00 );
						}
						else
						{
							Gl.glColor3ub( 0xff, 0xff, 0xff );
						}

						byte code = 0;
						code |= GetCode( grid, 0, tileX, tileY, -1, -1 );
						code |= GetCode( grid, 1, tileX, tileY,  0, -1 );
						code |= GetCode( grid, 2, tileX, tileY,  1, -1 );
						code |= GetCode( grid, 3, tileX, tileY,  1,  0 );
						code |= GetCode( grid, 4, tileX, tileY,  1,  1 );
						code |= GetCode( grid, 5, tileX, tileY,  0,  1 );
						code |= GetCode( grid, 6, tileX, tileY, -1,  1 );
						code |= GetCode( grid, 7, tileX, tileY, -1,  0 );

						TileTransitionMasks.TextureRect maskRect = masks.GetTextureCoords( code );

						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, minU, minV );
						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE1_ARB, maskRect.TopLeft.X, maskRect.TopLeft.Y );
						Gl.glVertex2i( screenX, screenY );

						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, maxU, minV );
						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE1_ARB, maskRect.TopRight.X, maskRect.TopRight.Y );
						Gl.glVertex2i( screenX + TileScreenWidth, screenY );

						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, maxU, maxV );
						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE1_ARB, maskRect.BottomRight.X, maskRect.BottomRight.Y );
						Gl.glVertex2i( screenX + TileScreenWidth, screenY + TileScreenHeight );

						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, minU, maxV );
						Gl.glMultiTexCoord2f( Gl.GL_TEXTURE1_ARB, maskRect.BottomLeft.X, maskRect.BottomLeft.Y );
						Gl.glVertex2i( screenX, screenY + TileScreenHeight );
					}
				}
				Gl.glEnd( );
				Gl.glEndList( );

				m_DisplayList = displayList;
			}

			public void Render( )
			{
				Gl.glCallList( m_DisplayList );
			}

			private int m_X;
			private int m_Y;
			private int m_MaxX;
			private int m_MaxY;
			private int m_DisplayList = InvalidDisplayList;
		}

		#endregion

		#region Private members

		/// <summary>
		/// The constant that represents an invalid display list in opengl
		/// </summary>
		private const int InvalidDisplayList = -1;

		private const int TileBlockSize		= 8;
		private const int TileScreenWidth	= 32;
		private const int TileScreenHeight	= 32;

		private readonly TileTransitionMasks m_Masks;

		/// <summary>
		/// Renders the specified tile grid
		/// </summary>
		public override void Render( IRenderContext context )
		{
			RenderGridGraphics( );
			RenderTileUnderCursor( );
		}

		private void RenderTileUnderCursor( )
		{
			if ( EditState == null || EditState.TileUnderCursor == null )
			{
				return;
			}
			Tile tile = EditState.TileUnderCursor;

			Renderer.Instance.PushRenderState( m_EditRenderState );

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

			Renderer.Instance.PopRenderState( );
		}

		#endregion
	}
}
