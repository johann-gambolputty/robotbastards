using System;
using RbEngine.Rendering;

namespace RbOpenGlRendering.RbCg
{
	/// <summary>
	/// Implementation of ShaderParameter using CGparameter handle
	/// </summary>
	public class CgShaderParameter : ShaderParameter
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="parameterHandle"> CGparameter handle </param>
		public CgShaderParameter( IntPtr parameterHandle )
		{
			m_Parameter = parameterHandle;
		}

		/// <summary>
		/// Sets the shader parameter to a single float value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public override void	Set( float val )
		{
			Tao.Cg.Cg.cgSetParameter1f( m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to a single integer value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public override void	Set( int val )
		{
			Tao.Cg.Cg.cgSetParameter1i( m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public override void	Set( float x, float y )
		{
			Tao.Cg.Cg.cgSetParameter2f( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public override void	Set( int x, int y )
		{
			Tao.Cg.Cg.cgSetParameter2i( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public override void	Set( float x, float y, float z )
		{
			Tao.Cg.Cg.cgSetParameter3f( m_Parameter, x, y, z );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public override void	Set( int x, int y, int z )
		{
			Tao.Cg.Cg.cgSetParameter3i( m_Parameter, x, y, z );
		}

		/// <summary>
		/// Sets the shader parameter to a float array
		/// </summary>
		/// <param name="val"> New parameter value </param>
		/// <remarks>
		/// Unimplemented - Tao does not appear to provide correct CG function signature (cgSetParameterValuefc())
		/// </remarks>
		public override void	Set( float[] val )
		{
			//	TODO: CG function is incorrect
		//	Tao.Cg.Cg.cgSetParameterValuefc( m_Parameter, val.Length, out val );
		}

		/// <summary>
		/// Sets the shader parameter to an integer array
		/// </summary>
		/// <param name="val"> New parameter value </param>
		/// <remarks>
		/// Unimplemented - Tao does not appear to provide correct CG function signature (cgSetParameterValueic())
		/// </remarks>
		public override void	Set( int[] val )
		{
			//	TODO: CG function is incorrect (requires float[] parameter)
			//	Tao.Cg.Cg.cgSetParameterValueic( m_Parameter, val.Length, out val );
		}

		/// <summary>
		/// Gets the CG shader parameter handle
		/// </summary>
		public IntPtr Parameter
		{
			get
			{
				return m_Parameter;
			}
		}

		private IntPtr	m_Parameter;
	}
}
