using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Summary description for Utilities.
	/// </summary>
	public class Utils
	{
		/// <summary>
		/// Swaps two values
		/// </summary>
		public static void Swap( ref float val0, ref float val1 )
		{
			float tmp = val0;
			val0 = val1;
			val1 = tmp;
		}

		/// <summary>
		/// Closeness check
		/// </summary>
		/// <param name="val"> Value </param>
		/// <param name="target"> Target </param>
		/// <returns> Returns true if value is within 0.0001f of target </returns>
		public static bool CloseTo( float val, float target )
		{
			return CloseTo( val, target, 0.0001f );
		}

		/// <summary>
		/// Closeness check
		/// </summary>
		/// <param name="val"> Value </param>
		/// <param name="target"> Target </param>
		/// <param name="epsilon"> Epsilon </param>
		/// <returns> Returns true if value is within epsilon of target </returns>
		public static bool CloseTo( float val, float target, float epsilon )
		{
			return ( ( val - epsilon ) <= target ) && ( ( val + epsilon ) >= target );
		}

		/// <summary>
		/// Close to zero check
		/// </summary>
		/// <param name="val"> Value </param>
		/// <returns> Returns true if val is within 0.0001f of zero </returns>
		public static bool CloseToZero( float val )
		{
			return CloseTo( val, 0 );
		}

		/// <summary>
		/// Close to zero check
		/// </summary>
		/// <param name="val">Value</param>
		/// <param name="epsilon">If val is within this tolerance of zero, this function returns true</param>
		/// <returns> Returns true if val is within epsilon of zero </returns>
		public static bool CloseToZero( float val, float epsilon )
		{
			return CloseTo( val, 0, epsilon );
		}

		/// <summary>
		/// Linear interpolation between two values
		/// </summary>
		public static float Lerp( float min, float max, float t )
		{
			return min + ( max - min ) * t;
		}

		/// <summary>
		/// Clamps an integer to the range [min,max]
		/// </summary>
		public static int Clamp( int value, int min, int max )
		{
			return value < min ? min : ( value > max ? max : value );
		}

		/// <summary>
		/// Clamps a floating point value to the range [min,max]
		/// </summary>
		public static float Clamp( float value, float min, float max )
		{
			return value < min ? min : ( value > max ? max : value );
		}

		/// <summary>
		/// Returns the minimum of two floating point values
		/// </summary>
		public static float Min( float val1, float val2 )
		{
			return val1 < val2 ? val1 : val2;
		}

		/// <summary>
		/// Returns the minimum of two integer values
		/// </summary>
		public static int Min( int val1, int val2 )
		{
			return val1 < val2 ? val1 : val2;
		}

		/// <summary>
		/// Returns the maximum of two floating point values
		/// </summary>
		public static float Max( float val1, float val2 )
		{
			return val1 > val2 ? val1 : val2;
		}

		/// <summary>
		/// Returns the maximum of two integer values
		/// </summary>
		public static int Max( int val1, int val2 )
		{
			return val1 > val2 ? val1 : val2;
		}

		/// <summary>
		/// Wraps a floating point value into the range [min,max)
		/// </summary>
		/// <example>
		/// float degrees = 361.0f;
		/// Wrap( degrees, 0, 360 );  // Returns 1.0f
		/// </example>
		public static float Wrap( float val, float min, float max )
		{
			float range = max - min;
			while ( val < min )
			{
				val += range;
			}
			while ( val >= max )
			{
				val -= range;
			}

			return val;
		}

		/// <summary>
		/// Wraps an integer value into the range [min,max)
		/// </summary>
		/// <example>
		/// int degrees = 361;
		/// Wrap( degrees, 0, 360 );  // Returns 1.0f
		/// </example>
		public static int Wrap( int val, int min, int max )
		{
			int range = max - min;
			while ( val < min )
			{
				val += range;
			}
			while ( val > max )
			{
				val -= range;
			}

			return val;
		}
	}
}
