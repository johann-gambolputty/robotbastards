using System;
using System.Collections;
using RbEngine.Maths;
using RbEngine.Rendering;
using Tao.Cg;

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
		public CgShaderParameterDefaultBinding( ShaderParameterDefaultBinding binding ) :
			base( binding.ToString( ) )
		{
			m_Binding = binding;
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
					CgShaderParameter.cgSetMatrixParameterfc( param, modelMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.ViewMatrix	:
				{
					Matrix44 viewMatrix = Renderer.Inst.GetTransform( Transform.WorldToView );
					CgShaderParameter.cgSetMatrixParameterfc( param, viewMatrix.Elements );
					break;
				}

				case ShaderParameterDefaultBinding.InverseTransposeModelMatrix :
				{
					Matrix44 itModelMatrix	= Renderer.Inst.GetTransform( Transform.LocalToWorld );
					itModelMatrix.Translation = Point3.Origin;
					itModelMatrix.Transpose( );
					itModelMatrix.Invert( );

					CgShaderParameter.cgSetMatrixParameterfc( param, itModelMatrix.Elements );
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
					RbEngine.Cameras.Camera3 curCam = ( ( RbEngine.Cameras.Camera3 )Renderer.Inst.Camera );
					if ( curCam != null )
					{
						Point3 eyePos = curCam.Position;
						Cg.cgSetParameter3f( param, eyePos.X, eyePos.Y, eyePos.Z );
					}
					break;
				}

				case ShaderParameterDefaultBinding.EyeZAxis :
				{
					RbEngine.Cameras.Camera3 curCam = ( ( RbEngine.Cameras.Camera3 )Renderer.Inst.Camera );
					if ( curCam != null )
					{
						Vector3 eyeVec = curCam.ZAxis;
						Cg.cgSetParameter3f( param, eyeVec.X, eyeVec.Y, eyeVec.Z );
					}
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
							IntPtr arcParam			= Cg.cgGetArrayParameter( Cg.cgGetNamedStructParameter( param, "m_CosArc" ), numSpotLights );
							Cg.cgSetParameter3f( positionParam, curLight.Position.X, curLight.Position.Y, curLight.Position.Z );
							Cg.cgSetParameter3f( directionParam, curLight.Direction.X, curLight.Direction.Y, curLight.Direction.Z );
							Cg.cgSetParameter1f( arcParam, ( float )System.Math.Cos( curLight.ArcDegrees * Constants.DegreesToRadians ) );
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

		private ShaderParameterDefaultBinding m_Binding;
	}
}
