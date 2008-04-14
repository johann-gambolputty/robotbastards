
using System;

namespace Poc1.Universe.OpenGl
{
	public static class Fractals
	{

		public static float SimpleFractal( float x, float y, float z, float freq, int numOctaves, float persistence )
		{
			float amp = 1;
			float total = 0;
			float max = 0;
			for ( int octave = 0; octave < numOctaves; ++octave )
			{
				total += SimpleNoise.Noise( x, y, z ) * amp * 2;
				max += amp;
				amp *= persistence;
				x *= freq;
				y *= freq;
				z *= freq;
			}

			return ( total + max ) / ( max * 2 );
		}

		public static float RidgedFractal( float x, float y, float z, float freq, int numOctaves, float persistence )
		{
			float amp = 1;
			float total = 0;
			float max = 0;
			for ( int octave = 0; octave < numOctaves; ++octave )
			{
				total += Math.Abs( SimpleNoise.Noise( x, y, z ) * 2 ) * amp;
				max += amp;
				amp *= persistence;
				x *= freq;
				y *= freq;
				z *= freq;
			}

			return total / max;
		}
	}
}
