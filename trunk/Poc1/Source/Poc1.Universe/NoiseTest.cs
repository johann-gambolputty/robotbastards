using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Fast;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rectangle=System.Drawing.Rectangle;

namespace Poc1.Universe
{
	/// <summary>
	/// Tests out various noise and fractal generation code, for speed and output
	/// </summary>
	/// <remarks>
	/// 
	/// Really rough test timings
	/// Note that in debug builds, timings for unmanaged code are 2x to 3x longer.
	/// 
	/// Results so far (Release Build, plain noise filling a 512x512 r8g8b8 bitmap):
	///		Plain vanilla C# Noise: 0.26 seconds
	///		C++ noise with C# outer loop: 0.18 seconds
	///		C++ noise with C++ outer loop: 0.02 seconds
	/// 
	/// Results so far (Release Build, ridged fractal filling a 512x512 r8g8b8 bitmap):
	///		Plain vanilla C# Noise: 0.90 seconds
	///		C++ noise with C++ outer loop: 0.17 seconds
	/// 
	/// </remarks>
	public class NoiseTest
	{
		private const float IncX = 2 / 512.0f;
		private static readonly Point3 RowStart = new Point3( 1, -1, 1 );
		private static readonly Vector3 IncCol = new Vector3(0, 0, -IncX);
		private static readonly Vector3 IncRow = new Vector3(0, 2 / 512.0f, 0);
		private const int Res = 512;

