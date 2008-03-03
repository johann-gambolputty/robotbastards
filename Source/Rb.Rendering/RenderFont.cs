using System.Drawing;
using System.Collections.Generic;

namespace Rb.Rendering
{
	/// <summary>
	/// Abstract base class for fonts
	/// </summary>
	public abstract class RenderFont
	{
		/// <summary>
		/// Specifies alignment values - where text is rendered relative to the supplied point
		/// </summary>
		public enum Alignment
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

		//	TODO: AP: Duplicates must be removed from CharacterSet
		/// <summary>
		/// Stores a list of characters that BuildFontImage() uses to generate its font... image... thing
		/// </summary>
		public class CharacterSet
		{
			/// <summary>
			/// Adds a character to the set
			/// </summary>
			public void Add( char c )
			{
				m_Chars.Add( c );
			}

			/// <summary>
			/// Adds a range of characters to the set
			/// </summary>
			public void Add( char start, char end )
			{
				for ( char cur = start; cur <= end; ++cur )
				{
					Add( cur );
				}
			}

			/// <summary>
			/// Adds an array of characters to the set
			/// </summary>
			public void Add( char[] chars )
			{
				for ( int index = 0; index < chars.Length; ++index )
				{
					Add( chars[ index ] );
				}
			}

			/// <summary>
			/// Adds an string of characters to the set
			/// </summary>
			public void Add( string charString )
			{
				for ( int index = 0; index < charString.Length; ++index )
				{
					Add( charString[ index ] );
				}
			}

			/// <summary>
			/// Returns the character set array
			/// </summary>
			public char[] Chars
			{
				get { return m_Chars.ToArray( ); }
			}

			private readonly List< char > m_Chars = new List< char >( );
		}

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <returns>Returns this</returns>
		public RenderFont Setup( Font font )
		{
			CharacterSet chars = new CharacterSet( );
			chars.Add( 'a', 'z' );
			chars.Add( 'A', 'Z' );
			chars.Add( '0', '9' );
			chars.Add( "_.:,'!?£$%^&*()[]{}|~#/" );

			return Setup( font, chars );
		}

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <param name="characters">Characters in built font</param>
		/// <returns>Returns this</returns>
		public abstract RenderFont Setup( Font font, CharacterSet characters );

		/// <summary>
		/// Gets the height of the largest letter in the font
		/// </summary>
		public abstract int MaxHeight
		{
			get;
		}
		
		/// <summary>
		/// Measures the dimensions of a given string
		/// </summary>
		/// <param name="str">String to measure</param>
		/// <returns>Size of the string in pixels</returns>
		public abstract Size MeasureString( string str );

		/// <summary>
		/// Draws formatted text using this font, at a given position
		/// </summary>
		public void DrawText( int x, int y, Color colour, string str, params object[] formatArgs )
		{
			DrawText( Alignment.TopLeft, x, y, colour, string.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Draws formatted text using this font, at a given position
		/// </summary>
		public void DrawText( int x, int y, Color colour, Color outlineColour, string str, params object[] formatArgs )
		{
			DrawText( Alignment.TopLeft, x, y, colour, outlineColour, string.Format( str, formatArgs ) );
		}
		
		/// <summary>
		/// Draws formatted text using this font, at a given position
		/// </summary>
		public void DrawText( Alignment align, int x, int y, Color colour, string str, params object[] formatArgs )
		{
			DrawText( align, x, y, colour, string.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Draws formatted text using this font, at a given position
		/// </summary>
		public void DrawText( float x, float y, float z, Color colour, string str, params object[] formatArgs )
		{
			DrawText( Alignment.TopLeft, x, y, z, colour, string.Format( str, formatArgs ) );
		}
		
		/// <summary>
		/// Draws formatted text using this font, at a given position
		/// </summary>
		public void DrawText( Alignment align, float x, float y, float z, Color colour, string str, params object[] formatArgs )
		{
			DrawText( align, x, y, z, colour, string.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		public abstract void DrawText( Alignment align, int x, int y, Color colour, string str );

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		public abstract void DrawText( Alignment align, int x, int y, Color colour, Color outlineColour, string str );

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		public abstract void DrawText( Alignment align, float x, float y, float z, Color colour, string str );
	}
}
