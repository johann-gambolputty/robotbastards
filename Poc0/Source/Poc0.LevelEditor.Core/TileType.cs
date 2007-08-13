using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Defines the type of a tile
	/// </summary>
	public class TileType
	{
		/// <summary>
		/// Setup constructor. Generates transition bitmaps automatically
		/// </summary>
		/// <param name="set">Tile type set that this type belongs to</param>
		/// <param name="name">Name of this type</param>
		/// <param name="bmp">Tile type image</param>
		/// <param name="hardEdgeSize">Size of the hard edge in the automatically generated transition textures</param>
		/// <param name="softEdgeSize">Size of the soft edge in the automatically generated transition textures</param>
		public TileType( TileTypeSet set, string name, Bitmap bmp, int hardEdgeSize, int softEdgeSize )
		{
			Image = bmp;
			Name = name;
			Set = set;

			GenerateTransitions( m_Set.TileTexture, hardEdgeSize, softEdgeSize );
		}

		/// <summary>
		/// Sets a tile's type. Used as a delegate in <see cref="TileGridEditState.OnPaint"/>
		/// </summary>
		public void SetTileToType( Tile tile, float x, float y )
		{
			tile.TileType = this;
		}

		/// <summary>
		/// Gets the tile texture area used by this tile type for a given transition code
		/// </summary>
		/// <param name="code">Transition code</param>
		/// <returns>Tile texture area</returns>
		public TileTexture.Rect GetTextureRectangle(byte code)
		{
			if ( code == TransitionCodes.All )
			{
				return m_NoTransRect;
			}

			if ( ( code & TransitionCodes.Corners ) != 0 )
			{
				return m_CornerRects[ TileTexture.CornerCodeToIndex( code ) ];
			}
			return m_EdgeRects[ TileTexture.EdgeCodeToIndex( code ) ];
		}

		/// <summary>
		/// Gets the image associated with this tile type
		/// </summary>
		public Bitmap Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}

		/// <summary>
		/// Gets the <see cref="TileTypeSet"/> that this type belongs to
		/// </summary>
		public TileTypeSet Set
		{
			get { return m_Set; }
			set
			{
				if ( m_Set != null )
				{
					throw new ApplicationException( "Can only set a tile type's set once" );
				}
				m_Set = value;
				m_Index = m_Set.Add( this );
			}
		}

		/// <summary>
		/// The name of the tile type
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// The tile index
		/// </summary>
		public int Index
		{
			get { return m_Index; }
		}

		/// <summary>
		/// Tile type base height
		/// </summary>
		public float BaseHeight
		{
			get { return m_BaseHeight; }
			set { m_BaseHeight = value; }
		}

		#region Private members

		private string						m_Name;
		private TileTypeSet					m_Set;
		private int							m_Index;
		private Bitmap						m_Image;
		private TileTexture.Rect			m_NoTransRect;
		private float						m_BaseHeight;
		private readonly TileTexture.Rect[]	m_CornerRects	= new TileTexture.Rect[ 16 ];
		private readonly TileTexture.Rect[] m_EdgeRects		= new TileTexture.Rect[ 16 ];

		/// <summary>
		/// Adds this tile type to a given tile texture, generating transition textures automatically
		/// </summary>
		private void GenerateTransitions( TileTexture texture, int hardEdgeSize, int softEdgeSize )
		{
			m_NoTransRect = new TileTexture.Rect( texture.Add( Image ) );

			//	TODO: AP: Use mirroring and rotation of texture rectangles to save space on tile set display texture

			for ( int cornerIndex = 1; cornerIndex < 16; ++cornerIndex )
			{
				m_CornerRects[ cornerIndex ] = new TileTexture.Rect( texture.Generate( this, TileTexture.IndexToCornerCode( cornerIndex ), hardEdgeSize, softEdgeSize ) );
			}
			for ( int edgeIndex = 1; edgeIndex < 16; ++edgeIndex )
			{
				m_EdgeRects[ edgeIndex ] = new TileTexture.Rect( texture.Generate( this, TileTexture.IndexToEdgeCode( edgeIndex ), hardEdgeSize, softEdgeSize ) );
			}
		}

		#endregion

	}
}