		public static unsafe void TestSlowNoise( )
		{
			Noise n = new Noise( );
			Bitmap bmp = new Bitmap( Res, Res, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* curRow = ( byte* )bmpData.Scan0;

			Point3 rowPos = RowStart;
			long start = TinyTime.CurrentTime;
			for ( int y = 0; y < bmp.Height; ++y, curRow += bmpData.Stride )
			{
				byte* curPixel = curRow;

				Point3 pt0 = rowPos;
				for ( int x = 0; x < bmp.Width; ++x )
				{
					
					curPixel[ 0 ] = ( byte )( 128 + ( byte )( n.GetNoise( pt0.X, pt0.Y, pt0.Z ) * 127.0f ) );
				//	curPixel[ 0 ] = ( byte )( Fractals.RidgedFractal( pt0.X, pt0.Y, pt0.Z, 1.2f, 6, 0.6f, Fractals.Noise3dBasis ) * 255.0f );
					curPixel[ 1 ] = curPixel[ 0 ];
					curPixel[ 2 ] = curPixel[ 0 ];

					curPixel += 3;
					pt0.X += IncX;
				}
				rowPos += IncRow;
			}
			GraphicsLog.Info( "Time taken to generate slow noise: {0:F2} seconds", TinyTime.ToSeconds( start, TinyTime.CurrentTime ) );
			bmp.UnlockBits( bmpData );

			bmp.Save( "SlowNoiseTest.bmp", ImageFormat.Bmp );
		}

		public static unsafe void TestFastNoiseSP( )
		{
			FastNoise n = new FastNoise( );
			Bitmap bmp = new Bitmap( Res, Res, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* curRow = ( byte* )bmpData.Scan0;
			Point3 rowPos = RowStart;

			long start = TinyTime.CurrentTime;
			for ( int y = 0; y < bmp.Height; ++y, curRow += bmpData.Stride )
			{
				byte* curPixel = curRow;

				Point3 pt0 = rowPos;
				for ( int x = 0; x < bmp.Width; ++x )
				{
					FastNoiseResult res = n.Noise( pt0, pt0, pt0, pt0 );
					curPixel[ 0 ] = ( byte )( 128 + ( byte )( res.X * 127.0f ) );
					curPixel[ 1 ] = curPixel[ 0 ];
					curPixel[ 2 ] = curPixel[ 0 ];
					curPixel += 3;
					pt0.X += IncX;
				}
				rowPos += IncRow;
			}
			GraphicsLog.Info( "Time taken to generate fast noise SP: {0:F2} seconds", TinyTime.ToSeconds( start, TinyTime.CurrentTime ) );
			bmp.UnlockBits( bmpData );

			bmp.Save( "FastNoiseTestSP.bmp", ImageFormat.Bmp );
		}

		public static unsafe void TestFastSphereCloudsGenerator( )
		{
			Bitmap bmp = new Bitmap(Res, Res, PixelFormat.Format24bppRgb);
			BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

			SphereCloudsBitmap gen = new SphereCloudsBitmap( );
			long start = TinyTime.CurrentTime;
			gen.GenerateFace( CubeMapFace.PositiveX, bmp.PixelFormat, bmp.Width, bmp.Height, bmpData.Stride, ( byte* )bmpData.Scan0);

			GraphicsLog.Info("Time taken to generate fast very noise: {0:F2} seconds", TinyTime.ToSeconds(start, TinyTime.CurrentTime));

			bmp.UnlockBits(bmpData);
			bmp.Save("TestFastSphereCloudsGenerator.bmp", ImageFormat.Bmp);
		}

		
		public static unsafe void TestVeryFastNoise( )
		{
			FastNoise n = new FastNoise( );
			Bitmap bmp = new Bitmap( Res, Res, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );

			long start = TinyTime.CurrentTime;
			n.GenerateRgbBitmap( bmp.Width, bmp.Height, ( byte* )bmpData.Scan0, RowStart, IncCol, IncRow );

			GraphicsLog.Info( "Time taken to generate fast very noise: {0:F2} seconds", TinyTime.ToSeconds( start, TinyTime.CurrentTime ) );
			bmp.UnlockBits( bmpData );

			bmp.Save( "FastVeryNoiseTest.bmp", ImageFormat.Bmp );
			
		}

		public static unsafe void TestFastNoise( )
		{
			FastNoise n = new FastNoise( );
			Bitmap bmp = new Bitmap( Res, Res, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* curRow = ( byte* )bmpData.Scan0;
			Point3 rowPos = RowStart;
			Vector3 next4Col = IncCol * 4;
			long start = TinyTime.CurrentTime;
			for ( int y = 0; y < bmp.Height; ++y, curRow += bmpData.Stride )
			{
				byte* curPixel = curRow;

				Point3 pt0 = rowPos;
				Point3 pt1 = rowPos + new Vector3( IncX, 0, 0 );
				Point3 pt2 = rowPos + new Vector3( IncX * 2, 0, 0 );
				Point3 pt3 = rowPos + new Vector3( IncX * 3, 0, 0 );
				for ( int x = 0; x < bmp.Width / 4; ++x )
				{
					FastNoiseResult res = n.Noise( pt0, pt1, pt2, pt3 );
					curPixel[ 0 ] = ( byte )( 128 + ( byte )( res.X * 127.0f ) );
					curPixel[ 1 ] = curPixel[ 0 ];
					curPixel[ 2 ] = curPixel[ 0 ];
					curPixel[ 3 ] = ( byte )( 128 + ( byte )( res.Y * 127.0f ) );
					curPixel[ 4 ] = curPixel[ 3 ];
					curPixel[ 5 ] = curPixel[ 3 ];
					curPixel[ 6 ] = ( byte )( 128 + ( byte )( res.Z * 127.0f ) );
					curPixel[ 7 ] = curPixel[ 6 ];
					curPixel[ 8 ] = curPixel[ 6 ];
					curPixel[ 9 ] = ( byte )( 128 + ( byte )( res.W * 127.0f ) );
					curPixel[ 10 ] = curPixel[ 9 ];
					curPixel[ 11 ] = curPixel[ 9 ];

					curPixel += 12;
					pt0 += next4Col;
					pt1 += next4Col;
					pt2 += next4Col;
					pt3 += next4Col;
				}
				rowPos += IncRow;
			}
			GraphicsLog.Info( "Time taken to generate fast noise: {0:F2} seconds", TinyTime.ToSeconds( start, TinyTime.CurrentTime ) );
			bmp.UnlockBits( bmpData );

			bmp.Save( "FastNoiseTest.bmp", ImageFormat.Bmp );
		}

	}
}
