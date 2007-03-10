using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using RbEngine.Rendering;

namespace RbOpenGlRendering.RbCg
{
	/// <summary>
	/// CG shader parameter default binding
	/// </summary>
	public class CgShaderParameterDefaultBinding : ShaderParameterBinding
	{
		/// <summary>
		/// Sets up this binding
		/// </summary>
		/// <param name="cgContext"></param>
		/// <param name="binding"></param>
		public CgShaderParameterDefaultBinding( ValueType type, ShaderParameterDefaultBinding binding )
		{
			m_Type		= type;
			m_Binding	= binding;
		}

		/// <summary>
		/// Adds a parameter to this binding
		/// </summary>
		public override void	Bind( ShaderParameter parameter )
		{
		}

		/// <summary>
		/// Removes a parameter from this binding
		/// </summary>
		public override void	Unbind( ShaderParameter parameter )
		{
		}

		//	NOTE: These functions were incorrectly imported into Tao (declared matrix parameter as "out float")
		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetMatrixParameterfc( IntPtr param, float[] matrix );

		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetMatrixParameterfr( IntPtr param, float[] matrix );

		/// <summary>
		/// Applies this binding to a particular shader parameter
		/// </summary>
		public override void	ApplyTo( ShaderParameter parameter )
		{
			CgShaderParameter cgParam = ( ( CgShaderParameter )parameter );
			IntPtr param = cgParam.Parameter;
			IntPtr context = cgParam.Context;
			switch ( m_Binding )
			{
				case ShaderParameterDefaultBinding.ModelMatrix :
				{
					Matrix44 modelMatrix = Renderer.Inst.GetTransform( Transform.LocalToWorld );
					cgSetMatrixParameterfc( param, modelMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.ViewMatrix	:
				{
					Matrix44 viewMatrix = Renderer.Inst.GetTransform( Transform.WorldToView );
					cgSetMatrixParameterfc( param, viewMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.InverseTransposeModelMatrix :
				{
					Matrix44 itModelMatrix	= Renderer.Inst.GetTransform( Transform.LocalToWorld );
					itModelMatrix.Translation = Point3.Origin;
					itModelMatrix.Transpose( );
					itModelMatrix.Invert( );

					cgSetMatrixParameterfc( param, itModelMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.ProjectionMatrix :
				{
					CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_PROJECTION_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.TextureMatrix :
				{
					CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_TEXTURE_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.ModelViewMatrix :
				{
					CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.InverseModelViewMatrix :
				{
					CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_MATRIX, CgGl.CG_GL_MATRIX_INVERSE );
					break;
				}

				case ShaderParameterDefaultBinding.InverseTransposeModelViewMatrix	:
				{
					CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_MATRIX, CgGl.CG_GL_MATRIX_INVERSE_TRANSPOSE );
					break;
				}

				case ShaderParameterDefaultBinding.ModelViewProjectionMatrix :
				{
					CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_PROJECTION_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.EyePosition :
				{
					Point3 eyePos = ( ( RbEngine.Cameras.Camera3 )Renderer.Inst.Camera ).Position;
					Cg.cgSetParameter3f( param, eyePos.X, eyePos.Y, eyePos.Z );
					break;
				}

				case ShaderParameterDefaultBinding.EyeZAxis :
				{
					Vector3 eyeVec = ( ( RbEngine.Cameras.Camera3 )Renderer.Inst.Camera ).ZAxis;
					Cg.cgSetParameter3f( param, eyeVec.X, eyeVec.Y, eyeVec.Z );
					break;
				}

				case ShaderParameterDefaultBinding.PointLights :
				{
					//	TODO: This is REALLY SHIT. Need to refactor this mess
					int numActiveLights = Renderer.Inst.NumActiveLights;
					int numPointLights	= 0;
					for ( int lightIndex = 0; lightIndex < numActiveLights; ++lightIndex )
					{
						PointLight curLight = Renderer.Inst.GetLight( lightIndex ) as PointLight;
						if ( curLight != null )
						{
							IntPtr positionParam = Cg.cgGetArrayParameter( Cg.cgGetNamedStructParameter( param, "m_Positions" ), numPointLights );
							Cg.cgSetParameter4f( positionParam, curLight.Position.X, curLight.Position.Y, curLight.Position.Z, 0 );
							++numPointLights;
						}
					}

					Cg.cgSetParameter1i( Cg.cgGetNamedStructParameter( param, "m_NumLights" ), numPointLights );

					break;
				}

				case ShaderParameterDefaultBinding.SpotLights :
				{
					//	TODO: This is REALLY SHIT. Need to refactor this mess
					int numActiveLights = Renderer.Inst.NumActiveLights;
					int numSpotLights	= 0;
					for ( int lightIndex = 0; lightIndex < numActiveLights; ++lightIndex )
					{
						SpotLight curLight = Renderer.Inst.GetLight( lightIndex ) as SpotLight;
						if ( curLight != null )
						{
							IntPtr positionParam	= Cg.cgGetArrayParameter( Cg.cgGetNamedStructParameter( param, "m_Positions" ), numSpotLights );
							IntPtr directionParam	= Cg.cgGetArrayParameter( Cg.cgGetNamedStructParameter( param, "m_Directions" ), numSpotLights );
							IntPtr arcParam			= Cg.cgGetArrayParameter( Cg.cgGetNamedStructParameter( param, "m_ArcRadians" ), numSpotLights );
							Cg.cgSetParameter4f( positionParam, curLight.Position.X, curLight.Position.Y, curLight.Position.Z, 0 );
							Cg.cgSetParameter4f( directionParam, curLight.Direction.X, curLight.Direction.Y, curLight.Direction.Z, 0 );
							Cg.cgSetParameter1f( arcParam, curLight.ArcDegrees * Constants.DegreesToRadians );
							++numSpotLights;
						}
					}

					Cg.cgSetParameter1i( Cg.cgGetNamedStructParameter( param, "m_NumLights" ), numSpotLights );

					break;
				}

				case ShaderParameterDefaultBinding.Texture0 :
				{
					CgShaderParameter.BindTexture( context, param, Renderer.Inst.GetTexture( 0 ) );
					break;
				}

				default :
				{
					throw new ApplicationException( string.Format( "Unhandled shader parameter binding \"{0}\"", m_Binding.ToString( ) ) );
				}
			}
		}

		private ValueType						m_Type;
		private ShaderParameterDefaultBinding	m_Binding;
		private Object							m_Value;
	}
}
