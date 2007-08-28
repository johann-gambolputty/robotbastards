using System;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Summary description for OpenGlRenderState.
	/// </summary>
	[Serializable]
	public class OpenGlRenderState : RenderState
	{
		/// <summary>
		/// Applies this render state
		/// </summary>
		public override void Begin( )
		{
			//	Apply flag caps
			ApplyFlagCap( RenderStateFlag.Lighting, Gl.GL_LIGHTING );
			ApplyFlagCap( RenderStateFlag.DepthTest, Gl.GL_DEPTH_TEST );
			ApplyFlagCap( RenderStateFlag.Blend, Gl.GL_BLEND );
			ApplyFlagCap( RenderStateFlag.Texture2d, Gl.GL_TEXTURE_2D );
			Gl.glDepthMask( ( m_CapFlags & RenderStateFlag.DepthWrite ) != 0 ? 1 : 0 );

			EnableTextureUnit( RenderStateFlag.Texture2dUnit1, 1 );
			EnableTextureUnit( RenderStateFlag.Texture2dUnit2, 2 );
			EnableTextureUnit( RenderStateFlag.Texture2dUnit3, 3 );
			EnableTextureUnit( RenderStateFlag.Texture2dUnit4, 4 );
			EnableTextureUnit( RenderStateFlag.Texture2dUnit5, 5 );
			EnableTextureUnit( RenderStateFlag.Texture2dUnit6, 6 );
			EnableTextureUnit( RenderStateFlag.Texture2dUnit7, 7 );
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB );

			switch ( m_DepthTest )
			{
				case DepthTestPass.Never			:	Gl.glDepthFunc( Gl.GL_NEVER );		break;
				case DepthTestPass.Less				:	Gl.glDepthFunc( Gl.GL_LESS );		break;
				case DepthTestPass.LessOrEqual		:	Gl.glDepthFunc( Gl.GL_LEQUAL );		break;
				case DepthTestPass.Equal			:	Gl.glDepthFunc( Gl.GL_EQUAL );		break;
				case DepthTestPass.NotEqual			:	Gl.glDepthFunc( Gl.GL_NOTEQUAL );	break;
				case DepthTestPass.GreaterOrEqual	:	Gl.glDepthFunc( Gl.GL_GEQUAL );		break;
				case DepthTestPass.Greater			:	Gl.glDepthFunc( Gl.GL_GREATER );	break;
				case DepthTestPass.Always			:	Gl.glDepthFunc( Gl.GL_ALWAYS );		break;
			}

			if ( ( m_CapFlags & ( RenderStateFlag.CullFrontFaces | RenderStateFlag.CullBackFaces ) ) == ( RenderStateFlag.CullFrontFaces | RenderStateFlag.CullBackFaces ) )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_FRONT_AND_BACK );
			}
			else if ( ( m_CapFlags & RenderStateFlag.CullFrontFaces ) != 0 )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_FRONT );
			}
			else if ( ( m_CapFlags & RenderStateFlag.CullBackFaces ) != 0 )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_BACK );
			}
			else
			{
				Gl.glDisable( Gl.GL_CULL_FACE );
			}

			Gl.glFrontFace( ( m_CapFlags & RenderStateFlag.BackFacesAreClockwiseWound ) != 0 ? Gl.GL_CW : Gl.GL_CCW );

			//	Apply polygon rendering modes
			switch ( m_ShadeMode )
			{
				case PolygonShadeMode.Flat		: Gl.glShadeModel( Gl.GL_FLAT );	break;
				case PolygonShadeMode.Smooth	: Gl.glShadeModel( Gl.GL_SMOOTH );	break;
			}
			switch ( m_FrontPolyMode )
			{
				case PolygonRenderMode.Lines	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_LINE );	break;
				case PolygonRenderMode.Points	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_POINT );	break;
				case PolygonRenderMode.Fill		: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_FILL );	break;
			}
			switch ( m_BackPolyMode )
			{
				case PolygonRenderMode.Lines	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_LINE );	break;
				case PolygonRenderMode.Points	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_POINT );	break;
				case PolygonRenderMode.Fill		: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_FILL );	break;
			}

			int srcBlend = GetBlendFactor( m_SrcBlend );
			int dstBlend = GetBlendFactor( m_DstBlend );
			Gl.glBlendFunc( srcBlend, dstBlend );

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

			if ( m_DepthOffset != 0 )
			{
				//	TODO: Assumes depth offsets will only be used for line rendering (can be used for decals, etc.)
				Gl.glEnable( Gl.GL_POLYGON_OFFSET_LINE );
				Gl.glPolygonOffset( m_DepthOffset, m_DepthOffset );
			}
			else
			{
				Gl.glDisable( Gl.GL_POLYGON_OFFSET_LINE );
			}

			Gl.glColor3ub( m_Colour.R, m_Colour.G, m_Colour.B );
		}

		/// <summary>
		/// Stops applying this render state
		/// </summary>
		public override void End( )
		{
		}

		private void EnableTextureUnit( RenderStateFlag flag, int unit )
		{
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB + unit );
			if ( ( m_CapFlags & flag ) != 0 )
			{
				Gl.glEnable( Gl.GL_TEXTURE_2D );
			}
			else
			{
				Gl.glDisable( Gl.GL_TEXTURE_2D );
			}
		}

		private static int GetBlendFactor( BlendFactor factor )
		{
			switch ( factor )
			{
				case BlendFactor.Zero				: return Gl.GL_ZERO;

				case BlendFactor.DstColour			: return Gl.GL_DST_COLOR;
				case BlendFactor.OneMinusDstColour	: return Gl.GL_ONE_MINUS_DST_COLOR;
				case BlendFactor.DstAlpha			: return Gl.GL_DST_ALPHA;
				case BlendFactor.OneMinusDstAlpha	: return Gl.GL_ONE_MINUS_DST_ALPHA;

				case BlendFactor.SrcColour			: return Gl.GL_SRC_COLOR;
				case BlendFactor.OneMinusSrcColour	: return Gl.GL_ONE_MINUS_SRC_COLOR;
				case BlendFactor.SrcAlpha			: return Gl.GL_SRC_ALPHA;
				case BlendFactor.OneMinusSrcAlpha	: return Gl.GL_ONE_MINUS_SRC_ALPHA;
				case BlendFactor.SrcAlphaSaturate	: return Gl.GL_SRC_ALPHA_SATURATE;

				case BlendFactor.One				: return Gl.GL_ONE;
			}

			throw new ApplicationException( string.Format( "Unhandled blend factor \"{0}\"", factor.ToString( ) ) );
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
