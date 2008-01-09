using System.Drawing;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Specifies alignment values for IFont.Write - where text is rendered relative to the supplied point
	/// </summary>
	public enum FontAlignment
	{
		TopLeft,
		TopCentre,
		TopRight,
		MiddleLeft,
		MiddleCentre,
		MiddleRight,
		BottomLeft,
		BottomCentre,
		BottomRight
	}

	/// <summary>
	/// A font that can render text. Created by <see cref="IGraphicsFactory.CreateFont"/>
	/// </summary>
	public interface IFont
	{
		#region String measurement

		/// <summary>
		/// Measures how large a string rendered in this font would be
		/// </summary>
		/// <param name="text">Text to measure</param>
		/// <returns>Size of the rendered area, in pixels</returns>
		Size MeasureString( string text );

		#endregion

		#region 2d positioned text

		/// <summary>
		/// Writes a formatted string to the screen, at position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="alignment">Position to display text relative to (x,y)</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, FontAlignment alignment, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="alignment">Position to display text relative to (x,y)</param>
		/// <param name="foreColour">Foreground colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, FontAlignment alignment, Color foreColour, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="foreColour">Foreground colour</param>
		/// <param name="backColour">Background colour</param>
		/// <param name="alignment">Position to display text relative to (x,y)</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, Color foreColour, Color backColour, FontAlignment alignment, string format, params object[] args );

		#endregion

		#region 3d positioned text

		/// <summary>
		/// Writes a formatted string to the screen, relative to screen space position of (x,y,z) (using the current transform pipeline)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="z">Z position to render font at</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( float x, float y, float z, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to screen space position of (x,y,z) (using the current transform pipeline)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="z">Z position to render font at</param>
		/// <param name="alignment">Position to display text relative to the screen position of (x,y,z)</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( float x, float y, float z, FontAlignment alignment, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to screen space position of (x,y,z) (using the current transform pipeline)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="z">Z position to render font at</param>
		/// <param name="alignment">Position to display text relative to the screen position of (x,y,z)</param>
		/// <param name="foreColour">Foreground colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( float x, float y, float z, FontAlignment alignment, Color foreColour, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to screen space position of (x,y,z) (using the current transform pipeline)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="z">Z position to render font at</param>
		/// <param name="alignment">Position to display text relative to the screen position of (x,y,z)</param>
		/// <param name="foreColour">Foreground colour</param>
		/// <param name="backColour">Background colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( float x, float y, float z, FontAlignment alignment, Color foreColour, Color backColour, string format, params object[] args );

		#endregion
	}
}
