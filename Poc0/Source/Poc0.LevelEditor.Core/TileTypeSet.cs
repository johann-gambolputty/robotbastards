using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores a set of TileType objects
	/// </summary>
	[Serializable]
	public class TileTypeSet
	{
		#region Public properties

		/// <summary>
		/// Gets an indexed tile type
		/// </summary>
		public TileType this[ int index ]
		{
			get { return m_TileTypes[ index ]; }
			set { m_TileTypes[ index ] = value; }
		}

		/// <summary>
		/// Gets the tile texture
		/// </summary>
		public TileTexture TileTexture
		{
			get { return m_DisplayTexture; }
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
		
		#endregion

		#region Public methods
		
		/// <summary>
		/// Creates a default tile type set
		/// </summary>
		public static TileTypeSet CreateDefaultTileTypeSet( )
		{
			TileTypeSet set = new TileTypeSet( );

			set.AddTileType( "tile0", Properties.Resources.tile0, 4, 4 );
			set.AddTileType( "tile1", Properties.Resources.tile1, 8, 6 );
			//set.AddTileType( "tile2", Properties.Resources.tile2, 3, 0 );

			return set;
		}

		/// <summary>
		/// Adds a tile type to the set, automatically generating transition textures
		/// </summary>
		/// <param name="name">Name of the tile type</param>
		/// <param name="bmp">Base tile type image</param>
		/// <param name="hardEdgeSize">Size of the hard edge, used when generating transition textures</param>
		/// <param name="softEdgeSize">Size of the soft edge, used when generating transition textures</param>
		public TileType AddTileType( string name, Bitmap bmp, int hardEdgeSize, int softEdgeSize )
		{
			return new TileType( this, name, bmp, hardEdgeSize, softEdgeSize );
		}

		/// <summary>
		/// Adds a tile type to the set
		/// </summary>
		public int Add( TileType tileType )
		{
			int index = m_TileTypes.Count;
			m_TileTypes.Add( tileType );
			m_DisplayTextureBitmap = null;
			return index;
		}

		#endregion

		#region Private members

		private readonly List< TileType >	m_TileTypes = new List< TileType >( );

		//	TODO: AP: Enable serialization
		[NonSerialized]
		private readonly TileTexture		m_DisplayTexture = new TileTexture( );

		[NonSerialized]
		private Bitmap						m_DisplayTextureBitmap;

		#endregion

	}
}
