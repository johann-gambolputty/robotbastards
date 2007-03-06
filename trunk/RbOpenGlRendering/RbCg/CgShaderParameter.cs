using System;
using RbEngine.Rendering;
using Tao.Cg;

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
		/// <param name="context">CG context</param>
		/// <param name="parameterHandle"> CGparameter handle </param>
		public CgShaderParameter( IntPtr context, IntPtr parameterHandle )
		{
			m_Context	= context;
			m_Parameter = parameterHandle;
		}

		/// <summary>
		/// Sets the shader parameter to a texture
		/// </summary>
		/// <param name="val">New parameter value</param>
		/// <remarks>
		/// Assumes that this parameter is a texture sampler of some description
		/// </remarks>
		public override void	Set( Texture2d val )
		{
			val.Begin( );
			CgGl.cgGLSetTextureParameter( m_Parameter, ( ( OpenGlTexture2d )val ).TextureHandle );
			Cg.cgSetSamplerState( m_Parameter );
			Tao.Cg.CgGl.cgGLSetManageTextureParameters( m_Context, true );
			val.End( );
		}

		/// <summary>
		/// Sets the shader parameter to a single float value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public override void	Set( float val )
		{
			Cg.cgSetParameter1f( m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to a single integer value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public override void	Set( int val )
		{
			Cg.cgSetParameter1i( m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public override void	Set( float x, float y )
		{
			Cg.cgSetParameter2f( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public override void	Set( int x, int y )
		{
			Cg.cgSetParameter2i( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public override void	Set( float x, float y, float z )
		{
			Cg.cgSetParameter3f( m_Parameter, x, y, z );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public override void	Set( int x, int y, int z )
		{
			Cg.cgSetParameter3i( m_Parameter, x, y, z );
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
		//	Cg.cgSetParameterValuefc( m_Parameter, val.Length, out val );
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
			//	Cg.cgSetParameterValueic( m_Parameter, val.Length, out val );
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
		private IntPtr	m_Context;
	}
}
