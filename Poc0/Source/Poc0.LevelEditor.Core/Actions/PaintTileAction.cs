
namespace Poc0.LevelEditor.Core.Actions
{
	class PaintTileAction : IAction
	{

		/// <summary>
		/// Applies the action
		/// </summary>
		/// <param name="tile">Tile to change</param>
		/// <param name="tileType">Tile's new type</param>
		public PaintTileAction( Tile tile, TileType tileType )
		{
			m_Tile = tile;
			m_OldTileType = tile.TileType;
			m_NewTileType = tileType;

			m_Tile.TileType = tileType;
		}

		private readonly Tile		m_Tile;
		private readonly TileType	m_OldTileType;
		private readonly TileType	m_NewTileType;

		#region IAction Members

		public void Undo( )
		{
			m_Tile.TileType = m_OldTileType;
		}

		public void Redo( )
		{
			m_Tile.TileType = m_NewTileType;
		}

		#endregion
	}
}
