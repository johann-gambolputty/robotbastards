using System.Drawing;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Forms.Graphs
{
	/// <summary>
	/// Implements <see cref="IGraphCanvas"/> interface using an underlying <see cref="System.Drawing.Graphics"/> object
	/// </summary>
	internal class GraphGraphicsCanvas : IGraphCanvas
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public GraphGraphicsCanvas( Graphics graphics )
		{
			m_Graphics = graphics;
		}

		#region IGraphCanvas Members

		/// <summary>
		/// Draws a filled rectangle
		/// </summary>
		public void FillRectangle( Brush brush, Rectangle rect )
		{
			m_Graphics.FillRectangle( brush, rect );
		}

		/// <summary>
		/// Draws a filled circle
		/// </summary>
		public void FillCircle( Brush brush, float x, float y, float radius )
		{
			float radius2 = radius * 2;
			m_Graphics.FillEllipse( brush, x - radius, y - radius, radius2, radius2 );
		}
		
		/// <summary>
		/// Draws a circle
		/// </summary>
		public void DrawCircle( Pen pen, float x, float y, float radius )
		{
			float radius2 = radius * 2;
			m_Graphics.DrawEllipse( pen, x - radius, y - radius, radius2, radius2 );
		}

		/// <summary>
		/// Draws text
		/// </summary>
		public void DrawText( Font font, Brush brush, float x, float y, string text )
		{
			m_Graphics.DrawString( text, font, brush, x, y );
		}

		/// <summary>
		/// Draws a line
		/// </summary>
		public void DrawLine( Pen pen, float startX, float startY, float endX, float endY )
		{
			m_Graphics.DrawLine( pen, startX, startY, endX, endY );
		}

		#endregion

		#region Private Members

		private readonly Graphics m_Graphics;

		#endregion
	}
}
