using System;
using System.Collections.Generic;
using Rb.Core.Components;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores a selection of tiles
	/// </summary>
	public class TileGridEditState
	{
		/// <summary>
		/// Delegate invoked to paint an object into the tile grid
		/// </summary>
		public delegate void PaintDelegate( Tile tile, float x, float y );

		#region Public properties

		/// <summary>
		/// Paint event
		/// </summary>
		public PaintDelegate OnPaint
		{
			get { return m_OnPaint; }
			set { m_OnPaint = value; }
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
		/// Selects the specified object, operating according to the <see cref="AddToSelection"/> property
		/// </summary>
		public void ApplySelect( object obj )
		{
			if ( AddToSelection )
			{
				if ( m_Selection.Contains( obj ) )
				{
					Deselect( obj );
				}
				else
				{
					Select( obj );
				}
			}
			else
			{
				bool select = !m_Selection.Contains( obj );
				ClearSelection( );

				if ( select )
				{
					Select( obj );
				}
			}
		}

		/// <summary>
		/// Adds an object to the selection
		/// </summary>
		public void Select( object obj )
		{
			if ( !m_Selection.Contains( obj ) )
			{
				if ( ObjectSelected != null )
				{
					ObjectSelected( obj );
				}
				m_Selection.Add( obj );
				SetSelectedFlag( obj, true );
			}
		}

		/// <summary>
		/// Removes an object from the selection
		/// </summary>
		public void Deselect( object obj )
		{
			SetSelectedFlag( obj, false );
			if ( ObjectDeselected != null )
			{
				ObjectDeselected( obj );
			}
			m_Selection.Remove( obj );
		}

		/// <summary>
		/// Clears the selection
		/// </summary>
		public void ClearSelection( )
		{
			foreach ( object obj in m_Selection )
			{
				SetSelectedFlag( obj, false );

				if ( ObjectDeselected != null )
				{
					ObjectDeselected( obj );
				}
			}
			m_Selection.Clear( );
		}

		#endregion

		#region Public events

		/// <summary>
		/// Invoked when a tile is added to the selection
		/// </summary>
		public event Action< object > ObjectSelected;

		/// <summary>
		/// Invoked when a tile is removed from the selection
		/// </summary>
		public event Action< object > ObjectDeselected;

		#endregion

		#region Private stuff

		private PaintDelegate m_OnPaint;

		private bool m_AddToSelection;
		private Tile m_CursorTile;
		private readonly List< object > m_Selection = new List< object >( );

		/// <summary>
		/// Sets the selected flag in an object
		/// </summary>
		private static void SetSelectedFlag( object obj, bool selected )
		{
			ISelectable selectable = ( obj as ISelectable );
			if ( selectable != null )
			{
				selectable.Selected = selected;
			}
			else
			{
				//	object is not ISelectable, but may have a child component that is
				IParent parent = obj as IParent;
				if ( parent != null )
				{
					selectable = ParentHelpers.GetChildOfType<ISelectable>( parent );
					if ( selectable != null )
					{
						selectable.Selected = selected;
					}
				}
			}
		}

		#endregion
	}
}
