using System;
using System.Drawing;
using Poc0.LevelEditor.Core.Rendering;
using Rb.Rendering;
using Rb.Rendering.Textures;
using Tao.OpenGl;
using Graphics=Rb.Rendering.Graphics;
using System.Runtime.Serialization;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	/// <summary>
	/// OpenGL implementation of GroundPlaneGrid
	/// </summary>
	/// <remarks>
	/// Not sure what looks better - the grid rendered as lines or as a repeating texture. The static bool
	/// ms_UseTextureRenderer switches between the two options
	/// </remarks>
	[Serializable]
	public class OpenGlGroundPlaneGrid : GroundPlaneGrid
	{
		/// <summary>
		/// Renders the grid
		/// </summary>
		/// <param name="context">The rendering context</param>
		public override void Render( IRenderContext context )
		{
			if ( ms_UseTextureRenderer )
			{
				RenderTexture( );
			}
			else
			{
				RenderLines( );
			}
		}

		private void RenderLines( )
		{
			if ( m_LineDisplayList == -1 )
			{
				m_LineDisplayList = Gl.glGenLists( 1 );
				Gl.glNewList( m_LineDisplayList, Gl.GL_COMPILE );

				RenderState state = Graphics.Factory.NewRenderState( );
				state.Begin( );

				Gl.glBegin( Gl.GL_QUADS );

				Gl.glVertex3f( MinX, Y, MinZ );
				Gl.glVertex3f( MaxX, Y, MinZ );
				Gl.glVertex3f( MaxX, Y, MaxZ );
				Gl.glVertex3f( MinX, Y, MaxZ );

				Gl.glEnd( );
				state.End( );
				
				state.SetColour( Color.White );
				state.DisableCap( RenderStateFlag.DepthTest );
				state.Begin( );
				Gl.glBegin( Gl.GL_LINES );

				for ( float x = MinX; x < MaxX; x += MaxU )
				{
					Gl.glVertex3f( x, Y, MinZ );
					Gl.glVertex3f( x, Y, MaxZ );
				}
				for ( float z = MinZ; z < MaxZ; z += MaxV )
				{
					Gl.glVertex3f( MinX, Y, z );
					Gl.glVertex3f( MaxX, Y, z );
				}
				Gl.glEnd( );
				state.End( );

				Gl.glEndList( );
			}

			Gl.glCallList( m_LineDisplayList );
		}

		/// <summary>
		/// Renders the grid as a texture
		/// </summary>
		private void RenderTexture( )
		{
			if ( m_TextureState == null )
			{
				m_TextureState = Graphics.Factory.NewRenderState( );
				m_TextureState.DisableLighting( );
				m_TextureState.EnableCap( RenderStateFlag.Texture2d );
				m_TextureState.EnableCap( RenderStateFlag.Texture2dUnit0 );

				ITexture2d texture = Graphics.Factory.NewTexture2d( );
				texture.Load( GridSquareBitmap, true );

				m_Sampler = Graphics.Factory.NewTextureSampler2d( );
				m_Sampler.Texture = texture;
				m_Sampler.Mode = TextureMode.Replace;
				m_Sampler.MinFilter = TextureFilter.NearestTexelLinearMipMap;
				m_Sampler.MagFilter = TextureFilter.NearestTexelLinearMipMap;
				m_Sampler.WrapS = TextureWrap.Repeat;
				m_Sampler.WrapT = TextureWrap.Repeat;
			}
			
			//	For the moment, let's just render a giant quad with a texture :(
			//	TODO: AP: Render a grid of quads centered on the camera position projected onto the ground plane
			Graphics.Renderer.PushRenderState( m_TextureState );
			m_Sampler.Begin( );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glTexCoord2f( MinU, MinV );
			Gl.glVertex3f( MinX, Y, MinZ );

			Gl.glTexCoord2f( MaxU, MinV );
			Gl.glVertex3f( MaxX, Y, MinZ );
			
			Gl.glTexCoord2f( MaxU, MaxV );
			Gl.glVertex3f( MaxX, Y, MaxZ );

			Gl.glTexCoord2f( MinU, MaxV );
			Gl.glVertex3f( MinX, Y, MaxZ );

			Gl.glEnd( );
			
			m_Sampler.End( );
			Graphics.Renderer.PopRenderState( );
		}

		/// <summary>
		/// Make sure that the object is properly initialized after deserialization
		/// </summary>
		[OnDeserialized]
		public void OnDeserialized( StreamingContext context )
		{
			m_LineDisplayList = -1;
		}

		private readonly static bool	ms_UseTextureRenderer = false;

		[NonSerialized]
		private int						m_LineDisplayList = -1;

		[NonSerialized]
		private RenderState				m_TextureState;

		[NonSerialized]
		private TextureSampler2d		m_Sampler;

		private const float 			Width	= 100;
		private const float 			Depth	= 100;

		private const float 			MinX	= -Width;
		private const float 			Y		= -0.05f;
		private const float 			MinZ 	= -Depth;
		private const float 			MaxX 	= Width;
		private const float 			MaxZ 	= Depth;
		private const float 			MinU 	= 0;
		private const float 			MinV 	= 0;
		private const float 			MaxU 	= 20;
		private const float 			MaxV 	= 20;

	}
}
