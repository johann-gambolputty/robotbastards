using System.Collections.Generic;
using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Collects lines of debug text, rendering them at the end of each frame
	/// </summary>
	/// <remarks>
	/// Text added via the <see cref="Write"/> method gets stored in a list. When <see cref="Render()"/> 
	/// is called, all the lines are rendered to the screen using the debug font, and the line list
	/// is cleared.
	/// </remarks>
	public static class DebugText
	{
		#region Public Members

		/// <summary>
		/// Writes a formatted string, adds it to the list of debug information to render at the end of the current frame
		/// </summary>
		public static void Write( string format, params object[] args )
		{
			string line = string.Format( format, args );
			s_Lines.Add( line );
			int width = Graphics.Fonts.DebugFont.MeasureString( line ).Width;
			s_MaxWidth = width > s_MaxWidth ? width : s_MaxWidth;
		}

		#endregion

		#region Private Members
		
		private readonly static List<string> s_Lines = new List<string>( );
		private readonly static DrawBase.IBrush s_WindowBrush;
		private static int s_MaxWidth;

		static DebugText( )
		{
			int alpha = 0xb0;
			DrawBase.IBrush brush = Graphics.Draw.NewBrush( Color.FromArgb( alpha, Color.DarkBlue ), Color.FromArgb( alpha, Color.LightSteelBlue ) );
			brush.State.Blend = true;
			brush.State.SourceBlend = BlendFactor.SrcAlpha;
			brush.State.DestinationBlend = BlendFactor.OneMinusSrcAlpha;

			s_WindowBrush = brush;

			Graphics.Renderer.FrameStart += s_Lines.Clear;
			Graphics.Renderer.FrameEnd += Render;
		}

		private static void Render( )
		{
			if ( s_Lines.Count == 0 )
			{
				return;
			}

			Graphics.Renderer.Push2d( );
			int incY = Graphics.Fonts.DebugFont.MaximumHeight;
			Graphics.Draw.Rectangle( s_WindowBrush, 1, 1, 4 + s_MaxWidth, 3 + s_Lines.Count * incY );
			Graphics.Renderer.Pop2d( );

			int y = 2;
			for ( int lineIndex = 0; lineIndex < s_Lines.Count; ++lineIndex )
			{
				Graphics.Fonts.DebugFont.Write( 3, y, Color.White, s_Lines[ lineIndex ] );
				y += incY;
			}
		}

		#endregion
	}
}
