
using Rb.Core.Maths;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Interface for effect parameters
	/// </summary>
	public interface IEffectParameter
	{
		/// <summary>
		/// Gets the effect that owns this parameter
		/// </summary>
		IEffect Owner
		{
			get;
		}

		/// <summary>
		/// Sets the value of this parameter to a single integer
		/// </summary>
		void Set( int val );

		/// <summary>
		/// Sets the value of this parameter to a single float
		/// </summary>
		void Set( float val );

		/// <summary>
		/// Sets the value of this parameter to a 2d float vector
		/// </summary>
		void Set( float x, float y );

		/// <summary>
		/// Sets the value of this parameter to a 3d float vector
		/// </summary>
		void Set( float x, float y, float z );

		/// <summary>
		/// Sets the value of this parameter to a 4d float vector
		/// </summary>
		void Set( float x, float y, float z, float w );

		/// <summary>
		/// Sets the value of this parameter to an integer array
		/// </summary>
		void Set( int[] val );

		/// <summary>
		/// Sets the value of this parameter to an float array
		/// </summary>
		void Set( float[] val );

		/// <summary>
		/// Sets the value of this parameter to a matrix
		/// </summary>
		void Set( Matrix44 val );
	
	}
}
