using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using RbEngine;
using RbEngine.Rendering;
using Tao.Cg;

namespace RbOpenGlRendering.RbCg
{
	/// <summary>
	/// Summary description for CgRenderEffect.
	/// </summary>
	public class CgRenderEffect : RenderEffect
	{
		/// <summary>
		/// Creates the effect
		/// </summary>
		/// <param name="context"> Handle to the CG context that created this effect </param>
		public CgRenderEffect( IntPtr context )
		{
			m_Context = context;
		}

		/// <summary>
		/// Creates the effect, loading it from a .cgfx file
		/// </summary>
		/// <param name="context"> Handle to the CG context that created this effect </param>
		/// <param name="path"> Path to the effect file </param>
		public CgRenderEffect( IntPtr context, string path )
		{
			m_Context = context;
			Load( path );
		}

		/// <summary>
		/// Creates the effect, loading it from a .cgfx stream
		/// </summary>
		/// <param name="context"> Handle to the CG context that created this effect </param>
		/// <param name="path"> Path to the effect file </param>
		public CgRenderEffect( IntPtr context, System.IO.Stream input, string inputSource )
		{
			m_Context = context;
			Load( input, inputSource );
		}

		#region	Effect application

		//	NOTE: These functions were incorrectly imported into Tao (declared matrix parameter as "out float")
		
		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetMatrixParameterfc( IntPtr param, float[] matrix );

