using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Implements ICubeMapTexture using OpenGL cube map extensions
	/// </summary>
	public class OpenGlCubeMapTexture : ICubeMapTexture, IOpenGlTexture
	{
		#region Private Members

		private TextureFormat m_Format = TextureFormat.Undefined;
		private int m_Width;
		private int m_Height;


		#endregion

		#region IDisposable Members

		/// <summary>
		/// Destroys this object
		/// </summary>
		public void Dispose( )
		{
			if ( m_Handle != -1 )
			{
				Gl.glDeleteTextures( 1, new int[] { m_Handle } );
				m_Handle = -1;
			}
		}

		#endregion

		#region ITexture Members

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		public TextureFormat Format
		{
			get { return m_Format; }
		}

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		public void Bind( int unit )
		{
			Gl.glEnable( Gl.GL_TEXTURE_CUBE_MAP );
			Gl.glBindTexture( Gl.GL_TEXTURE_CUBE_MAP, m_Handle );
		}

		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public void Unbind( int unit )
		{
			Gl.glDisable( Gl.GL_TEXTURE_CUBE_MAP );
		}

		#endregion

		private int m_Handle = -1;

		#region ICubeMapTexture Members

		/// <summary>
		/// Builds this cube map from 6 bitmaps
		/// </summary>
		/// <param name="posX">Positive X axis bitmap</param>
		/// <param name="negX">Negative X axis bitmap</param>
		/// <param name="posY">Positive Y axis bitmap</param>
		/// <param name="negY">Negative Y axis bitmap</param>
		/// <param name="posZ">Positive Z axis bitmap</param>
		/// <param name="negZ">Negative Z axis bitmap</param>
		/// <param name="generateMipMaps">If true, mipmaps are generated for the cube map faces</param>
		public unsafe void Build( Bitmap posX, Bitmap negX, Bitmap posY, Bitmap negY, Bitmap posZ, Bitmap negZ, bool generateMipMaps )
		{
			int[] textureHandles = new int[ 1 ];
			Gl.glGenTextures( 1, textureHandles );
			m_Handle = textureHandles[ 0 ];

			Gl.glEnable( Gl.GL_TEXTURE_CUBE_MAP );
			Gl.glBindTexture( Gl.GL_TEXTURE_CUBE_MAP, m_Handle );

			Gl.glTexParameteri( Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE );
			Gl.glTexParameteri( Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE );
			Gl.glTexParameteri( Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_R, Gl.GL_CLAMP_TO_EDGE );

			m_Width = posX.Width;
			m_Height = posX.Height;
			m_Format = OpenGlTexture2d.CreateTextureImageFromBitmap( posX, generateMipMaps, Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_X );
			OpenGlTexture2d.CreateTextureImageFromBitmap( negX, generateMipMaps, Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_X );
			OpenGlTexture2d.CreateTextureImageFromBitmap( posY, generateMipMaps, Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y );
			OpenGlTexture2d.CreateTextureImageFromBitmap( negY, generateMipMaps, Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Y );
			OpenGlTexture2d.CreateTextureImageFromBitmap( posZ, generateMipMaps, Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z );
			OpenGlTexture2d.CreateTextureImageFromBitmap( negZ, generateMipMaps, Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Z );

			Gl.glDisable( Gl.GL_TEXTURE_CUBE_MAP );
		}

		/// <summary>
		/// Renders this cubemap texture to a series of bitmaps
		/// </summary>
		public Bitmap[] ToBitmaps( )
		{
			if ( m_Handle == -1 )
			{
				GraphicsLog.Warning( "COuld not convert cube map texture to images - handle was invalid" );
				return null;
			}
			Gl.glEnable( Gl.GL_TEXTURE_CUBE_MAP );
			Gl.glBindTexture( Gl.GL_TEXTURE_CUBE_MAP, m_Handle );
			List< Bitmap > bitmaps = new List<Bitmap>( );

			bitmaps.Add( GetFaceBitmap( Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_X ) );
			bitmaps.Add( GetFaceBitmap( Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_X ) );
			bitmaps.Add( GetFaceBitmap( Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y ) );
			bitmaps.Add( GetFaceBitmap( Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Y ) );
			bitmaps.Add( GetFaceBitmap( Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z ) );
			bitmaps.Add( GetFaceBitmap( Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Z ) );

			Gl.glDisable( Gl.GL_TEXTURE_CUBE_MAP );

			return bitmaps.ToArray( );
		}

		private unsafe Bitmap GetFaceBitmap( int imageTarget )
		{
			//	Get texture memory
			int bytesPerPixel = TextureFormatInfo.GetBitSize( Format ) / 8;

			byte[] textureMemory = new byte[ m_Width * m_Height * bytesPerPixel ];

			int internalFormat;
			int format;
			int type;
			OpenGlTexture2d.CheckTextureFormat( m_Format, out internalFormat, out format, out type );

			Gl.glGetTexImage( imageTarget, 0, format, Gl.GL_UNSIGNED_BYTE, textureMemory );

			PixelFormat pixelFormat = OpenGlTexture2d.TextureFormatToPixelFormat( Format );
			Bitmap bmp = new Bitmap( m_Width, m_Height, pixelFormat );
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, m_Width, m_Height ), ImageLockMode.WriteOnly, pixelFormat );
			Marshal.Copy( textureMemory, 0, bmpData.Scan0, textureMemory.Length );
			bmp.UnlockBits( bmpData );

			return bmp;
		}

		#endregion

		#region IOpenGlTexture Members

		/// <summary>
		/// Gets the opengl handle for this texture
		/// </summary>
		public int TextureHandle
		{
			get { return m_Handle; }
		}

		#endregion
	}
}