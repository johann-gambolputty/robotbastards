using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using Rb.Core.Maths;
using Rb.Rendering.Textures;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;
using Rectangle=System.Drawing.Rectangle;

namespace Rb.Rendering.OpenGl
{

	/// <summary>
	/// OpenGL implementation of Texture2d
	/// </summary>
	[Serializable]
	public class OpenGlTexture2d : Texture2dBase, IOpenGlTexture
	{
		/// <summary>
		/// Returns the internal texture handle
		/// </summary>
		public int TextureHandle
		{
			get { return m_TextureHandle; }
		}

		/// <summary>
		/// Gets the GL texture target (always GL_TEXTURE_2D - legacy cube map design)
		/// </summary>
		public int Target
		{
			get { return m_Target;  }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public OpenGlTexture2d( )
		{	
		}

		/// <summary>
		/// Serialization constructor
		/// </summary>
		public OpenGlTexture2d( SerializationInfo info, StreamingContext context ) :
			base( info, context )
		{
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
		public unsafe override void Create( int width, int height, TextureFormat format )
		{
			DestroyCurrent( );

			//	Get the GL format of the bitmap, possibly converting it in the process
			m_Format	= CheckTextureFormat( format, out m_InternalGlFormat, out m_GlFormat, out m_GlType );
			m_Width		= width;
			m_Height	= height;
			m_MipMapped = false;
			m_Target	= Gl.GL_TEXTURE_2D;

			//	Generate a texture name
			fixed ( int* handleMem = &m_TextureHandle )
			{
				Gl.glGenTextures( 1, ( IntPtr )handleMem );
			}
			Gl.glBindTexture( m_Target, m_TextureHandle );

			byte[] nullArray = null;
			Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, m_InternalGlFormat, width, height, 0, m_GlFormat, m_GlType, nullArray );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public override void Load( Bitmap bmp, bool generateMipMaps )
		{
			//	Get the GL format of the bitmap, possibly converting it in the process
			int glInternalFormat;
			int glFormat;
			int glType;
			bmp = CheckBmpFormat( bmp, out glInternalFormat, out glFormat, out glType );

			//	Lock the bitmap, and create the texture
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, bmp.PixelFormat );

			//	Argh... we don't quite handle bitmaps with loony strides
			int expectedStride = bmp.Width * ( Image.GetPixelFormatSize( bmpData.PixelFormat ) / 8 );
			if ( bmpData.Stride !=  expectedStride )
			{
				throw new ArgumentException( string.Format( "Unexpected stride in bitmap (was {0}, expected {1})", bmpData.Stride, expectedStride ) );
			}

			int target = Gl.GL_TEXTURE_2D;
			int imageTarget = Gl.GL_TEXTURE_2D;
			Create( target, imageTarget, bmpData.Width, bmpData.Stride, bmpData.Height, glInternalFormat, glFormat, glType, generateMipMaps, bmpData.Scan0 );
			bmp.UnlockBits( bmpData );

			m_Format = PixelFormatToTextureFormat( bmp.PixelFormat );
		}

		public static TextureFormat CreateTextureImageFromBitmap( Bitmap bmp, bool generateMipMaps, int imageTarget )
		{
			//	Get the GL format of the bitmap, possibly converting it in the process
			int glInternalFormat;
			int glFormat;
			int glType;
			bmp = CheckBmpFormat( bmp, out glInternalFormat, out glFormat, out glType );

			//	Lock the bitmap, and create the texture
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, bmp.PixelFormat );

			//	Argh... we don't quite handle bitmaps with loony strides
			int expectedStride = bmp.Width * ( Image.GetPixelFormatSize( bmpData.PixelFormat ) / 8 );
			if ( bmpData.Stride != expectedStride )
			{
				throw new ArgumentException( string.Format( "Unexpected stride in bitmap (was {0}, expected {1})", bmpData.Stride, expectedStride ) );
			}

			if ( generateMipMaps )
			{
				Glu.gluBuild2DMipmaps( imageTarget, glInternalFormat, bmpData.Width, bmpData.Height, glFormat, glType, bmpData.Scan0 );
			}
			else
			{
				Gl.glTexImage2D( imageTarget, 0, glInternalFormat, bmpData.Width, bmpData.Height, 0, glFormat, glType, bmpData.Scan0 );
			}
			bmp.UnlockBits( bmpData );

			return PixelFormatToTextureFormat( bmp.PixelFormat );
		}

