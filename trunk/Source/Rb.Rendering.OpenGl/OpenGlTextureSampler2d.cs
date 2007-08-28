using System;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Summary description for OpenGlApplyTexture2d.
	/// </summary>
	[Serializable]
	public class OpenGlTextureSampler2d : TextureSampler2d
	{
		/// <summary>
		/// OpenGl texture access
		/// </summary>
		public new OpenGlTexture2d Texture
		{
			get { return ( OpenGlTexture2d )base.Texture;  }
			set { base.Texture = value;  }
		}

		/// <summary>
		/// Applies the associated texture and texture parameters
		/// </summary>
		public override void Begin( )
		{
			OpenGlRenderer.Instance.BindTexture( Texture );

			ApplyTextureFilter( Gl.GL_TEXTURE_MIN_FILTER, MinFilter );
			ApplyTextureFilter( Gl.GL_TEXTURE_MAG_FILTER, MagFilter );

			ApplyTextureWrap( Gl.GL_TEXTURE_WRAP_S, WrapS );
			ApplyTextureWrap( Gl.GL_TEXTURE_WRAP_T, WrapT );

			switch ( Mode )
			{
				case TextureMode.Replace	:	Gl.glTexEnvi( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE	);	break;
				case TextureMode.Decal		:	Gl.glTexEnvi( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL	);	break;
				case TextureMode.Blend		:	Gl.glTexEnvi( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_BLEND	);	break;
				case TextureMode.Modulate	:	Gl.glTexEnvi( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE	);	break;
			}
		}

		/// <summary>
		/// Stops applying this sampler
		/// </summary>
		public override void End( )
		{
			OpenGlRenderer.Instance.UnbindTexture( Texture );
		}

		private static void ApplyTextureWrap( int dir, TextureWrap wrap )
		{
			switch ( wrap )
			{
				case TextureWrap.Repeat		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, dir, Gl.GL_REPEAT );	break;
				case TextureWrap.Clamp		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, dir, Gl.GL_CLAMP );	break;
			}
		}

		private static void ApplyTextureFilter( int filterType, TextureFilter filter )
		{
			switch ( filter )
			{
				case TextureFilter.NearestTexel					:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, filterType, Gl.GL_NEAREST					);	break;
				case TextureFilter.LinearTexel					:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, filterType, Gl.GL_LINEAR					);	break;
				case TextureFilter.NearestTexelNearestMipMap	:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, filterType, Gl.GL_NEAREST_MIPMAP_NEAREST	);	break;
				case TextureFilter.LinearTexelNearestMipMap		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, filterType, Gl.GL_LINEAR_MIPMAP_NEAREST	);	break;
				case TextureFilter.NearestTexelLinearMipMap		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, filterType, Gl.GL_NEAREST_MIPMAP_LINEAR	);	break;
				case TextureFilter.LinearTexelLinearMipMap		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, filterType, Gl.GL_LINEAR_MIPMAP_LINEAR	);	break;
			}
		}

	}
}
