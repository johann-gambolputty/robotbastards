using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Numerical integration, using a gaussian quadrature
	/// </summary>
	public class GaussianQuadrature : Integral
	{
		/// <summary>
		/// Calculates the integral of 1d function "function" over the interval [min..max]
		/// </summary>
		public static float Integrate( float min, float max, Integral.FunctionDelegate function )
		{
			float radius = 0.5f * ( max - min );
			float centre = 0.5f * ( max + min );

			float result = 0;
			for ( int degree = 0; degree < kDegrees; ++degree )
			{
				result += Coeefficients[ degree ] * function( radius * Roots[ degree ] + centre );
			}
			result *= radius;

			return result;
		}

		#region	Private stuff

		private const int kDegrees = 5;

		private static readonly float[] Roots = new float[ kDegrees ]
			{
				-0.9061798459f,
				-0.5384693101f,
				0.0f,
				+0.5384693101f,
				+0.9061798459f
			};

		private static readonly float[] Coeefficients = new float[ kDegrees ]
			{
				0.2369268850f,
				0.4786286705f,
				0.5688888889f,
				0.4786286705f,
				0.2369268850f
			};

		#endregion

	}
}
