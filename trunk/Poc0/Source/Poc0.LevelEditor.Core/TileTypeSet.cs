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

			set.DisplayTexture = CreateDefaultDisplayTexture( );

			int x = 0;
			int width = 64;
			int height = 64;

			new TileType( set, "tile0", x, 0, width, height );
			x += width;

			new TileType( set, "tile1", x, 0, width, height );
			x += width;
			
			new TileType( set, "tile2", x, 0, width, height );

			return set;
		}

		/// <summary>
		/// Gets the default tile type
		/// </summary>
		public TileType DefaultTileType
		{
			get { return m_DefaultType; }
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
			get { return m_DisplayTexture; }
			set
			{
				m_DisplayTexture = value;
				m_DisplayTextureImage = null;
			}
		}

		/// <summary>
		/// Gets the display texture, converted to a bitmap
		/// </summary>
		public Bitmap DisplayTextureBitmap
		{
			get
			{
				if ( ( m_DisplayTextureImage == null ) && ( DisplayTexture != null ) )
				{
					m_DisplayTextureImage = DisplayTexture.ToBitmap( );
				}
				return m_DisplayTextureImage;
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
		}

		/// <summary>
		/// Sets the texture used when displaying the tile type
		/// </summary>
		public void SetDisplayTexture( Bitmap bmp )
		{
			if ( m_DisplayTexture != null )
			{
				m_DisplayTexture.Dispose( );
			}

			if ( RenderFactory.Instance != null )
			{
				m_DisplayTexture = RenderFactory.Instance.NewTexture2d( );
				m_DisplayTexture.Load( bmp );
			}
		}

		#region Private members

		private readonly List< TileType >	m_TileTypes			= new List< TileType >( );
		private readonly TileType			m_DefaultType;
		private Texture2d					m_DisplayTexture;
		private Bitmap						m_DisplayTextureImage;

		/// <summary>
		/// Creates a texture from the default tile texture in the resources
		/// </summary>
		private static Texture2d CreateDefaultDisplayTexture( )
		{
			Texture2d displayTexture = RenderFactory.Instance.NewTexture2d( );
			displayTexture.Load( Properties.Resources.DefaultTileType );
			return displayTexture;
		}

		/// <summary>
		/// Creates a new default tile type
		/// </summary>
		private TileType CreateDefaultTileType( )
		{
			return new TileType( this, "Default", 0, 0, 64, 64 );
		}

		#endregion

	}
}
