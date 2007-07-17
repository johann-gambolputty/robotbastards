using System;
using System.Collections.Generic;
using System.Text;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// Tile grid data
	/// </summary>
	internal class TileGrid
	{
		/// <summary>
		/// The default width of a starting TileGrid
		/// </summary>
		public const int DefaultWidth	= 64;

		/// <summary>
		/// The default height of a starting TileGrid
		/// </summary>
		public const int DefaultHeight	= 64;

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
		/// Sets the dimensions of the tile grid (in tiles)
		/// </summary>
		/// <param name="width">Number of tiles making up the width of the grid</param>
		/// <param name="height">Number of tiles making up the height of the grid</param>
		public void SetDimensions( int width, int height )
		{
			m_Tiles = NewTileGrid( m_Tiles, width, height );
		}

		#region Private stuff

		/// <summary>
		/// Creates a new tile grid with the given dimensions. Copies tiles from an old grid
		/// </summary>
		/// <param name="oldGrid">Old tile grid</param>
		/// <param name="width">New tile grid width (in tiles)</param>
		/// <param name="height">New tile grid height (in tiles)</param>
		/// <returns>Returns the new Tile table</returns>
		private static Tile[,] NewTileGrid( Tile[,] oldGrid, int width, int height )
		{
			if ( oldGrid == null )
			{
				return NewTileGrid( width, height );
			}

			int oldWidth = oldGrid.GetLength( 0 );
			int oldHeight = oldGrid.GetLength( 1 );

			Tile[,] tiles = new Tile[ width, height ];
			for ( int x = 0; x < width; ++x )
			{
				for ( int y = 0; y < height; ++y )
				{
					tiles[ x, y ] = ( x < oldWidth ) && ( y < oldHeight ) ? oldGrid[ x, y ] : new Tile( );
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
		private static Tile[,] NewTileGrid( int width, int height )
		{
			Tile[,] tiles = new Tile[ width, height ];
			for ( int x = 0; x < width; ++x )
			{
				for ( int y = 0; y < height; ++y )
				{
					tiles[ x, y ] = new Tile( );
				}
			}
			return tiles;
		}

		public Tile[,] m_Tiles = NewTileGrid( DefaultWidth, DefaultHeight );

		#endregion
	}
}
