using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Generates transition masks for all 256 possible combinations of tiles surrounding a 
	/// </summary>
	public class TileTransitionMaskGenerator
	{
		/// <summary>
		/// Transition mask
		/// </summary>
		public class Mask
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="masks">Collection that this mask is a part of</param>
			/// <param name="code">Code that generated this mask</param>
			/// <param name="mask">Mask 2d array</param>
			public Mask( MaskCollection masks, byte code, byte[,] mask )
			{
				m_Masks = masks;
				m_Code = code;
				m_Mask = mask;
			}

			public byte Code
			{
				get { return m_Code; }
			}

			public byte[,] MaskMap
			{
				get { return m_Mask; }
			}

			/// <summary>
			/// Converts this mask to a bitmap
			/// </summary>
			/// <returns></returns>
			public Bitmap ToBitmap( )
			{
				byte[,] mask = m_Mask;

				int width = mask.GetLength( 0 );
				int height = mask.GetLength( 1 );

				Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );
				for ( int y = 0; y < height; ++y )
				{
					for ( int x = 0; x < width; ++x )
					{
						byte val = mask[ x, y ];
						bmp.SetPixel( x, y, Color.FromArgb( val, val, val ) );
					}
				}

				return bmp;
			}

			private readonly MaskCollection m_Masks;
			private readonly byte[,] m_Mask;
			private readonly byte m_Code;
		}

		public class MaskCollection
		{
			public MaskCollection( int length )
			{
				m_Masks = new Mask[ length ];
			}

			public Mask this[ int index ]
			{
				set { m_Masks[ index ] = value; }
				get { return m_Masks[ index ]; }
			}

			public int Length
			{
				get { return m_Masks.Length; }
			}

			private readonly Mask[] m_Masks;
		}

		/// <summary>
		/// Rounds val up to the next power of 2
		/// </summary>
		private static int RoundUpToPowerOf2( int val )
		{
			//	TODO: AP: Update to proper method
			int highest = 0;
			for ( int i = 0; i < 32; ++i )
			{
				if ( ( val & ( 1 << i ) ) != 0 )
				{
					highest = i;
				}
			}
			return ( val & ( ( 1 << highest ) - 1 ) ) == 0 ? val : ( 1 << ( highest + 1 ) );
		}

		/// <summary>
		/// Creates tile transition masks
		/// </summary>
		/// <param name="maskWidth">Width of all masks</param>
		/// <param name="maskHeight">Height of all masks</param>
		/// <returns>Returns a new <see cref="TileTransitionMasks"/> containing all 256 transition masks</returns>
		public unsafe TileTransitionMasks Create( int maskWidth, int maskHeight )
		{
			int numMasks		= TileByteCodes.OriginalBitCodes.Length;
			int masksPerLine	= ( int )Math.Sqrt( numMasks );
			int lines			= ( numMasks / masksPerLine ) + 1;

			int bitmapWidth		= RoundUpToPowerOf2( masksPerLine * ( maskWidth + 1 ) );
			int bitmapHeight	= RoundUpToPowerOf2( lines * ( maskHeight + 1 ) );
			Bitmap bmp = new Bitmap( bitmapWidth, bitmapHeight, PixelFormat.Format24bppRgb );

			int numMasksOnLine = 0;
			int startX = 0;
			int startY = 0;

			float textureWidth = maskWidth / ( float )bitmapWidth;
			float textureHeight = maskHeight / ( float )bitmapHeight;
			TileTransitionMasks.TextureRect[] textureRects = new TileTransitionMasks.TextureRect[ 256 ];

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* pixelBytes = ( byte* )bmpData.Scan0.ToPointer( );
			{
				foreach ( byte cornerCode in TileByteCodes.OriginalBitCodes )
				{
					byte[ , ] mask = new byte[ maskWidth, maskHeight ];
					GenerateMask( cornerCode, maskWidth, maskHeight, mask );

					if ( ++numMasksOnLine >= masksPerLine )
					{
						numMasksOnLine = 0;
						startX = 0;
						startY += maskHeight + 1;
					}
					
					float u = startX / ( float )bitmapWidth;
					float v = startY / ( float )bitmapHeight;
					textureRects[ cornerCode ] = new TileTransitionMasks.TextureRect( u, v, textureWidth, textureHeight );

					byte* curLine = pixelBytes + ( startX * 3 ) + ( startY * bmpData.Stride );
					for ( int y = 0; y < maskHeight; ++y )
					{
						byte* curPixel = curLine; 
						for ( int x = 0; x < maskWidth; ++x )
						{
							byte val = mask[ x, y ];
							curPixel[ 0 ] = val;
							curPixel[ 1 ] = val;
							curPixel[ 2 ] = val;
							curPixel += 3;
						}
						curLine += bmpData.Stride;
					}

					startX += maskWidth + 1;
				}
			}
			bmp.UnlockBits( bmpData );

			for ( int cornerCodeCounter = 0; cornerCodeCounter < 256; ++cornerCodeCounter )
			{
				byte cornerCode = ( byte )cornerCodeCounter;
				byte original = TileByteCodes.Original( cornerCode );
				if ( cornerCode == original )
				{
					continue;
				}
				TileTransitionMasks.TextureRect orgTextureRect = textureRects[ original ];
				TileTransitionMasks.TextureRect textureRect = new TileTransitionMasks.TextureRect( orgTextureRect );
				if ( TileByteCodes.MirrorRequired( cornerCode ) )
				{
					textureRect.Mirror( );
				}
				textureRect.Rotate( TileByteCodes.RotationsRequired( cornerCode ) );

				textureRects[ cornerCode ] = textureRect;
			}

			Texture2d texture = RenderFactory.Instance.NewTexture2d( );
			texture.Load( bmp );

			TileTransitionMasks masks = new TileTransitionMasks( texture, textureRects );

			return masks;
		}

		/// <summary>
		/// Creates an array of 256 transition masks for every possible corner code combination
		/// </summary>
		/// <param name="maskWidth">Width of the masks</param>
		/// <param name="maskHeight">Height of the masks</param>
		/// <returns>
		/// Returns all 256 masks
		/// </returns>
		public MaskCollection Generate( int maskWidth, int maskHeight )
		{
			MaskCollection masks = new MaskCollection( 256 );

			foreach ( byte cornerCode in TileByteCodes.OriginalBitCodes )
			{
				byte[ , ] mask = new byte[ maskWidth, maskHeight ];
				GenerateMask( cornerCode, maskWidth, maskHeight, mask );
				masks[ cornerCode ] = new Mask( masks, cornerCode, mask );
			}

			for ( int cornerCodeCounter = 0; cornerCodeCounter < masks.Length; ++cornerCodeCounter )
			{
				GenerateMaskFromOriginal( ( byte )cornerCodeCounter, maskWidth, maskHeight, masks );
			}

			return masks;
		}

		#region Private members

		/// <summary>
		/// Generates a mask from an original, by mirroring then rotating it. (not essential - just a test of this system)
		/// </summary>
		private static void GenerateMaskFromOriginal( byte code, int maskWidth, int maskHeight, MaskCollection masks )
		{
			if ( masks[ code ] != null )
			{
				//	Mask was already generated
				return;
			}

			byte[,] original = masks[ TileByteCodes.Original( code ) ].MaskMap;
			byte[,] mask = new byte[ maskWidth, maskHeight ];

			if ( TileByteCodes.MirrorRequired( code ) )
			{
				int mirrorWidth = maskWidth - 1;
				for ( int y = 0; y < maskHeight; ++y )
				{
					for ( int x = 0; x < maskWidth; ++x )
					{
						mask[x, y] = original[ mirrorWidth - x, y ];
					}
				}
				original = ( byte[,] )mask.Clone( );
			}
			int rotations = TileByteCodes.RotationsRequired( code );
			if ( rotations > 0 )
			{
				int startX = 0;
				int startY = 0;
				int incX = 1;
				int incY = 1;
				bool swapAxis = false;
				switch (rotations)
				{
					case 1:
						startX = maskHeight - 1;
						startY = 0;
						incX = -1;
						incY = 1;
						swapAxis = true;
						break;
					case 2:
						startX = maskWidth - 1;
						startY = maskHeight - 1;
						incX = -1;
						incY = -1;
						swapAxis = false;
						break;
					case 3:
						startX = 0;
						startY = maskWidth - 1;
						incX = 1;
						incY = -1;
						swapAxis = true;
						break;
				}

				int lookupY = startY;
				for ( int y = 0; y < maskHeight; ++y, lookupY += incY )
				{
					int lookupX = startX;
					for ( int x = 0; x < maskWidth; ++x, lookupX += incX )
					{
						mask[ x, y ] = swapAxis ? original[ lookupY, lookupX ] : original[ lookupX, lookupY ];
					}
				}
			}

			masks[ code ] = new Mask( masks, code, mask );
		}

		/// <summary>
		/// Returns either 0 or 0xff depending on the value of the bit in code at position offset
		/// </summary>
		private static byte CodeColour( byte code, int offset )
		{
			return ( byte )( ( code & ( 1 << offset ) ) == 0 ? 0x00 : 0xff );
		}

		/// <summary>
		/// Returns 0 unless both bytes at positions (x0,y0) and (x1,y1) are non-zero, in which case 0xff is returned
		/// </summary>
		private static byte GetCombinedValue( byte[,] mask, int x0, int y0, int x1, int y1 )
		{
			byte val0 = mask[ x0, y0 ];
			byte val1 = mask[ x1, y1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 ) ? 0xff : 0 );
		}

		/// <summary>
		/// Returns 0 unless both bytes at positions (x,y) and (x+1,y+1) are non-zero, and bytes at positions (x+1,y) or (x,y+1) are non zero
		/// </summary>
		private static byte GetCombinedDiagonalValue( byte[ , ] smoothMask, int x0, int y0 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x0 + 1, y0 + 1 ];
			byte val2 = smoothMask[ x0 + 1, y0 ];
			byte val3 = smoothMask[ x0, y0 + 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 && ( ( val2 > 0 ) || ( val3 > 0 ) ) ) ? 0xff : 0 );
		}

		/// <summary>
		/// Returns 0 unless both bytes at positions (x,y) and (x-1,y+1) are non-zero, and bytes at positions (x-1,y) or (x,y+1) are non zero
		/// </summary>
		private static byte GetCombinedDiagonalValueInvX( byte[ , ] smoothMask, int x0, int y0 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x0 - 1, y0 + 1 ];
			byte val2 = smoothMask[ x0 - 1, y0 ];
			byte val3 = smoothMask[ x0, y0 + 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 && ( ( val2 > 0 ) || ( val3 > 0 ) ) ) ? 0xff : 0 );
		}

		/// <summary>
		/// Returns 0 unless both bytes at positions (x,y) and (x+1,y-1) are non-zero, and bytes at positions (x+1,y) or (x,y-1) are non zero
		/// </summary>
		private static byte GetCombinedDiagonalValueInvY( byte[ , ] smoothMask, int x0, int y0 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x0 + 1, y0 - 1 ];
			byte val2 = smoothMask[ x0 + 1, y0 ];
			byte val3 = smoothMask[ x0, y0 - 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 && ( ( val2 > 0 ) || ( val3 > 0 ) ) ) ? 0xff : 0 );
		}

		/// <summary>
		/// Makes a 7x7 map that is used to smooth out the nearestMask
		/// </summary>
		private static byte[,] MakeSmoothMask( byte[,] nearestMask, bool fatMiddle )
		{
			byte[,] mask = new byte[ 7, 7 ];

			//	Set values in the mask
			for ( int y = 0; y < 7; ++y )
			{
				int nearY = y / 3;
				bool gridY = ( y % 3 ) == 0;
				for ( int x = 0; x < 7; ++x )
				{
					int nearX = x / 3;
					bool gridX = ( x % 3 ) == 0;
					if ( gridX && gridY )
					{
						mask[ x, y ] = nearestMask[ nearX, nearY ];
					}
					else if ( gridX )
					{
						mask[ x, y ] = GetCombinedValue( nearestMask, nearX, nearY, nearX, nearY + 1 );
					}
					else if ( gridY )
					{
						mask[ x, y ] = GetCombinedValue( nearestMask, nearX, nearY, nearX + 1, nearY );
					}
				}
			}

			if ( fatMiddle )
			{
				mask[ 2, 2 ] = mask[ 3, 3 ];
				mask[ 3, 2 ] = mask[ 3, 3 ];
				mask[ 4, 2 ] = mask[ 3, 3 ];

				mask[ 2, 3 ] = mask[ 3, 3 ];
				mask[ 4, 3 ] = mask[ 3, 3 ];

				mask[ 2, 4 ] = mask[ 3, 3 ];
				mask[ 3, 4 ] = mask[ 3, 3 ];
				mask[ 4, 4 ] = mask[ 3, 3 ];
			}

			mask[ 1, 1 ] = GetCombinedDiagonalValue( nearestMask, 0, 0 );
			mask[ 1, 5 ] = GetCombinedDiagonalValueInvY( nearestMask, 0, 2 );
			mask[ 5, 1 ] = GetCombinedDiagonalValueInvX( nearestMask, 2, 0 );
			mask[ 5, 5 ] = GetCombinedDiagonalValue( nearestMask, 1, 1 );

			if ( !fatMiddle )
			{
				mask[ 2, 2 ] = mask[ 1, 1 ];
				mask[ 2, 4 ] = mask[ 1, 5 ];
				mask[ 4, 2 ] = mask[ 5, 1 ];
				mask[ 4, 4 ] = mask[ 5, 5 ];
			}


			mask[ 2, 1 ] = GetCombinedValue( mask, 1, 1, 3, 1 );
			mask[ 1, 2 ] = GetCombinedValue( mask, 1, 1, 1, 3 );

			mask[ 4, 1 ] = GetCombinedValue( mask, 3, 1, 5, 1 );
			mask[ 5, 2 ] = GetCombinedValue( mask, 5, 1, 5, 3 );

			mask[ 1, 4 ] = GetCombinedValue( mask, 1, 3, 1, 5 );
			mask[ 2, 5 ] = GetCombinedValue( mask, 1, 5, 3, 5 );

			mask[ 4, 5 ] = GetCombinedValue( mask, 3, 5, 5, 5 );
			mask[ 5, 4 ] = GetCombinedValue( mask, 5, 3, 5, 5 );

			return mask;
		}

		/// <summary>
		/// Generates a mask for a given corner code
		/// </summary>
		private static void GenerateMask( byte cornerCode, int width, int height, byte[,] mask )
		{
			byte[ , ] nearestMask = new byte[ 3, 3 ]
				{
					{ CodeColour( cornerCode, 0 ), CodeColour( cornerCode, 7 ), CodeColour( cornerCode, 6 ) },
					{ CodeColour( cornerCode, 1 ),			0xff,				CodeColour( cornerCode, 5 ) },
					{ CodeColour( cornerCode, 2 ), CodeColour( cornerCode, 3 ), CodeColour( cornerCode, 4 ) }
				};

			int[] smoothYEnds = new int[ 6 ]
			{
				( int )( height * 1.0f / 6.0f ),
				( int )( height * 2.0f / 6.0f ),
				( int )( height * 3.0f / 6.0f ),
				( int )( height * 4.0f / 6.0f ),
				( int )( height * 5.0f / 6.0f ),
				height
			};
			int[] smoothXEnds = new int[ 6 ]
			{
				( int )( width * 1.0f / 6.0f ),
				( int )( width * 2.0f / 6.0f ),
				( int )( width * 3.0f / 6.0f ),
				( int )( width * 4.0f / 6.0f ),
				( int )( width * 5.0f / 6.0f ),
				width
			};

			float smoothWidth = width / 6.0f;
			float smoothHeight = height / 6.0f;
			int smoothY = 0;
			int smoothYStart = 0;
			int smoothYEnd = smoothYEnds[ 0 ];
			byte[ , ] smoothMask = MakeSmoothMask( nearestMask, false );

			for ( int y = 0; y < height; ++y )
			{
				if ( y >= smoothYEnd )
				{
					smoothYStart = smoothYEnd;
					smoothYEnd = smoothYEnds[ ++smoothY ];
				}

				float localY = ( y - smoothYStart ) / smoothHeight;

				int smoothX = 0;
				int smoothXStart = 0;
				int smoothXEnd = smoothXEnds[ 0 ];
				byte tl = smoothMask[ smoothX, smoothY ];
				byte tr = smoothMask[ smoothX + 1, smoothY ];
				byte bl = smoothMask[ smoothX, smoothY + 1 ];
				byte br = smoothMask[ smoothX + 1, smoothY + 1 ];

				for ( int x = 0; x < width; ++x )
				{
					if ( x >= smoothXEnd )
					{
						smoothXStart = smoothXEnd;
						smoothXEnd = smoothXEnds[ ++smoothX ];

						tl = tr;
						tr = smoothMask[ smoothX + 1, smoothY ];
						bl = br;
						br = smoothMask[ smoothX + 1, smoothY + 1 ];
					}

					float localX = ( x - smoothXStart ) / smoothWidth;

					float val0 = Utils.Lerp( tl, tr, localX );
					float val1 = Utils.Lerp( bl, br, localX );
					float val2 = Utils.Lerp( val0, val1, localY );

					mask[ x, y ] = ( byte )( val2 < 0 ? 0 : val2 > 255 ? 255 : val2);
				}
			}
		}

		#endregion
	}
}
