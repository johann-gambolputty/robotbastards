using System.Collections.Generic;
using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Collects lines of debug text, rendering them at the end of each frame
	/// </summary>
	public static class DebugText
	{
		#region Public Members

		/// <summary>
		/// Writes a formatted string, adds it to the list of debug information to render at the end of the current frame
		/// </summary>
		public static void Write( string format, params object[] args )
		{
			string line = string.Format( format, args );
			ms_Lines.Add( line );
			int width = Graphics.Fonts.DebugFont.MeasureString( line ).Width;
			ms_MaxWidth = width > ms_MaxWidth ? width : ms_MaxWidth;
		}

		#endregion

		#region Private Members
		
		private readonly static List<string> ms_Lines = new List<string>( );
		private readonly static DrawBase.IBrush ms_WindowBrush;
		private static int ms_MaxWidth;

		static DebugText( )
		{
			DrawBase.IBrush brush = Graphics.Draw.NewBrush( Color.DarkBlue, Color.LightBlue );
			brush.State.Blend = true;
			brush.State.SourceBlend = BlendFactor.One;
			brush.State.DestinationBlend = BlendFactor.One;

			ms_WindowBrush = brush;

			Graphics.Renderer.FrameStart += ms_Lines.Clear;
			Graphics.Renderer.FrameEnd += Render;
		}

		private static void Render( )
		{
			if ( ms_Lines.Count == 0 )
			{
				return;
			}

			Graphics.Renderer.Push2d( );
			int incY = Graphics.Fonts.DebugFont.MaximumHeight;
			Graphics.Draw.Rectangle( ms_WindowBrush, 1, 1, 4 + ms_MaxWidth, 3 + ms_Lines.Count * incY );
			Graphics.Renderer.Pop2d( );

			int y = 2;
			for ( int lineIndex = 0; lineIndex < ms_Lines.Count; ++lineIndex )
			{
				Graphics.Fonts.DebugFont.Write( 3, y, Color.White, ms_Lines[ lineIndex ] );
				y += incY;
			}
		}

		#endregion
	}
}
