using System;
using System.Collections;
using RbEngine;
using RbEngine.Rendering;

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

		public override void Apply()
		{
			IntPtr param = m_Bindings[ ( int )ShaderParameterBinding.ModelViewMatrix ];
			if ( param != IntPtr.Zero )
			{
				Tao.Cg.CgGl.cgGLSetStateMatrixParameter( param, Tao.Cg.CgGl.CG_GL_MODELVIEW_MATRIX, Tao.Cg.CgGl.CG_GL_MATRIX_IDENTITY );
			}

			param = m_Bindings[ ( int )ShaderParameterBinding.InverseModelViewMatrix ];
			if ( param != IntPtr.Zero )
			{
				Tao.Cg.CgGl.cgGLSetStateMatrixParameter( param, Tao.Cg.CgGl.CG_GL_MODELVIEW_MATRIX, Tao.Cg.CgGl.CG_GL_MATRIX_INVERSE );
			}

			param = m_Bindings[ ( int )ShaderParameterBinding.InverseTransposeModelViewMatrix ];
			if ( param != IntPtr.Zero )
			{
				Tao.Cg.CgGl.cgGLSetStateMatrixParameter( param, Tao.Cg.CgGl.CG_GL_MODELVIEW_MATRIX, Tao.Cg.CgGl.CG_GL_MATRIX_INVERSE_TRANSPOSE );
			}

			param = m_Bindings[ ( int )ShaderParameterBinding.ModelViewProjectionMatrix ];
			if ( param != IntPtr.Zero )
			{
				Tao.Cg.CgGl.cgGLSetStateMatrixParameter( param, Tao.Cg.CgGl.CG_GL_MODELVIEW_PROJECTION_MATRIX, Tao.Cg.CgGl.CG_GL_MATRIX_IDENTITY );
			}
			
			param = m_Bindings[ ( int )ShaderParameterBinding.EyePosition ];
			if ( param != IntPtr.Zero )
			{
				//	TODO: Setup camera frame and frustum in the renderer
			//	Tao.Cg.Cg.cgSetParameter3f( );
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


		#endregion

		#region	Effect loading and creation

		/// <summary>
		/// Loads this effect from a .cgfx file
		/// </summary>
		/// <param name="path"> Path to the effect file </param>
		public void	Load( string path )
		{
			if ( !CreateFromHandle( Tao.Cg.Cg.cgCreateEffectFromFile( m_Context, path, null ) ) )
			{
				throw new System.ApplicationException( String.Format( "Unable to create CG effect from path \"{0}\"\n{1}", path, Tao.Cg.Cg.cgGetLastListing( m_Context ) ) );
			}

			Output.WriteLineCall( Output.RenderingInfo, "Successfully loaded effect \"{0}\" from file", path );
		}

		/// <summary>
		/// Loads this effect from a .cgfx stream
		/// </summary>
		/// <param name="path"> Stream containing the .cgfx file </param>
		public void Load( System.IO.Stream input, string inputSource )
		{
			System.IO.StreamReader reader = new System.IO.StreamReader( input );
			string str = reader.ReadToEnd( );
			if ( !CreateFromHandle( Tao.Cg.Cg.cgCreateEffect( m_Context, str, null ) ) )
			{
				throw new System.ApplicationException( String.Format( "Unable to create CG effect from stream \"{0}\"\n{1}", inputSource, Tao.Cg.Cg.cgGetLastListing( m_Context ) ) );
			}

			Output.WriteLineCall( Output.RenderingInfo, "Successfully loaded effect \"{0}\" from stream", inputSource );
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
			for ( IntPtr curTechnique = Tao.Cg.Cg.cgGetFirstTechnique( m_EffectHandle ); curTechnique != IntPtr.Zero; curTechnique = Tao.Cg.Cg.cgGetNextTechnique( curTechnique ) )
			{
				string techniqueName = Tao.Cg.Cg.cgGetTechniqueName( curTechnique );
				if ( Tao.Cg.Cg.cgValidateTechnique( curTechnique ) == 0 )
				{
					Output.WriteLineCall( Output.RenderingWarning, "Unable to validate technique \"{0}\" - {1}", techniqueName, Tao.Cg.Cg.cgGetLastListing( m_Context ) );
					continue;
				}

				//	Create a RenderTechnique wrapper around the current technique
				//	TODO: This is failing in the current Tao implementation (was working fine on previous version...)
				RenderTechnique newTechnique = new RenderTechnique( techniqueName );

				//	Run through all the CG passes in the current technique
				for ( IntPtr curPass = Tao.Cg.Cg.cgGetFirstPass( curTechnique ); curPass != IntPtr.Zero; curPass = Tao.Cg.Cg.cgGetNextPass( curPass ) )
				{
					//	Create a CgRenderPass wrapper around the current pass, and add it to the current technique
					newTechnique.Add( new CgRenderPass( curPass ) );
				}
			}

			//	Run through all the parameters in the effect, creating CgShaderParameter objects for each
			for ( IntPtr curParam = Tao.Cg.Cg.cgGetFirstEffectParameter( m_EffectHandle ); curParam != IntPtr.Zero; curParam = Tao.Cg.Cg.cgGetNextParameter( curParam ) )
			{
				m_Parameters.Add( new CgShaderParameter( curParam ) );
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
				if ( Tao.Cg.Cg.cgGetParameterName( curParam.Parameter ) == name )
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
		/// This need only be called once, to set up the binding. Every time that the shader to which this parameter belongs is applied (IApplicable::Apply())
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
		private ArrayList			m_Parameters	= new ArrayList( );
		private IntPtr[]			m_Bindings		= new IntPtr[ ( int )ShaderParameterBinding.NumBindings ];
	}
}
