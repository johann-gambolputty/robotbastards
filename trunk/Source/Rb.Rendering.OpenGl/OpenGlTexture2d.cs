using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using Rb.Rendering.Textures;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{

	/// <summary>
	/// OpenGL implementation of Texture2d
	/// </summary>
	/// <remarks>
	/// See http://www.flipcode.com/archives/internaltable.html for notes on OpenGL internal texture formats
	/// </remarks>
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
		/// Gets the GL texture target
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
		/// Creates this texture from a bitmap
		/// </summary>
		public override unsafe void Create( Bitmap bmp, bool generateMipMaps )
		{
			DestroyCurrent( );

			m_TextureHandle = OpenGlTextureHandle.CreateAndBindHandle( m_Target );
			OpenGlTexture2dBuilder.TextureInfo info = OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( m_Target, bmp, generateMipMaps );
			m_Format = info.TextureFormat;
			UpdateDimensions( );
		}

		/// <summary>
		/// Creates the texture from an array of bitmaps
		/// </summary>
		/// <param name="bitmaps">Source bitmap data</param>
		public override void Create( Bitmap[] bitmaps )
		{
			DestroyCurrent( );
			m_TextureHandle = OpenGlTextureHandle.CreateAndBindHandle( m_Target );

			OpenGlTexture2dBuilder.TextureInfo info = OpenGlTexture2dBuilder.CreateTextureImageFromBitmap( m_Target, bitmaps );
			m_Format = info.TextureFormat;
			UpdateDimensions( );
		}

		/// <summary>
		/// Creates the texture from a texture data model
		/// </summary>
		/// <param name="data">Texture data</param>
		/// <param name="generateMipMaps">Generate mipmaps flag</param>
		public unsafe override void Create( Texture2dData data, bool generateMipMaps )
		{
			DestroyCurrent( );

			m_TextureHandle = OpenGlTextureHandle.CreateAndBindHandle( m_Target );
			OpenGlTexture2dBuilder.TextureInfo info = OpenGlTexture2dBuilder.CreateTextureImageFromTextureData( m_Target, data, generateMipMaps );
			m_Format = info.TextureFormat;
			UpdateDimensions( );
		}

		/// <summary>
		/// Creates the texture from a texture data model
		/// </summary>
		/// <param name="data">Texture data</param>
		public unsafe override void Create( Texture2dData[] data )
		{
			DestroyCurrent( );

			m_TextureHandle = OpenGlTextureHandle.CreateAndBindHandle( m_Target );
			m_Format = OpenGlTexture2dBuilder.CreateTextureImageFromTextureData( m_Target, data ).TextureFormat;
			UpdateDimensions( );
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

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		public override void Bind( int unit )
		{
			Gl.glActiveTexture( Gl.GL_TEXTURE0 + unit );
			Gl.glBindTexture( m_Target, TextureHandle );
		}
		
		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public override void Unbind( int unit )
		{
			Gl.glActiveTexture( Gl.GL_TEXTURE0 + unit );
			Gl.glBindTexture( m_Target, 0 );
		}

		/// <summary>
		/// Gets texture data from this texture
		/// </summary>
		/// <param name="getMipMaps">If true, texture data for all mipmap levels are retrieved</param>
		/// <returns>
		/// Returns texture data extracted from this texture. If getMipMaps is false, only one <see cref="Texture2dData"/>
		/// object is returned. Otherwise, the array contains a <see cref="Texture2dData"/> object for each mipmap
		/// level.
		/// </returns>
		public override Texture2dData[] ToTextureData( bool getMipMaps )
		{
			if ( m_TextureHandle == OpenGlTextureHandle.InvalidHandle )
			{
				GraphicsLog.Error( "Could not convert texture to image - handle was invalid" );
				return new Texture2dData[ 0 ];
			}

			bool disableTexturing = BindThisOnly( );
			try
			{
				return OpenGlTexture2dBuilder.CreateTextureDataFromTexture( m_Target, m_Format );
			}
			finally
			{
				UnbindThisOnly( disableTexturing );
			}
		}


		/// <summary>
		/// Converts this texture to an image
		/// </summary>
		public unsafe override Bitmap[] ToBitmap( bool getMipMaps )
		{
			if ( m_TextureHandle == OpenGlTextureHandle.InvalidHandle )
			{
				GraphicsLog.Warning( "Could not convert texture to image - handle was invalid" );
				return new Bitmap[ 0 ];
			}

			bool disableTexturing = BindThisOnly( );
			List< Bitmap > result = new List< Bitmap >( );
			try
			{
				if ( !getMipMaps )
				{
					result.Add( ToBitmap( 0 ) );
				}
				else
				{
					int level = 0;
					Bitmap bmp;
					do
					{
						bmp = ToBitmap( level++ );
						result.Add( bmp );
					} while ( ( bmp.Width > 1 ) && ( bmp.Height > 1 ) );
				}
			}
			finally
			{
				UnbindThisOnly( disableTexturing );
			}

			return result.ToArray( );

			/*
			try
			{
				//	HACK: Makes lots of assumptions about format...

			}
			finally
			{
				//	Unbind the texture
				Graphics.Renderer.UnbindTexture( this );
				Graphics.Renderer.PopTextures( );
			}

			//	Disable 2D texturing
			if ( requires2DTexturesEnabled )
			{
				Gl.glDisable( Gl.GL_TEXTURE_2D );
			}

			return bmp;
			*/
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
		private int m_Target = Gl.GL_TEXTURE_2D;

		/// <summary>
		/// Converts this texture to an image
		/// </summary>
		private unsafe Bitmap ToBitmap( int level )
		{
			return OpenGlTexture2dBuilder.CreateBitmapFromTexture( m_Target, level, m_Format );
		}

		/// <summary>
		/// Unbinds all current textures, then binds this texture.
		/// </summary>
		private bool BindThisOnly( )
		{
			Graphics.Renderer.PushTextures( );
			Graphics.Renderer.BindTexture( this );

			bool requiresTexturesEnabled = Gl.glIsEnabled( m_Target ) == 0;
			if ( requiresTexturesEnabled )
			{
				Gl.glEnable( m_Target );
			}
			return requiresTexturesEnabled;
		}

		/// <summary>
		/// Restores texture state 
		/// </summary>
		private void UnbindThisOnly( bool disable2dTextures )
		{
			Graphics.Renderer.PopTextures( );
			if ( disable2dTextures )
			{
				Gl.glDisable( m_Target );
			}
		}

		/// <summary>
		/// Updates the internal dimension fields to those of the current texture (texture must be bound)
		/// </summary>
		private void UpdateDimensions( )
		{
			m_Width = OpenGlTexture2dBuilder.GetTextureLevelParameterInt32( m_Target, 0, Gl.GL_TEXTURE_WIDTH );
			m_Height = OpenGlTexture2dBuilder.GetTextureLevelParameterInt32( m_Target, 0, Gl.GL_TEXTURE_HEIGHT );
		}

		#endregion
	}
}
