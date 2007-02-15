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
		public const float kE					= ( float )System.Math.E;

		/// <summary>
		/// Pi
		/// </summary>
		public const float kPi					= ( float )System.Math.PI;

		/// <summary>
		/// Pi divided by 2
		/// </summary>
		public const float kHalfPi				= kPi / 2;

		/// <summary>
		/// Pi times 2
		/// </summary>
		public const float kTwoPi				= kPi * 2;

		/// <summary>
		/// Conversion factor for turning a value in degrees into radians
		/// </summary>
		public const float kDegreesToRadians	= kTwoPi / 360.0f;

		/// <summary>
		/// Conversion factor for turning a value in radians into degrees
		/// </summary>
		public const float kRadiansToDegrees	= 360.0f / kTwoPi;
	}
}
