using System;

namespace RbEngine.Rendering
{

	/// <summary>
	/// Summary description for ShaderParameter.
	/// </summary>
	public abstract class ShaderParameter
	{
		/// <summary>
		/// Gets the shader that this parameter came from
		/// </summary>
		public Shader	Source
		{
			get
			{
				return m_Source;
			}
		}

		/// <summary>
		/// Sets the source of this parameter
		/// </summary>
		public ShaderParameter( Shader source )
		{
			m_Source = source;
		}

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

		private Shader			m_Source;
	}
}
