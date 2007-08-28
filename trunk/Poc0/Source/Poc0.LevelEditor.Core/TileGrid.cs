
using System;
using System.Runtime.Serialization;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Tile grid data
	/// </summary>
	[Serializable]
	public class TileGrid : ISerializable
	{
		#region Public constants

		/// <summary>
		/// The default width of a starting TileGrid
		/// </summary>
		public const int DefaultWidth	= 64;

		/// <summary>
		/// The default height of a starting TileGrid
		/// </summary>
		public const int DefaultHeight	= 64;

		#endregion

		#region Public properties

		/// <summary>
		/// Gets a tile at a given (x,y) position in the gird
		/// </summary>
		public Tile this[ int x, int y ]
		{
			get { return m_Tiles[ x, y ]; }
		}

		/// <summary>
		/// Gets the width of the tile grid (in tiles)
		/// </summary>
		public int Width
		{
			get { return m_Tiles.GetLength( 0 ); }
		}

		/// <summary>
		/// Gets the height of the tile grid (in tiles)
		/// </summary>
		public int Height
		{
			get { return m_Tiles.GetLength( 1 ); }
		}

		/// <summary>
		/// Gets the tile type set associated with this grid
		/// </summary>
		public TileTypeSet Set
		{
			get { return m_TileTypes; }
		}

		#endregion

		#region Public construction

		/// <summary>
		/// Serialization constructor
		/// </summary>
		public TileGrid( SerializationInfo info, StreamingContext context )
		{
			m_TileTypes = ( TileTypeSet )info.GetValue( "types", typeof( TileTypeSet ) );
			m_Tiles = ( Tile[ , ] )info.GetValue( "tiles", typeof( Tile[ , ] ) );
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public TileGrid( TileTypeSet set )
		{
			m_TileTypes = set;
			m_Tiles = NewTileGrid( DefaultWidth, DefaultHeight );
		}

		#endregion

		#region Public events

		/// <summary>
		/// Invoked when the specified tile has been altered
		/// </summary>
		public event Action< Tile > TileChanged;

		#endregion

		#region Public methods

		/// <summary>
		/// Calls the TileChanged event
		/// </summary>
		/// <param name="tile">Tile that was altered</param>
		public void OnTileChanged( Tile tile )
		{
			if ( TileChanged != null )
			{
				TileChanged( tile );
			}
		}

		/// <summary>
		/// Sets the dimensions of the tile grid (in tiles)
		/// </summary>
		/// <param name="width">Number of tiles making up the width of the grid</param>
		/// <param name="height">Number of tiles making up the height of the grid</param>
		public void SetDimensions( int width, int height )
		{
			m_Tiles = NewTileGrid( m_Tiles, width, height );
		}

		public bool Contains( int x, int y )
		{
			return ( ( x >= 0 ) && ( x < Width ) && ( y >= 0 ) && ( y < Height ) );
		}

		#endregion

		#region Private stuff

		/// <summary>
		/// Creates a new tile grid with the given dimensions. Copies tiles from an old grid
		/// </summary>
		/// <param name="oldGrid">Old tile grid</param>
		/// <param name="width">New tile grid width (in tiles)</param>
		/// <param name="height">New tile grid height (in tiles)</param>
		/// <returns>Returns the new Tile table</returns>
		private Tile[,] NewTileGrid( Tile[,] oldGrid, int width, int height )
		{
			if ( oldGrid == null )
			{
				return NewTileGrid( width, height );
			}

			int oldWidth = oldGrid.GetLength( 0 );
			int oldHeight = oldGrid.GetLength( 1 );

			TileType newTileType = Set[ 0 ];

			Tile[,] tiles = new Tile[ width, height ];
			for ( int x = 0; x < width; ++x )
			{
				for ( int y = 0; y < height; ++y )
				{
					tiles[ x, y ] = ( x < oldWidth ) && ( y < oldHeight ) ? oldGrid[ x, y ] : new Tile( this, x, y, newTileType );
				}
			}
			return tiles;
		}

		/// <summary>
		/// Creates a new tile grid
		/// </summary>
		/// <param name="width">New tile grid width (in tiles)</param>
		/// <param name="height">New tile grid height (in tiles)</param>
		/// <returns>Returns the new Tile table</returns>
		private Tile[,] NewTileGrid( int width, int height )
		{
			TileType tileType = Set[ 0 ];

			Tile[,] tiles = new Tile[ width, height ];
			for ( int x = 0; x < width; ++x )
			{
				for ( int y = 0; y < height; ++y )
				{
					tiles[ x, y ] = new Tile( this, x, y, tileType );
				}
			}
			return tiles;
		}

		private Tile[,] m_Tiles;
		private readonly TileTypeSet m_TileTypes;

		#endregion

		#region ISerializable Members

		public void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( "types", m_TileTypes );
			info.AddValue( "tiles", m_Tiles );
		}

		#endregion
	}
}
