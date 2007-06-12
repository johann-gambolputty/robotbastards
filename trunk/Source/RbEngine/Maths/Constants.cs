using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Helpful maths constants
	/// </summary>
	public class Constants
	{
		/// <summary>
		/// Base of natural logarithm
		/// </summary>
		public const float E				= ( float )System.Math.E;

		/// <summary>
		/// Pi
		/// </summary>
		public const float Pi				= ( float )System.Math.PI;

		/// <summary>
		/// Pi divided by 2
		/// </summary>
		public const float HalfPi			= Pi / 2;

		/// <summary>
		/// Pi times 2
		/// </summary>
		public const float TwoPi			= Pi * 2;

		/// <summary>
		/// Conversion factor for turning a value in degrees into radians
		/// </summary>
		public const float DegreesToRadians	= TwoPi / 360.0f;

		/// <summary>
		/// Conversion factor for turning a value in radians into degrees
		/// </summary>
		public const float RadiansToDegrees	= 360.0f / TwoPi;
	}
}
