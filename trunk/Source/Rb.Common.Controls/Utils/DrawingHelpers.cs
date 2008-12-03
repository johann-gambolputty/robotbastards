
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Rb.Common.Controls.Utils
{
	/// <summary>
	/// Drawing helper methods
	/// </summary>
	public static class DrawingHelpers
	{
		/// <summary>
		/// Creates a path representing a rounded rectangle
		/// </summary>
		public static GraphicsPath CreateRoundedRectanglePath( Rectangle bounds, float radius )
		{
			return CreateRoundedRectanglePath( bounds.Left, bounds.Top, bounds.Width, bounds.Height, radius );
		}

		/// <summary>
		/// Creates a path representing a rounded rectangle
		/// </summary>
		public static GraphicsPath CreateRoundedRectanglePath( float x, float y, float w, float h, float radius )
		{
			GraphicsPath path = new GraphicsPath( );
			float diameter = radius * 2;
			float endX = x + w - diameter;
			float endY = y + h - diameter;
			path.AddArc( x, y, diameter, diameter, 180, 90 );
			path.AddArc( endX, y, diameter, diameter, 270, 90 );
			path.AddArc( endX, endY, diameter, diameter, 0, 90 );
			path.AddArc( x, endY, diameter, diameter, 90, 90 );
			path.CloseFigure( );
			return path;
		}

	}
}
