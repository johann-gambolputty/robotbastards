using System.Collections.Generic;
using System.Drawing;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores a set of TileType objects
	/// </summary>
	public class TileTypeSet
	{
		/// <summary>
		/// Creates a default tile type set
		/// </summary>
		public static TileTypeSet CreateDefaultTileTypeSet( )
		{
			TileTypeSet set = new TileTypeSet( );

			new TileType( set, "tile0", Properties.Resources.tile0, 0 );

			//new TileType( set, "tile1", Properties.Resources.tile1, x, 0, width, height, 1 );
			//x += width;

			//new TileType( set, "tile2", Properties.Resources.tile2, x, 0, width, height, 2 );

			return set;
		}

		/// <summary>
		/// Gets an indexed tile type
		/// </summary>
		public TileType this[ int index ]
		{
			get { return m_TileTypes[ index ]; }
			set { m_TileTypes[ index ] = value; }
		}

		/// <summary>
		/// Access to the display texture
		/// </summary>
		public Texture2d DisplayTexture
		{
			get { return m_DisplayTexture.Texture; }
		}

		/// <summary>
		/// Gets the display texture, converted to a bitmap
		/// </summary>
		public Bitmap DisplayTextureBitmap
		{
			get
			{
				if ( ( m_DisplayTextureBitmap == null ) && ( DisplayTexture != null ) )
				{
					m_DisplayTextureBitmap = DisplayTexture.ToBitmap( );
				}
				return m_DisplayTextureBitmap;
			}
		}

		/// <summary>
		/// Gets the number of types
		/// </summary>
		public int Count
		{
			get { return m_TileTypes.Count; }
		}

		/// <summary>
		/// Gets the tile types collection
		/// </summary>
		public IEnumerable< TileType > TileTypes
		{
			get { return m_TileTypes; }
		}

		/// <summary>
		/// Adds a tile type to the set
		/// </summary>
		public void Add( TileType tileType )
		{
			m_TileTypes.Add( tileType );
			tileType.AddToTileTexture( m_DisplayTexture );
			m_DisplayTextureBitmap = null;
		}

		#region Private members

		private readonly List< TileType >	m_TileTypes = new List< TileType >( );
		private readonly TileTexture		m_DisplayTexture = new TileTexture( );
		private Bitmap						m_DisplayTextureBitmap;

		#endregion

	}
}
