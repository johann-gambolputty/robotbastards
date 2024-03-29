using System;
using System.Collections.Generic;
using System.Drawing;
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

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Destroys this object
		/// </summary>
		public void Dispose( )
		{
			if ( m_Handle != OpenGlTextureHandle.InvalidHandle )
			{
				( ( OpenGlRenderer )Graphics.Renderer ).DisposeRenderingResource( new OpenGlTextureHandle( m_Handle ) );
				m_Handle = OpenGlTextureHandle.InvalidHandle;
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
		public void Build( Bitmap posX, Bitmap negX, Bitmap posY, Bitmap negY, Bitmap posZ, Bitmap negZ, bool generateMipMaps )
		{
			if ( m_Handle == -1 )
			{
				int[] textureHandles = new int[ 1 ];
				Gl.glGenTextures( 1, textureHandles );
				m_Handle = textureHandles[ 0 ];
			}

			Gl.glEnable( Gl.GL_TEXTURE_CUBE_MAP );
			Gl.glBindTexture( Gl.GL_TEXTURE_CUBE_MAP, m_Handle );

			Gl.glTexParameteri( Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE );
			Gl.glTexParameteri( Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE );
			Gl.glTexParameteri( Gl.GL_TEXTURE_CUBE_MAP, Gl.GL_TEXTURE_WRAP_R, Gl.GL_CLAMP_TO_EDGE );

			OpenGlTexture2dBuilder.TextureInfo info = OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_X, posX, generateMipMaps );
			OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_X, negX, generateMipMaps );
			OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, posY, generateMipMaps );
			OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Y, negY, generateMipMaps );
			OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, posZ, generateMipMaps );
			OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Z, negZ, generateMipMaps );

			m_Format = info.TextureFormat;

			Gl.glDisable( Gl.GL_TEXTURE_CUBE_MAP );
		}

		/// <summary>
		/// Converts a given cube map face to a bitmap
		/// </summary>
		public Bitmap ToBitmap( CubeMapFace face )
		{
			if ( m_Handle == -1 )
			{
				GraphicsLog.Warning( "COuld not convert cube map texture to images - handle was invalid" );
				return null;
			}

			Gl.glEnable( Gl.GL_TEXTURE_CUBE_MAP );
			Gl.glBindTexture( Gl.GL_TEXTURE_CUBE_MAP, m_Handle );

			Bitmap result = GetFaceBitmap( GetGlCubeFaceEnum( face ) );

			Gl.glDisable( Gl.GL_TEXTURE_CUBE_MAP );

			return result;
		}

		/// <summary>
		/// Converts a value from <see cref="CubeMapFace"/> to its associated GL_TEXTURE_CUBE_MAP_... enum value
		/// </summary>
		public static int GetGlCubeFaceEnum( CubeMapFace face )
		{
			switch ( face )
			{
				case CubeMapFace.NegativeX : return Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_X;
				case CubeMapFace.PositiveX : return Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_X;
				case CubeMapFace.NegativeY : return Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y;
				case CubeMapFace.PositiveY : return Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Y;
				case CubeMapFace.NegativeZ : return Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z;
				case CubeMapFace.PositiveZ : return Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Z;
			}
			throw new NotImplementedException( "Unsupported cube face enum value " + face );
			
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

		#region Private Members

		private int m_Handle = -1;

		/// <summary>
		/// Gets a bitmap from a particular cube map face
		/// </summary>
		private unsafe Bitmap GetFaceBitmap( int imageTarget )
		{
			return OpenGlTexture2dBuilder.CreateBitmapFromTexture( imageTarget, 0, m_Format );
		}

		#endregion
	}
}
