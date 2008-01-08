using System.Drawing;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Specifies alignment values - where text is rendered relative to the supplied point
	/// </summary>
	public enum RelativeTextPosition
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
	/// A font that can render text
	/// </summary>
	public interface IFont
	{
		/// <summary>
		/// Measures how large a string rendered in this font would be
		/// </summary>
		/// <param name="text">Text to measure</param>
		/// <returns>Size of the rendered area, in pixels</returns>
		Size MeasureString( string text );

		/// <summary>
		/// Writes a formatted string to the screen, relative to position (x,y)
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
		/// <param name="foreColour">Foreground colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, Color foreColour, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="foreColour">Foreground colour</param>
		/// <param name="backColour">Background colour</param>
		/// <param name="relPos">Position to display text relative to (x,y)</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, Color foreColour, Color backColour, RelativeTextPosition relPos, string format, params object[] args );
	}
}
