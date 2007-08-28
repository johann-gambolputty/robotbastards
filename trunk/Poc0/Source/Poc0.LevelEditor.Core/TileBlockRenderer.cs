using System;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Renders blocks of tiles
	/// </summary>
	[Serializable]
	public abstract class TileBlockRenderer : TileGridRenderer
	{
		#region Public constructor

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		/// <param name="grid">Grid to render</param>
		/// <param name="editContext">Edit context to render on top of the grid</param>
		public TileBlockRenderer( TileGrid grid, EditModes.EditModeContext editContext ) :
			base( grid, editContext )
		{
			SetupGridGraphics( );
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Access to the tile edit context
		/// </summary>
		public override EditModes.EditModeContext EditContext
		{
			set
			{
				if ( EditContext != null )
				{
					EditContext.Selection.ObjectSelected -= OnObjectSelected;
					EditContext.Selection.ObjectDeselected -= OnObjectSelected;
				}
				base.EditContext = value;
				if ( EditContext != null )
				{
					EditContext.Selection.ObjectSelected += OnObjectSelected;
					EditContext.Selection.ObjectDeselected += OnObjectSelected;
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

		#region Public methods

		/// <summary>
		/// Renders the specified tile grid
		/// </summary>
		public override void Render( IRenderContext context )
		{
			RenderGridGraphics( );
		}

		#endregion

		#region Grid rendering

		/// <summary>
		/// Gets the number of tile blocks in a row of m_Blocks
		/// </summary>
		private int BlocksWidth
		{
			get { return m_Blocks.GetLength( 0 ); }
		}

		/// <summary>
		/// Gets the number of tile blocks in a column of m_Blocks
		/// </summary>
		private int BlocksHeight
		{
			get { return m_Blocks.GetLength( 1 ); }
		}

		/// <summary>
		/// Destroys a block that contains a given tile position
		/// </summary>
		/// <param name="x">Tile x coordinate</param>
		/// <param name="y">Tile y coordinate</param>
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
		/// Called when an object is added or removed from the current selection
		/// </summary>
		private void OnObjectSelected( object obj )
		{
			Tile tile = obj as Tile;
			if ( tile != null )
			{
				OnTileChanged( tile );
			}
		}

		/// <summary>
		/// Called when a tile in the grid changes
		/// </summary>
		private void OnTileChanged( Tile tile )
		{
			//	Invalidate grid graphics - they'll get regenerated before the next render
			//	The blocks associated with adjacent tiles are being destroyed to deal with tile being on a corner, or edge (i.e. changing
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

			m_TileTextureSampler = RenderFactory.Instance.NewTextureSampler2d( );
			m_TileTextureSampler.Mode = TextureMode.Modulate;
			m_TileTextureSampler.MinFilter = TextureFilter.LinearTexel;
			m_TileTextureSampler.MagFilter = TextureFilter.LinearTexel;
			m_TileTextureSampler.WrapS = TextureWrap.Repeat;
			m_TileTextureSampler.WrapT = TextureWrap.Repeat;

			m_TileTextureSampler.Texture = Grid.Set.DisplayTexture;
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
			if ( m_TileRenderer == null )
			{
				m_TileRenderer = NewTileRenderer( Grid.Set );
			}

			Renderer.Instance.PushRenderState( m_TileRenderState );
			m_TileTextureSampler.Begin( );

			foreach ( TileBlock block in m_Blocks )
			{
				if ( !block.Valid )
				{
					block.Update( Grid, m_TileRenderer );
				}
				block.Render( );
			}

			m_TileTextureSampler.End( );
			Renderer.Instance.PopRenderState( );
		}

		#endregion

		#region Protected types

		/// <summary>
		/// Helper for rendering individual tiles by layering together contributions from adjacent tiles
		/// </summary>
		protected abstract class TileRenderer
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public TileRenderer( TileTypeSet set )
			{
				m_Set = set;
				m_Codes = new byte[ set.Count ];
			}

			/// <summary>
			/// Adds a tile to be rendered
			/// </summary>
			public void AddTile( TileGrid grid, int x, int y, byte code )
			{
				if ( grid.Contains( x, y ) )
				{
					int index = grid[ x, y ].TileType.Index;
					m_Codes[ index ] |= code;
					if ( code == TransitionCodes.All )
					{
						m_CentreIndex = index;
					}
				}
			}

			/// <summary>
			/// Renders the tiles added by <see cref="AddTile"/>
			/// </summary>
			/// <remarks>
			/// Classes extending TileRenderer should handle the selected flag, 'cos it ain't done here
			/// </remarks>
			public virtual void Render( )
			{
				//	First pass: Because the tile being rendered will overwrite any lower-index tiles, don't render those tiles at all
				for ( int index = 0; index < m_CentreIndex; ++index )
				{
					m_Codes[ index ] = 0;
				}

				for ( int index = 0; index < m_Codes.Length; ++index )
				{
					if ( m_Codes[ index ] == 0 )
					{
						continue;
					}

					if ( index == m_CentreIndex )
					{
						RenderType( m_Set[ index ], TransitionCodes.All );
					}
					else
					{
						//	TODO: AP: Corners covered by edges should not be rendered
						RenderType( m_Set[ index ], ( byte )( m_Codes[ index ] & TransitionCodes.Edges ) );
						RenderType( m_Set[ index ], ( byte )( m_Codes[ index ] & TransitionCodes.Corners ) );
					}

					m_Codes[ index ] = 0;
				}
			}

			/// <summary>
			/// Renders a tile type at a given screen position
			/// </summary>
			protected abstract void RenderType( TileType type, byte code );

			private int m_CentreIndex;
			private readonly TileTypeSet m_Set;
			private readonly byte[] m_Codes;
		}

		/// <summary>
		/// A block of tiles, rendered as a single lump for rendering and editing speed
		/// </summary>
		/// <remarks>
		/// A tile block is a block of tiles with a width and height of <see cref="TileBlockSize"/>
		/// </remarks>
		protected abstract class TileBlock
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
			public abstract bool Valid
			{
				get;
			}

			/// <summary>
			/// Destroys tile block rendering resources
			/// </summary>
			public abstract void Destroy( );

			/// <summary>
			/// Updates the tile block
			/// </summary>
			public abstract void Update( TileGrid grid, TileRenderer renderer );

			/// <summary>
			/// Renders this tile block
			/// </summary>
			public abstract void Render( );

			protected readonly int m_X;
			protected readonly int m_Y;
			protected readonly int m_MaxX;
			protected readonly int m_MaxY;
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Creates a new tile block
		/// </summary>
		protected abstract TileBlock NewTileBlock( int x, int y, int width, int height );

		/// <summary>
		/// Creates a new tile renderer
		/// </summary>
		protected abstract TileRenderer NewTileRenderer( TileTypeSet set );

		#endregion

		#region Private members

		/// <summary>
		/// Width of a tile block (in tiles)
		/// </summary>
		private const int TileBlockSize = 8;

		private RenderState m_TileRenderState;
		private TextureSampler2d m_TileTextureSampler;
		private TileBlock[ , ] m_Blocks;
		private TileRenderer m_TileRenderer;

		/// <summary>
		/// Creates tile blocks that cover an entire grid
		/// </summary>
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

					TileBlock block = NewTileBlock( x, y, width, height );
					m_Blocks[ blockX, blockY ] = block;
				}
			}
		}

		/// <summary>
		/// Sets up the renderer (I don't like the warnings in the constructor D-: )
		/// </summary>
		protected void Setup( TileGrid grid, EditModes.EditModeContext editContext )
		{
			Grid = grid;
			EditContext = editContext;
			SetupGridGraphics( );
		}


		#endregion
	}
}
