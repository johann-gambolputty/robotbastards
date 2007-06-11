using System;
using System.Drawing;
using System.Drawing.Imaging;
using RbEngine;
using RbEngine.Rendering;
using Tao.OpenGl;

namespace RbOpenGlRendering
{

	/// <summary>
	/// OpenGL implementation of Texture2d
	/// </summary>
	public class OpenGlTexture2d : Texture2d, IDisposable
	{
		/// <summary>
		/// Returns the internal texture handle
		/// </summary>
		public int	TextureHandle
		{
			get
			{
				return m_TextureHandle;
			}
		}

		/// <summary>
		/// Makes sure the associated texture data has been released
		/// </summary>
		~OpenGlTexture2d( )
		{
			Dispose( );
		}


		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		public override void Create( int width, int height, TextureFormat format )
		{
			if ( m_TextureHandle != -1 )
			{
				Gl.glDeleteTextures( 1, ref m_TextureHandle );
			}

			//	Get the GL format of the bitmap, possibly converting it in the process
			m_Format	= CheckTextureFormat( format, out m_InternalGlFormat, out m_GlFormat, out m_GlType );
			m_Width		= width;
			m_Height	= height;

			//	Generate a texture name
			Gl.glGenTextures( 1, out m_TextureHandle );
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, m_TextureHandle );

			byte[] nullArray = null;
			Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, m_InternalGlFormat, width, height, 0, m_GlFormat, m_GlType, nullArray );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public override void Load( System.Drawing.Bitmap bmp )
		{
			//	Get the GL format of the bitmap, possibly converting it in the process
			int glInternalFormat = 0;
			int glFormat = 0;
			int glType = 0;
			bmp = CheckBmpFormat( bmp, out glInternalFormat, out glFormat, out glType );

			//	Lock the bitmap, and create the texture
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, bmp.PixelFormat );
			Create( bmpData.Width, bmpData.Height, glInternalFormat, glFormat, glType, bmpData.Scan0 );
			bmp.UnlockBits( bmpData );