		[ DllImport( "cg.dll", CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity ]
		public static extern void cgSetMatrixParameterfr( IntPtr param, float[] matrix );

		/// <summary>
		/// Applies this effect
		/// </summary>
		public override unsafe void Begin( )
		{
			//	Set bound parameters
			for ( int bindingIndex = 0; bindingIndex < ( int )ShaderParameterBinding.NumBindings; ++bindingIndex )
			{
				IntPtr param = m_Bindings[ bindingIndex ];
				if ( param == IntPtr.Zero )
				{
					continue;
				}
				switch ( ( ShaderParameterBinding )bindingIndex )
				{
					case ShaderParameterBinding.ModelMatrix :
					{
						RbEngine.Maths.Matrix44 modelMatrix = Renderer.Inst.GetTransform( Transform.LocalToWorld );
						cgSetMatrixParameterfc( param, modelMatrix.Elements );
						break;
					}

					case ShaderParameterBinding.ViewMatrix	:
					{
						RbEngine.Maths.Matrix44 viewMatrix = Renderer.Inst.GetTransform( Transform.WorldToView );
						cgSetMatrixParameterfc( param, viewMatrix.Elements );
						break;
					}

					case ShaderParameterBinding.InverseTransposeModelMatrix :
					{
						RbEngine.Maths.Matrix44 itModelMatrix = new RbEngine.Maths.Matrix44( Renderer.Inst.GetTransform( Transform.LocalToWorld ) );
						itModelMatrix.Translation = RbEngine.Maths.Point3.Origin;
					//	itModelMatrix.Transpose( );
					//	itModelMatrix.Invert( );
					//	itModelMatrix.StoreInverse( Renderer.Inst.GetTransform( Transform.LocalToWorld ) );

						cgSetMatrixParameterfc( param, itModelMatrix.Elements );
						break;
					}

					case ShaderParameterBinding.ProjectionMatrix :
					{
						CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_PROJECTION_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

					case ShaderParameterBinding.TextureMatrix :
					{
						CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_TEXTURE_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

					case ShaderParameterBinding.ModelViewMatrix :
					{
						CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}
					case ShaderParameterBinding.InverseModelViewMatrix :
					{
						CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_MATRIX, CgGl.CG_GL_MATRIX_INVERSE );
						break;
					}

					case ShaderParameterBinding.InverseTransposeModelViewMatrix	:
					{
						CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_MATRIX, CgGl.CG_GL_MATRIX_INVERSE_TRANSPOSE );
						break;
					}

					case ShaderParameterBinding.ModelViewProjectionMatrix :
					{
						CgGl.cgGLSetStateMatrixParameter( param, CgGl.CG_GL_MODELVIEW_PROJECTION_MATRIX, CgGl.CG_GL_MATRIX_IDENTITY );
						break;
					}

					case ShaderParameterBinding.EyePosition :
					{
						RbEngine.Maths.Point3 eyePos = ( ( RbEngine.Cameras.Camera3 )Renderer.Inst.Camera ).Position;
						Cg.cgSetParameter3f( param, eyePos.X, eyePos.Y, eyePos.Z );
						break;
					}

					case ShaderParameterBinding.PointLights :
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

					case ShaderParameterBinding.SpotLights :
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
								Cg.cgSetParameter1f( arcParam, curLight.ArcDegrees * RbEngine.Maths.Constants.DegreesToRadians );
								++numSpotLights;
							}
						}

						Cg.cgSetParameter1i( Cg.cgGetNamedStructParameter( param, "m_NumLights" ), numSpotLights );

						break;
					}

					default :
					{
						throw new ApplicationException( string.Format( "Unhandled shader parameter binding \"{0}\"", ( ( ShaderParameterBinding )bindingIndex ).ToString( ) ) );
					}
				}
			}
			/*
		
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
		*/
		}

		/// <summary>
		/// Stops applying this effect
		/// </summary>
		public override void End( )
		{
		}


		#endregion

		#region	Effect loading and creation

		/// <summary>
		/// Loads this effect from a .cgfx file
		/// </summary>
		/// <param name="path"> Path to the effect file </param>
		public void	Load( string path )
		{
			if ( !CreateFromHandle( Cg.cgCreateEffectFromFile( m_Context, path, null ) ) )
			{
				throw new System.ApplicationException( String.Format( "Unable to create CG effect from path \"{0}\"\n{1}", path, Cg.cgGetLastListing( m_Context ) ) );
			}
		}

		/// <summary>
		/// Loads this effect from a .cgfx stream
		/// </summary>
		/// <param name="path"> Stream containing the .cgfx file </param>
		public void Load( System.IO.Stream input, string inputSource )
		{
			System.IO.StreamReader reader = new System.IO.StreamReader( input );
			string str = reader.ReadToEnd( );
			if ( !CreateFromHandle( Cg.cgCreateEffect( m_Context, str, null ) ) )
			{
				throw new System.ApplicationException( String.Format( "Unable to create CG effect from stream \"{0}\"\n{1}", inputSource, Cg.cgGetLastListing( m_Context ) ) );
			}
		}

		/// <summary>
		/// Creates this effect from an existing CGeffect handle
		/// </summary>
		/// <param name="effectHandle"> Handle to the CG effect. If this is null, nothing happens </param>
		private bool	CreateFromHandle( IntPtr effectHandle )
		{
			if ( effectHandle == IntPtr.Zero )
			{
				return false;
			}

			m_EffectHandle = effectHandle;

			//	Run through all the techniques in the effect
			for ( IntPtr curTechnique = Cg.cgGetFirstTechnique( m_EffectHandle ); curTechnique != IntPtr.Zero; curTechnique = Cg.cgGetNextTechnique( curTechnique ) )
			{
				string techniqueName = Cg.cgGetTechniqueName( curTechnique );
				if ( Cg.cgValidateTechnique( curTechnique ) == 0 )
				{
					Output.WriteLineCall( Output.RenderingWarning, "Unable to validate technique \"{0}\" - {1}", techniqueName, Cg.cgGetLastListing( m_Context ) );
					continue;
				}

				//	Create a RenderTechnique wrapper around the current technique
				RenderTechnique newTechnique = new RenderTechnique( techniqueName );

				//	Run through all the CG passes in the current technique
				for ( IntPtr curPass = Cg.cgGetFirstPass( curTechnique ); curPass != IntPtr.Zero; curPass = Cg.cgGetNextPass( curPass ) )
				{
					//	Create a CgRenderPass wrapper around the current pass, and add it to the current technique
					newTechnique.Add( new CgRenderPass( curPass ) );
				}

				Add( newTechnique );
			}

			//	Run through all the parameters in the effect, creating CgShaderParameter objects for each
			for ( IntPtr curParam = Cg.cgGetFirstEffectParameter( m_EffectHandle ); curParam != IntPtr.Zero; curParam = Cg.cgGetNextParameter( curParam ) )
			{
				//	Default bindings
				//	HACK: This is a bodge; should really use parameter annotations to determine such bindings
				ShaderParameterBinding binding = ShaderParameter.GetBindingFromParameterName( Cg.cgGetParameterName( curParam ) );
				if ( binding != ShaderParameterBinding.NumBindings )
				{
					m_Bindings[ ( int )binding ] = curParam;
				}

				m_Parameters.Add( new CgShaderParameter( m_Context, curParam ) );
			}

			return true;
		}

		#endregion

		#region	Effect parameters

		/// <summary>
		/// Finds a shader parameter by name
		/// </summary>
		/// <param name="name"> Name of the parameter to search for </param>
		/// <returns> Returns the named parameter, or null if the parameter could not be found </returns>
		public override ShaderParameter	GetParameter( string name )
		{
			foreach ( CgShaderParameter curParam in m_Parameters )
			{
				if ( Cg.cgGetParameterName( curParam.Parameter ) == name )
				{
					return curParam;
				}
			}

			return null;
		}

		/// <summary>
		/// Binds a parameter from this shader to a render state
		/// </summary>
		/// <param name="parameter"> Parameter to bind </param>
		/// <param name="binding"> Render state variable to bind to </param>
		/// <remarks>
		/// This need only be called once, to set up the binding. Every time that the shader to which this parameter belongs is applied (IAppliance::Apply())
		/// the parameter is updated to match the value of bound variable.
		/// If the parameter binding is set to ShaderParameterBinding.NoBinding, the parameter is unbound, and will no longer get updated.
		/// </remarks>
		public override void				BindParameter( ShaderParameter parameter, ShaderParameterBinding binding )
		{
			m_Bindings[ ( int )binding ] = ( ( CgShaderParameter )parameter ).Parameter;
		}

		#endregion


		private IntPtr				m_Context;
		private IntPtr				m_EffectHandle;
		private ArrayList			m_Parameters		= new ArrayList( );
		private IntPtr[]			m_Bindings			= new IntPtr[ ( int )ShaderParameterBinding.NumBindings ];
	}
}
