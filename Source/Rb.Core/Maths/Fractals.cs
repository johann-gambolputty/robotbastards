
using System;

namespace Rb.Core.Maths
{
	public static class Fractals
	{
		#region 2D Fractals

		/// <summary>
		/// Basis function for 2d fractals
		/// </summary>
		public delegate float Basis2dFunction( float x, float y );

		/// <summary>
		/// Handy reference to the default noise instance 3d noise function
		/// </summary>
		public static Basis2dFunction Noise2dBasis = Noise.Instance.GetNoise;

		/// <summary>
		/// Simple fractal
		/// </summary>
		/// <param name="x">Initial x coordinate</param>
		/// <param name="y">Initial y coordinate</param>
		/// <param name="freq">Frequency</param>
		/// <param name="numOctaves">Number of octaves</param>
		/// <param name="persistence">Persistence</param>
		/// <param name="basis">Basis function. Generates values between 0 and 1</param>
		/// <returns>Returns the generated fractal value between 0 and 1</returns>
		public static float SimpleFractal( float x, float y, float freq, int numOctaves, float persistence, Basis2dFunction basis )
		{
			float amp = 1;
			float total = 0;
			float max = 0;
			for ( int octave = 0; octave < numOctaves; ++octave )
			{
				total += basis( x, y ) * amp;
				max += amp;
				amp *= persistence;
				x *= freq;
				y *= freq;
			}

			return ( total + max ) / ( max * 2 );
		}

		/// <summary>
		/// Ridged multi-fractal
		/// </summary>
		/// <param name="x">Initial x coordinate</param>
		/// <param name="y">Initial y coordinate</param>
		/// <param name="freq">Frequency</param>
		/// <param name="numOctaves">Number of octaves</param>
		/// <param name="persistence">Persistence</param>
		/// <param name="basis">Basis function. Generates values between 0 and 1</param>
		/// <returns>Returns the generated fractal value between 0 and 1</returns>
		public static float RidgedFractal( float x, float y, float freq, int numOctaves, float persistence, Basis2dFunction basis )
		{
			float offset = 1.0f;
			float signal = Math.Abs( basis( x, y ) );
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

				float weight = Utils.Clamp( signal * gain, 0, 1 );
				signal = offset - Math.Abs( basis( x, y ) );
				signal *= signal;
				signal *= weight;
				max += 1.0f / exp;
				result += signal / exp;
				exp *= freq;
			}

			return ( result / max );
		}


		#endregion

		#region 3D Fractals

		/// <summary>
		/// Basis function for 3d fractals
		/// </summary>
		public delegate float Basis3dFunction( float x, float y, float z );

		/// <summary>
		/// Handy reference to the default noise instance 3d noise function
		/// </summary>
		public static Basis3dFunction Noise3dBasis = Noise.Instance.GetNoise;

		/// <summary>
		/// Simple fractal
		/// </summary>
		/// <param name="x">Initial x coordinate</param>
		/// <param name="y">Initial y coordinate</param>
		/// <param name="z">Initial z coordinate</param>
		/// <param name="freq">Frequency</param>
		/// <param name="numOctaves">Number of octaves</param>
		/// <param name="persistence">Persistence</param>
		/// <param name="basis">Basis function. Generates values between 0 and 1</param>
		/// <returns>Returns the generated fractal value between 0 and 1</returns>
		public static float SimpleFractal( float x, float y, float z, float freq, int numOctaves, float persistence, Basis3dFunction basis )
		{
			float amp = 1;
			float total = 0;
			float max = 0;
			for ( int octave = 0; octave < numOctaves; ++octave )
			{
				total += basis( x, y, z ) * amp;
				max += amp;
				amp *= persistence;
				x *= freq;
				y *= freq;
				z *= freq;
			}

			return ( total + max ) / ( max * 2 );
		}

		/// <summary>
		/// Ridged multi-fractal
		/// </summary>
		/// <param name="x">Initial x coordinate</param>
		/// <param name="y">Initial y coordinate</param>
		/// <param name="z">Initial z coordinate</param>
		/// <param name="freq">Frequency</param>
		/// <param name="numOctaves">Number of octaves</param>
		/// <param name="persistence">Persistence</param>
		/// <param name="basis">Basis function. Generates values between 0 and 1</param>
		/// <returns>Returns the generated fractal value between 0 and 1</returns>
		public static float RidgedFractal( float x, float y, float z, float freq, int numOctaves, float persistence, Basis3dFunction basis )
		{
			float offset = 1.0f;
			float signal = Math.Abs( basis( x, y, z ) );
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
				signal = offset - Math.Abs( basis( x, y, z ) );
				signal *= signal;
				signal *= weight;
				max += 1.0f / exp;
				result += signal / exp;
				exp *= freq;
			}

			return ( result / max );
		}


		#endregion
	}
}
