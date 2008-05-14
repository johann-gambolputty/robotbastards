using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// Implementation of ShaderParameter using CGparameter handle
	/// </summary>
	public class CgEffectParameter : IEffectParameter
	{
		#region Construction

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="owner">Effect owner</param>
		/// <param name="context">Effect context handle</param>
		/// <param name="parameter">Effect parameter handle</param>
		public CgEffectParameter( IEffect owner, IntPtr context, IntPtr parameter )
		{
			m_Owner = owner;
			m_Context = context;
			m_Parameter = parameter;
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Returns the name of this parameter
		/// </summary>
		public override string ToString( )
		{
			return Name;
		}

		/// <summary>
		/// Gets the CG shader context handle
		/// </summary>
		public IntPtr Context
		{
			get { return m_Context; }
		}

		/// <summary>
		/// Gets the CG shader parameter handle
		/// </summary>
		public IntPtr Parameter
		{
			get { return m_Parameter; }
		}

		/// <summary>
		/// Binds a texture to a sampler parameter (shared with CgRenderEffect)
		/// </summary>
		public static void BindTexture( IntPtr context, IntPtr parameter, ITexture tex )
		{
			if ( tex != null )
			{
				int texHandle = ( ( IOpenGlTexture )tex ).TextureHandle;
				Tao.Cg.CgGl.cgGLSetManageTextureParameters( context, true );
				Tao.Cg.CgGl.cgGLSetupSampler( parameter, texHandle );

			}
		}

		#endregion

		#region IEffectParameter Members

		/// <summary>
		/// Gets the semantic of this parameter
		/// </summary>
		public string Semantic
		{
			get
			{
				return TaoCg.cgGetParameterSemantic( m_Parameter );
			}
		}

		/// <summary>
		/// Gets the name of this parameter
		/// </summary>
		public string Name
		{
			get { return TaoCg.cgGetParameterName( m_Parameter ); }
		}

		/// <summary>
		/// Gets the effect that owns this parameter
		/// </summary>
		public IEffect Owner
		{
			get { return m_Owner; }
		}

		/// <summary>
		/// Gets a named child parameter
		/// </summary>
		/// <param name="name">Child parameter name</param>
		/// <returns>Returns the named effect parameter</returns>
		/// <exception cref="System.ArgumentException">Thrown if the named parameter does not exist</exception>
		public IEffectParameter this[ string name ]
		{
			get { return m_Parameters[ name ]; }
			set { m_Parameters[ name ] = value; }
		}

		/// <summary>
		/// Gets the data source used to set the value of this parameter
		/// </summary>
		public IEffectDataSource DataSource
		{
			get { return m_DataSource; }
			set { m_DataSource = value; }
		}

		/// <summary>
		/// Sets the value of this parameter to a texture
		/// </summary>
		public void Set( ITexture texture )
		{
			BindTexture( m_Context, m_Parameter, texture );
		}

		/// <summary>
		/// Sets the value of this parameter to a single integer
		/// </summary>
		public void Set( int val )
		{
			TaoCg.cgSetParameter1i( m_Parameter, val );
		}

		/// <summary>
		/// Sets the value of this parameter to a single float
		/// </summary>
		public void Set( float val )
		{
			TaoCg.cgSetParameter1f( m_Parameter, val );
		}

		/// <summary>
		/// Sets the value of this parameter to 2 floats
		/// </summary>
		public void Set( float x, float y )
		{
			TaoCg.cgSetParameter2f( m_Parameter, x, y );
		}

		/// <summary>
		/// Sets the value of this parameter to 3 floats
		/// </summary>
		public void Set( float x, float y, float z )
		{
			TaoCg.cgSetParameter3f( m_Parameter, x, y, z );
		}

		/// <summary>
		/// Sets the value of this parameter to 4 floats
		/// </summary>
		public void Set( float x, float y, float z, float w )
		{
			TaoCg.cgSetParameter4f( m_Parameter, x, y, z, w );
		}

		/// <summary>
		/// Sets the value of this parameter to an array of integers
		/// </summary>
		public void Set( int[] val )
		{
			cgSetParameterValueic( m_Parameter, val );
		}

		/// <summary>
		/// Sets the value of this parameter to an array of floats
		/// </summary>
		public void Set( float[] val )
		{
			cgSetParameterValuefc( m_Parameter, val );
		}

		/// <summary>
		/// Sets the value of this parameter to a matrix
		/// </summary>
		public void Set( Matrix44 val )
		{
			cgSetMatrixParameterfr( m_Parameter, val.Elements );
		}

		#endregion

		#region Private Members

		private readonly Dictionary<string, IEffectParameter> m_Parameters = new Dictionary<string, IEffectParameter>( );
		private readonly IntPtr m_Parameter;
		private readonly IntPtr m_Context;
		private readonly IEffect m_Owner;
		private IEffectDataSource m_DataSource;

		#endregion

		#region P/Invoke

		//	NOTE: These functions were incorrectly imported into Tao (declared matrix/array parameters as "out float")

		[DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
		public static extern void cgSetParameterValueic( IntPtr param, int[] array );

		[DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
		public static extern void cgSetParameterValuefc( IntPtr param, float[] array );

		[DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
		public static extern void cgSetMatrixParameterfc( IntPtr param, float[] matrix );

		[DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
		public static extern void cgSetMatrixParameterfr( IntPtr param, float[] matrix );

		#endregion
	}
}
