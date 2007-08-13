using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Tile edit command enumeration
	/// </summary>
	public enum TileEditCommands
	{
		[CommandDescription( "Move", "Moves the selection cursor" )]
		Move,

		[CommandDescription( "Paint", "Sets the current tile type")]
		Paint,

		[CommandDescription( "Add to Selection", "Adds or removes the current tile from the selection")]
		AddToSelection,

		[CommandDescription( "Replace selection", "Replaces the current selection" )]
		ReplaceSelection,

		[CommandDescription( "Select", "Selects or deselects the current tile")]
		Select
	}

	/// <summary>
	/// Handles commands from the <see cref="TileEditCommands"/> enumeration
	/// </summary>
	public class TileEditCommandHandler : Component
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="user">User that provides edit commands</param>
		/// <param name="grid">Grid being edited</param>
		/// <param name="editState">Edit state of the grid</param>
		public TileEditCommandHandler( CommandUser user, TileGrid grid, TileGridEditState editState )
		{
			m_Grid = grid;
			m_EditState = editState;

			CommandList commands = CommandList.FromEnum( typeof( TileEditCommands ) );
			new CommandInputListener( this, user, commands );
		}

		private TileGrid m_Grid;
		private TileGridEditState m_EditState;

		/// <summary>
		/// Gets the tile grid being edited
		/// </summary>
		private TileGrid Grid
		{
			get { return m_Grid; }
		}

		/// <summary>
		/// Gets the grid edit state
		/// </summary>
		private TileGridEditState EditState
		{
			get { return m_EditState; }
		}

		/// <summary>
		/// Gets the tile picker used to convert cursor positions to grid tiles
		/// </summary>
		private ITilePicker TilePicker
		{
			get
			{
				return ( ITilePicker )Parent;
			}
		}

		/// <summary>
		/// Called when the handler receives a command message
		/// </summary>
		[Dispatch]
		public MessageRecipientResult OnCommand( CommandMessage msg )
		{
			if ( EditState == null || Grid == null )
			{
				return MessageRecipientResult.DeliverToNext;
			}

			switch ( ( TileEditCommands )msg.CommandId )
			{
				case TileEditCommands.Move:
					{
						CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;

						Tile tile = TilePicker.PickTile( Grid, cursorMsg.X, cursorMsg.Y );
						EditState.TileUnderCursor = tile;
						break;
					}

				case TileEditCommands.Paint:
					{
						CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
						if ( EditState.OnPaint != null )
						{
							Tile tile = TilePicker.PickTile( Grid, cursorMsg.X, cursorMsg.Y );
							if ( tile != null )
							{
								Point2 pt = TilePicker.CursorToGrid( cursorMsg.X, cursorMsg.Y );
								EditState.OnPaint( tile, pt.X, pt.Y );
							}
						}
						break;
					}

				case TileEditCommands.Select:
					{
						CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;

						Tile tile = TilePicker.PickTile( Grid, cursorMsg.X, cursorMsg.Y );
						if ( tile != null )
						{
							EditState.ApplySelect( tile );
						}
						break;
					}

				case TileEditCommands.AddToSelection:
					{
						EditState.AddToSelection = true;
						break;
					}

				case TileEditCommands.ReplaceSelection:
					{
						EditState.AddToSelection = false;
						break;
					}
			}
			return MessageRecipientResult.DeliverToNext;
		}
	}
}
