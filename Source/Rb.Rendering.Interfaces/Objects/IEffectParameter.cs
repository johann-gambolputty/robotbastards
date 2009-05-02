using Rb.Core.Maths;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Interface for effect parameters
	/// </summary>
	/// <remarks>
	/// </remarks>
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
		/// Gets the name of this parameter
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets a named child parameter
		/// </summary>
		/// <param name="name">Child parameter name</param>
		/// <returns>Returns the named effect parameter</returns>
		/// <exception cref="System.ArgumentException">Thrown if the named parameter does not exist</exception>
		IEffectParameter this[ string name ]
		{
			get;
		}

		/// <summary>
		/// Gets the data source used to set the value of this parameter
		/// </summary>
		IEffectDataSource DataSource
		{
			get; set;
		}

		/// <summary>
		/// Gets the semantic of this parameter
		/// </summary>
		string Semantic
		{
			get;
		}

		/// <summary>
		/// Sets the value of this parameter to a texture
		/// </summary>
		void Set( ITexture texture );

		/// <summary>
		/// Sets the value of this parameter to a boolean value
		/// </summary>
		void Set( bool val );

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
		/// Sets the value of this parameter to a point
		/// </summary>
		void Set( Point3 val );

		/// <summary>
		/// Sets the value of this parameter to a vector
		/// </summary>
		void Set( Vector3 val );

		/// <summary>
		/// Sets the value of this parameter to a matrix
		/// </summary>
		void Set( Matrix44 val );
	}
}
