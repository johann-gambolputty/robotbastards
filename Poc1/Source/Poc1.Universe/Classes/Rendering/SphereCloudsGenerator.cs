using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	public class SphereCloudsGenerator
	{
		public ICubeMapTexture CreateCloudTexture( int res )
		{
			ICubeMapTexture texture = Graphics.Factory.CreateCubeMapTexture( );
			texture.Build
				(
					CreateFaceBitmap( CubeMapFace.PositiveX, res ),
					CreateFaceBitmap( CubeMapFace.NegativeX, res ),
					CreateFaceBitmap( CubeMapFace.PositiveY, res ),
					CreateFaceBitmap( CubeMapFace.NegativeY, res ),
					CreateFaceBitmap( CubeMapFace.PositiveZ, res ),
					CreateFaceBitmap( CubeMapFace.NegativeZ, res ),
					true
				);
			return texture;
		}

		#region Private Members

		private unsafe Bitmap CreateFaceBitmap( CubeMapFace face, int res )
		{
			PixelFormat format = PixelFormat.Format32bppArgb;
			Bitmap bmp = new Bitmap( res, res, format );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, res, res ), ImageLockMode.WriteOnly, format );
			GenerateSide( face, ( byte* )bmpData.Scan0, res, res, bmpData.Stride );
			bmp.UnlockBits( bmpData );

			return bmp;
		}

		private unsafe void GenerateSide( CubeMapFace face, byte* pixels, int width, int height, int stride )
		{
			Fractals.Basis3dFunction basis = m_Noise.GetNoise;

			float incU = 2.0f / width;
			float incV = 2.0f / height;
			float v = -1;
			for ( int row = 0; row < height; ++row, v += incV )
			{
				float u = -1;
				byte* curPixel = pixels + row * stride;
				for ( int col = 0; col < width; ++col, u += incU )
				{
					float x, y, z;
					SphereTerrainGenerator.UvToXyz( u, v, face, out x, out y, out z );

					float val = Fractals.RidgedFractal( x, y, z, 1.8f, 8, 1.6f, basis );
					float alpha = 0;
					if ( val < CloudCutoff )
					{
						val = 0;
					}
					else
					{
						alpha = val < CloudBorder ? ( val - CloudCutoff ) / ( CloudBorder - CloudCutoff ) : 1.0f;
					}

					byte colour = ( byte )( val * 255.0f );
					curPixel[ 0 ] = colour;
					curPixel[ 1 ] = colour;
					curPixel[ 2 ] = colour;
					curPixel[ 3 ] = ( byte )( alpha * 255.0f );

					curPixel += 4;
				}
			}
		}
		const float CloudCutoff = 0.6f;
		const float CloudBorder = 0.7f;

		private readonly Noise m_Noise = new Noise( );


		#endregion
	}
}
