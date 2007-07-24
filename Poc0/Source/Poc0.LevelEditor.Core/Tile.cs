
namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A tile
	/// </summary>
	public class Tile
	{
		#region Construction

		/// <summary>
		/// Tile setup constructor
		/// </summary>
		/// <param name="grid">Grid that this tile belongs to</param>
		/// <param name="x">X position of this tile in the grid</param>
		/// <param name="y">Y position of this tile in the grid</param>
		/// <param name="type">Type of this tile</param>
		public Tile( TileGrid grid, int x, int y, TileType type )
		{
			m_Grid = grid;
			m_GridX = x;
			m_GridY = y;
			m_TileType = type;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets the X position of this tile in the tile grid
		/// </summary>
		public int GridX
		{
			get { return m_GridX; }
		}

		/// <summary>
		/// Gets the Y position of this tile in the tile grid
		/// </summary>
		public int GridY
		{
			get { return m_GridY; }
		}

		/// <summary>
		/// Selection state of this tile
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}

		/// <summary>
		/// Type of this tile
		/// </summary>
		public TileType TileType
		{
			get { return m_TileType; }
			set
			{
				bool changed = ( m_TileType != value );
				m_TileType = value;
				if ( ( m_Grid != null ) && ( changed ) )
				{
					m_Grid.OnTileChanged( this );
				}
			}
		}

		#endregion

		#region Private members

		private int m_GridX;
		private int m_GridY;
		private bool m_Selected;
		private readonly TileGrid m_Grid;
		//private readonly List< TileObject > m_Objects	= new List< TileObject >( );
		private TileType m_TileType;

		#endregion
	}
}
