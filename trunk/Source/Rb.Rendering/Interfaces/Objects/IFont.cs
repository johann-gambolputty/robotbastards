using System.Drawing;

namespace Rb.Rendering.Interfaces.Objects
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
		/// Measures how large a string rendered by this font in 2D would be, in pixels
		/// </summary>
		/// <param name="text">Text to measure</param>
		/// <returns>Size of the rendered area, in pixels</returns>
		Size MeasureString( string text );

		/// <summary>
		/// Gets the maximum height of any character rendered by this font
		/// </summary>
		int MaximumHeight
		{
			get;
		}

		#endregion

		#region 2d positioned text

		/// <summary>
		/// Writes a formatted string to the screen, at position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="colour">Text colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, Color colour, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to position (x,y)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="alignment">Position to display text relative to (x,y)</param>
		/// <param name="colour">Text colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( int x, int y, FontAlignment alignment, Color colour, string format, params object[] args );

		#endregion

		#region 3d positioned text

		/// <summary>
		/// Writes a formatted string to the screen, at position (x,y,z) (using the current transform pipeline)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="z">Z position to render font at</param>
		/// <param name="colour">Text colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( float x, float y, float z, Color colour, string format, params object[] args );

		/// <summary>
		/// Writes a formatted string to the screen, relative to screen space position of (x,y,z) (using the current transform pipeline)
		/// </summary>
		/// <param name="x">X position to render font at</param>
		/// <param name="y">Y position to render font at</param>
		/// <param name="z">Z position to render font at</param>
		/// <param name="alignment">Position to display text relative to the screen position of (x,y,z)</param>
		/// <param name="colour">Text colour</param>
		/// <param name="format">Format string</param>
		/// <param name="args">Format string arguments</param>
		void Write( float x, float y, float z, FontAlignment alignment, Color colour, string format, params object[] args );

		#endregion
	}
}
