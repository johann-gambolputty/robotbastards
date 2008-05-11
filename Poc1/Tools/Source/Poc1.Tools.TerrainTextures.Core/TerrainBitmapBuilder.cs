using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;

namespace Poc1.Tools.TerrainTextures.Core
{
	public class TerrainBitmapBuilder
	{
		public unsafe static Bitmap Build( int width, int height )
		{
			Bitmap bmp = new Bitmap( width, height, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, width, height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			byte* rowFirstPixel = ( byte* )bmpData.Scan0;

			float y = 7.1f;
			float xInc = 0.0341f;
			float yInc = 0.0341f;
			for ( int row = 0; row < height; ++row, y += yInc )
			{
				float x = 7.1f;
				byte* curPixel = rowFirstPixel;
				for ( int col = 0; col < width; ++col, x += xInc )
				{
					float f = Fractals.SimpleFractal( x, y, 7.1f, 1.5f, 8, 0.9f, Fractals.Noise3dBasis );
					byte b = ( byte )( f * 255.0f );
					curPixel[ 0 ] = b;
					curPixel[ 1 ] = b;
					curPixel[ 2 ] = b;
					curPixel += 3;
				}

				rowFirstPixel += bmpData.Stride;
			}

			bmp.UnlockBits( bmpData );
			return bmp;
		}
	}
}