			m_Format = PixelFormatToTextureFormat( bmp.PixelFormat );
		}

		/// <summary>
		/// Creates the texture
		/// </summary>
		private void Create( int width, int height, int glInternalFormat, int glFormat, int glType, IntPtr bytes )
		{
			if ( m_TextureHandle != -1 )
			{
				Gl.glDeleteTextures( 1, ref m_TextureHandle );
			}

			//	Generate a texture name
			Gl.glGenTextures( 1, out m_TextureHandle );
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, m_TextureHandle );

			Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, glInternalFormat, width, height, 0, glFormat, glType, bytes );

			m_InternalGlFormat	= glInternalFormat;
			m_GlFormat			= glFormat;
			m_GlType			= glType;
			m_Width				= width;
			m_Height			= height;
		}

		/// <summary>
		/// Converts a PixelFormat value into a TextureFormat
		/// </summary>
		private TextureFormat	PixelFormatToTextureFormat( PixelFormat pixFormat )
		{
			switch ( pixFormat )
			{
				case PixelFormat.Format24bppRgb		:	return TextureFormat.R8G8B8;
				case PixelFormat.Format32bppArgb	:	return TextureFormat.A8R8G8B8;
				case PixelFormat.Format32bppRgb		:	return TextureFormat.R8G8B8X8;
			}

			Output.WriteLineCall( Output.RenderingWarning, "No mapping available between pixel format \"{0}\" and TextureFormat - defaulting to R8G8B8", pixFormat.ToString( ) );
			return TextureFormat.R8G8B8;
		}

		/// <summary>
		/// Converts a TextureFormat value into a PixelFormat value
		/// </summary>
		private PixelFormat	TextureFormatToPixelFormat( TextureFormat texFormat )
		{
			switch ( texFormat )
			{
				case TextureFormat.Depth16			:	break;	//	No mapping
				case TextureFormat.Depth24			:	break;	//	No mapping
				case TextureFormat.Depth32			:	break;	//	No mapping

				case TextureFormat.R8G8B8			:	return PixelFormat.Format24bppRgb;
				case TextureFormat.B8G8R8			:	break;	//	No mapping

				case TextureFormat.R8G8B8X8			:	return PixelFormat.Format32bppRgb;
				case TextureFormat.B8G8R8X8			:	break;	//	No mapping

				case TextureFormat.R8G8B8A8			:	break;	//	No mapping
				case TextureFormat.B8G8R8A8			:	break;	//	No mapping

				case TextureFormat.A8R8G8B8			:	return PixelFormat.Format32bppArgb;
				case TextureFormat.A8B8G8R8			:	break;	//	No mapping
			}

			return PixelFormat.Undefined;
		}

		/// <summary>
		/// Checks if a texture format is valid
		/// </summary>
		/// <param name="format">Format to check</param>
		/// <param name="glInternalFormat">Output GL internal texture format</param>
		/// <param name="glFormat">Output GL texture format</param>
		/// <param name="glType">OUtput GL texel type</param>
		/// <returns>Returns either format, if it was directly supported by OpenGL, or a reasonable alternative</returns>
		private TextureFormat CheckTextureFormat( TextureFormat format, out int glInternalFormat, out int glFormat, out int glType )
		{
			//	Handle direct mappings to GL texture image formats
			switch ( format )
			{
				case TextureFormat.Depth16			:
				{
					glInternalFormat	= Gl.GL_DEPTH_COMPONENT16;
					glFormat			= Gl.GL_DEPTH_COMPONENT;
					glType				= Gl.GL_FLOAT;	//	TODO: Is this correct?
					return format;
				}
				case TextureFormat.Depth24			:
				{
					glInternalFormat	= Gl.GL_DEPTH_COMPONENT24;
					glFormat			= Gl.GL_DEPTH_COMPONENT;
					glType				= Gl.GL_UNSIGNED_INT;	//	TODO: Is this correct?
					return format;
				}
				case TextureFormat.Depth32			:
				{
					glInternalFormat	= Gl.GL_DEPTH_COMPONENT32;
					glFormat			= Gl.GL_DEPTH_COMPONENT;
					glType				= Gl.GL_UNSIGNED_INT;	//	TODO: Is this correct?
					return format;
				}
				case TextureFormat.R8G8B8			:
				{
					glFormat			= Gl.GL_BGR_EXT;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case TextureFormat.B8G8R8			:
				{
					glFormat			= Gl.GL_RGB;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case TextureFormat.R8G8B8X8			:
				{
					//	TODO: Not right...
					glFormat			= Gl.GL_BGR_EXT;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case TextureFormat.B8G8R8X8			:
				{
					//	TODO: Not right...
					glFormat			= Gl.GL_RGB;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case TextureFormat.R8G8B8A8			:
				{
					glFormat			= Gl.GL_ABGR_EXT;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case TextureFormat.A8R8G8B8			:
				{
					glFormat			= Gl.GL_BGRA_EXT;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case TextureFormat.A8B8G8R8			:
				{
					glFormat			= Gl.GL_RGBA;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
			}

			glFormat			= Gl.GL_BGR_EXT;
			glInternalFormat	= Gl.GL_RGB;
			glType				= Gl.GL_UNSIGNED_BYTE;

			return TextureFormat.R8G8B8;
		}

		/// <summary>
		/// Checks if a pixel format is valid
		/// </summary>
		/// <param name="format">Format to check</param>
		/// <param name="glInternalFormat">Output GL internal texture format</param>
		/// <param name="glFormat">Output GL texture format</param>
		/// <param name="glType">Output GL texel type</param>
		/// <returns>Returns either format, if it was directly supported by OpenGL, or a reasonable alternative</returns>
		private PixelFormat CheckPixelFormat( PixelFormat format, out int glInternalFormat, out int glFormat, out int glType )
		{
			//	Handle direct mappings to GL texture image formats
			switch ( format )
			{
				case PixelFormat.Alpha					:	break;
				case PixelFormat.Canonical				:	break;
				case PixelFormat.DontCare				:	break;
				case PixelFormat.Extended				:	break;
				case PixelFormat.Format16bppArgb1555	:	break;
				case PixelFormat.Format16bppGrayScale	:	break;
				case PixelFormat.Format16bppRgb555 		:	break;
				case PixelFormat.Format16bppRgb565 		:	break;
				case PixelFormat.Format1bppIndexed 		:	break;
				case PixelFormat.Format24bppRgb			:
				{
					glFormat			= Gl.GL_BGR_EXT;
					glInternalFormat	= Gl.GL_RGB;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case PixelFormat.Format32bppArgb		:
				{
					glInternalFormat	= Gl.GL_RGBA;
					glFormat			= Gl.GL_BGRA_EXT;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case PixelFormat.Format32bppPArgb		:	break;
				case PixelFormat.Format32bppRgb			:
				{
					glInternalFormat	= Gl.GL_RGBA;
					glFormat			= Gl.GL_BGRA_EXT;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case PixelFormat.Format48bppRgb			:	break;
				case PixelFormat.Format4bppIndexed		:	break;
				case PixelFormat.Format64bppArgb		:	break;
				case PixelFormat.Format64bppPArgb		:	break;
				case PixelFormat.Format8bppIndexed		:	break;
				case PixelFormat.Gdi					:	break;
				case PixelFormat.Indexed				:	break;
				case PixelFormat.Max					:	break;
				case PixelFormat.PAlpha					:	break;
			}

			glInternalFormat	= Gl.GL_RGB;
			glFormat			= Gl.GL_BGR_EXT;
			glType				= Gl.GL_UNSIGNED_BYTE;

			return PixelFormat.Format24bppRgb;
		}

		/// <summary>
		/// Checks the format of a Bitmap. If it's not compatible with any OpenGL formats, it's converted to a format that is
		/// </summary>
		private Bitmap CheckBmpFormat( Bitmap bmp, out int glInternalFormat, out int glFormat, out int glType )
		{
			PixelFormat format = CheckPixelFormat( bmp.PixelFormat, out glInternalFormat, out glFormat, out glType );
			if ( format == bmp.PixelFormat )
			{
				//	No format change required in the bitmap
				return bmp;
			}

			//	Unhandled format. Convert the bitmap into a more manageable form whose GL format we know
			return bmp.Clone( new System.Drawing.Rectangle( 0, 0, bmp.Width, bmp.Height ), format );
		}

		/// <summary>
		/// Converts this texture to an image
		/// </summary>
		public unsafe override System.Drawing.Image ToImage( )
		{
			if ( m_TextureHandle == -1 )
			{
				Output.WriteLineCall( Output.RenderingWarning, "COuld not convert texture to image - handle was invalid" );
				return null;
			}

			//	Enable 2D texturing
			bool requires2DTexturesEnabled = Gl.glIsEnabled( Gl.GL_TEXTURE_2D ) == 0;
			if ( requires2DTexturesEnabled )
			{
				Gl.glEnable( Gl.GL_TEXTURE_2D );
			}

			//	Bind the texture
			Renderer.Inst.BindTexture( 0, this );

			//	HACK: Makes lots of assumptions about format...

			System.Drawing.Bitmap bmp;
			if ( ( Format == TextureFormat.Depth16 ) || ( Format == TextureFormat.Depth24 ) || ( Format == TextureFormat.Depth32 ) )
			{
				//	Handle depth textures

				//	Get texture memory
				float[] textureMemory = new float[ Width * Height ];
				Gl.glGetTexImage( Gl.GL_TEXTURE_2D, 0, m_GlFormat, Gl.GL_FLOAT, textureMemory );

				//	Create bitmap
				bmp = new System.Drawing.Bitmap( Width, Height, PixelFormat.Format24bppRgb );

				//	Create image memory
				int texIndex = 0;
				for ( int h = 0; h < Height; ++h )
				{
					for ( int w = 0; w < Width; ++w, ++texIndex )
					{
						float depth = ( ( float )textureMemory[ texIndex ] );
					//	byte grey = ( byte )( System.Math.Exp( depth ) * 255.0f);
						byte grey = ( byte )( depth * 255.0f );
						bmp.SetPixel( w, h, System.Drawing.Color.FromArgb( grey, grey, grey ) );
					}
				}

				//	NOTE: Was using this code, but it was crashing occasionally when using the bitmap, making me think that the
				//	garbage collector was cleaning up imageMemory while is was being used, and the System.Drawing.Bitmap is
				//	expecting the memory given to it to be permanent (maybe...)

				/*
			//	byte[] imageMemory = new byte[ Width * Height * 3 ];
			//	int imageIndex = 0;
				for ( int texIndex = 0; texIndex < textureMemory.Length; ++texIndex )
				{
					float depth = ( ( float )textureMemory[ texIndex ] );
					byte grey = ( byte )( System.Math.Exp( depth ) * 255.0f);
					imageMemory[ imageIndex++ ] = grey;
					imageMemory[ imageIndex++ ] = grey;
					imageMemory[ imageIndex++ ] = grey;
				}

				//	Create a Bitmap object from the image memory
				fixed ( byte* imageMemoryPtr = imageMemory )
				{
					bmp = new System.Drawing.Bitmap( Width, Height, Width * 3, PixelFormat.Format24bppRgb, ( IntPtr )imageMemoryPtr );
				}
				*/
			}
			else
			{
				//	Handle colour textures

				//	Get texture memory
				int bytesPerPixel = GetTextureFormatSize( Format ) / 8;
				byte[] textureMemory = new byte[ Width * Height * bytesPerPixel ];
				Gl.glGetTexImage( Gl.GL_TEXTURE_2D, 0, m_GlFormat, m_GlType, textureMemory );

				//	TODO: Same problem as above...

				//	Create a Bitmap object from image memory
				fixed ( byte* textureMemoryPtr = textureMemory )
				{
					//	TODO: Add per-case check of Format - in cases with no mapping (see TextureFormatToPixelFormat()), do a manual conversion
					bmp = new System.Drawing.Bitmap( Width, Height, Width * bytesPerPixel, TextureFormatToPixelFormat( Format ), ( IntPtr )textureMemoryPtr );
				}
			}

			//	Unbind the texture
			Renderer.Inst.UnbindTexture( 0 );

			//	Disable 2D texturing
			if ( requires2DTexturesEnabled )
			{
				Gl.glDisable( Gl.GL_TEXTURE_2D );
			}

			return bmp;
		}


		#region IDisposable Members

		/// <summary>
		/// Deletes the associated texture handle
		/// </summary>
		public void Dispose( )
		{
			if ( m_TextureHandle != -1 )
			{
				//	TODO: ... can't delete, because the GL context has disappeared
			//	Gl.glDeleteTextures( 1, ref m_TextureHandle );
				m_TextureHandle = -1;
			}
		}

		#endregion

		private int	m_TextureHandle		= -1;
		private int m_InternalGlFormat;
		private int m_GlFormat;
		private int m_GlType;
	}
}