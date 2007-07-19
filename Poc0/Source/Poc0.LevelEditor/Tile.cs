
using System.Collections.Generic;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// A tile
	/// </summary>
	internal class Tile
	{
		#region Construction

		/// <summary>
		/// Tile default constructor
		/// </summary>
		public Tile( )
		{
			m_TileType = TileType.Default;
		}

		/// <summary>
		/// Tile setup constructor
		/// </summary>
		/// <param name="type">Type of this tile</param>
		public Tile( TileType type )
		{
			m_TileType = type;
		}

		#endregion

		#region Public properties

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

		#endregion

		#region Private members

		private readonly List< TileObject > m_Objects	= new List< TileObject >( );
		private TileType m_TileType;

		#endregion
	}
}
