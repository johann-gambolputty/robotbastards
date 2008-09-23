using System;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	class OpenGlTexture3d : ITexture3d, IOpenGlTexture
	{

		~OpenGlTexture3d( )
		{
			Dispose( );
		}
		
		/// <summary>
		/// Destroys the current texture
		/// </summary>
		private void DestroyCurrent( )
		{
			if ( m_Handle != OpenGlTextureHandle.InvalidHandle )
			{
				( ( OpenGlRenderer )Graphics.Renderer ).DisposeRenderingResource( new OpenGlTextureHandle( m_Handle ) );
				m_Handle = OpenGlTextureHandle.InvalidHandle;
			}
		}

		#region ITexture3d Members

		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		public int Width
		{
			get { return m_Width; }
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		public int Height
		{
			get { return m_Height; }
		}

		/// <summary>
		/// Gets the depth of the texture
		/// </summary>
		public int Depth
		{
			get { return m_Depth; }
		}

		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="depth">Depth of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		public void Create( int width, int height, int depth, TextureFormat format )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Creates this texture from a texture data object
		/// </summary>
		public unsafe void Create( Texture3dData data )
		{
			m_Width = data.Width;
			m_Height = data.Height;
			m_Depth = data.Depth;
			m_Format = data.Format;
			m_Handle = OpenGlTextureHandle.CreateHandle( );

			OpenGlTexture2dBuilder.TextureInfo info = OpenGlTexture2dBuilder.CheckTextureFormat( m_Format );

			int target = Gl.GL_TEXTURE_3D;
			int border = 0;

			Gl.glEnable( target );
			Gl.glBindTexture( target, m_Handle );
			fixed ( void* bytes = data.Bytes )
			{
				Gl.glTexImage3D( target, 0, info.GlInternalFormat, m_Width, m_Height, m_Depth, border, info.GlFormat, info.GlType, new IntPtr( bytes ) );
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
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB + unit );
			Gl.glBindTexture( Gl.GL_TEXTURE_3D, TextureHandle );
		}

		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public void Unbind( int unit )
		{
			Gl.glActiveTextureARB( Gl.GL_TEXTURE0_ARB + unit );
			Gl.glBindTexture( Gl.GL_TEXTURE_3D, 0 );
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Destroys this texture
		/// </summary>
		public void Dispose( )
		{
			DestroyCurrent( );
		}

		#endregion

		#region IOpenGlTexture Members

		public int TextureHandle
		{
			get { return m_Handle; }
		}

		#endregion

		#region Private Members

		private int m_Width;
		private int m_Height;
		private int m_Depth;
		private TextureFormat m_Format;
		private int m_Handle = OpenGlTextureHandle.InvalidHandle;

		#endregion

	}
}
