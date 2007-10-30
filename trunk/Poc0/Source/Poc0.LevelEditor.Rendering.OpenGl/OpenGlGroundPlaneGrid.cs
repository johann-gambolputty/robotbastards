using System;
using Poc0.LevelEditor.Core.Rendering;
using Rb.Rendering;
using Tao.OpenGl;

namespace Poc0.LevelEditor.Rendering.OpenGl
{
	/// <summary>
	/// OpenGL implementation of GroundPlaneGrid
	/// </summary>
	[Serializable]
	public class OpenGlGroundPlaneGrid : GroundPlaneGrid
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public OpenGlGroundPlaneGrid( )
		{
			m_State = Graphics.Factory.NewRenderState( );
			m_State.DisableLighting( );
			m_State.EnableCap( RenderStateFlag.Texture2d );
			m_State.EnableCap( RenderStateFlag.Texture2dUnit0 );

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

		/// <summary>
		/// Renders the grid
		/// </summary>
		/// <param name="context">The rendering context</param>
		public override void Render( IRenderContext context )
		{
			float minX = -Width;
			float y = -0.05f;
			float minZ = -Depth;
			float maxX = Width;
			float maxZ = Depth;
			float minU = 0;
			float minV = 0;
			float maxU = 20;
			float maxV = 20;

			//	For the moment, let's just render a giant quad with a texture :(
			//	TODO: AP: Render a grid of quads centered on the camera position projected onto the ground plane
			Graphics.Renderer.PushRenderState( m_State );
			m_Sampler.Begin( );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glTexCoord2f( minU, minV );
			Gl.glVertex3f( minX, y, minZ );

			Gl.glTexCoord2f( maxU, minV );
			Gl.glVertex3f( maxX, y, minZ );
			
			Gl.glTexCoord2f( maxU, maxV );
			Gl.glVertex3f( maxX, y, maxZ );

			Gl.glTexCoord2f( minU, maxV );
			Gl.glVertex3f( minX, y, maxZ );

			Gl.glEnd( );
			
			m_Sampler.End( );
			Graphics.Renderer.PopRenderState( );
		}

		private readonly RenderState m_State;
		private readonly TextureSampler2d m_Sampler;

		private const float Width = 100;
		private const float Depth = 100;

	}
}
