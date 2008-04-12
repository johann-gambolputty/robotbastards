using System;
using Rb.Rendering.Textures;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Summary description for OpenGlApplyTexture2d.
	/// </summary>
	[Serializable]
	public class OpenGlTextureSampler2d : Texture2dSamplerBase
	{
		/// <summary>
		/// OpenGl texture access
		/// </summary>
		public OpenGlTexture2d OpenGlTexture
		{
			get { return ( OpenGlTexture2d )Texture;  }
		}


		/// <summary>
		/// Applies a texture filter to a given gl texture target
		/// </summary>
		public static void ApplyTextureFilter( int target, int filterType, TextureFilter filter )
		{
			switch ( filter )
			{
				case TextureFilter.NearestTexel				: Gl.glTexParameteri( target, filterType, Gl.GL_NEAREST ); break;
				case TextureFilter.LinearTexel				: Gl.glTexParameteri( target, filterType, Gl.GL_LINEAR ); break;
				case TextureFilter.NearestTexelNearestMipMap: Gl.glTexParameteri( target, filterType, Gl.GL_NEAREST_MIPMAP_NEAREST ); break;
				case TextureFilter.LinearTexelNearestMipMap	: Gl.glTexParameteri( target, filterType, Gl.GL_LINEAR_MIPMAP_NEAREST ); break;
				case TextureFilter.NearestTexelLinearMipMap	: Gl.glTexParameteri( target, filterType, Gl.GL_NEAREST_MIPMAP_LINEAR ); break;
				case TextureFilter.LinearTexelLinearMipMap	: Gl.glTexParameteri( target, filterType, Gl.GL_LINEAR_MIPMAP_LINEAR ); break;
			}
		}


		/// <summary>
		/// Applies a texture wrap style to a given gl texture target
		/// </summary>
		public static void ApplyTextureWrap( int target, int dir, TextureWrap wrap )
		{
			switch ( wrap )
			{
				case TextureWrap.Repeat	: Gl.glTexParameteri( target, dir, Gl.GL_REPEAT ); break;
				case TextureWrap.Clamp	: Gl.glTexParameteri( target, dir, Gl.GL_CLAMP ); break;
			}
		}

		/// <summary>
		/// Applies the associated texture and texture parameters
		/// </summary>
		public override void Begin( )
		{
		//	Gl.glEnable( Gl.GL_TEXTURE_2D );
			Graphics.Renderer.BindTexture( Texture );

			int target = OpenGlTexture.Target;

			ApplyTextureFilter( target, Gl.GL_TEXTURE_MIN_FILTER, MinFilter );
			ApplyTextureFilter( target, Gl.GL_TEXTURE_MAG_FILTER, MagFilter );

			ApplyTextureWrap( target, Gl.GL_TEXTURE_WRAP_S, WrapS );
			ApplyTextureWrap( target, Gl.GL_TEXTURE_WRAP_T, WrapT );

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
			Graphics.Renderer.UnbindTexture( Texture );
		}

	}
}
