using System;
using System.Runtime.InteropServices;
using System.Security;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Implementation of ShaderParameter using CGparameter handle
	/// </summary>
	public class CgShaderParameter : ShaderParameter
	{
		//	NOTE: These functions were incorrectly imported into Tao (declared matrix/array parameters as "out float")

		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetParameterValueic( IntPtr param, int[] array );

		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetParameterValuefc( IntPtr param, float[] array );

		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetMatrixParameterfc( IntPtr param, float[] matrix );

		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetMatrixParameterfr( IntPtr param, float[] matrix );

		/// <summary>
		/// Gets the name of this parameter
		/// </summary>
		public string					Name
		{
			get
			{
				return TaoCg.cgGetParameterName( m_Parameter );
			}
		}

			/// <summary>
			/// Gets the CG context that this parameter is from
			/// </summary>
			public IntPtr					Context
		{
			get
			{
				return m_Context;
			}
		}

		/// <summary>
		/// Gets the parameter binding associated with this parameter
		/// </summary>
		public ShaderParameterBinding	Binding
		{
			get
			{
				return m_Binding;
			}
			set
			{
				m_Binding = value;
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="context">CG context</param>
		/// <param name="source">Parent effect</param>
		/// <param name="parameterHandle"> CGparameter handle </param>
		public CgShaderParameter( IntPtr context, IEffect source, IntPtr parameterHandle ) :
			base( source )
		{
			m_Context	= context;
			m_Parameter = parameterHandle;
		}


		/// <summary>
		/// Binds a texture to a sampler parameter (shared with CgRenderEffect)
		/// </summary>
		public static void BindTexture( IntPtr context, IntPtr parameter, ITexture2d tex )
		{
			if ( tex != null )
			{
				int texHandle = ( ( OpenGlTexture2d )tex ).TextureHandle;
				Tao.Cg.CgGl.cgGLSetTextureParameter( parameter, texHandle );
				TaoCg.cgSetSamplerState( parameter );
				Tao.Cg.CgGl.cgGLSetManageTextureParameters( context, true );
			}
		}

		/// <summary>
		/// Sets the shader parameter to a texture
		/// </summary>
		/// <param name="val">New parameter value</param>
		/// <remarks>
		/// Assumes that this parameter is a texture sampler of some description. Texture must already be bound to the
		/// renderer
		/// </remarks>
		public override void Set( ITexture2d val )
		{
			BindTexture( m_Context, m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to a single float value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public override void Set( float val )
		{
			TaoCg.cgSetParameter1f( m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to a single integer value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public override void	Set( int val )
		{
			TaoCg.cgSetParameter1i( m_Parameter, val );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public override void	Set( float x, float y )
		{
			TaoCg.cgSetParameter2f( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public override void	Set( int x, int y )
		{
			TaoCg.cgSetParameter2i( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public override void	Set( float x, float y, float z )
		{
			TaoCg.cgSetParameter3f( m_Parameter, x, y, z );
		}

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public override void	Set( int x, int y, int z )
		{
			TaoCg.cgSetParameter3i( m_Parameter, x, y, z );
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
			//	TaoCg.cgSetParameterValuefc( m_Parameter, val.Length, out val );
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
			//	TaoCg.cgSetParameterValueic( m_Parameter, val.Length, out val );
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

		private ShaderParameterBinding	m_Binding;
		private IntPtr					m_Parameter;
		private IntPtr					m_Context;
	}
}
