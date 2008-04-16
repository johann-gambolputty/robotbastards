
using System.Drawing.Imaging;

namespace Poc1.Universe.OpenGl
{
	internal class TestCloudGenerator : IPlanetTerrainGenerator
	{
		#region IPlanetTerrainGenerator Members

		public PixelFormat CubeMapFormat
		{
			get { return PixelFormat.Format32bppRgb; }
		}

		public unsafe void GenerateSide( PlanetMapFace face, byte* pixels, int width, int height, int stride )
		{
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
					PlanetMap.UvToXyz( u, v, face, out x, out y, out z );

					float val = Fractals.RidgedFractal( x, y, z, 1.8f, 8, 1.6f );
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

		#endregion
	}
}
