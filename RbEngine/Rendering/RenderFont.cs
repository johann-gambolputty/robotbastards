using System;
using System.Collections;
using System.Drawing;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Abstract base class for fonts
	/// </summary>
	public abstract class RenderFont
	{
		//	TODO: Duplicates must be removed from CharacterSet

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
			public char[]		Chars
			{
				get
				{
					return ( char[] )m_Chars.ToArray( typeof( char ) );
				}
			}

			private ArrayList	m_Chars = new ArrayList( );
		}

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <param name="characters">Set of characters to build the font texture from</param>
		/// <returns>Returns this</returns>
		public RenderFont				Setup( Font font )
		{
			CharacterSet chars = new CharacterSet( );
			chars.Add( 'a', 'z' );
			chars.Add( 'A', 'Z' );
			chars.Add( '0', '9' );

			return Setup( font, chars );
		}

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <returns>Returns this</returns>
		public abstract RenderFont		Setup( Font font, CharacterSet characters );

		/// <summary>
		/// Draws formatted text using this font, at a given position
		/// </summary>
		public void						DrawText( int x, int y, string str, params object[] formatArgs )
		{
			DrawText( x, y, string.Format( str, formatArgs ) );
		}

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		public abstract void			DrawText( int x, int y, string str );

	}
}
