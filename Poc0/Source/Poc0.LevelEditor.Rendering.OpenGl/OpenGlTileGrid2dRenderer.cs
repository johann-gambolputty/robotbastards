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


		private int BlocksWidth
		{
			get { return m_Blocks.GetLength( 0 ); }
		}

		private int BlocksHeight
		{
			get { return m_Blocks.GetLength( 1 ); }
		}

		private void DestroyBlock( int x, int y )
		{
			int blockX = x / TileBlockSize;
			int blockY = y / TileBlockSize;

			if ( ( blockX >= 0 ) && ( blockX < BlocksWidth ) && ( blockY >= 0 ) && ( blockY < BlocksHeight ) )
			{
				m_Blocks[ blockX, blockY ].Destroy( );
			}
		}

		/// <summary>
		/// Called when a tile in the grid changes
		/// </summary>
		private void OnTileChanged( Tile tile )
		{
			//	Invalidate grid graphics - they'll get regenerated before the next render
			//	Note: The blocks associated with adjacent tiles are being destroyed to deal with tile being on a corner, or edge (i.e. changing
			//	tile will affect the graphics of an adjacent block)
			DestroyBlock( tile.GridX, tile.GridY );
			DestroyBlock( tile.GridX - 1, tile.GridY - 1 );
			DestroyBlock( tile.GridX, tile.GridY - 1 );
			DestroyBlock( tile.GridX + 1, tile.GridY - 1 );
			DestroyBlock( tile.GridX - 1, tile.GridY );
			DestroyBlock( tile.GridX + 1, tile.GridY - 1 );
			DestroyBlock( tile.GridX - 1, tile.GridY + 1 );
			DestroyBlock( tile.GridX, tile.GridY + 1 );
			DestroyBlock( tile.GridX + 1, tile.GridY + 1 );
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
				.EnableCap( RenderStateFlag.Blend )
				.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha )
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

			//m_MaskTextureSampler.Begin( );

			foreach ( TileBlock block in m_Blocks )
			{
				if ( !block.Valid )
				{
					block.Update( Grid, m_Masks );
				}
				block.Render( );
			}

			//m_MaskTextureSampler.End( );
			m_TileTextureSampler.End( );
			Renderer.Instance.PopRenderState( );
		}

		#endregion

		#region Private types

		private class TileRenderer
		{
			public TileRenderer( TileTypeSet set )
			{
				m_Set = set;
				m_Codes = new byte[ set.Count ];
			}

			public void AddTile( TileGrid grid, int x, int y, byte code )
			{
				if ( grid.Contains( x, y ) )
				{
					//	TODO: AP: Should use TileType.Index
					int index = grid[ x, y ].TileType.Precedence;
					m_Codes[ index ] |= code;
					if ( code == TransitionCodes.All )
					{
						m_CentreIndex = index;
					}
				}
			}

			public void Render( int x, int y, bool selected )
			{
				if ( selected )
				{
					Gl.glColor3ub( 0xff, 0x00, 0x00 );
				}
				else
				{
					Gl.glColor3ub( 0xff, 0xff, 0xff );
				}

				for ( int index = 0; index < m_Codes.Length; ++index )
				{
					if ( m_Codes[ index ] == 0 )
					{
						continue;
					}

					if ( index == m_CentreIndex )
					{
						RenderType( m_Set[ index ], x, y, TransitionCodes.All );
					}
					else
					{
						//	TODO: AP: This is lazy
						RenderType( m_Set[ index ], x, y, ( byte )( m_Codes[ index ] & TransitionCodes.Corners ) );
						RenderType( m_Set[ index ], x, y, ( byte )( m_Codes[ index ] & TransitionCodes.Edges ) );
					}

					m_Codes[ index ] = 0;
				}
			}

			private static void RenderType( TileType type, int x, int y, byte code )
			{
				if ( code == 0 )
				{
					return;
				}
				TileTexture.Rect rect = type.GetTextureRectangle( code );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MinU, rect.MinV );
				Gl.glVertex2i( x, y );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MaxU, rect.MinV );
				Gl.glVertex2i( x + TileScreenWidth, y );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MaxU, rect.MaxV );
				Gl.glVertex2i( x + TileScreenWidth, y + TileScreenHeight );

				Gl.glMultiTexCoord2f( Gl.GL_TEXTURE0_ARB, rect.MinU, rect.MaxV );
				Gl.glVertex2i( x, y + TileScreenHeight );	
			}

			private int m_CentreIndex;
			private readonly TileTypeSet m_Set;
			private readonly byte[] m_Codes;
		}

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
			public void Update( TileGrid grid, TileTransitionMasks masks )
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
				TileRenderer renderer = new TileRenderer( grid.Set );

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

						renderer.Render( screenX, screenY, grid[ tileX, tileY ].Selected );
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

			private readonly int m_X;
			private readonly int m_Y;
			private readonly int m_MaxX;
			private readonly int m_MaxY;
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
