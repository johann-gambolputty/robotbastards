using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores tile bitmaps in a render texture
	/// </summary>
	[Serializable]
	public class TileTexture
	{
		/// <summary>
		/// UV coordinate rectangle
		/// </summary>
		[Serializable]
		public class Rect
		{
			/// <summary>
			/// Deep-copy constructor
			/// </summary>
			/// <param name="rect">Source rectangle</param>
			public Rect( Rect rect )
			{
				m_TopLeft = new Point2( rect.m_TopLeft );
				m_TopRight = new Point2( rect.m_TopRight );
				m_BottomLeft = new Point2( rect.m_BottomLeft );
				m_BottomRight = new Point2( rect.m_BottomRight );
			}
			
			/// <summary>
			/// Setup constructor
			/// </summary>
			public Rect( RectangleF rect )
			{
				m_TopLeft = new Point2( rect.Left, rect.Top );
				m_TopRight = new Point2( rect.Right, rect.Top );
				m_BottomLeft = new Point2( rect.Left, rect.Bottom );
				m_BottomRight = new Point2( rect.Right, rect.Bottom );
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			public Rect( float u, float v, float width, float height )
			{
				m_TopLeft = new Point2( u, v );
				m_TopRight = new Point2( u + width, v );
				m_BottomLeft = new Point2( u, v + height );
				m_BottomRight = new Point2( u + width, v + height );
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			public Rect( Point2 tl, Point2 tr, Point2 bl, Point2 br )
			{
				m_TopLeft = tl;
				m_TopRight = tr;
				m_BottomLeft = bl;
				m_BottomRight = br;
			}

			/// <summary>
			/// Mirrors the texture coordinates around the local Y axis of the texture rectangle
			/// </summary>
			public void MirrorAroundLocalYAxis( )
			{
				float tmp = m_TopLeft.X;
				m_TopLeft.X = m_TopRight.X;
				m_TopRight.X = tmp;

				tmp = m_BottomLeft.X;
				m_BottomLeft.X = m_BottomRight.X;
				m_BottomRight.X = tmp;
			}

			/// <summary>
			/// Rotates the texture by some multiple of 90 degrees around its local origin
			/// </summary>
			/// <param name="rotations">Number of 90 degree rotations</param>
			public void Rotate90( int rotations )
			{
				if ( ( rotations % 4 ) == 0 )
				{
					return;
				}

				for ( int rotation = 0; rotation < rotations; ++rotation )
				{
					float tmpX = m_BottomLeft.X;
					float tmpY = m_BottomLeft.Y;
					m_BottomLeft.Set( m_BottomRight.X, m_BottomRight.Y );
					m_BottomRight.Set( m_TopRight.X, m_TopRight.Y );
					m_TopRight.Set( m_TopLeft.X, m_TopLeft.Y );
					m_TopLeft.Set( tmpX, tmpY );
				}
			}

			public float MinU
			{
				get { return m_TopLeft.X;  }
			}

			public float MinV
			{
				get { return m_TopLeft.Y; }
			}

			public float MaxU
			{
				get { return m_BottomRight.X; }
			}

			public float MaxV
			{
				get { return m_BottomRight.Y; }
			}

			/// <summary>
			/// Gets the top left texture coordinate
			/// </summary>
			public Point2 TopLeft
			{
				get { return m_TopLeft; }
			}

			/// <summary>
			/// Gets the top right texture coordinate
			/// </summary>
			public Point2 TopRight
			{
				get { return m_TopRight; }
			}

			/// <summary>
			/// Gets the bottom left texture coordinate
			/// </summary>
			public Point2 BottomLeft
			{
				get { return m_BottomLeft; }
			}

			/// <summary>
			/// Gets the bottom right texture coordinate
			/// </summary>
			public Point2 BottomRight
			{
				get { return m_BottomRight; }
			}

			private Point2 m_TopLeft;
			private Point2 m_TopRight;
			private Point2 m_BottomLeft;
			private Point2 m_BottomRight;
		}

		
		/// <summary>
		/// Adds a new bitmap to the texture
		/// </summary>
		/// <param name="bmp">Bitmap to add</param>
		/// <returns>Texture coordinates of the bitmap on <see cref="Texture"/></returns>
		public RectangleF Add( Bitmap bmp )
		{
			//	Check input bitmap is alright
			if ( bmp.Width >= m_Combiner.Width )
			{
				throw new ArgumentException( "Bitmap was too wide" );
			}
			if ( bmp.PixelFormat != m_Combiner.PixelFormat )
			{
				bmp = CloneBitmapR8G8B8A8( bmp );
				//throw new ArgumentException( string.Format( "Input bitmap must have pixel format of {0} (was {1})", m_Combiner.PixelFormat, bmp.PixelFormat ) );
			}

			//	This will force the texture to be re-created, when it's accessed via the Texture property
			m_Texture = null;

			//	Move to the next row if the bitmap won't fit on the current row
			if ( ( m_AddX + bmp.Width ) > m_Combiner.Width )
			{
				m_AddX = 0;
				m_AddY += m_HighestOnRow;
				if ( m_AddY >= m_Combiner.Height )
				{
					throw new ArgumentException( "No more space in combiner bitmap" );
				}
			}

			int x = m_AddX;
			int y = m_AddY;
			m_AddX += bmp.Width + 1;

			if ( bmp.Height > m_HighestOnRow )
			{
				m_HighestOnRow = bmp.Height;
			}

			//	Render the bitmap into the combiner bitmap
			System.Drawing.Graphics bmpGraphics = System.Drawing.Graphics.FromImage( m_Combiner );
			bmpGraphics.DrawImage( bmp, x, y );
			bmpGraphics.Dispose( );

			//	Return the texture coordinates of the bitmap in the combiner bitmap
			return new RectangleF( x * InvCombinedWidth, y * InvCombinedHeight, bmp.Width * InvCombinedWidth, bmp.Height * InvCombinedHeight );
		}

		/// <summary>
		/// Gets the texture, re-generating it if <see cref="Add"/> has been called
		/// </summary>
		public Texture2d Texture
		{
			get
			{
				if ( m_Texture == null )
				{
					m_Texture = Rb.Rendering.Graphics.Factory.NewTexture2d( );
					m_Texture.Load( m_Combiner );
				}
				return m_Texture;
			}
		}

		/// <summary>
		/// Generates a transition bitmap for the given tile type
		/// </summary>
		/// <param name="type">Tile type</param>
		/// <param name="code">Transition code</param>
		/// <param name="hardEdgeSize">Size of the hard edge in the automatically generated transition textures</param>
		/// <param name="softEdgeSize">Size of the soft edge in the automatically generated transition textures</param>
		/// <returns>Texture coordinates of the generated image on <see cref="Texture"/></returns>
		public RectangleF Generate( TileType type, byte code, int hardEdgeSize, int softEdgeSize )
		{
			return Add( GenerateTransitionBitmap( type.Image, code, hardEdgeSize, softEdgeSize ) );
		}

		#region Transition bitmap generation

		public static byte IndexToCornerCode( int index )
		{
			return ( byte )index;
		}

		public static byte IndexToEdgeCode( int index )
		{
			return ( byte )( index << 4 );
		}

		public static int CornerCodeToIndex( byte code )
		{
			return ( code & TransitionCodes.Corners );
		}

		public static int EdgeCodeToIndex( byte code )
		{
			return ( code & TransitionCodes.Edges ) >> 4;
		}

		private static int		BandMin;
		private static int		BandSize;
		private static int		BandMax;
		private static float	InvBandSize;
		private static float	PosToAlpha;

		/// <summary>
		/// Creates a transition bitmap
		/// </summary>
		/// <param name="bmp">Bitmap to generate transition for</param>
		/// <param name="code">Transition code</param>
		/// <param name="hardEdgeSize">Size of the hard edge in the automatically generated transition textures</param>
		/// <param name="softEdgeSize">Size of the soft edge in the automatically generated transition textures</param>
		/// <returns>Returns a new bitmap, combining an alpha channel with bmp</returns>
		private static Bitmap GenerateTransitionBitmap( Bitmap bmp, byte code, int hardEdgeSize, int softEdgeSize )
		{
			BandMin = hardEdgeSize;
			BandSize = softEdgeSize;
			BandMax = BandMin + BandSize;
			InvBandSize = 1.0f / BandSize;
			PosToAlpha = InvBandSize * 255.0f;

			if ( code == TransitionCodes.All )
			{
				return GenerateNoTransitionBitmap( bmp );
			}

			if ( ( code & TransitionCodes.Corners ) != 0 )
			{
				return GenerateCornerTransitionBitmap( bmp, code );
			}

			return GenerateEdgeTransitionBitmap( bmp, code );
		}

		private unsafe static Bitmap CloneBitmapR8G8B8A8( Bitmap bmp )
		{
			Bitmap src = bmp;
			if ( src.PixelFormat == PixelFormat.Format32bppArgb )
			{
				return src;
			}

			System.Drawing.Rectangle lockRect = new System.Drawing.Rectangle( 0, 0, src.Width, src.Height );

			src = src.Clone( lockRect, PixelFormat.Format32bppArgb );
			if ( src.PixelFormat == PixelFormat.Format32bppArgb )
			{
				return src;
			}
			if ( src.PixelFormat != PixelFormat.Format24bppRgb )
			{
				throw new InvalidOperationException( "Expected pixel format of cloned bitmap to be at least 24bpp RGB" );
			}

			Bitmap clone = new Bitmap( src.Width, src.Height, PixelFormat.Format32bppArgb );

			BitmapData srcData = src.LockBits( lockRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );
			byte* srcPixels = ( byte* )srcData.Scan0;

			BitmapData dstData = clone.LockBits(lockRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
			byte* dstPixels = (byte*)dstData.Scan0;

			for ( int y = 0; y < src.Height; ++y )
			{
				byte* srcPixel = srcPixels + ( y * srcData.Stride );
				byte* dstPixel = dstPixels + ( y * dstData.Stride );

				for ( int x = 0; x < src.Width; ++x )
				{
					dstPixel[ 0 ] = srcPixel[ 0 ];
					dstPixel[ 1 ] = srcPixel[ 1 ];
					dstPixel[ 2 ] = srcPixel[ 2 ];
					dstPixel[ 3 ] = 0xff;

					dstPixel += 4;
					srcPixel += 3;
				}
			}

			src.UnlockBits( srcData );
			clone.UnlockBits( dstData );

			return clone;
		}

		/// <summary>
		/// Generates a bitmap without transitions (basically just ensures that a clone of bmp has the right pixel format, with a solid alpha channel)
		/// </summary>
		/// <param name="bmp">Input bitmap</param>
		/// <returns>Transition bitmap</returns>
		private static Bitmap GenerateNoTransitionBitmap( Bitmap bmp )
		{
			return CloneBitmapR8G8B8A8( bmp );
		}

		private static float DistanceToAlpha( float distance )
		{
			if ( distance <= BandMin )
			{
				return 255.0f;
			}
			if ( distance >= BandMax )
			{
				return 0.0f;
			}
			return Utils.Clamp( ( BandMax - distance ) * PosToAlpha, 0.0f, 255.0f );
		}

        private static int GetCornerCode( int x, int y, int width, int height )
        {
            if ( x < BandMax )
            {
                return y < BandMax ? 1 : ( y + BandMax > height ) ? 4 : 0;
            }
            else if ( x + BandMax > width )
            {
                return y < BandMax ? 2 : ( y + BandMax > height ) ? 3 : 0;
            }
            return 0;

        }

		const int RIndex = 0;
		const int GIndex = 1;
		const int BIndex = 2;
		const int AIndex = 3;

		private unsafe static Bitmap GenerateCornerTransitionBitmap( Bitmap bmp, byte code )
		{
			System.Drawing.Rectangle lockRect = new System.Drawing.Rectangle( 0, 0, bmp.Width, bmp.Height );

			bmp = CloneBitmapR8G8B8A8( bmp );

			Bitmap transitionBmp = new Bitmap( bmp.Width, bmp.Height, CombinedPixelFormat );

			BitmapData srcData = bmp.LockBits( lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat );
			BitmapData dstData = transitionBmp.LockBits( lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat );

			byte* srcPixels = ( byte* )srcData.Scan0;
			byte* dstPixels = ( byte* )dstData.Scan0;

			//	TODO: AP: Would be much faster to process corners separately, rather than doing calculations in the inner loop

			Point[] corners = new Point[ 4 ]{ new Point( 0, 0 ), new Point( bmp.Width, 0 ), new Point( bmp.Width, bmp.Height ), new Point( 0, bmp.Height ) };

			for ( int y = 0; y < bmp.Height; ++y )
			{
				byte* srcPixel = srcPixels + ( srcData.Stride * y );
				byte* dstPixel = dstPixels + ( dstData.Stride * y );

				for ( int x = 0; x < bmp.Width; ++x )
				{
                    int cornerIndex = GetCornerCode( x, y, bmp.Width, bmp.Height );
                    byte cornerCode = ( byte )( 1 << ( cornerIndex - 1 ) );

					if ( ( cornerIndex == 0 ) || ( ( cornerCode & code ) == 0 ) )
					{
						//	Not in any corner
						dstPixel[ 0 ] = 0x0;
						dstPixel[ 1 ] = 0x0;
						dstPixel[ 2 ] = 0x0;
						dstPixel[ 3 ] = 0x0;
					}
					else
					{
						Point corner = corners[ cornerIndex - 1 ];
						int diffX = corner.X - x;
						int diffY = corner.Y - y;
						int sqrDistToCorner = ( diffX * diffX ) + ( diffY * diffY );

						dstPixel[ RIndex ] = srcPixel[ RIndex ];
						dstPixel[ GIndex ] = srcPixel[ GIndex ];
						dstPixel[ BIndex ] = srcPixel[ BIndex ];
						dstPixel[ AIndex ] = ( byte )( DistanceToAlpha( ( float )Math.Sqrt( sqrDistToCorner ) ) );
					}

					dstPixel += 4;
					srcPixel += 4;
				}
			}
			transitionBmp.UnlockBits( dstData );

			return transitionBmp;
		}

		private unsafe static Bitmap GenerateEdgeTransitionBitmap( Bitmap bmp, byte code )
		{
			System.Drawing.Rectangle lockRect = new System.Drawing.Rectangle( 0, 0, bmp.Width, bmp.Height );
			if ( bmp.PixelFormat != CombinedPixelFormat )
			{
				//	Convert to appropriate pixel format
				bmp = CloneBitmapR8G8B8A8( bmp );
			}

			Bitmap transitionBmp = new Bitmap( bmp.Width, bmp.Height, CombinedPixelFormat );

			BitmapData srcData = bmp.LockBits( lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat );
			BitmapData dstData = transitionBmp.LockBits( lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat );

			byte* srcPixels = ( byte* )srcData.Scan0;
			byte* dstPixels = ( byte* )dstData.Scan0;

			for ( int y = 0; y < bmp.Height; ++y )
			{
				byte* srcPixel = srcPixels + ( srcData.Stride * y );
				byte* dstPixel = dstPixels + ( dstData.Stride * y );

				for ( int x = 0; x < bmp.Width; ++x )
				{
					float alpha = 0.0f;

					if ( x < BandMax )
					{
						if ( ( code & TransitionCodes.LeftEdge ) != 0 )
						{
							alpha = DistanceToAlpha( x );
						}
					}
					else if ( x + BandMax > bmp.Width )
					{
						if ( ( code & TransitionCodes.RightEdge ) != 0 )
						{
							alpha = DistanceToAlpha( bmp.Width - x );
						}
					}

					if ( y < BandMax )
					{
						if ( ( code & TransitionCodes.TopEdge ) != 0 )
						{
							alpha = Utils.Max( DistanceToAlpha( y ), alpha );
						}
					}
					else if ( y + BandMax > bmp.Height )
					{
						if ( ( code & TransitionCodes.BottomEdge ) != 0 )
						{
							alpha = Utils.Max( DistanceToAlpha( bmp.Height - y ), alpha );
						}
					}

					if ( alpha > 0 )
					{
						dstPixel[ RIndex ] = srcPixel[ RIndex ];
						dstPixel[ GIndex ] = srcPixel[ GIndex ];
						dstPixel[ BIndex ] = srcPixel[ BIndex ];
						dstPixel[ AIndex ] = ( byte )alpha;
					}
					else
					{
						//	Not close to any edge
						dstPixel[ 0 ] = 0x0;
						dstPixel[ 1 ] = 0x0;
						dstPixel[ 2 ] = 0x0;
						dstPixel[ 3 ] = 0x0;
					}

					dstPixel += 4;
					srcPixel += 4;
				}
			}
			transitionBmp.UnlockBits( dstData );

			return transitionBmp;
		}

		#endregion

		#region Private stuff

		private const int 			CombinedWidth = 1024;
		private const int 			CombinedHeight = 1024;

		private const float 		InvCombinedWidth = 1.0f / CombinedWidth;
		private const float 		InvCombinedHeight = 1.0f / CombinedHeight;

		private const PixelFormat	CombinedPixelFormat = PixelFormat.Format32bppArgb;

		private int 				m_AddX;
		private int 				m_AddY;
		private int 				m_HighestOnRow;
		private readonly Bitmap 	m_Combiner = new Bitmap( CombinedWidth, CombinedHeight, CombinedPixelFormat );

		[NonSerialized]
		private Texture2d			m_Texture;

		#endregion
	}
}