		/// <summary>
		/// Destroys the current texture
		/// </summary>
		private void DestroyCurrent( )
		{
			if ( m_TextureHandle != OpenGlTextureHandle.InvalidHandle )
			{
				( ( OpenGlRenderer )Graphics.Renderer ).DisposeRenderingResource( new OpenGlTextureHandle( m_TextureHandle ) );
				m_TextureHandle = OpenGlTextureHandle.InvalidHandle;
			}
		}

		private static unsafe int CreateStatic( int target, int imageTarget, int width, int stride, int height, int glInternalFormat, int glFormat, int glType, bool generateMipMaps, IntPtr bytes )
		{
			//	Generate a texture name
			int[] handles = new int[ 1 ];
			Gl.glGenTextures( 1, handles );
			int textureHandle = handles[ 0 ];
			Gl.glBindTexture( target, textureHandle );

			try
			{
				if ( generateMipMaps )
				{
					Glu.gluBuild2DMipmaps( imageTarget, glInternalFormat, width, height, glFormat, glType, bytes );
				}
				else
				{
					Gl.glTexImage2D( imageTarget, 0, glInternalFormat, width, height, 0, glFormat, glType, bytes );
				}
			}
			catch ( AccessViolationException )
			{
				GraphicsLog.Warning( "Access violation occurred during texture creation (was image resource used as input?) - attempting managed buffer transfer" );

				int length = stride * height;
				byte[] init = new byte[ length ];

				//	Oh dear oh dear...
				byte* bmpAddress = ( byte* )bytes.ToPointer( );
				for ( int index = 0; index < length; ++index )
				{
					init[ index ] = bmpAddress[ index ];
				}

				Gl.glTexImage2D( imageTarget, 0, glInternalFormat, width, height, 0, glFormat, glType, init );
			}

			return textureHandle;
		}

