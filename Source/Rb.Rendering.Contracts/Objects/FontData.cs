using System.Collections.Generic;
using System.Drawing;
using Rb.Rendering.Contracts;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Data used by <see cref="IGraphicsFactory.CreateFont"/> to create an <see cref="IFont"/> object. 
	/// </summary>
	public class FontData
	{
		/// <summary>
		/// Sets up the font data, using a default character set
		/// </summary>
		/// <param name="font">Standard font to use as basis for render font</param>
		public FontData( Font font )
		{
			m_Font = font;

			CharacterSet chars = new CharacterSet( );
			chars.Add( 'a', 'z' );
			chars.Add( 'A', 'Z' );
			chars.Add( '0', '9' );
			chars.Add( "_.:,'!?£$%^&*()[]{}|~#/" );
			m_Characters = chars;
		}

		/// <summary>
		/// Sets up the font data
		/// </summary>
		/// <param name="font">Standard font to use as basis for render font</param>
		/// <param name="chars">Characters to include</param>
		public FontData( Font font, CharacterSet chars )
		{
			m_Font = font;
			m_Characters = chars;
		}

		#region Public properties

		/// <summary>
		/// Gets the base font
		/// </summary>
		public Font Font
		{
			get { return m_Font; }
		}

		/// <summary>
		/// Gets the set of characters to include in the created font
		/// </summary>
		public CharacterSet Characters
		{
			get { return m_Characters; }
		}

		#endregion

		#region Public CharacterSet class

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

		#endregion

		#region Private members

		private readonly Font m_Font;
		private readonly CharacterSet m_Characters;

		#endregion
	}
}
