using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// OpenGL implementation of RenderFont.
	/// </summary>
	public class OpenGlFont : FontBase
	{


		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="fontData">Font data</param>
		public OpenGlFont( FontData fontData )
		{
			m_RenderState					= Graphics.Factory.CreateRenderState( );
			m_RenderState.DepthTest			= false;
			m_RenderState.Blend				= true;
			m_RenderState.SourceBlend		= BlendFactor.SrcAlpha;
			m_RenderState.DestinationBlend	= BlendFactor.OneMinusSrcAlpha;
			m_RenderState.FaceRenderMode	= PolygonRenderMode.Fill;
			m_RenderState.Enable2dTextures	= true;
			m_RenderState.Lighting			= false;

			Setup( fontData.Font, fontData.Characters );
		}

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <param name="characters">Set of characters to build the font texture from</param>
		/// <returns>Returns this</returns>
		public void Setup( Font font, FontData.CharacterSet characters )
		{
			Bitmap img = BuildFontImage( font, characters );
			img.Save( string.Format( "{0}{1}.png", font.Name, font.Size ), ImageFormat.Png );

			m_FontTextureSampler			= Graphics.Factory.CreateTexture2dSampler( );
			m_FontTextureSampler.Texture	= Graphics.Factory.CreateTexture2d( );
			m_FontTextureSampler.Texture.Load( img, false );
			m_FontTextureSampler.Mode		= TextureMode.Modulate;
			m_FontTextureSampler.MinFilter	= TextureFilter.NearestTexel;
			m_FontTextureSampler.MagFilter	= TextureFilter.NearestTexel;
		}

		#region FontBase Members

		/// <summary>
		/// Gets the height of the largest letter in the font
		/// </summary>
		public override int MaximumHeight
		{
			get { return m_MaxHeight; }
		}

		/// <summary>
		/// Measures the dimensions of a given string
		/// </summary>
		/// <param name="str">String to measure</param>
		/// <returns>Size of the string in pixels</returns>
		public override Size MeasureString( string str )
		{
			int width = 0;
			int height = 0;
			for ( int charIndex = 0; charIndex < str.Length; ++charIndex )
			{
				char curCh = str[ charIndex ];

				if ( curCh == ' ' )
				{
					width += 8;
				}
				else
				{
					width += m_CharacterData[ curCh ].Width;
					height = Utils.Max( m_CharacterData[ curCh ].Height, height );
				}
			}
			return new Size( width, height );
		}

		public override void Write( int x, int y, FontAlignment align, Color colour, string str )
		{
			Graphics.Renderer.Push2d( );
			Graphics.Renderer.PushTextures( );
			Graphics.Renderer.PushRenderState( m_RenderState );
			m_FontTextureSampler.Begin( );

			Gl.glColor3ub( colour.R, colour.G, colour.B );
			Gl.glBegin( Gl.GL_QUADS );
			int curX = x;
			int curY = y;
			float rcpWidth = 1.0f / m_FontTextureSampler.Texture.Width;
			float rcpHeight = 1.0f / m_FontTextureSampler.Texture.Height;

			AlignText( align, str, ref curX, ref curY );

			for ( int charIndex = 0; charIndex < str.Length; ++charIndex )
			{
				char curCh = str[ charIndex ];

				if ( curCh == ' ' )
				{
					curX += 8;
				}

				CharacterData charData = m_CharacterData[ curCh ];

				float u = ( charData.U ) * rcpWidth;
				float v = ( charData.V ) * rcpHeight;
				float maxU = u + ( charData.Width ) * rcpWidth;
				float maxV = v + ( charData.Height ) * rcpWidth;

				int maxX = curX + charData.Width;
				int maxY = curY + charData.Height;

				Gl.glTexCoord2f( u, v );
				Gl.glVertex2i( curX, curY );

				Gl.glTexCoord2f( maxU, v );
				Gl.glVertex2i( maxX, curY );

				Gl.glTexCoord2f( maxU, maxV );
				Gl.glVertex2i( maxX, maxY );

				Gl.glTexCoord2f( u, maxV );
				Gl.glVertex2i( curX, maxY );

				curX = maxX + 1;
			}

			Gl.glEnd( );
			m_FontTextureSampler.End( );
			Graphics.Renderer.PopTextures( );
			Graphics.Renderer.PopRenderState( );
			Graphics.Renderer.Pop2d( );
		}

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		public override void Write( float x, float y, float z, FontAlignment align, Color colour, string str )
		{
			Point3 screenPt = Graphics.Renderer.Project( new Point3( x, y, z ) );
			Write( ( int )screenPt.X, ( int )screenPt.Y, align, colour, str );
		}

		#endregion

		#region CharacterData

		/// <summary>
		/// Character UV data
		/// </summary>
		private struct CharacterData
		{
			public readonly short U;
			public readonly short V;
			public readonly byte Width;
			public readonly byte Height;

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

		#endregion

		#region Private Members

		private int m_MaxHeight;
		private ITexture2dSampler m_FontTextureSampler;
		private readonly CharacterData[] m_CharacterData = new CharacterData[ 256 ];
		private readonly IRenderState m_RenderState;

		/// <summary>
		/// Sets the (x,y) starting position of text, for a given alignment
		/// </summary>
		private void AlignText( FontAlignment align, string str, ref int x, ref int y )
		{
			if ( align == FontAlignment.TopLeft )
			{
				return;
			}
			Size size = MeasureString( str );

			switch ( align )
			{
				case FontAlignment.TopCentre :
					{
						x -= size.Width / 2;
						break;
					}
				case FontAlignment.TopRight:
					{
						x -= size.Width;
						break;
					}
				case FontAlignment.MiddleLeft :
					{
						y -= size.Height / 2;
						break;
					}
				case FontAlignment.MiddleCentre:
					{
						x -= size.Width / 2;
						y -= size.Height / 2;
						break;
					}
				case FontAlignment.MiddleRight:
					{
						x -= size.Width;
						y -= size.Height / 2;
						break;
					}
				case FontAlignment.BottomLeft:
					{
						y -= size.Height;
						break;
					}
				case FontAlignment.BottomCentre:
					{
						x -= size.Width / 2;
						y -= size.Height;
						break;
					}
				case FontAlignment.BottomRight:
					{
						x -= size.Width;
						y -= size.Height;
						break;
					}
			}

		}

		/// <summary>
		/// Measures the dimensions of a string, as required by BuildFontImage()
		/// </summary>
		private static Size MeasureString( System.Drawing.Graphics graphics, string str, Font font )
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
		private Bitmap BuildFontImage( Font font, FontData.CharacterSet characterSet )
		{
			string chars = new string( characterSet.Chars );
			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage( new Bitmap( 1, 1 ) );

			Size charSetSize = MeasureString( graphics, chars, font );
			graphics.Dispose( );

			m_MaxHeight = charSetSize.Height;

			//	HACK: Add a fair bit of padding to width and height when calculating required area
			int area = ( charSetSize.Width + 1 ) * ( charSetSize.Height + 1 );
			int size = 128;
			for ( ; ( size * size ) < area; size *= 2 ) { }

			//	Set up new image and graphics object to render to it
			Bitmap img = new Bitmap( size, size, PixelFormat.Format32bppRgb );
			graphics = System.Drawing.Graphics.FromImage( img );
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			StringFormat format = new StringFormat( StringFormatFlags.FitBlackBox );
			format.Alignment = StringAlignment.Near;

			int x = 0;
			int y = 0;
			for ( int charIndex = 0; charIndex < chars.Length; ++charIndex )
			{
				string charStr = chars.Substring( charIndex, 1 );
				SizeF charSize = graphics.MeasureString( charStr, font );
				int charWidth = ( int )charSize.Width;
				if ( ( x + charWidth ) >= size )
				{
					x = 0;
					y += ( charSetSize.Height );
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
				graphics.DrawString( charStr, font, Brushes.White, x, y, format );
				x += charWidth;
			}

			graphics.Dispose( );

			FillBmpAlpha( img );

			return img;
		}

		private unsafe static void FillBmpAlpha( Bitmap img )
		{
			BitmapData bmpData = img.LockBits( new System.Drawing.Rectangle( 0, 0, img.Width, img.Height ), ImageLockMode.ReadWrite, img.PixelFormat );
			byte* pixelMem = ( byte* )bmpData.Scan0.ToPointer( );
			byte* scanline = pixelMem;
			for ( int y = 0; y < bmpData.Height; ++y )
			{
				byte* pixel = scanline;
				for ( int x = 0; x < bmpData.Width; ++x )
				{
					pixel[ 3 ] = ( pixel[ 2 ] < 120 ? pixel[ 2 ] : ( byte )255 );
					pixel += 4;
				}
				scanline += bmpData.Stride;
			}
			img.UnlockBits( bmpData );
		}

		#endregion

	}
}