		/// <summary>
		/// Creates the texture
		/// </summary>
		private unsafe void Create( int target, int imageTarget, int width, int stride, int height, int glInternalFormat, int glFormat, int glType, bool generateMipMaps, IntPtr bytes )
		{
			DestroyCurrent( );

			m_TextureHandle = CreateStatic( target, imageTarget, width, stride, height, glInternalFormat, glFormat, glType, generateMipMaps, bytes );

			m_InternalGlFormat	= glInternalFormat;
			m_GlFormat			= glFormat;
			m_GlType			= glType;
			m_Width				= width;
			m_Height			= height;
			m_MipMapped			= generateMipMaps;
			m_Target			= target;
		}

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		public override void Bind( int unit )
		{
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB + unit );
			Gl.glBindTexture( m_Target, TextureHandle );
		}
		
		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public override void Unbind( int unit )
		{
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB + unit );
			Gl.glBindTexture( m_Target, 0 );
		}

		/// <summary>
		/// Converts a PixelFormat value into a TextureFormat
		/// </summary>
		private static TextureFormat PixelFormatToTextureFormat( PixelFormat pixFormat )
		{
			switch ( pixFormat )
			{
				case PixelFormat.Format24bppRgb		:	return TextureFormat.R8G8B8;
				case PixelFormat.Format32bppArgb	:	return TextureFormat.A8R8G8B8;
				case PixelFormat.Format32bppRgb		:	return TextureFormat.R8G8B8X8;
			}

			GraphicsLog.Warning( "No mapping available between pixel format \"{0}\" and TextureFormat - defaulting to R8G8B8", pixFormat.ToString( ) );
			return TextureFormat.R8G8B8;
		}

		/// <summary>
		/// Converts a TextureFormat value into a PixelFormat value
		/// </summary>
		public static PixelFormat TextureFormatToPixelFormat( TextureFormat texFormat )
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

				case TextureFormat.R8G8B8A8			:	return PixelFormat.Format32bppRgb;
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
		public static TextureFormat CheckTextureFormat( TextureFormat format, out int glInternalFormat, out int glFormat, out int glType )
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
					glInternalFormat	= Gl.GL_RGBA;
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
		private static PixelFormat CheckPixelFormat( PixelFormat format, out int glInternalFormat, out int glFormat, out int glType )
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
					glFormat			= Gl.GL_ABGR_EXT;	//	NOTE: AP: Think this is correct...
					glInternalFormat	= Gl.GL_RGBA;
					glType				= Gl.GL_UNSIGNED_BYTE;
					return format;
				}
				case PixelFormat.Format32bppPArgb		:	break;
				case PixelFormat.Format32bppRgb			:
				{
					glFormat			= Gl.GL_ABGR_EXT;
					glInternalFormat	= Gl.GL_RGBA;
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
		public static Bitmap CheckBmpFormat( Bitmap bmp, out int glInternalFormat, out int glFormat, out int glType )
		{
			PixelFormat format = CheckPixelFormat( bmp.PixelFormat, out glInternalFormat, out glFormat, out glType );
			if ( format == bmp.PixelFormat )
			{
				//	No format change required in the bitmap
				return bmp;
			}

			//	Unhandled format. Convert the bitmap into a more manageable form whose GL format we know
			return bmp.Clone( new Rectangle( 0, 0, bmp.Width, bmp.Height ), format );
		}

		/// <summary>
		/// Converts this texture to an image
		/// </summary>
		public unsafe override Bitmap ToBitmap( )
		{
			if ( m_TextureHandle == OpenGlTextureHandle.InvalidHandle )
			{
				GraphicsLog.Warning( "COuld not convert texture to image - handle was invalid" );
				return null;
			}

			//	Enable 2D texturing
			bool requires2DTexturesEnabled = Gl.glIsEnabled( Gl.GL_TEXTURE_2D ) == 0;
			if ( requires2DTexturesEnabled )
			{
				Gl.glEnable( Gl.GL_TEXTURE_2D );
			}

			//	Bind the texture
            Graphics.Renderer.UnbindAllTextures( );
			Graphics.Renderer.BindTexture( this );

			//	HACK: Makes lots of assumptions about format...

			Bitmap bmp;
			if ( ( Format == TextureFormat.Depth16 ) || ( Format == TextureFormat.Depth24 ) || ( Format == TextureFormat.Depth32 ) )
			{
				//	Handle depth textures

				//	Get texture memory
				float[] textureMemory = new float[ Width * Height ];
				Gl.glGetTexImage( Gl.GL_TEXTURE_2D, 0, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, textureMemory );

				float minDepth = float.MaxValue;
				float maxDepth = float.MinValue;
				for ( int textureIndex = 0; textureIndex < textureMemory.Length; ++textureIndex )
				{
					minDepth = Utils.Min( textureMemory[ textureIndex ], minDepth );
					maxDepth = Utils.Max( textureMemory[ textureIndex ], maxDepth );
				}

				bmp = new Bitmap( Width, Height, PixelFormat.Format24bppRgb );
				BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, Width, Height ), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb );

				byte* bmpMem = ( byte* )bmpData.Scan0;
				{
					int texOffset = 0;
					for ( int y = Height - 1; y >= 0; --y )
					{
						byte* curBmpPixel = bmpMem + ( y * bmpData.Stride );
						for ( int x = 0; x < Width; ++x )
						{
							float depth = textureMemory[ texOffset++ ];
							depth = ( depth - minDepth ) / ( maxDepth - minDepth );
							byte grey = ( byte )( depth * 255.0f );
							*( curBmpPixel++ ) = grey;
							*( curBmpPixel++ ) = grey;
							*( curBmpPixel++ ) = grey;
						}
					}
				}
			}
			else
			{
				//	Handle colour textures

				//	Get texture memory
				int bytesPerPixel = TextureFormatInfo.GetBitSize( Format ) / 8;
				byte[] textureMemory = new byte[ Width * Height * bytesPerPixel ];
				Gl.glGetTexImage( Gl.GL_TEXTURE_2D, 0, m_GlFormat, m_GlType, textureMemory );

				//	TODO: Same problem as above...

				//	Create a Bitmap object from image memory
				fixed ( byte* textureMemoryPtr = textureMemory )
				{
					//	TODO: Add per-case check of Format - in cases with no mapping (see TextureFormatToPixelFormat()), do a manual conversion
					bmp = new Bitmap( Width, Height, Width * bytesPerPixel, TextureFormatToPixelFormat( Format ), ( IntPtr )textureMemoryPtr );
				}
			}

			//	Unbind the texture
			Graphics.Renderer.UnbindTexture( this );

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
		public override void Dispose( )
		{
			DestroyCurrent( );
		}

		#endregion

		#region Private Members

		private int m_TextureHandle = OpenGlTextureHandle.InvalidHandle;
		private int	m_InternalGlFormat;
		private int	m_GlFormat;
		private int	m_GlType;
		private int m_Target;

		#endregion
	}
}
