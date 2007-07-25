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
		/// <param name="x">X coordinate of top left of texture rectangle</param>
		/// <param name="y">Y coordinate of top left of texture rectangle</param>
		/// <param name="width">Width of the texture rectangle</param>
		/// <param name="height">Height of the texture rectangle</param>
		public TileType( TileTypeSet set, string name, int x, int y, int width, int height )
		{
			Name = name;
			Set = set;
			m_TextureRect = new Rectangle( x, y, width, height );
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
		/// Texture rectangle (area on the tile type set display texture that this type uses)
		/// </summary>
		public Rectangle TextureRectangle
		{
			get { return m_TextureRect; }
			set { m_TextureRect = value; }
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
		/// Creates a bitmap of the tile type
		/// </summary>
		public Bitmap CreateBitmap( PixelFormat format )
		{
			Bitmap setBmp = Set.DisplayTextureBitmap;
			return setBmp.Clone( m_TextureRect, format );
		}

		private string		m_Name;
		private Rectangle	m_TextureRect = new Rectangle( 0, 0, 32, 32 );
		private TileTypeSet	m_Set;
	}
}
