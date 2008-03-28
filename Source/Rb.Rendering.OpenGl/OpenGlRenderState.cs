using System;
using System.Drawing;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Summary description for OpenGlRenderState.
	/// </summary>
	[Serializable]
	public class OpenGlRenderState : IRenderState
	{
		/// <summary>
		/// Applies this render state
		/// </summary>
		public void Begin( )
		{
			//	Apply flag caps
			ApplyFlagCap( Lighting, Gl.GL_LIGHTING );
			ApplyFlagCap( DepthTest, Gl.GL_DEPTH_TEST );
			ApplyFlagCap( Blend, Gl.GL_BLEND );
			ApplyFlagCap( Enable2dTextures, Gl.GL_TEXTURE_2D );
			Gl.glDepthMask( DepthWrite ? 1 : 0 );

			EnableTextureUnit( 1 );
			EnableTextureUnit( 2 );
			EnableTextureUnit( 3 );
			EnableTextureUnit( 4 );
			EnableTextureUnit( 5 );
			EnableTextureUnit( 6 );
			EnableTextureUnit( 7 );
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB );

			switch ( PassDepthTest )
			{
				case PassDepthTest.Never			:	Gl.glDepthFunc( Gl.GL_NEVER );		break;
				case PassDepthTest.Less				:	Gl.glDepthFunc( Gl.GL_LESS );		break;
				case PassDepthTest.LessOrEqual		:	Gl.glDepthFunc( Gl.GL_LEQUAL );		break;
				case PassDepthTest.Equal			:	Gl.glDepthFunc( Gl.GL_EQUAL );		break;
				case PassDepthTest.NotEqual			:	Gl.glDepthFunc( Gl.GL_NOTEQUAL );	break;
				case PassDepthTest.GreaterOrEqual	:	Gl.glDepthFunc( Gl.GL_GEQUAL );		break;
				case PassDepthTest.Greater			:	Gl.glDepthFunc( Gl.GL_GREATER );	break;
				case PassDepthTest.Always			:	Gl.glDepthFunc( Gl.GL_ALWAYS );		break;
			}

			if ( CullFaces )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_FRONT_AND_BACK );
			}
			else if ( CullFrontFaces )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_FRONT );
			}
			else if ( CullBackFaces )
			{
				Gl.glEnable( Gl.GL_CULL_FACE );
				Gl.glCullFace( Gl.GL_BACK );
			}
			else
			{
				Gl.glDisable( Gl.GL_CULL_FACE );
			}

			if ( FaceWinding == Winding.Clockwise )
			{
				Gl.glFrontFace( Gl.GL_CW );
			}
			else
			{
				Gl.glFrontFace( Gl.GL_CCW );
			}

			//	Apply polygon rendering modes
			switch ( ShadeMode )
			{
				case PolygonShadeMode.Flat		: Gl.glShadeModel( Gl.GL_FLAT );	break;
				case PolygonShadeMode.Smooth	: Gl.glShadeModel( Gl.GL_SMOOTH );	break;
			}
			switch ( FrontFaceRenderMode )
			{
				case PolygonRenderMode.Lines	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_LINE );	break;
				case PolygonRenderMode.Points	: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_POINT );	break;
				case PolygonRenderMode.Fill		: Gl.glPolygonMode( Gl.GL_FRONT, Gl.GL_FILL );	break;
			}
			switch ( BackFaceRenderMode )
			{
				case PolygonRenderMode.Lines	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_LINE );	break;
				case PolygonRenderMode.Points	: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_POINT );	break;
				case PolygonRenderMode.Fill		: Gl.glPolygonMode( Gl.GL_BACK, Gl.GL_FILL );	break;
			}

			int srcBlend = GetBlendFactor( SourceBlend );
			int dstBlend = GetBlendFactor( DestinationBlend );
			Gl.glBlendFunc( srcBlend, dstBlend );

			////	Apply lights
			//for ( int lightIndex = 0 ; lightIndex < m_Lights.Length; ++lightIndex )
			//{
			//    if ( !m_Lights[ lightIndex ] ) 
			//    {
			//        Gl.glDisable( Gl.GL_LIGHT0 + lightIndex );
			//    }
			//    else
			//    {
			//        Gl.glEnable( Gl.GL_LIGHT0 + lightIndex );
			//    }
			//}

			Gl.glPointSize( PointSize );
			Gl.glLineWidth( LineWidth );

			if ( DepthOffset != 0 )
			{
				//	TODO: Assumes depth offsets will only be used for line rendering (can be used for decals, etc.)
				Gl.glEnable( Gl.GL_POLYGON_OFFSET_LINE );
				Gl.glPolygonOffset( DepthOffset, DepthOffset );
			}
			else
			{
				Gl.glDisable( Gl.GL_POLYGON_OFFSET_LINE );
			}

			Gl.glColor3ub( Colour.R, Colour.G, Colour.B );
		}

		/// <summary>
		/// Stops applying this render state
		/// </summary>
		public void End( )
		{
		}

		private void EnableTextureUnit( int unit )
		{
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB + unit );
			if ( ( m_TextureUnits & ( 1 << unit ) ) != 0 )
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

			throw new ApplicationException( string.Format( "Unhandled blend factor \"{0}\"", factor ) );
		}

		/// <summary>
		/// Applies a single OpenGL flagged capability
		/// </summary>
		private static void ApplyFlagCap( bool cap, int oglCap )
		{
			if ( cap )
			{
				Gl.glEnable( oglCap );
			}
			else
			{
				Gl.glDisable( oglCap );
			}
		}

		private bool m_Lighting;
		private float m_DepthOffset;
		private PassDepthTest m_PassDepthTest;
		private bool m_DepthTest;
		private bool m_DepthWrite;
		private bool m_CullBackFaces;
		private bool m_CullFrontFaces;
		private Winding m_FaceWinding;
		private PolygonRenderMode m_FrontFaceRenderMode;
		private PolygonRenderMode m_BackFaceRenderMode;
		private Color m_Colour;
		private PolygonShadeMode m_ShadeMode;
		private bool m_Enable2dTextures;
		private bool m_Blend;
		private BlendFactor m_SourceBlend;
		private BlendFactor m_DestinationBlend;
		private float m_PointSize;
		private float m_LineWidth;
		private uint m_TextureUnits;

		#region IRenderState Members

		public bool Lighting
		{
			get { return m_Lighting; }
			set { m_Lighting = value; }
		}

		public bool CullFaces
		{
			get
			{
				return CullBackFaces && CullFrontFaces;
			}
			set
			{
				CullBackFaces = value;
				CullFrontFaces = value;
			}
		}

		public void Enable2dTextureUnit( int unit, bool enabled )
		{
			if ( enabled )
			{
				m_TextureUnits |= ( uint )( 1 << unit );
			}
			else
			{
				m_TextureUnits &= ( uint )( 1 << unit );
			}
		}

		public bool Is2dTextureUnitEnabled( int unit )
		{
			return ( m_TextureUnits & ( uint )( 1 << unit ) ) != 0;
		}

		public float DepthOffset
		{
			get { return m_DepthOffset; }
			set { m_DepthOffset = value; }
		}

		public PassDepthTest PassDepthTest
		{
			get { return m_PassDepthTest; }
			set { m_PassDepthTest = value; }
		}

		public bool DepthTest
		{
			get { return m_DepthTest; }
			set { m_DepthTest = value; }
		}

		public bool DepthWrite
		{
			get { return m_DepthWrite; }
			set { m_DepthWrite = value; }
		}

		public bool CullBackFaces
		{
			get { return m_CullBackFaces; }
			set { m_CullBackFaces = value; }
		}

		public bool CullFrontFaces
		{
			get { return m_CullFrontFaces; }
			set { m_CullFrontFaces = value; }
		}

		public Winding FaceWinding
		{
			get { return m_FaceWinding; }
			set { m_FaceWinding = value; }
		}

		public PolygonRenderMode FrontFaceRenderMode
		{
			get { return m_FrontFaceRenderMode; }
			set { m_FrontFaceRenderMode = value; }
		}

		public PolygonRenderMode BackFaceRenderMode
		{
			get { return m_BackFaceRenderMode; }
			set { m_BackFaceRenderMode = value; }
		}

		public Color Colour
		{
			get { return m_Colour; }
			set { m_Colour = value; }
		}

		public PolygonShadeMode ShadeMode
		{
			get { return m_ShadeMode; }
			set { m_ShadeMode = value; }
		}

		public bool Enable2dTextures
		{
			get { return m_Enable2dTextures; }
			set { m_Enable2dTextures = value; }
		}

		public bool Blend
		{
			get { return m_Blend; }
			set { m_Blend = value; }
		}

		public BlendFactor SourceBlend
		{
			get { return m_SourceBlend; }
			set { m_SourceBlend = value; }
		}

		public BlendFactor DestinationBlend
		{
			get { return m_DestinationBlend; }
			set { m_DestinationBlend = value; }
		}

		public float PointSize
		{
			get { return m_PointSize; }
			set { m_PointSize = value; }
		}

		public float LineWidth
		{
			get { return m_LineWidth; }
			set { m_LineWidth = value; }
		}

		#endregion
	}
}
