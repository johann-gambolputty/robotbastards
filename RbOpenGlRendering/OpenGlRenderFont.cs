using System;
using System.Drawing;
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

		private struct CharacterData
		{
			public short	U;
			public short	V;
			public byte		Width;
			public byte		Height;

			public CharacterData( short u, short v, byte width, byte height )
			{
				U = u;
				V = v;
				Width = width;
				Height = height;
			}
		}

		private CharacterData[]	m_AsciiData = new CharacterData[ 256 ];

		/// <summary>
		/// Builds this font from a System.Drawing.Font object
		/// </summary>
		/// <param name="font">Font to build from</param>
		/// <param name="characters">Set of characters to build the font texture from</param>
		/// <returns>Returns this</returns>
		public override RenderFont	Setup( System.Drawing.Font font, CharacterSet characters )
		{
			Bitmap img = BuildFontImage( font, characters );
			img.Save( string.Format( "{0}{1}.bmp", font.Name, font.Size ), System.Drawing.Imaging.ImageFormat.Bmp );

			m_FontTextureSampler			= RenderFactory.Inst.NewTextureSampler2d( );
			m_FontTextureSampler.Texture	= RenderFactory.Inst.NewTexture2d( );
			m_FontTextureSampler.Texture.Load( img );

			return this;
		}

		/// <summary>
		/// Draws text using this font, at a given position
		/// </summary>
		/// <param name="str">Text to draw</param>
		public override void		DrawText( int x, int y, string str )
		{
			Gl.glEnable( Gl.GL_TEXTURE_2D );
			Gl.glPolygonMode( Gl.GL_FRONT_AND_BACK, Gl.GL_FILL );
			m_FontTextureSampler.Apply( );

			Renderer.Inst.Push2d( );

			Gl.glBegin( Gl.GL_QUADS );

				Gl.glVertex2f( -1, -1 );
				Gl.glTexCoord2i( 0, 0 );

				Gl.glTexCoord2i( 0, 256 );
				Gl.glVertex2f( -1, 1 );

				Gl.glVertex2f( 1, 1 );
				Gl.glTexCoord2i( 256, 256 );

				Gl.glVertex2f( 1, -1 );
				Gl.glTexCoord2i( 256, 0 );


			Gl.glEnd( );

			Renderer.Inst.Pop2d( );
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
					m_AsciiData[ chars[ charIndex ] ] = new CharacterData( checked( ( short )x ), checked( ( short )y ), checked( ( byte )charSize.Width ), checked( ( byte )charSize.Height ) );
				}
				else
				{
					throw new ApplicationException( "not handling non-ascii characters yet, sorry" );
				}

				graphics.DrawRectangle( System.Drawing.Pens.Red, x, y, charWidth, charSetSize.Height );
				graphics.DrawString( charStr, font, System.Drawing.Brushes.White, x, y, format );

				x += charWidth;
			}

			graphics.Dispose( );

			return img;

			/*

			//	Was the font creation succesful?
			if (hFont == NULL)
			{
				G_MESSAGEF(eMsgLvl_MinorError,_T("Unable to create font object for font '%s'\n"),Params.m_pFontName);
				return false;
			}

			const TCHAR MinChar		= Params.m_MinChar;
			const TCHAR MaxChar		= Params.m_MaxChar;
			const int	NumChars	= MaxChar - MinChar;

			Font.m_MinCh			= MinChar;
			Font.m_MaxCh			= MaxChar;

			//	Create a device context to render to and select the font in it
			HDC			hDC	 = CreateCompatibleDC(NULL);
			SetMapMode(hDC,MM_TEXT);
			SelectObject(hDC,hFont);

			//	Find out the dimensions of the largest character in the following range
			int TotalFontArea;
			{
				TCHAR	AllChars[256];
				int	CurCharIn = 0;
				for (TCHAR CurChar = MinChar; CurChar < MaxChar; AllChars[CurCharIn++] = CurChar++);
				AllChars[CurCharIn] = _T('\0');

				SIZE TxtSize;
				GetTextExtentPoint32(hDC,AllChars,CurCharIn,&TxtSize);

				TotalFontArea = int((TxtSize.cx + NumChars) * (TxtSize.cy + 1));
			}

			//	Calculate the minimum size of the font image (in powers of 2)
			const TVector2I	MaxTexDims	= static_cast<CPCRenderer&>(Renderer).GetMaxTextureDims();
			float			TextScale	= float(1);
			const int		MaxTexSize	= _Min(MaxTexDims[0],MaxTexDims[1]);
			int				TexWidth	= 0;
			int				TexHeight	= 0;

			{
				int	CurSize	= 32;
				for (; (CurSize * CurSize) < TotalFontArea; CurSize *= 2);

				if (CurSize > MaxTexSize)
				{
					TexWidth	= MaxTexSize;
					TexHeight	= MaxTexSize;
					TextScale	= float(MaxTexSize) / float(CurSize); 
				}
				else
				{
					TexWidth	= CurSize;
					TexHeight	= CurSize;
				}
			} 

			//	Create a bitmap for the DC to render to
			BITMAPINFO	BMPInfo;
			ZeroMemory(&BMPInfo,sizeof(BMPInfo));

			//	Setup a surface to render to
			BMPInfo.bmiHeader.biSize			= sizeof(BITMAPINFOHEADER);
			BMPInfo.bmiHeader.biBitCount		= 32;
			BMPInfo.bmiHeader.biClrImportant	= 0;
			BMPInfo.bmiHeader.biClrUsed			= 0;
			BMPInfo.bmiHeader.biCompression		= BI_RGB;
			BMPInfo.bmiHeader.biWidth			= TexWidth;
			BMPInfo.bmiHeader.biHeight			= -TexHeight;
			BMPInfo.bmiHeader.biPlanes			= 1;

			//	Create the DIB
			unsigned char * pDIBBits;
			HBITMAP	hBitmap	= CreateDIBSection(hDC,&BMPInfo,DIB_RGB_COLORS,(void**)&pDIBBits,NULL,0);

			//	Check that the bitmap was created ok
			if (hBitmap == NULL)
			{
				DeleteDC(hDC);
				DeleteObject(hFont);
				TCHAR	Err[1024];
				G_MESSAGEF(eMsgLvl_MinorError,_T("Failed to create bitmap for font '%s' ('%s')\n"),Params.m_pFontName,_GetOSError(sizeof(Err) / sizeof(Err[0]),Err));
				return false;
			}

			//	Setup the text rendering properties
			SetTextColor(hDC,RGB(255,255,255));
			SetBkColor(hDC,0);
			SetTextAlign(hDC,TA_TOP);
			SelectObject(hDC,hBitmap);

			//	Render the font using hDC
			{
				Font.m_UVs.resize(NumChars);

				int							CurX			= 0;
				int							CurY			= 0;
				int							MaxCharWidth	= 0;
				int							MaxCharHeight	= 0;
				int							AvgCharWidth	= 0;
				int							AvgCharHeight	= 0;
				TCHAR						TmpString[2]	= { '\0','\0' };
				int							CurCharIn		= 0;
				const float					InvTexWidth		= float(1) / float(TexWidth);
				const float					InvTexHeight	= float(1) / float(TexHeight);
				CPCFont::TUVList::iterator	pCurRect		= Font.m_UVs.begin();

				for (TCHAR CurChar = MinChar; CurChar < MaxChar; ++CurChar, ++CurCharIn, ++pCurRect)
				{

					//	Get the extent of the current character
					TmpString[0] = CurChar;
					SIZE TxtSize;
					GetTextExtentPoint32(hDC,TmpString,1,&TxtSize);

					//	Check character sizes
					if (TxtSize.cx > MaxCharWidth)
					{
						MaxCharWidth	= TxtSize.cx;
					}

					if (TxtSize.cy > MaxCharHeight)
					{
						MaxCharHeight	= TxtSize.cy;
					}

					AvgCharWidth += TxtSize.cx;
					AvgCharHeight += TxtSize.cy;

					//	Check if CurX should wrap to the next line
					if (CurX + TxtSize.cx + 1 >= TexWidth)
					{
						CurY += (TxtSize.cy + 1);
						CurX  = 0;
					}
					TextOut(hDC,CurX,CurY,TmpString,1);

					//	Setup the current entry in the character size array
					pCurRect->Set(float(CurX) * InvTexWidth,
								float(CurY) * InvTexHeight,
								float(TxtSize.cx) * InvTexWidth,
								float(TxtSize.cy) * InvTexHeight);

					//	Increment the x position
					CurX += (TxtSize.cx + 1);
				}

				AvgCharWidth	/= (MaxChar - MinChar);
				AvgCharHeight	/= (MaxChar - MinChar);

				//	Store the largest character sizes
				Font.m_MaxWidth		= MaxCharWidth;
				Font.m_MaxHeight	= MaxCharHeight;

				//	Store the average character sizes
				Font.m_AvgWidth		= AvgCharWidth;
				Font.m_AvgHeight	= AvgCharHeight;
			}
			
			//	Create a texture to store the font image
			CTexture2D* pDstTex = _CreateGraphicsResource<CTexture2D>(Renderer.GetResourceFactory());
			if (pDstTex == 0)
			{
				G_MESSAGEF(eMsgLvl_MinorError,_T("Failed to create texture for font '%s'\n"),Params.m_pFontName);
				return false;
			}

			//	Allocate texture memory
			pDstTex->Create(Renderer,CTexture2D::SCreateParams(TexWidth,TexHeight,eCF_A8R8G8B8,CTexture2D::eUsage_Static,false));

			//	Copy the DIB bits to the texture memory
			const CTexture2D::SLockParams	LockParams;
			CTexture2D::SLockResult			LockRes = pDstTex->Lock(LockParams);
			if (LockRes.IsValid())
			{
				const unsigned int *	pSrcIm		= reinterpret_cast<unsigned int*>(pDIBBits);
				unsigned int *			pDstIm		= reinterpret_cast<unsigned int*>(LockRes.m_pTexels);
				const int				SrcImInc	= -TexWidth;
				for (unsigned int i = 0, Max = TexWidth * TexHeight; i < Max; i++)
				{
					if (*pSrcIm > 0)
					{
						*(pDstIm++) = (*(pSrcIm++) | 0xff000000);
					}
					else
					{
						*(pDstIm++) = (*(pSrcIm++) & 0x00ffffff);
					}
				}

				pDstTex->Unlock(LockParams);
				Font.m_pTexture = pDstTex;
			}
			else
			{
				G_MESSAGEF(eMsgLvl_MinorError,_T("Failed to lock texture memory for font '%s'\n"),Params.m_pFontName);
				Renderer.GetResourceFactory().Destroy(pDstTex);
			}

			//	Clean up the bitmap and the DC
			DeleteObject(hBitmap);
			DeleteDC(hDC);
			DeleteObject(hFont);

			//	Return the texture
			return true;
			*/
		}
	}
}
