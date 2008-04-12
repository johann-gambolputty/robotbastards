using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	public class OpenGlCubeMapTextureSampler : ICubeMapTextureSampler
	{
		#region Private Members

		private ICubeMapTexture m_Texture;
		private TextureFilter m_MinFilter;
		private TextureFilter m_MagFilter;

		#endregion

		#region ICubeMapTextureSampler Members

		/// <summary>
		/// Access to the bound texture
		/// </summary>
		public ICubeMapTexture Texture
		{
			get { return m_Texture; }
			set { m_Texture =  value; }
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is greater than the area of a texel
		/// </summary>
		public TextureFilter MinFilter
		{
			get { return m_MinFilter; }
			set { m_MinFilter = value; }
		}

		/// <summary>
		/// The filter used when the area covered by a fragment is less than the area of a texel. Can be either kNearest or kLinear
		/// </summary>
		public TextureFilter MagFilter
		{
			get { return m_MagFilter; }
			set { m_MagFilter = value; }
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Begins the pass
		/// </summary>
		public void Begin( )
		{
			int target = Gl.GL_TEXTURE_CUBE_MAP;

			Gl.glEnable( target );
			Graphics.Renderer.BindTexture( Texture );

			OpenGlTextureSampler2d.ApplyTextureFilter( target, Gl.GL_TEXTURE_MIN_FILTER, MinFilter );
			OpenGlTextureSampler2d.ApplyTextureFilter( target, Gl.GL_TEXTURE_MAG_FILTER, MagFilter );

			Gl.glTexEnvi( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL );

			int genMode = Gl.GL_REFLECTION_MAP;
			Gl.glTexGeni( Gl.GL_S, Gl.GL_TEXTURE_GEN_MODE, genMode );
			Gl.glTexGeni( Gl.GL_T, Gl.GL_TEXTURE_GEN_MODE, genMode );
			Gl.glTexGeni( Gl.GL_R, Gl.GL_TEXTURE_GEN_MODE, genMode );
			Gl.glEnable( Gl.GL_TEXTURE_GEN_S );
			Gl.glEnable( Gl.GL_TEXTURE_GEN_T );
			Gl.glEnable( Gl.GL_TEXTURE_GEN_R );
		}

		/// <summary>
		/// Ends the pass
		/// </summary>
		public void End( )
		{
			Graphics.Renderer.UnbindTexture( Texture );
			Gl.glDisable( Gl.GL_TEXTURE_GEN_S );
			Gl.glDisable( Gl.GL_TEXTURE_GEN_T );
			Gl.glDisable( Gl.GL_TEXTURE_GEN_R );
		}

		#endregion
	}
}
