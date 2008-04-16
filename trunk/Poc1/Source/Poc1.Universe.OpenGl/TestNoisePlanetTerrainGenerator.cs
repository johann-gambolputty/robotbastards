using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;

namespace Poc1.Universe.OpenGl
{
	internal class TestNoisePlanetTerrainGenerator : IPlanetTerrainGenerator
	{
		#region IPlanetTerrainGenerator Members

		private struct TerrainRange
		{
			public readonly Color m_Colour;
			public readonly int m_Height;
			public readonly bool m_Cutoff;

			public TerrainRange( Color colour, int height )
			{
				m_Colour = colour;
				m_Height = height;
				m_Cutoff = false;
			}

			public TerrainRange( Color colour, int height, bool cutoff )
			{
				m_Colour = colour;
				m_Height = height;
				m_Cutoff = cutoff;
			}
		}

		public static float TerrainHeight( float x, float y, float z )
		{
			float fVal = Fractals.SimpleFractal( x, y, z, 0.9f, 8, 1.0f ) * 0.8f;
			fVal += ( Fractals.RidgedFractal( x, y, z, 2.1f, 8, 2.0f ) - 0.5f ) * 0.2f;
			fVal = Utils.Clamp( fVal, 0, 1 );

			return fVal;
		}

		//https://www.internal.pandromeda.com/engineering/musgrave/unsecure/S01_Course_Notes.html

		static TestNoisePlanetTerrainGenerator( )
		{
			List<TerrainRange> ranges = new List<TerrainRange>( );

			ranges.Add( new TerrainRange( Color.DarkBlue, 32 ) );
			ranges.Add( new TerrainRange( Color.Blue, 48 ) );
			ranges.Add( new TerrainRange( Color.LightBlue, 4, true ) );
			ranges.Add( new TerrainRange( Color.BlanchedAlmond, 2 ) );
			ranges.Add( new TerrainRange( Color.Brown, 4 ) );
			ranges.Add( new TerrainRange( Color.Khaki, 10 ) );
			ranges.Add( new TerrainRange( Color.ForestGreen, 20 ) );
			ranges.Add( new TerrainRange( Color.DarkGreen, 20 ) );
			ranges.Add( new TerrainRange( Color.White, 42 ) );

			int index = 0;
			for ( int range = 0; range < ranges.Count; ++range )
			{
				float r = ranges[ range ].m_Colour.R;
				float g = ranges[ range ].m_Colour.G;
				float b = ranges[ range ].m_Colour.B;

				bool noLastRange = ( range == 0 || ranges[ range - 1 ].m_Cutoff );
				float lastR = noLastRange ? r : ranges[ range - 1 ].m_Colour.R;
				float lastG = noLastRange ? g : ranges[ range - 1 ].m_Colour.G;
				float lastB = noLastRange ? b : ranges[ range - 1 ].m_Colour.B;

				int boundary = ranges[ range ].m_Height;
				for ( int count = 0; count < ranges[ range ].m_Height; ++count, ++index )
				{
					Color col;

					if ((range == 0) || (count >= boundary))
					{
						col = ranges[range].m_Colour;
					}
					else
					{
						float t = count / ( float )boundary;
						float newR = lastR + ( r - lastR ) * t;
						float newG = lastG + ( g - lastG ) * t;
						float newB = lastB + ( b - lastB ) * t;

						col = Color.FromArgb( ( byte )newR, ( byte )newG, ( byte )newB );
					}

					TerrainColourMap[ 0, index ] = col;
					TerrainColourMap[ 1, index ] = col;
					TerrainColourMap[ 2, index ] = col;
					TerrainColourMap[ 3, index ] = col;
				}
			}

			for (; index < 256; ++index )
			{
				TerrainColourMap[ 0, index ] = ranges[ ranges.Count - 1 ].m_Colour;
				TerrainColourMap[ 1, index ] = ranges[ ranges.Count - 1 ].m_Colour;
				TerrainColourMap[ 2, index ] = ranges[ ranges.Count - 1 ].m_Colour;
				TerrainColourMap[ 3, index ] = ranges[ ranges.Count - 1 ].m_Colour;
			}
		}

		private const int LatitudeCount = 4;

		private readonly static Color[ , ] TerrainColourMap = new Color[ LatitudeCount, 256 ];

		public PixelFormat CubeMapFormat
		{
			get { return PixelFormat.Format24bppRgb; }
		}

