using System;
using System.Collections.Generic;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A tile
	/// </summary>
	[Serializable]
	public class Tile : ISelectable
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
		/// Gets the offset of this tile from the tile type base height
		/// </summary>
		public float HeightOffset
		{
			get { return m_HeightOffset; }
			set { m_HeightOffset = value; }
		}

		/// <summary>
		/// Gets the height of the tile (tile type base height + height offset)
		/// </summary>
		public float Height
		{
			get { return TileType.BaseHeight + HeightOffset; }
		}

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

		/// <summary>
		/// Gets the objects associated with this tile
		/// </summary>
		public IEnumerable< object > TileObjects
		{
			get { return m_Objects; }
		}

		/// <summary>
		/// Adds an object to this tile
		/// </summary>
		/// <param name="obj">Object to add</param>
		public void AddTileObject( object obj )
		{
			m_Objects.Add( obj );
			if ( m_Grid != null )
			{
				m_Grid.OnTileChanged( this );
			}
		}

		#endregion
		
		#region ISelectable Members

		/// <summary>
		/// Selection state of this tile
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}

		/// <summary>
		/// Highlight state of this tile
		/// </summary>
		public bool Highlight
		{
			get { return m_Highlight; }
			set { m_Highlight = value; }
		}

		#endregion

		#region Private members

		private readonly int m_GridX;
		private readonly int m_GridY;
		private readonly TileGrid m_Grid;
		private readonly List< object > m_Objects	= new List< object >( );

		private bool m_Selected;
		private bool m_Highlight;
		private float m_HeightOffset;
		private TileType m_TileType;

		#endregion

	}
}
