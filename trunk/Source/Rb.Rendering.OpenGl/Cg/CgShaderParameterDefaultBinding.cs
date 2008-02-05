using System;
using Rb.Core.Maths;
using Rb.Rendering.Lights;
using TaoCgGl = Tao.Cg.CgGl;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// CG shader parameter default binding
	/// </summary>
	public class CgShaderParameterDefaultBinding : ShaderParameterBinding
	{
		/// <summary>
		/// Sets up this binding
		/// </summary>
		public CgShaderParameterDefaultBinding( ShaderParameterDefaultBinding binding ) :
			base( binding.ToString( ) )
		{
			m_Binding = binding;
		}

		/// <summary>
		/// Adds a parameter to this binding
		/// </summary>
		public override void Bind( ShaderParameter parameter )
		{
		}

		/// <summary>
		/// Removes a parameter from this binding
		/// </summary>
		public override void Unbind( ShaderParameter parameter )
		{
		}


		/// <summary>
		/// Applies this binding to a particular shader parameter
		/// </summary>
		public override void ApplyTo( ShaderParameter parameter )
		{
			CgShaderParameter cgParam = ( ( CgShaderParameter )parameter );
			IntPtr param = cgParam.Parameter;
			IntPtr context = cgParam.Context;
			switch ( m_Binding )
			{
				case ShaderParameterDefaultBinding.ModelMatrix :
				{
					Matrix44 modelMatrix = Graphics.Renderer.GetTransform( Transform.LocalToWorld );
					CgShaderParameter.cgSetMatrixParameterfc( param, modelMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.ViewMatrix	:
				{
					Matrix44 viewMatrix = Graphics.Renderer.GetTransform( Transform.WorldToView );
					CgShaderParameter.cgSetMatrixParameterfc( param, viewMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.InverseTransposeModelMatrix :
				{
					Matrix44 itModelMatrix	= Graphics.Renderer.GetTransform( Transform.LocalToWorld );
					itModelMatrix.Translation = Point3.Origin;
					itModelMatrix.Transpose( );
					itModelMatrix.Invert( );

					CgShaderParameter.cgSetMatrixParameterfc( param, itModelMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.ProjectionMatrix :
				{
					TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_PROJECTION_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.TextureMatrix :
				{
					TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_TEXTURE_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.ModelViewMatrix :
				{
					TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.InverseModelViewMatrix :
				{
					TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_MATRIX, TaoCgGl.CG_GL_MATRIX_INVERSE );
					break;
				}

				case ShaderParameterDefaultBinding.InverseTransposeModelViewMatrix	:
				{
					TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_MATRIX, TaoCgGl.CG_GL_MATRIX_INVERSE_TRANSPOSE );
					break;
				}

				case ShaderParameterDefaultBinding.ModelViewProjectionMatrix :
				{
					TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_PROJECTION_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
					break;
				}

				case ShaderParameterDefaultBinding.EyePosition :
				{
					Cameras.Camera3 curCam = ( ( Cameras.Camera3 )Graphics.Renderer.Camera );
					if ( curCam != null )
					{
						Point3 eyePos = curCam.Position;
						TaoCg.cgSetParameter3f( param, eyePos.X, eyePos.Y, eyePos.Z );
					}
					break;
				}

				case ShaderParameterDefaultBinding.EyeZAxis :
				{
					Cameras.Camera3 curCam = ( ( Cameras.Camera3 )Graphics.Renderer.Camera );
					if ( curCam != null )
					{
						Vector3 eyeVec = curCam.ZAxis;
						TaoCg.cgSetParameter3f( param, eyeVec.X, eyeVec.Y, eyeVec.Z );
					}
					break;
				}

				case ShaderParameterDefaultBinding.PointLights :
				{
					//	TODO: This is REALLY SHIT. Need to refactor this mess
					int numActiveLights = Graphics.Renderer.NumActiveLights;
					int numPointLights	= 0;
					for ( int lightIndex = 0; lightIndex < numActiveLights; ++lightIndex )
					{
						IPointLight curLight = Graphics.Renderer.GetLight( lightIndex ) as IPointLight;
						if ( curLight != null )
						{
							IntPtr positionParam = TaoCg.cgGetArrayParameter( TaoCg.cgGetNamedStructParameter( param, "m_Positions" ), numPointLights );
							TaoCg.cgSetParameter4f( positionParam, curLight.Position.X, curLight.Position.Y, curLight.Position.Z, 0 );
							++numPointLights;
						}
					}

					TaoCg.cgSetParameter1i( TaoCg.cgGetNamedStructParameter( param, "m_NumLights" ), numPointLights );

					break;
				}

				case ShaderParameterDefaultBinding.SpotLights :
				{
					//	TODO: This is REALLY SHIT. Need to refactor this mess
					int numActiveLights = Graphics.Renderer.NumActiveLights;
					int numSpotLights	= 0;
					for ( int lightIndex = 0; lightIndex < numActiveLights; ++lightIndex )
					{
						ISpotLight curLight = Graphics.Renderer.GetLight( lightIndex ) as ISpotLight;
						if ( curLight != null )
						{
							IntPtr positionParam = TaoCg.cgGetArrayParameter( TaoCg.cgGetNamedStructParameter( param, "m_Positions" ), numSpotLights );
							IntPtr directionParam = TaoCg.cgGetArrayParameter( TaoCg.cgGetNamedStructParameter( param, "m_Directions" ), numSpotLights );
							IntPtr arcParam = TaoCg.cgGetArrayParameter( TaoCg.cgGetNamedStructParameter( param, "m_Arcs" ), numSpotLights );
							TaoCg.cgSetParameter3f( positionParam, curLight.Position.X, curLight.Position.Y, curLight.Position.Z );
							TaoCg.cgSetParameter3f( directionParam, curLight.Direction.X, curLight.Direction.Y, curLight.Direction.Z );
							TaoCg.cgSetParameter1f( arcParam, Functions.Cos( curLight.ArcDegrees * Constants.DegreesToRadians * 0.5f ) );
							++numSpotLights;
						}
					}

					TaoCg.cgSetParameter1i( TaoCg.cgGetNamedStructParameter( param, "m_NumLights" ), numSpotLights );

					break;
				}

				case ShaderParameterDefaultBinding.Texture0 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 0 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture1 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 1 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture2 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 2 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture3 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 3 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture4 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 4 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture5 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 5 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture6 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 6 ) );
					break;
				}
				case ShaderParameterDefaultBinding.Texture7 :
				{
					CgShaderParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 7 ) );
					break;
				}
				default :
				{
					throw new ApplicationException( string.Format( "Unhandled shader parameter binding \"{0}\"", m_Binding ) );
				}
			}
		}

		private readonly ShaderParameterDefaultBinding m_Binding;
	}
}
