
using System;
using Rb.Core.Maths;
using System.Drawing.Imaging;

namespace Poc1.Universe.OpenGl
{
	internal class TestStPlanetTerrainGenerator : IPlanetTerrainGenerator
	{
		#region IPlanetTerrainGenerator Members

		public PixelFormat CubeMapFormat
		{
			get { return PixelFormat.Format24bppRgb; }
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
					float s, t;
					PlanetMap.UvToSt( u, v, face, out s, out t );

					curPixel[ 0 ] = ( byte )( Math.Abs( s ) * ( 255.0f / Constants.Pi ) );
					curPixel[ 1 ] = ( byte )( t * ( 255.0f / Constants.Pi ) );
					curPixel[ 2 ] = 0;

					curPixel += 3;
				}
			}
		}

		#endregion
	}
}
