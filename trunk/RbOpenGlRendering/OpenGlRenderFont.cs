using System;
using System.Drawing;
using System.Drawing.Imaging;
using RbEngine.Rendering;
using RbEngine.Maths;

using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Summary description for OpenGlRenderFont.
	/// </summary>
	public class OpenGlRenderFont : RenderFont
	{

		/// <summary>
		/// Character UV data
		/// </summary>
		private struct CharacterData
		{
			public short	U;
			public short	V;
			public byte		Width;
			public byte		Height;

			/// <summary>
			/// Sets up the character UV rectangle
			/// </summary>
			public CharacterData( short u, short v, byte width, byte height )
			{
				U = u;
				V = v;
				Width = width;
				Height = height;
			}
		}

		private CharacterData[]	m_CharacterData = new CharacterData[ 256 ];

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <param name="characters">Set of characters to build the font texture from</param>
		/// <returns>Returns this</returns>
		public override RenderFont	Setup( System.Drawing.Font font, CharacterSet characters )
		{
			Bitmap img = BuildFontImage( font, characters );
			img.Save( string.Format( "{0}{1}.png", font.Name, font.Size ), System.Drawing.Imaging.ImageFormat.Png );

			m_FontTextureSampler			= RenderFactory.Inst.NewTextureSampler2d( );
			m_FontTextureSampler.Texture	= RenderFactory.Inst.NewTexture2d( );
			m_FontTextureSampler.Texture.Load( img );
			m_FontTextureSampler.Mode		= TextureMode.Modulate;

			return this;
		}

		private RenderState	m_RenderState =
			RenderFactory.Inst.NewRenderState( )
				.DisableCap( RenderStateFlag.DepthTest )
				.SetBlendMode( BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha )
				.SetPolygonRenderingMode( PolygonRenderMode.Fill )
				.EnableCap( RenderStateFlag.Texture2d )
			;

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		/// <param name="str">Text to draw</param>
		public override void		DrawText( int x, int y, System.Drawing.Color colour, string str )
		{
			Renderer.Inst.PushRenderState( m_RenderState );
			m_FontTextureSampler.Begin( );

			Renderer.Inst.Push2d( );

			Gl.glColor3ub( colour.R, colour.G, colour.B );

			Gl.glBegin( Gl.GL_QUADS );
			int		curX		= x;
			int		curY		= y;
			float	rcpWidth	= 1.0f / ( float )m_FontTextureSampler.Texture.Width;
			float	rcpHeight	= 1.0f / ( float )m_FontTextureSampler.Texture.Height;
			for ( int charIndex = 0; charIndex < str.Length; ++charIndex )
			{
				char curCh = str[ charIndex ];

				if ( curCh == ' ' )
				{
					curX += 8;
				}

				CharacterData charData = m_CharacterData[ curCh ];

				float u		= ( float )( charData.U ) * rcpWidth;
				float v		= ( float )( charData.V ) * rcpHeight;
				float maxU	= u + ( float )( charData.Width ) * rcpWidth;
				float maxV	= v + ( float )( charData.Height ) * rcpWidth;

				int maxX = curX + charData.Width;
				int maxY = curY + charData.Height;

				Gl.glTexCoord2f( u, v );
				Gl.glVertex2i( curX, curY );

				Gl.glTexCoord2f( maxU, v );
				Gl.glVertex2f( maxX, curY );

				Gl.glTexCoord2f( maxU, maxV );
				Gl.glVertex2f( maxX, maxY );

				Gl.glTexCoord2f( u, maxV );
				Gl.glVertex2f( curX, maxY );

				curX = maxX;
			}

			Gl.glEnd( );
			m_FontTextureSampler.End( );
			Renderer.Inst.Pop2d( );
			Renderer.Inst.PopRenderState( );
		}

		private TextureSampler2d	m_FontTextureSampler;

		/// <summary>
		/// Measures the dimensions of a string, as required by BuildFontImage()
		/// </summary>
		private Size				MeasureString( Graphics graphics, string str, Font font )
		{
			Size stringSize = new Size( );
			for ( int charIndex = 0; charIndex < str.Length; ++charIndex )
			{
				string subString = str.Substring( charIndex, 1 );
				SizeF charSize = graphics.MeasureString( subString, font );

				stringSize.Width += ( int )( charSize.Width );
				stringSize.Height = Utils.Max( stringSize.Height, ( int )charSize.Height );
			}

			return stringSize;
		}

		/// <summary>
		/// Builds an image from this font
		/// </summary>
		private Bitmap				BuildFontImage( Font font, CharacterSet characterSet )
		{
			string		chars		= new string( characterSet.Chars );
			Graphics	graphics	= Graphics.FromImage( new Bitmap( 1, 1 ) );
			Size		charSetSize	= MeasureString( graphics, chars, font );
			graphics.Dispose( );

			//	HACK: Add a fair bit of padding to width and height when calculating required area
			int area		= ( charSetSize.Width + 1 ) * ( charSetSize.Height + 1 );
			int size		= 128;
			for ( ; ( size * size ) < area; size *= 2 );

			//	Set up new image and graphics object to render to it
			Bitmap img = new Bitmap( size, size, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
			graphics = Graphics.FromImage( img );
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			StringFormat format = new StringFormat( StringFormatFlags.FitBlackBox );
			format.Alignment = StringAlignment.Near;

			int x = 0;
			int y = 0;
			for ( int charIndex = 0; charIndex < chars.Length; ++charIndex )
			{
				string	charStr		= chars.Substring( charIndex, 1 );
				SizeF	charSize	= graphics.MeasureString( charStr, font );
				int		charWidth	= ( int )charSize.Width;
				if ( ( x + charWidth ) >= size )
				{
					x = 0;
					y += ( int )( charSetSize.Height );
				}

				if ( chars[ charIndex ] < 256 )
				{
					int xPadding = 3;
					int yPadding = 0;
					m_CharacterData[ chars[ charIndex ] ] = new CharacterData
						(
							checked( ( short )( x + xPadding ) ),
							checked( ( short )( y + yPadding ) ),
							checked( ( byte )( charSize.Width - xPadding * 2 ) ),
							checked( ( byte )( charSize.Height - yPadding * 2 ) )
						);
				}
				else
				{
					throw new ApplicationException( "not handling non-ascii characters yet, sorry" );
				}

			//	graphics.DrawRectangle( System.Drawing.Pens.Red, x, y, charWidth, charSetSize.Height );
				graphics.DrawString( charStr, font, System.Drawing.Brushes.White, x, y, format );

				x += charWidth;
			}

			graphics.Dispose( );

			FillBmpAlpha( img );

			return img;
		}

		private unsafe void FillBmpAlpha( Bitmap img )
		{
			BitmapData bmpData = img.LockBits( new Rectangle( 0, 0, img.Width, img.Height ), ImageLockMode.ReadWrite, img.PixelFormat );
			byte* pixelMem = ( byte* )bmpData.Scan0.ToPointer( );
			byte* scanline = pixelMem;
			for ( int y = 0; y < bmpData.Height; ++y )
			{
				byte* pixel = scanline;
				for ( int x = 0; x < bmpData.Width; ++x )
				{
					pixel[ 3 ] = pixel[ 2 ];
					pixel += 4;
				}
				scanline += bmpData.Stride;
			}
			img.UnlockBits( bmpData );
		}
	}
}
