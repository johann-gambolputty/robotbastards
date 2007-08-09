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
		/// Setup constructor
		/// </summary>
		/// <param name="set">Tile type set that this type belongs to</param>
		/// <param name="name">Name of this type</param>
		/// <param name="bmp">Tile type image</param>
		/// <param name="precedence">Tile precedence (determines rendering order)</param>
		public TileType( TileTypeSet set, string name, Bitmap bmp, int precedence )
		{
			Image = bmp;
			Name = name;
			Set = set;
			m_Precedence = precedence;
		}

		public void AddToTileTexture( TileTexture texture )
		{
			m_NoTransRect = new TileTexture.Rect( texture.Generate( this, TransitionCodes.All ) );

			//	TODO: AP: Use mirroring and rotation of texture rectangles to save space on tile set display texture

			for ( int cornerIndex = 1; cornerIndex < 16; ++cornerIndex )
			{
				m_CornerRects[ cornerIndex ] = new TileTexture.Rect( texture.Generate( this, TileTexture.IndexToCornerCode( cornerIndex ) ) );
			}
			for ( int edgeIndex = 1; edgeIndex < 16; ++edgeIndex )
			{
				m_EdgeRects[ edgeIndex ] = new TileTexture.Rect( texture.Generate( this, TileTexture.IndexToEdgeCode( edgeIndex ) ) );
			}
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
				m_Set.Add( this );
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
		/// The tile precedence
		/// </summary>
		public int Precedence
		{
			get { return m_Precedence; }
			set { m_Precedence = value; }
		}

		private string						m_Name;
		private TileTypeSet					m_Set;
		private int							m_Precedence;
		private Bitmap						m_Image;
		private TileTexture.Rect			m_NoTransRect;
		private readonly TileTexture.Rect[]	m_CornerRects	= new TileTexture.Rect[ 16 ];
		private readonly TileTexture.Rect[] m_EdgeRects		= new TileTexture.Rect[ 16 ];
	}
}
