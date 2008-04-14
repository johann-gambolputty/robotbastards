
using System;
using Rb.Core.Maths;

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
			float offset = 1.0f;
			float signal = Math.Abs( SimpleNoise.Noise( x, y, z ) );
			signal = offset - signal;	//	invert and translate (note that "offset" should be ~= 1.0)
			signal *= signal;			//	square the signal, to increase "sharpness" of ridges

			float result = signal;
			float gain = 1.8f;
			float exp = 1;
			float max = 1;

			for ( int octave = 1; octave < numOctaves; ++octave )
			{
				x *= freq;
				y *= freq;
				z *= freq;

				float weight = Utils.Clamp( signal * gain, 0, 1 );
				signal = offset - Math.Abs( SimpleNoise.Noise( x, y, z ) );
				signal *= signal;
				signal *= weight;
				max += 1.0f / exp;
				result += signal / exp;
				exp *= freq;
			}

			return( result / max );
		}
	}
}
