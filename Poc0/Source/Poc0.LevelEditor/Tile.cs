
using System.Collections.Generic;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// A tile
	/// </summary>
	internal class Tile
	{
		/// <summary>
		/// Type of this tile
		/// </summary>
		public TileType TileType
		{
			get { return m_TileType; }
			set { m_TileType = value; }
		}

		/// <summary>
		/// Gets the list of objects associated with this tile
		/// </summary>
		public IList< TileObject > TileObjects
		{
			get { return m_Objects;  }
		}

		private List< TileObject >	m_Objects	= new List< TileObject >( );
		private TileType			m_TileType;
	}
}
