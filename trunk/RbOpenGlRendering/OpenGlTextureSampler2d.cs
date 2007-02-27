using System;
using RbEngine.Rendering;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Summary description for OpenGlApplyTexture2d.
	/// </summary>
	public class OpenGlTextureSampler2d : TextureSampler2d
	{
		/// <summary>
		/// Applies the associated texture and texture parameters
		/// </summary>
		public override void Apply( )
		{
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, ( ( OpenGlTexture2d )Texture ).TextureHandle );

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
		
		private void ApplyTextureWrap( int dir, TextureWrap wrap )
		{
			switch ( wrap )
			{
				case TextureWrap.Repeat		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, dir, Gl.GL_REPEAT );	break;
				case TextureWrap.Clamp		:	Gl.glTexParameteri( Gl.GL_TEXTURE_2D, dir, Gl.GL_CLAMP );	break;
			}
		}

		private void ApplyTextureFilter( int filterType, TextureFilter filter )
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
