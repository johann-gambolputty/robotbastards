
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Rectangle=System.Drawing.Rectangle;

namespace Poc1.Tools.StarField
{
	public static class StarFieldBuilder
	{
		/// <summary>
		/// Generates 6 faces of a cube map, containing a star field
		/// </summary>
		public static Bitmap[] Build( StarFieldBuildParameters parameters )
		{
			if ( parameters == null )
			{
				throw new ArgumentNullException( "parameters" );
			}

			Bitmap[] faceBitmaps = new Bitmap[ Enum.GetNames( typeof( CubeMapFace ) ).Length ];
			foreach ( CubeMapFace face in Enum.GetValues( typeof( CubeMapFace ) ) )
			{
				faceBitmaps[ ( int )face ] = CreateFaceBitmap( face, parameters );
			}

			return faceBitmaps;
		}

		/// <summary>
		/// Creates a bitmap for a specified face
		/// </summary>
		private unsafe static Bitmap CreateFaceBitmap( CubeMapFace face, StarFieldBuildParameters parameters )
		{
			Bitmap bmp = new Bitmap( parameters.Resolution, parameters.Resolution, PixelFormat.Format24bppRgb );

			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );

			//	TODO: AP: Accelerate - add SIMD implementation to Poc1.Fast (or equivalent)
			Noise noise = new Noise( );

			byte* rowPixel = ( byte* )bmpData.Scan0;
			float v = -1;
			float uInc = 2 / ( float )( bmpData.Width - 1 );
			float vInc = 2 / ( float )( bmpData.Height - 1 );

			float minNoise = 0.01f;
			for ( int y = 0; y < bmpData.Height; ++y, v += vInc )
			{
				float u = -1;
				byte* curPixel = rowPixel;
				for ( int x = 0; x < bmpData.Width; ++x, u += uInc )
				{
					float fX, fY, fZ;
					CubeMapFaceUv.ToXyz( face, u, v, out fX, out fY, out fZ );
					float invLen = parameters.FunctionRadius / Functions.Sqrt( ( fX * fX ) + ( fY * fY ) + ( fZ * fZ ) );
					fX *= invLen;
					fY *= invLen;
					fZ *= invLen;

					float val = noise.GetNoise( fX, fY, fZ );
					if ( ( val > 0 ) && ( val < minNoise ) )
					{
						curPixel[ 0 ] = ( byte )( ( 1.0f - ( val / minNoise ) ) * 256.0f );
						curPixel[ 1 ] = ( byte )( ( 1.0f - ( val / minNoise ) ) * 256.0f );
						curPixel[ 2 ] = ( byte )( ( 1.0f - ( val / minNoise ) ) * 256.0f );
					}

					curPixel += 3;
				}
				rowPixel += bmpData.Stride;
			}

			bmp.UnlockBits( bmpData );

			return bmp;
			
		}
	}
}
