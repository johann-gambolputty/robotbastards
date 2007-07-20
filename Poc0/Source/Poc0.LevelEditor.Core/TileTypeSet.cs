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
		/// Default constructor
		/// </summary>
		public TileTypeSet( )
		{
			m_DefaultType = CreateDefaultTileType( );

			TileType defaultType2 = new TileType( );
			defaultType2.Set = this;
			defaultType2.TextureRectangle = new Rectangle( 33, 0, 32, 32 );
			defaultType2.Name = "Default2";
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
			set { m_DisplayTexture = value; }
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

		private readonly List< TileType >	m_TileTypes			= new List< TileType >( );
		private readonly TileType			m_DefaultType;
		private Texture2d					m_DisplayTexture	= CreateDefaultDisplayTexture( );

		private static Texture2d CreateDefaultDisplayTexture( )
		{
			Texture2d displayTexture = RenderFactory.Instance.NewTexture2d( );
			displayTexture.Load( Properties.Resources.DefaultTileType );
			return displayTexture;
		}

		private TileType CreateDefaultTileType( )
		{
			TileType result = new TileType( );
			result.Set = this;
			result.Name = "Default";

			return result;
		}

	}
}
