using System;
using System.Collections.Generic;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores a selection of tiles
	/// </summary>
	public class TileGridEditState
	{
		#region Public properties

		/// <summary>
		/// Type used when painting tiles
		/// </summary>
		public TileType TilePaintType
		{
			get { return m_PaintType; }
			set { m_PaintType = value; }
		}

		/// <summary>
		/// If AddToSelection is true, Select() adds tiles to the selection. If false, Select()
		/// replaces tiles in the selection
		/// </summary>
		public bool AddToSelection
		{
			get { return m_AddToSelection; }
			set { m_AddToSelection = value; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Returns the tile at cursor position
		/// </summary>
		public Tile TileUnderCursor
		{
			set { m_CursorTile = value; }
			get { return m_CursorTile; }
		}

		/// <summary>
		/// Selects the specified tile, operating according to the <see cref="AddToSelection"/> property
		/// </summary>
		public void ApplySelect( Tile tile )
		{
			if ( AddToSelection )
			{
				if ( m_SelectedTiles.Contains( tile ) )
				{
					Deselect( tile );
				}
				else
				{
					Select( tile );
				}
			}
			else
			{
				if ( !m_SelectedTiles.Contains( tile ) )
				{
					ClearSelection( );
					Select( tile );
				}
			}
		}

		/// <summary>
		/// Adds a tile to the selection
		/// </summary>
		public void Select( Tile tile )
		{
			if ( !m_SelectedTiles.Contains( tile ) )
			{
				if ( TileSelected != null )
				{
					TileSelected( tile );
				}
				m_SelectedTiles.Add( tile );
				tile.Selected = true;
			}
		}

		/// <summary>
		/// Removes a tile from the selection
		/// </summary>
		public void Deselect( Tile tile )
		{
			tile.Selected = false;
			if ( TileDeselected != null )
			{
				TileDeselected( tile );
			}
			m_SelectedTiles.Remove( tile );
		}

		/// <summary>
		/// Clears the selection
		/// </summary>
		public void ClearSelection( )
		{
			if ( TileDeselected != null )
			{
				foreach ( Tile tile in m_SelectedTiles )
				{
					tile.Selected = false;
					TileDeselected( tile );
				}
			}
			m_SelectedTiles.Clear( );
		}

		#endregion

		#region Public events

		/// <summary>
		/// Invoked when a tile is added to the selection
		/// </summary>
		public event Action< Tile > TileSelected;

		/// <summary>
		/// Invoked when a tile is removed from the selection
		/// </summary>
		public event Action< Tile > TileDeselected;

		#endregion

		#region Private stuff

		private TileType m_PaintType;
		private bool m_AddToSelection;
		private Tile m_CursorTile;
		private readonly List< Tile > m_SelectedTiles = new List< Tile >( );

		#endregion
	}
}
