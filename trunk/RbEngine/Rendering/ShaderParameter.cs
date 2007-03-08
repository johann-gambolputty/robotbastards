using System;

namespace RbEngine.Rendering
{

	/// <summary>
	/// Binding options for a shader parameter
	/// </summary>
	public enum ShaderParameterBinding
	{
		/// <summary>
		/// Unbound parameter
		/// </summary>
		NoBinding,

		/// <summary>
		/// Parameter bound to current model view matrix
		/// </summary>
		ModelViewMatrix,

		/// <summary>
		/// Parameter bound to the inverse of the current model view matrix
		/// </summary>
		InverseModelViewMatrix,

		/// <summary>
		/// Parameter bound to the inverse transpose of the current model view matrix
		/// </summary>
		InverseTransposeModelViewMatrix,
		
		/// <summary>
		/// Parameter bound to current model view projection matrix
		/// </summary>
		ModelViewProjectionMatrix,
		
		/// <summary>
		/// Parameter bound to current eye position (world space)
		/// </summary>
		EyePosition,
		
		/// <summary>
		/// Parameter bound to current eye x axis (world space)
		/// </summary>
		EyeXAxis,
		
		/// <summary>
		/// Parameter bound to current eye y axis(world space)
		/// </summary>
		EyeYAxis,
		
		/// <summary>
		/// Parameter bound to current eye z axis (world space)
		/// </summary>
		EyeZAxis,

		//	TODO: This is very specific - requires a certain data structure to be present in the shader... 
		/// <summary>
		/// Data about the active point lights in the renderer
		/// </summary>
		/// <remarks>
		/// Binds to a struct like this:
		/// <code>
		/// struct PointLights
		/// {
		///    int m_NumLights;
		///    float4 m_Positions[ 4 ];
		/// }
		/// </code>
		/// </remarks>
		PointLights,


		//	TODO: This is very specific - requires a certain data structure to be present in the shader... 
		/// <summary>
		/// Data about the active spotlights in the renderer
		/// </summary>
		/// <remarks>
		/// Binds to a struct like this:
		/// <code>
		/// struct SpotLights
		/// {
		///    int m_NumLights;
		///    float4 m_Positions[ 4 ];
		///    float4 m_Directions[ 4 ];
		///    float  m_ArcRadians[ 4 ];
		/// }
		/// </code>
		/// </remarks>
		SpotLights,

		/// <summary>
		/// Total number of bindings
		/// </summary>
		NumBindings
	}

	/// <summary>
	/// Summary description for ShaderParameter.
	/// </summary>
	public abstract class ShaderParameter
	{
		/// <summary>
		/// Sets the shader parameter to a texture
		/// </summary>
		/// <param name="val">New parameter value</param>
		/// <remarks>
		/// Assumes that this parameter is a texture sampler of some description
		/// </remarks>
		public abstract void	Set( Texture2d val );

		/// <summary>
		/// Sets the shader parameter to a single float value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( float val );

		/// <summary>
		/// Sets the shader parameter to a single integer value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( int val );

		/// <summary>
		/// Sets the shader parameter to an (x,y) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public abstract void	Set( float x, float y );

		/// <summary>
		/// Sets the shader parameter to an (x,y) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public abstract void	Set( int x, int y );

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public abstract void	Set( float x, float y, float z );

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public abstract void	Set( int x, int y, int z );

		/// <summary>
		/// Sets the shader parameter to a float array
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( float[] val );

		/// <summary>
		/// Sets the shader parameter to an integer array
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( int[] val );
	}
}
