using System;
using System.Collections.Generic;
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
			Mask[] masks = new Mask[ 8 ];

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

		/// <summary>
		/// Generates a mask for a given corner code
		/// </summary>
		private static void GenerateMask( byte cornerCode, int width, int height, byte[,] mask )
		{
			int yOffset = 0;
			int blockWidth = width / 3;
			int blockHeight = height / 3;

			Blend( 1, 0, 0xff, 0xff, 0x00, 0xff );

			byte[,] tinyMask = new byte[ 3, 3 ]
				{
					{ CodeColour( cornerCode, 0 ), CodeColour( cornerCode, 1 ), CodeColour( cornerCode, 2 ) },
					{ CodeColour( cornerCode, 3 ), 0xff, CodeColour( cornerCode, 4 ) },
					{ CodeColour( cornerCode, 5 ), CodeColour( cornerCode, 6 ), CodeColour( cornerCode, 7 ) }
				};

			int[] blockWidths = new int[3] { width / 3, ( width * 2 ) / 3, width };
			int[] blockHeights = new int[3] { height / 3, ( height * 2 ) / 3, height };

			float range = 1.0f;

			float fXScale = ( range * 2 ) / ( blockWidth + 1 );
			float fYScale = ( range * 2 ) / ( blockHeight + 1 );

			for ( int y = 0; y < height; ++y )
			{
				if ( y >= blockHeights[ yOffset ] )
				{
					++yOffset;
				}

				float fY = ( blockHeights[ yOffset ] - y ) * fYScale;
				fY -= range;
				
				int vOffset = ( blockHeights[ yOffset ] - y ) < blockHeight / 2 ? 1 : -1;

				int xOffset = 0;
			    for ( int x = 0; x < width; ++x )
			    {
					if ( x >= blockWidths[ xOffset ] )
					{
						++xOffset;
					}

			        //mask[ x, y ] = tinyMask[ xOffset, yOffset ];

					float fX = ( blockWidths[ xOffset ] - x ) * fXScale;
					fX -= range;
					
					int hOffset = ( blockWidths[ xOffset ] - x ) < blockWidth / 2 ? 1 : -1;

					byte origin = tinyMask[ xOffset, yOffset ];
					byte vertical = MapValue( xOffset, yOffset, tinyMask, 0, vOffset );
					byte horizontal = MapValue( xOffset, yOffset, tinyMask, hOffset, 0 );
					byte diagonal = MapValue( xOffset, yOffset, tinyMask, hOffset, vOffset );

					//mask[ x, y ] = Blend( fX < 0 ? -fX : fX, fY < 0 ? -fY : fY, origin, vertical, horizontal, diagonal );
					mask[ x, y ] = Blend2( fX < 0 ? -fX : fX, fY < 0 ? -fY : fY, origin, vertical, horizontal, diagonal );
			    }
			}
		}

		private static byte MapValue( int x, int y, byte[ , ] map, int deltaX, int deltaY )
		{
			int newX = x + deltaX;
			int newY = y + deltaY;

			if ( ( newX < 0 ) || ( newX >= map.GetLength( 0 ) ) || ( newY < 0 ) || ( newY >= map.GetLength( 1 ) ) )
			{
				return map[ x, y ];
			}
			return map[ newX, newY ];
		}

		private static float DistanceFactor( float distance, byte origin, byte value )
		{
			float top = ( origin > 0 ) && ( value > 0 ) ? 1.5f : 1.0f;
			float result = top - distance;
			return result < 0 ? 0 : result > 1 ? 1 : result;
		}

		private static byte Blend2( float x, float y, byte bOrigin, byte bVertical, byte bHorizontal, byte bDiagonal )
		{
			if ( ( bVertical == 0 ) && ( bHorizontal == 0 ) && ( bDiagonal == 0 ) )
			{
				if (bOrigin == 0)
				{
					return 0;
				}

				float invDist = 1.0f - ( float )Math.Sqrt( x * x + y * y );
				return invDist < 0 ? ( byte )0 : ( byte )( invDist * bOrigin );
			}

			float amt = 0;
			float avg = 0;

			if ( bVertical != 0 )
			{
				amt += ( 1 - x );
				++avg;
			}
			if ( bHorizontal != 0 )
			{
				amt += ( 1 - y );
				++avg;
			}
			if ( bDiagonal != 0 )
			{
				amt += 1 - Math.Abs( x - y );
				++avg;
			}

			return ( byte )( ( amt / avg ) * 255 );
		}

		private static byte Blend( float x, float y, byte bOrigin, byte bVertical, byte bHorizontal, byte bDiagonal )
		{
			float invX = 2.0f - x;
			float invY = 2.0f - y;

			float scaleOrigin = ( float )Math.Sqrt( x * x + y * y );
			scaleOrigin = DistanceFactor( scaleOrigin, 0, 0 );
			
			float scaleVertical = ( float )Math.Sqrt( x * x + invY * invY );
			scaleVertical = DistanceFactor( scaleVertical, bOrigin, bVertical );

			float scaleDiagonal = ( float )Math.Sqrt( invX * invX + invY * invY );
			scaleDiagonal = DistanceFactor( scaleDiagonal, bOrigin, bDiagonal );

			float scaleHorizontal = ( float )Math.Sqrt( invX * invX + y * y );
			scaleHorizontal = DistanceFactor( scaleHorizontal, bOrigin, bHorizontal );

			float combined = ( bOrigin * scaleOrigin ) + ( bVertical * scaleVertical ) + ( bHorizontal * scaleHorizontal ) + ( bDiagonal * scaleDiagonal );
			return ( combined > 255 ) ? ( byte )255 : ( byte )combined;
		}
	}
}
