using System;
using Rb.Core.Maths;

namespace Poc1.Universe.OpenGl
{
	internal class TestNoisePlanetTerrainGenerator : IPlanetTerrainGenerator
	{
		#region IPlanetTerrainGenerator Members

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

					byte val = ( byte )( FractalNoise( Math.Abs( s ) * 7, t * 7 ) * 250.0f );

					curPixel[ 0 ] = val;
					curPixel[ 1 ] = val;
					curPixel[ 2 ] = val;

					curPixel += 3;
				}
			}
		}

		#endregion

		#region Private Members

		//	Temporary nabbed this noise code: http://freespace.virgin.net/hugo.elias/models/m_perlin.htm

		private static float Noise2d( int x, int y )
		{
			uint n = ( uint )( x + y * 57 );
			n = ( n << 13 ) ^ n;
			return ( 1.0f - ( (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f );    
		}

		private static float SmoothNoise( float x, float y )
		{
			int iX = ( int )x;
			int iY = ( int )y;

			float corners = ( Noise2d( iX - 1, iY - 1 ) + Noise2d( iX + 1, iY - 1 ) + Noise2d( iX - 1, iY + 1 ) + Noise2d( iX + 1, iY + 1 ) ) / 16.0f;
			float sides = ( Noise2d( iX -1, iY ) + Noise2d( iX + 1, iY ) + Noise2d( iX, iY - 1) + Noise2d( iX, iY + 1 ) ) / 8.0f;
			float centre = Noise2d( iX, iY ) / 4;

			return corners + sides + centre;
		}

		private static float InterpolatedNoise( float x, float y )
		{
			int iX = ( int )x;
			float fX = x - iX;
			int iY = ( int )y;
			float fY = y - iY;


			float v1 = SmoothNoise( iX, iY );
			float v2 = SmoothNoise( iX + 1, iY );
			float v3 = SmoothNoise( iX, iY + 1 );
			float v4 = SmoothNoise( iX + 1, iY + 1 );

			float i1 = v1 + ( v2 - v1 ) * fX;
			float i2 = v3 + ( v4 - v3 ) * fX;

			return i1 + ( i2 - i1 ) * fY;
		}

		private static float FractalNoise( float x, float y )
		{
			float p = 0.9f;
			float total = 0;
			for ( int octave = 0; octave < 4; ++octave )
			{
				total += InterpolatedNoise( x, y ) * p;
				p *= p;
				x *= 2;
				y *= 2;
			}

			return total;
		}

		#endregion
	}
}
