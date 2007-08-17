
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Interface for cameras that view the tile grid, and can convert a mouse cursor
	/// position into a tile
	/// </summary>
	interface ITilePicker
	{
		/// <summary>
		/// Returns the tile under the mouse cursor
		/// </summary>
		/// <param name="grid">Grid to select tiles from</param>
		/// <param name="cursorX">Cursor x position</param>
		/// <param name="cursorY">Cursor y position</param>
		/// <returns>Returns the tile in the grid under mouse cursor. Returns null if no tile is under the cursor</returns>
		Tile PickTile( TileGrid grid, int cursorX, int cursorY );

		/// <summary>
		/// Returns the world position from a cursor position
		/// </summary>
		/// <param name="cursorX">Cursor x position</param>
		/// <param name="cursorY">Cursor y position</param>
		/// <returns>World point</returns>
		Point2 CursorToWorld( int cursorX, int cursorY );

	}
}
