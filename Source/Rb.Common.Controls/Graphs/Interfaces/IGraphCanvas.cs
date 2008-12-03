using System.Drawing;

namespace Rb.Common.Controls.Graphs.Interfaces
{
	/// <summary>
	/// Simple graph graphics interface. A limited subset of <see cref="System.Drawing.Graphics"/> commands.
	/// </summary>
	public interface IGraphCanvas
	{
		/// <summary>
		/// Draws a filled rectangle
		/// </summary>
		void FillRectangle( Brush brush, Rectangle rect );

		/// <summary>
		/// Draws a filled circle
		/// </summary>
		void FillCircle( Brush brush, float x, float y, float radius );

		/// <summary>
		/// Draws a circle
		/// </summary>
		void DrawCircle( Pen pen, float x, float y, float radius );

		/// <summary>
		/// Draws text
		/// </summary>
		void DrawText( Font font, Brush brush, float x, float y, string text );

		/// <summary>
		/// Draws a line
		/// </summary>
		void DrawLine( Pen pen, float startX, float startY, float endX, float endY );
	}
}
