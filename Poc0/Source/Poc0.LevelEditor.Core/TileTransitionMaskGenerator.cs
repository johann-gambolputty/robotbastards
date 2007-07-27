using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;

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
		public struct Mask
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="mask">Mask 2d array</param>
			public Mask( byte[,] mask )
			{
				m_Mask = mask;
			}

			/// <summary>
			/// Converts this mask to a bitmap
			/// </summary>
			/// <returns></returns>
			public Bitmap ToBitmap( )
			{
				int width = m_Mask.GetLength( 0 );
				int height = m_Mask.GetLength( 1 );

				Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );

				for ( int y = 0; y < height; ++y )
				{
					for ( int x = 0; x < width; ++x )
					{
						byte val = m_Mask[ x, y ];
						bmp.SetPixel( x, y, Color.FromArgb( val, val, val ) );
					}
				}

				return bmp;
			}

			/// <summary>
			/// Returns the mask map
			/// </summary>
			public byte[,] Map
			{
				get { return m_Mask; }
			}

			/// <summary>
			/// Mask accessor
			/// </summary>
			public byte this[ int x, int y ]
			{
				get { return m_Mask[ x, y ]; }
				set { m_Mask[ x, y ] = value; }
			}

			private readonly byte[,] m_Mask;
		}

		/// <summary>
		/// Creates an array of 256 transition masks for every possible corner code combination
		/// </summary>
		/// <param name="maskWidth">Width of the masks</param>
		/// <param name="maskHeight">Height of the masks</param>
		/// <returns>
		/// Returns all 256 masks
		/// </returns>
		public Mask[] Generate( int maskWidth,int maskHeight )
		{
			Mask[] masks = new Mask[ 256 ];

			for (int cornerCode = 0; cornerCode < masks.Length; ++cornerCode)
			{
				byte[ , ] mask = new byte[ maskWidth, maskHeight ];
				GenerateMask( ( byte )cornerCode, maskWidth, maskHeight, mask );
				masks[ cornerCode ] = new Mask( mask );
			}

			return masks;
		}

		private static byte CodeColour( byte code, int offset )
		{
			return ( byte )( ( code & ( 1 << offset ) ) == 0 ? 0x00 : 0xff );
		}

		private static byte SmoothMaskValueX( byte[,] nearestMask, int x, int y )
		{
			byte val0 = nearestMask[ x, y ];
			byte val1 = nearestMask[ x + 1, y ];
			return ( byte )( ( val0 > 0 && val1 > 0 ) ? 0xff : 0 );
		}

		private static byte SmoothMaskValueY( byte[ , ] nearestMask, int x, int y )
		{
			byte val0 = nearestMask[ x, y ];
			byte val1 = nearestMask[ x, y + 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 ) ? 0xff : 0 );
		}

		private static byte SmoothMaskValueXY( byte[ , ] nearestMask, int x, int y )
		{
			byte val0 = nearestMask[ x, y ];
			byte val1 = nearestMask[ x + 1, y + 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 ) ? 0xff : 0 );
		}

		private static byte SmoothMaskValueXInvY( byte[ , ] nearestMask, int x, int y )
		{
			byte val0 = nearestMask[ x, y ];
			byte val1 = nearestMask[ x + 1, y - 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 ) ? 0xff : 0 );
		}

		private static byte GetCombinedValue( byte[ , ] smoothMask, int x0, int y0 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x0 + 1, y0 + 1 ];
			byte val2 = smoothMask[ x0 + 1, y0 ];
			byte val3 = smoothMask[ x0, y0 + 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 && ( ( val2 > 0 ) || ( val3 > 0 ) ) ) ? 0xff : 0 );
		}

		private static byte GetCombinedValueInvX( byte[ , ] smoothMask, int x0, int y0 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x0 - 1, y0 + 1 ];
			byte val2 = smoothMask[ x0 - 1, y0 ];
			byte val3 = smoothMask[ x0, y0 + 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 && ( ( val2 > 0 ) || ( val3 > 0 ) ) ) ? 0xff : 0 );
		}
		private static byte GetCombinedValueInvY( byte[ , ] smoothMask, int x0, int y0 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x0 + 1, y0 - 1 ];
			byte val2 = smoothMask[ x0 + 1, y0 ];
			byte val3 = smoothMask[ x0, y0 - 1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 && ( ( val2 > 0 ) || ( val3 > 0 ) ) ) ? 0xff : 0 );
		}
		private static byte GetCombinedValue( byte[,] smoothMask, int x0, int y0, int x1, int y1 )
		{
			byte val0 = smoothMask[ x0, y0 ];
			byte val1 = smoothMask[ x1, y1 ];
			return ( byte )( ( val0 > 0 && val1 > 0 ) ? 0xff : 0 );
		}

		private static byte[,] MakeSmoothMask( byte[,] nearestMask )
		{
			byte[,] mask = new byte[ 7, 7 ];

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
						mask[ x, y ] = SmoothMaskValueY( nearestMask, nearX, nearY );
					}
					else if ( gridY )
					{
						mask[ x, y ] = SmoothMaskValueX( nearestMask, nearX, nearY );
					}
					//else if ( x == y )
					//{
					//    mask[ x, y ] = SmoothMaskValueXY( nearestMask, nearX, nearY );
					//}
					//else if ( x == ( 6 - y ) )
					//{
					//    mask[ x, y ] = SmoothMaskValueXInvY( nearestMask, nearX, nearY + 1 );
					//}
				}
			}

			bool fatMiddle = false;

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

			mask[ 1, 1 ] = GetCombinedValue( nearestMask, 0, 0 );
			mask[ 1, 5 ] = GetCombinedValueInvY( nearestMask, 0, 2 );
			mask[ 5, 1 ] = GetCombinedValueInvX( nearestMask, 2, 0 );
			mask[ 5, 5 ] = GetCombinedValue( nearestMask, 1, 1 );

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
					{ CodeColour( cornerCode, 0 ), CodeColour( cornerCode, 1 ), CodeColour( cornerCode, 2 ) },
					{ CodeColour( cornerCode, 3 ), 0xff, CodeColour( cornerCode, 4 ) },
					{ CodeColour( cornerCode, 5 ), CodeColour( cornerCode, 6 ), CodeColour( cornerCode, 7 ) }
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
			byte[ , ] smoothMask = MakeSmoothMask( nearestMask );

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
	}
}
