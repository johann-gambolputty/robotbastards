using System;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Fast;
using Rectangle=System.Drawing.Rectangle;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Noise bitmap creation
	/// </summary>
	public static class NoiseBitmapBuilder
	{
		/// <summary>
		/// Builds a noise texture
		/// </summary>
		/// <param name="parameters">Noise texture build parameters</param>
		public static unsafe Bitmap Build( NoiseBitmapBuilderParameters parameters )
		{
			if ( parameters == null )
			{
				throw new ArgumentNullException( "parameters" );
			}
			Bitmap bmp = new Bitmap( parameters.BitmapWidth, parameters.BitmapHeight, PixelFormat.Format24bppRgb );

			FastNoise noise = new FastNoise( );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );

			byte* pixels = ( byte* )bmpData.Scan0;
			noise.GenerateTiledBitmap( bmp.Width, bmp.Height, 3, pixels, parameters.NoiseX, parameters.NoiseY, parameters.NoiseWidth, parameters.NoiseHeight );
			noise.GenerateTiledBitmap( bmp.Width, bmp.Height, 3, pixels + 1, parameters.NoiseX + parameters.NoiseWidth, parameters.NoiseY, parameters.NoiseWidth, parameters.NoiseHeight );
			noise.GenerateTiledBitmap( bmp.Width, bmp.Height, 3, pixels + 2, parameters.NoiseX + parameters.NoiseWidth * 2, parameters.NoiseY, parameters.NoiseWidth, parameters.NoiseHeight );

			bmp.UnlockBits( bmpData );
			return bmp;
		}
	}
}
