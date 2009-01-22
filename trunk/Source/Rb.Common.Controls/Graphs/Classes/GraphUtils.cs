using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Common.Controls.Graphs.Interfaces;
using System.Drawing.Drawing2D;

namespace Rb.Common.Controls.Graphs.Classes
{
	/// <summary>
	/// Graphics utility class
	/// </summary>
	public static class GraphUtils
	{
		/// <summary>
		/// Creates a 2-colour colour blend
		/// </summary>
		public static ColorBlend CreateColourBlend( Color c0, float t0, Color c1, float t1 )
		{
			ColorBlend blend = new ColorBlend( 2 );
			blend.Colors[ 0 ] = c0;
			blend.Colors[ 1 ] = c1;
			blend.Positions[ 0 ] = t0;
			blend.Positions[ 1 ] = t1;
			return blend;
		}

		/// <summary>
		/// Creates a 3-colour colour blend
		/// </summary>
		public static ColorBlend CreateColourBlend( Color c0, float t0, Color c1, float t1, Color c2, float t2 )
		{
			ColorBlend blend = new ColorBlend( 3 );
			blend.Colors[ 0 ] = c0;
			blend.Colors[ 1 ] = c1;
			blend.Colors[ 2 ] = c2;
			blend.Positions[ 0 ] = t0;
			blend.Positions[ 1 ] = t1;
			blend.Positions[ 2 ] = t2;
			return blend;
		}

		/// <summary>
		/// Creates a 4-colour colour blend
		/// </summary>
		public static ColorBlend CreateColourBlend( Color c0, float t0, Color c1, float t1, Color c2, float t2, Color c3, float t3 )
		{
			ColorBlend blend = new ColorBlend( 4 );
			blend.Colors[ 0 ] = c0;
			blend.Colors[ 1 ] = c1;
			blend.Colors[ 2 ] = c2;
			blend.Colors[ 3 ] = c3;
			blend.Positions[ 0 ] = t0;
			blend.Positions[ 1 ] = t1;
			blend.Positions[ 2 ] = t2;
			blend.Positions[ 3 ] = t3;
			return blend;
		}

		/// <summary>
		/// Multiplies a colour by a specified fraction
		/// </summary>
		public static Color ScaleColour( Color src, float amount )
		{
			float r = Clamp( src.R * amount, 0, 255 );
			float g = Clamp( src.G * amount, 0, 255 );
			float b = Clamp( src.B * amount, 0, 255 );
			return Color.FromArgb( src.A, ( int )r, ( int )g, ( int )b );
		}

		/// <summary>
		/// Clamps a value to a range
		/// </summary>
		public static float Clamp(float val, float min, float max)
		{
			return val < min ? min : ( val > max ? max : val );
		}

		/// <summary>
		/// Draws legends for all the graphs
		/// </summary>
		public static void DrawLegends( IGraphCanvas graphics, Font font, IEnumerable< GraphComponent > graphComponents )
		{
			int x = 0;
			int y = 0;
			using ( SolidBrush fontBrush = new SolidBrush( Color.Black ) )
			{
				using ( Pen pen = new Pen( Color.Black ) )
				{
					foreach ( GraphComponent graphComponent in graphComponents )
					{
						fontBrush.Color = ( graphComponent.Renderer != null ) ? graphComponent.Renderer.Colour : Color.Black;
						pen.Color = fontBrush.Color;

						graphics.DrawText( font, fontBrush, x, y, graphComponent.Name );
						y += font.Height + 2;

						//graphics.DrawLine( pen, x, y, x + 30, y );
					}
				}
			}
		}

		/// <summary>
		/// Draws a grid
		/// </summary>
		public static void DrawGrid( IGraphCanvas graphics, Pen pen, Pen zeroPen, Rectangle drawBounds, RectangleF dataBounds, float cellDataWidth, float cellDataHeight )
		{
			float dataX = ( int )( dataBounds.Left / cellDataWidth ) * cellDataWidth;
			float dataY = ( int )( dataBounds.Top / cellDataHeight ) * cellDataHeight;
			int widthInCells = ( int )( dataBounds.Width / cellDataWidth ) + 1;
			int heightInCells = ( int )( dataBounds.Height / cellDataHeight ) + 1;
			for ( int cellX = 0; cellX < widthInCells; ++cellX )
			{
				Pen drawPen = ( Math.Abs( dataX ) < 0.001f ) ? zeroPen : pen;
				float drawX = ( ( ( dataX - dataBounds.Left ) / dataBounds.Width ) * drawBounds.Width ) + drawBounds.Left;
				graphics.DrawLine( drawPen, drawX, drawBounds.Bottom, drawX, drawBounds.Top );

				dataX += cellDataWidth;
			}
			for ( int cellY = 0; cellY < heightInCells; ++cellY )
			{
				Pen drawPen = ( Math.Abs( dataY ) < 0.001f ) ? zeroPen : pen;
				float drawY = InvY( ( ( ( dataY - dataBounds.Top ) / dataBounds.Height ) * drawBounds.Height ) + drawBounds.Top, drawBounds );
				graphics.DrawLine( drawPen, drawBounds.Left, drawY, drawBounds.Right, drawY );

				dataY += cellDataHeight;
			}
		}

		/// <summary>
		/// Inverts y wrt a bounding rectangle
		/// </summary>
		public static float InvY( float y, Rectangle bounds )
		{
			return bounds.Height - y;
		}
	}
}
