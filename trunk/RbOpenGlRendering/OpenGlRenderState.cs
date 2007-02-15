using System;
using RbEngine.Rendering;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Summary description for OpenGlRenderState.
	/// </summary>
	public class OpenGlRenderState : RenderState
	{
		/// <summary>
		/// Applies this render state
		/// </summary>
		public override void Apply( )
		{
			//	Apply flag caps
			ApplyFlagCap( RenderStateFlag.kLighting, Gl.GL_LIGHTING );
			ApplyFlagCap( RenderStateFlag.kDepthTest, Gl.GL_DEPTH_TEST );
			ApplyFlagCap( RenderStateFlag.k2dTextures, Gl.GL_TEXTURE_2D );
			Gl.glDepthMask( ( m_CapFlags & RenderStateFlag.kDepthWrite ) != 0 );

			switch ( m_DepthTest )
			{
				case DepthTestPass.kNever			:	Gl.glDepthFunc( Gl.GL_NEVER );		break;
				case DepthTestPass.kLess			:	Gl.glDepthFunc( Gl.GL_LESS );		break;
				case DepthTestPass.kLessOrEqual		:	Gl.glDepthFunc( Gl.GL_LEQUAL );		break;
				case DepthTestPass.kEqual			:	Gl.glDepthFunc( Gl.GL_EQUAL );		break;
				case DepthTestPass.kNotEqual		:	Gl.glDepthFunc( Gl.GL_NOTEQUAL );	break;
				case DepthTestPass.kGreaterOrEqual	:	Gl.glDepthFunc( Gl.GL_GEQUAL );		break;
				case DepthTestPass.kGreater			:	Gl.glDepthFunc( Gl.GL_GREATER );	break;
				case DepthTestPass.kAlways			:	Gl.glDepthFunc( Gl.GL_ALWAYS );		break;
			}

			if ( ( m_CapFlags & ( RenderStateFlag.kCullFrontFaces | RenderStateFlag.kCullBackFaces ) ) == ( RenderStateFlag.kCullFrontFaces | RenderStateFlag.kCullBackFaces ) )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_FRONT_AND_BACK );
			}
			else if ( ( m_CapFlags & RenderStateFlag.kCullFrontFaces ) != 0 )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_FRONT );
			}
			else if ( ( m_CapFlags & RenderStateFlag.kCullBackFaces ) != 0 )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_BACK );
			}
			else
			{
				Gl.glDisable( Gl.GL_CULL_FACE );
			}

			Gl.glFrontFace( ( m_CapFlags & RenderStateFlag.kBackFacesAreClockwiseWound ) != 0 ? Gl.GL_CW : Gl.GL_CCW );

			//	Apply polygon rendering modes
			switch ( m_FrontPolyMode )
			{
				case PolygonRenderMode.kLines	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_LINE );	break;
				case PolygonRenderMode.kPoints	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_POINT );	break;
				case PolygonRenderMode.kFill	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_FILL );	break;
			}
			switch ( m_BackPolyMode )
			{
				case PolygonRenderMode.kLines	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_LINE );	break;
				case PolygonRenderMode.kPoints	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_POINT );	break;
				case PolygonRenderMode.kFill	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_FILL );	break;
			}

			//	Apply lights
			for ( int lightIndex = 0 ; lightIndex < m_Lights.Length; ++lightIndex )
			{
				if ( !m_Lights[ lightIndex ] ) 
				{
					Gl.glDisable( Gl.GL_LIGHT0 + lightIndex );
				}
				else
				{
					Gl.glEnable( Gl.GL_LIGHT0 + lightIndex );
				}
			}

			Gl.glPointSize( m_PointSize );
			Gl.glLineWidth( m_LineWidth );

			Gl.glColor3ub( m_Colour.R, m_Colour.G, m_Colour.B );
		}

		/// <summary>
		/// Applies a single OpenGL flagged capability
		/// </summary>
		private void ApplyFlagCap( RenderStateFlag cap, int oglCap )
		{
			if ( ( m_CapFlags & cap ) != 0 )
			{
				Gl.glEnable( oglCap );
			}
			else
			{
				Gl.glDisable( oglCap );
			}
		}

	}
}