		public unsafe void GenerateSide( PlanetMapFace face, byte* pixels, int width, int height, int stride )
		{
			float incU = 2.0f / ( width - 1 );
			float incV = 2.0f / ( height - 1 );
			float v = -1;

			float res = 2.0f;
			for ( int row = 0; row < height; ++row, v += incV )
			{
				float u = -1;
				byte* curPixel = pixels + row * stride;
				for ( int col = 0; col < width; ++col, u += incU )
				{
					float x, y, z;
					PlanetMap.UvToXyz( u, v, face, out x, out y, out z );

					float invLength = res / Functions.Sqrt( u * u + v * v + 1 );
					x *= invLength;
					y *= invLength;
					z *= invLength;

					int latitude = ( int )( ( LatitudeCount / 2 ) * ( y / res ) ) + 2;

					float fVal = TerrainHeight( x, y, z );
					int val = ( int )( fVal * 255.0f );
					Color colour = TerrainColourMap[ latitude, val ];
					curPixel[ 0 ] = colour.R;
					curPixel[ 1 ] = colour.G;
					curPixel[ 2 ] = colour.B;

					curPixel += 3;
				}
			}

			//using ( Bitmap bmp = new Bitmap( width, height, stride, PixelFormat.Format24bppRgb, ( IntPtr )pixels ) )
			//{
			//    using ( Graphics g = Graphics.FromImage( bmp ) )
			//    {
			//        g.DrawString( face.ToString( ), new Font( "Arial", 8 ), Brushes.White, 0, 0 );
			//    }
			//}
		}

		#endregion

		#region Private Members

		//	Temporary nabbed this noise code: http://freespace.virgin.net/hugo.elias/models/m_perlin.htm
		//	Perlin noise FAQ http://www.cs.cmu.edu/~mzucker/code/perlin-noise-math-faq.html
		
		private static float CubicInterpolation( float v0, float v1, float v2, float v3, float x )
		{
			float p = (v3 - v2) - (v0 - v1);
			float q = (v0 - v1) - p;
			float r = v2 - v0;
			float s = v1;
			
			float x2 = x * x;
			float x3 = x2 * x;
			
			return p * x3 + q * x2 + r * x + s;
		}

		private static float Noise2d( int x, int y )
		{
			uint n = ( uint )( x + y * 57 );
			n = ( n << 13 ) ^ n;
			return ( 1.0f - ( (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f );    
		}

		private static float Noise3d( int x, int y, int z )
		{
			uint n = ( uint )( x + y * 57 + z * 13 );
			n = ( n << 13 ) ^ n;
			return ( 1.0f - ( (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f );    
		}

		private static float SmoothNoise( int iX, int iY )
		{
			float corners = ( Noise2d( iX - 1, iY - 1 ) + Noise2d( iX + 1, iY - 1 ) + Noise2d( iX - 1, iY + 1 ) + Noise2d( iX + 1, iY + 1 ) ) / 16.0f;
			float sides = ( Noise2d( iX -1, iY ) + Noise2d( iX + 1, iY ) + Noise2d( iX, iY - 1) + Noise2d( iX, iY + 1 ) ) / 8.0f;
			float centre = Noise2d( iX, iY ) / 4;

			return corners + sides + centre;
		}

		private static float SmoothNoise( int iX, int iY, int iZ )
		{
			float centre = Noise3d( iX, iY, iZ );
			return centre;
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

		private static float InterpolatedNoise( float x, float y, float z )
		{
			int iX = ( int )x;
			float fX = x - iX;
			int iY = ( int )y;
			float fY = y - iY;
			int iZ = ( int )z;
			float fZ = z - iZ;

			float v1 = SmoothNoise( iX, iY, iZ );
			float v2 = SmoothNoise( iX + 1, iY, iZ );
			float v3 = SmoothNoise( iX, iY + 1, iZ );
			float v4 = SmoothNoise( iX + 1, iY + 1, iZ );
			
			float v5 = SmoothNoise( iX, iY, iZ + 1 );
			float v6 = SmoothNoise( iX + 1, iY, iZ + 1 );
			float v7 = SmoothNoise( iX, iY + 1, iZ + 1 );
			float v8 = SmoothNoise( iX + 1, iY + 1, iZ + 1 );

			float x1 = v1 + ( v2 - v1 ) * fX;
			float x2 = v3 + ( v4 - v3 ) * fX;
			float x3 = v5 + ( v6 - v5 ) * fX;
			float x4 = v7 + ( v8 - v7 ) * fX;
			
			float y1 = x1 + ( x2 - x1 ) * fY;
			float y2 = x3 + ( x4 - x3 ) * fY;
			
			return y1 + ( y2 - y1 ) * fZ;
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
		

		private static float TilingFractalNoise( float x, float y, float t )
		{
			float f = FractalNoise( x, y );
			float invF = FractalNoise( x - t, y );

			return ( ( t - x ) * f + x * invF ) / t;
		}
		
		private static float FractalNoise( float x, float y, float z )
		{
			float p = 0.5f;
			float a = 1;
			float total = 0;
			float lim = 0;
			for ( int octave = 0; octave < 4; ++octave )
			{
				total += InterpolatedNoise( x, y, z ) * a;
				lim += a;
				a *= p;
				x *= 1.3f;
				y *= 1.3f;
				z *= 1.3f;
			}

			return ( total + lim ) / ( lim * 2 );
		}

		#endregion


	}
}
