using System;

namespace Rb.Rendering
{
	/// <summary>
	/// The set of default fonts 
	/// </summary>
	public enum DefaultFont
	{
		/// <summary>
		/// Debug display font
		/// </summary>
		Debug,

		/// <summary>
		/// Total number of default fonts
		/// </summary>
		Count
	};

	/// <summary>
	/// Singleton that stores a collection of RenderFont objects
	/// </summary>
	public class RenderFonts
	{
		/// <summary>
		/// Gets a default font
		/// </summary>
		public static RenderFont GetDefaultFont( DefaultFont font )
		{
			if ( ms_DefaultFonts[ ( int )font ] == null )
			{
				RenderFont newFont = RenderFactory.Inst.NewFont( );
				newFont.Setup( CreateDefaultSystemFont( font ) );
				ms_DefaultFonts[ ( int )font ] = newFont;
			}

			return ms_DefaultFonts[ ( int )font ];
		}

		/// <summary>
		/// Creates a default RenderFont
		/// </summary>
		private static System.Drawing.Font CreateDefaultSystemFont( DefaultFont font )
		{
			switch ( font )
			{
				case DefaultFont.Debug :
				{
					return new System.Drawing.Font( "arial", 12 );
				}
			}

			throw new ApplicationException( string.Format( "Unhandled default font \"{0}\"", font.ToString( ) ) );
		}

		private static RenderFont[]	ms_DefaultFonts = new RenderFont[ ( int )DefaultFont.Count ];

	}
}
