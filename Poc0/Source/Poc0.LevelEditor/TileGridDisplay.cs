
using System.Drawing;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Rendering.OpenGl;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Windows;

namespace Poc0.LevelEditor
{
	internal partial class TileGridDisplay : Display, ITilePicker
	{
		public TileGridDisplay()
		{
			InitializeComponent();
		}

		#region ITilePicker Members

		/// <summary>
		/// Picks a tile
		/// </summary>
		public Tile PickTile( TileGrid grid, int cursorX, int cursorY )
		{
			Viewer viewer = FindViewerUnderCursor( cursorX, cursorY );
			if ( viewer == null )
			{
				return null;
			}
			return ( ( ITilePicker ) viewer.Camera ).PickTile( grid, cursorX, cursorY );
		}

		/// <summary>
		/// Converts cursor position to world space
		/// </summary>
		public Point2 CursorToWorld( int cursorX, int cursorY )
		{
			Viewer viewer = FindViewerUnderCursor( cursorX, cursorY );
			if ( viewer == null )
			{
				return Point2.Origin;
			}
			return ( ( ITilePicker )viewer.Camera ).CursorToWorld( cursorX, cursorY );
		}

		#endregion

		/// <summary>
		/// Finds the Viewer object that has a screen rectangle that contains the cursor position
		/// </summary>
		private Viewer FindViewerUnderCursor( int cursorX, int cursorY )
		{
			System.Drawing.Rectangle winRect = Bounds;
			foreach ( Viewer viewer in Viewers )
			{
				if ( viewer.GetWindowRectangle( winRect ).Contains( cursorX, cursorY ) )
				{
					return viewer;
				}
			}
			return null;
		}
	}
}
