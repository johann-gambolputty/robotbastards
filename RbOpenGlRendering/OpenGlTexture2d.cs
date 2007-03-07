using System;
using System.Drawing;
using System.Drawing.Imaging;
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
		public override void Create( int width, int height, PixelFormat format )
		{
			if ( m_TextureHandle != -1 )
			{
				Gl.glDeleteTextures( 1, ref m_TextureHandle );
			}

			//	Get the GL format of the bitmap, possibly converting it in the process
			m_Format	= CheckPixelFormat( format, out m_InternalGlFormat, out m_GlFormat, out m_GlType );
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

			m_Format = bmp.PixelFormat;
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
		/// Checks if a pixel format is valid
		/// </summary>
		/// <param name="format">Format to check</param>
		/// <param name="glInternalFormat">Output GL internal texture format</param>
		/// <param name="glFormat">Output GL texture format</param>
		/// <param name="glType">OUtput GL texel type</param>
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
				return null;
			}

			//	Enable 2D texturing
			bool requires2DTexturesEnabled = Gl.glIsEnabled( Gl.GL_TEXTURE_2D ) == 0;
			if ( requires2DTexturesEnabled )
			{
				Gl.glEnable( Gl.GL_TEXTURE_2D );
			}

			//	Bind the texture
			Begin( );

			//	HACK: Makes lots of assumptions about format...
			int bytesPerPixel = System.Drawing.Image.GetPixelFormatSize( Format ) / 8;
			byte[] textureMemory = new byte[ Width * Height * bytesPerPixel ];

			Gl.glGetTexImage( Gl.GL_TEXTURE_2D, 0, m_GlFormat, m_GlType, textureMemory );

			System.Drawing.Bitmap bmp;
			fixed ( byte* textureMemoryPtr = textureMemory )
			{
				bmp = new System.Drawing.Bitmap( Width, Height, Width * bytesPerPixel, Format, ( IntPtr )textureMemoryPtr );
			}

			//	Unbind the texture
			End( );

			//	Disable 2D texturing
			if ( requires2DTexturesEnabled )
			{
				Gl.glDisable( Gl.GL_TEXTURE_2D );
			}

			return bmp;
		}


		/// <summary>
		/// Applies this texture
		/// </summary>
		public override void Begin( )
		{
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, TextureHandle );
		}

		/// <summary>
		/// Stops applying this texture
		/// </summary>
		public override void End( )
		{
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
