using System.Windows.Forms;
using Poc0.LevelEditor.Core.Actions;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Edit mode for painting tiles
	/// </summary>
	public class PaintTileEditMode : EditMode
	{
		/// <summary>
		/// Binds the edit mode to a control and viewer
		/// </summary>
		/// <param name="actionButton">The mouse button that this edit mode listens out for</param>
		/// <param name="paintType">Tile type to paint with</param>
		public PaintTileEditMode( MouseButtons actionButton, TileType paintType )
		{
			m_PaintType = paintType;
			m_ActionButton = actionButton;
		}

		#region Control event handlers

		private void PaintTile( Tile tile )
		{
			if ( tile.TileType != m_PaintType )
			{
				if ( m_Action == null )
				{
					m_Action = new ActionList( );
					EditModeContext.Instance.UndoStack.Push( m_Action );
				}
				m_Action.Add( new PaintTileAction( tile, m_PaintType ) );
			}
		}

		private void OnMouseDown( object sender, MouseEventArgs args )
		{
			Tile pickedTile = GetTileUnderCursor( sender, args.X, args.Y );
			if ( pickedTile != null )
			{
				if ( ( args.Button & m_ActionButton ) != 0 )
				{
					PaintTile( pickedTile );
				}
			}
		}

		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			Tile pickedTile = GetTileUnderCursor( sender, args.X, args.Y );
			EditModeContext.Instance.TileUnderCursor = pickedTile;
			if ( pickedTile != null )
			{
				if ( ( args.Button & m_ActionButton ) != 0 )
				{
					PaintTile( pickedTile );
				}
			}
		}
		
		private void OnMouseUp( object sender, MouseEventArgs args )
		{
			if ( ( args.Button & m_ActionButton ) != 0 )
			{
				m_Action = null;
			}
		}


		#endregion

		#region Private members

		private readonly TileType		m_PaintType;
		private readonly MouseButtons	m_ActionButton;
		private ActionList				m_Action;

		#endregion

		#region Public methods

		/// <summary>
		/// Called when the edit mode is activated
		/// </summary>
		public override void Start( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseDown += OnMouseDown;
				control.MouseMove += OnMouseMove;
				control.MouseUp += OnMouseUp;
			}
		}

		/// <summary>
		/// Called when the edit mode is deactivated
		/// </summary>
		public override void Stop( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseDown -= OnMouseDown;
				control.MouseMove -= OnMouseMove;
				control.MouseUp -= OnMouseUp;
			}
			EditModeContext.Instance.TileUnderCursor = null;
		}

		#endregion
	}
}
