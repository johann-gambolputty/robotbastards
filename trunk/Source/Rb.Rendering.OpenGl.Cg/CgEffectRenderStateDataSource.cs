using System;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Rendering.Interfaces.Objects.Lights;
using TaoCgGl = Tao.Cg.CgGl;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	public class CgEffectRenderStateDataSource : IEffectDataSource
	{
		public enum Source
		{
		}

		public CgEffectRenderStateDataSource( EffectRenderStateBinding source )
		{
			m_Binding = source;
		}

		private readonly EffectRenderStateBinding m_Binding;

		#region IEffectDataSource Members

		public void Bind( IEffectParameter parameter )
		{
			parameter.DataSource = this;
		}

		public void Apply( IEffectParameter parameter )
		{
			CgEffectParameter cgParam = ( ( CgEffectParameter )parameter );
			IntPtr context = cgParam.Context;
			IntPtr param = cgParam.Parameter;
			switch ( m_Binding )
			{
				case EffectRenderStateBinding.NearZ:
					{
						IProjectionCamera curCam = Graphics.Renderer.Camera as IProjectionCamera;
						if ( curCam != null )
						{
							TaoCg.cgSetParameter1f( param, curCam.PerspectiveZNear );
						}

						break;
					}

				case EffectRenderStateBinding.FarZ:
					{
						IProjectionCamera curCam = Graphics.Renderer.Camera as IProjectionCamera;
						if ( curCam != null )
						{
							TaoCg.cgSetParameter1f( param, curCam.PerspectiveZFar );
						}

						break;
					}

				case EffectRenderStateBinding.ModelMatrix:
					{
						Matrix44 modelMatrix = Graphics.Renderer.GetTransform( TransformType.LocalToWorld );
						CgEffectParameter.cgSetMatrixParameterfc( param, modelMatrix.Elements );
						break;
					}

				case EffectRenderStateBinding.ViewMatrix:
					{
						Matrix44 viewMatrix = Graphics.Renderer.GetTransform( TransformType.WorldToView );
						CgEffectParameter.cgSetMatrixParameterfc( param, viewMatrix.Elements );
						break;
					}

				case EffectRenderStateBinding.InverseTransposeModelMatrix:
					{
						Matrix44 itModelMatrix = Graphics.Renderer.GetTransform( TransformType.LocalToWorld );
						itModelMatrix.Translation = Point3.Origin;
						itModelMatrix.Transpose( );
						itModelMatrix.Invert( );

						CgEffectParameter.cgSetMatrixParameterfc( param, itModelMatrix.Elements );
						break;
					}

				case EffectRenderStateBinding.ProjectionMatrix:
					{
						TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_PROJECTION_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

				case EffectRenderStateBinding.TextureMatrix:
					{
						TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_TEXTURE_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

				case EffectRenderStateBinding.ModelViewMatrix:
					{
						TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

				case EffectRenderStateBinding.InverseModelViewMatrix:
					{
						TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_MATRIX, TaoCgGl.CG_GL_MATRIX_INVERSE );
						break;
					}

				case EffectRenderStateBinding.InverseTransposeModelViewMatrix:
					{
						TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_MATRIX, TaoCgGl.CG_GL_MATRIX_INVERSE_TRANSPOSE );
						break;
					}

				case EffectRenderStateBinding.ModelViewProjectionMatrix:
					{
						TaoCgGl.cgGLSetStateMatrixParameter( param, TaoCgGl.CG_GL_MODELVIEW_PROJECTION_MATRIX, TaoCgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

				case EffectRenderStateBinding.ViewportWidth:
					{
						throw new NotImplementedException( );
					}

				case EffectRenderStateBinding.ViewportHeight:
					{
						throw new NotImplementedException( );
					}

				case EffectRenderStateBinding.EyePosition:
					{
						ICamera3 curCam = ( ( ICamera3 )Graphics.Renderer.Camera );
						if ( curCam != null )
						{
							Point3 eyePos = curCam.Frame.Translation;
							TaoCg.cgSetParameter3f( param, eyePos.X, eyePos.Y, eyePos.Z );
						}
						break;
					}

				case EffectRenderStateBinding.EyeZAxis:
					{
						ICamera3 curCam = ( ( ICamera3 )Graphics.Renderer.Camera );
						if ( curCam != null )
						{
							Vector3 eyeVec = curCam.Frame.ZAxis;
							TaoCg.cgSetParameter3f( param, eyeVec.X, eyeVec.Y, eyeVec.Z );
						}
						break;
					}

				case EffectRenderStateBinding.PointLights:
					{
						//	TODO: This is REALLY SHIT. Need to refactor this mess
						int numActiveLights = Graphics.Renderer.NumActiveLights;
						int numPointLights = 0;
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

				case EffectRenderStateBinding.SpotLights:
					{
						//	TODO: This is REALLY SHIT. Need to refactor this mess
						int numActiveLights = Graphics.Renderer.NumActiveLights;
						int numSpotLights = 0;
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

				case EffectRenderStateBinding.Texture0:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 0 ) );
						break;
					}
				case EffectRenderStateBinding.Texture1:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 1 ) );
						break;
					}
				case EffectRenderStateBinding.Texture2:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 2 ) );
						break;
					}
				case EffectRenderStateBinding.Texture3:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 3 ) );
						break;
					}
				case EffectRenderStateBinding.Texture4:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 4 ) );
						break;
					}
				case EffectRenderStateBinding.Texture5:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 5 ) );
						break;
					}
				case EffectRenderStateBinding.Texture6:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 6 ) );
						break;
					}
				case EffectRenderStateBinding.Texture7:
					{
						CgEffectParameter.BindTexture( context, param, Graphics.Renderer.GetTexture( 7 ) );
						break;
					}
				default:
					{
						throw new ApplicationException( string.Format( "Unhandled effect render state binding \"{0}\"", m_Binding ) );
					}
			}
		}

		#endregion
	}
}
