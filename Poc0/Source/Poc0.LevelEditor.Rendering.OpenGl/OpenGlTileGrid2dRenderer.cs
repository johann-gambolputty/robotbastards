using System.Drawing;
using Poc0.LevelEditor.Core;
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
		public OpenGlTileGrid2dRenderer( TileGrid grid, TileGridEditState editState )
		{
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
				//.EnableCap( RenderStateFlag.Texture2d )
				.DisableLighting( );
		}

		#endregion

		#region Grid rendering

		private RenderState m_TileRenderState;
		private OpenGlTextureSampler2d m_TileTextureSampler;
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
				//.EnableCap( RenderStateFlag.Blend )
				//.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha )
				.SetPolygonRenderingMode( PolygonRenderMode.Fill )
				.EnableCap( RenderStateFlag.Texture2d )
				.DisableLighting( );

			m_TileTextureSampler = new OpenGlTextureSampler2d( );
			m_TileTextureSampler.Mode = TextureMode.Modulate;
			m_TileTextureSampler.MinFilter = TextureFilter.LinearTexel;
			m_TileTextureSampler.MagFilter = TextureFilter.LinearTexel;
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
			m_TileTextureSampler.Texture = ( OpenGlTexture2d )Grid.Set.DisplayTexture;
			m_TileTextureSampler.Begin( );

			foreach ( TileBlock block in m_Blocks )
			{
				if ( !block.Valid )
				{
					block.Update( Grid );
				}
				block.Render( );
			}

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

			/// <summary>
			/// Updates the tile block
			/// </summary>
			public void Update( TileGrid grid )
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

						Gl.glTexCoord2f( minU, minV );
						Gl.glVertex2i( screenX, screenY );

						Gl.glTexCoord2f( maxU, minV );
						Gl.glVertex2i( screenX + TileScreenWidth, screenY );

						Gl.glTexCoord2f( maxU, maxV );
						Gl.glVertex2i( screenX + TileScreenWidth, screenY + TileScreenHeight );

						Gl.glTexCoord2f( minU, maxV );
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
