using System;
using RbEngine;
using RbEngine.Rendering;
using Tao.OpenGl;


namespace RbOpenGlRendering
{
	/// <summary>
	/// Implements the RenderTarget abstract class using OpenGL frame buffer objects
	/// </summary>
	public class OpenGlRenderTarget : RbEngine.Rendering.RenderTarget, IDisposable
	{
		~OpenGlRenderTarget( )
		{
			Dispose( );
		}

		private static bool	ms_CheckForExtension	= true;
		private static bool ms_ExtensionPresent		= false;

		/// <summary>
		/// Creates the render target
		/// </summary>
		/// <param name="width">Width of the render target</param>
		/// <param name="height">Height of the render target</param>
		/// <param name="colourFormat">Format of the render target colour buffer. If this is Undefined, then no colour buffer is created</param>
		/// <param name="depthBits">Number of bits per element in the depth buffer (0 for no depth buffer)</param>
		/// <param name="stencilBits">Number of bits per element in the stencil buffer (0 for no stencil buffer)</param>
		/// <param name="depthBufferAsTexture">If true, then depth buffer storage is created in a texture</param>
		public override void	Create( int width, int height, TextureFormat colourFormat, int depthBits, int stencilBits, bool depthBufferAsTexture )
		{
			//	Requires the frame buffer extension
			if ( ms_CheckForExtension )
			{
				ms_ExtensionPresent		= GlExtensionLoader.IsExtensionSupported( "GL_EXT_framebuffer_object" );
				ms_CheckForExtension	= false;
			}

			if ( !ms_ExtensionPresent )
			{
				throw new System.ApplicationException( "FBO extension not available - can't create render target" );
			}

			//	Generate the frame buffer object
			Gl.glGenFramebuffersEXT( 1, out m_FboHandle );
			Gl.glBindFramebufferEXT( Gl.GL_FRAMEBUFFER_EXT, m_FboHandle );
			Output.Assert( Gl.glIsFramebufferEXT( m_FboHandle ) != 0, Output.ResourceError, "Expected FBO handle to be valid" );

			//	Generate the depth render buffer object
			if ( depthBits != 0 )
			{
				if ( depthBufferAsTexture )
				{
					//	TODO: Add check for extension
					OpenGlTexture2d texture = ( OpenGlTexture2d )RenderFactory.Inst.NewTexture2d( );

					//	TODO: Determine depth format
					texture.Create( width, height, TextureFormat.Depth24 );

					//	Add texture parameters (barfs otherwise - incomplete attachements)
					Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE );
					Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE );
					Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR );
					Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR );

					Gl.glFramebufferTexture2DEXT( Gl.GL_FRAMEBUFFER_EXT, Gl.GL_DEPTH_ATTACHMENT_EXT, Gl.GL_TEXTURE_2D, texture.TextureHandle, 0 );

					m_DepthTexture = texture;
				}
				else
				{

					Gl.glGenFramebuffersEXT( 1, out m_FboDepthHandle );
					Gl.glBindRenderbufferEXT( Gl.GL_RENDERBUFFER_EXT, m_FboDepthHandle );
					Gl.glRenderbufferStorageEXT( Gl.GL_RENDERBUFFER_EXT, Gl.GL_DEPTH_COMPONENT, width, height );
					Gl.glFramebufferRenderbufferEXT( Gl.GL_FRAMEBUFFER_EXT, Gl.GL_DEPTH_ATTACHMENT_EXT, Gl.GL_RENDERBUFFER_EXT, m_FboDepthHandle );
				}
				OpenGlRenderer.CheckErrors( );
			}

			//	Generate the texture
			if ( colourFormat != TextureFormat.Undefined )
			{
				//	Create a texture
				OpenGlTexture2d texture = ( OpenGlTexture2d )RenderFactory.Inst.NewTexture2d( );
				texture.Create( width, height, colourFormat );

				//	Add texture parameters (barfs otherwise - incomplete attachements)
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE );
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE );
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR );
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR );

				//	Bind the texture to the frame buffer (creating it has already bound the texture)
				Gl.glFramebufferTexture2DEXT( Gl.GL_FRAMEBUFFER_EXT, Gl.GL_COLOR_ATTACHMENT0_EXT, Gl.GL_TEXTURE_2D, texture.TextureHandle, 0 );
				OpenGlRenderer.CheckErrors( );

				m_Texture = texture;
			}
			else
			{
				// No color buffer to draw to or read from
				Gl.glDrawBuffer( Gl.GL_NONE );
				Gl.glReadBuffer( Gl.GL_NONE );
			}
			

			if ( stencilBits != 0 )
			{
				throw new ApplicationException( "OpenGL does not support render targets with stencil buffers - please set stencilBits to 0" );
			}


			//	Check if we've succesfully created the frame buffer object
			int status = Gl.glCheckFramebufferStatusEXT( Gl.GL_FRAMEBUFFER_EXT );
			if ( status != Gl.GL_FRAMEBUFFER_COMPLETE_EXT )
			{
				string problem = "";
				switch ( status )
				{
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENTS_EXT			:	problem = "GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENTS_EXT";			break;
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS_EXT			:	problem = "GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS_EXT";			break;
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER_EXT			:	problem = "GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER_EXT";			break;
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT	:	problem = "GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT";	break;
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_FORMATS_EXT				:	problem = "GL_FRAMEBUFFER_INCOMPLETE_FORMATS_EXT";				break;
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT_EXT	:	problem = "GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT_EXT";	break;
					case Gl.GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER_EXT			:	problem = "GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER_EXT";			break;
					case Gl.GL_FRAMEBUFFER_UNSUPPORTED_EXT						:	problem = "GL_FRAMEBUFFER_UNSUPPORTED_EXT";						break;
				}

				throw new ApplicationException( string.Format( "Failed to create render target ({0}x{1} at {2}, {3} depth bits, {4} stencil bits). GL status = {5} ({6})", width, height, colourFormat, depthBits, stencilBits, status, problem ) );
			}
			else
			{
				Output.WriteLineCall( Output.RenderingInfo, "Created render target ({0}x{1} at {2}, {3} depth bits, {4} stencil bits)", width, height, colourFormat, depthBits, stencilBits );
			}

			m_Width		= width;
			m_Height	= height;

			Gl.glBindFramebufferEXT( Gl.GL_FRAMEBUFFER_EXT, 0 );
		}

		/// <summary>
		/// Saves the depth buffer to a file. Render target must be bound for this to work (i.e. between Begin() and End() calls)
		/// </summary>
		public override unsafe void	SaveDepthBuffer( string path )
		{
			float[] depthPixels = new float[ Width * Height ];	//	depthels? dexels? who knows?
			Gl.glReadPixels( 0, 0, Width, Height, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, depthPixels );
			OpenGlRenderer.CheckErrors( );

			byte[] bufferMem = new byte[ Width * Height * 3 ];
			int bufferIndex = 0;
			for ( int depthIndex = 0; depthIndex < depthPixels.Length; ++depthIndex )
			{
				float scaledDepth = ( float )System.Math.Exp( depthPixels[ depthIndex ] );
				bufferMem[ bufferIndex++ ] = ( byte )( scaledDepth * 255.0f );
				bufferMem[ bufferIndex++ ] = ( byte )( scaledDepth * 255.0f );
				bufferMem[ bufferIndex++ ] = ( byte )( scaledDepth * 255.0f );
			}

			System.Drawing.Bitmap bmp;
			fixed ( byte* bufferMemPtr = bufferMem )
			{
				bmp = new System.Drawing.Bitmap( Width, Height, Width * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb, ( IntPtr )bufferMemPtr );
			}
			bmp.Save( path, System.Drawing.Imaging.ImageFormat.Png );
		}

		#region IAppliance Members

		/// <summary>
		/// Sets the render target as the current target
		/// </summary>
		public override void Begin( )
		{
			//	Bind the frame buffer
			if ( m_FboHandle != -1 )
			{
				Gl.glBindFramebufferEXT( Gl.GL_FRAMEBUFFER_EXT, m_FboHandle );
			}

			//	Save viewport properties
			Gl.glPushAttrib( Gl.GL_VIEWPORT_BIT );

			//	Setup the viewport to the size of the frame buffer
			Gl.glViewport( 0, 0, m_Width, m_Height );
		}

		/// <summary>
		/// Sets the frame buffer as the current target
		/// </summary>
		public override void End( )
		{
			//	Unbind the frame buffer
			if ( m_FboHandle != -1 )
			{
				Gl.glBindFramebufferEXT( Gl.GL_FRAMEBUFFER_EXT, 0 );
			}

			//	Restore viewport
			Gl.glPopAttrib( );
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Destroys associated buffers
		/// </summary>
		public void Dispose( )
		{
			if ( m_FboHandle != -1 )
			{
				//	TODO: GL context isn't available now, so delete will fail
			//	Gl.glDeleteFramebuffersEXT( 1, ref m_FboHandle );
				m_FboHandle = -1;
			}
			if ( m_FboDepthHandle != -1 )
			{
				//	TODO: GL context isn't available now, so delete will fail
			//	Gl.glDeleteRenderbuffersEXT( 1, ref m_FboDepthHandle );
				m_FboDepthHandle = -1;
			}
		}

		#endregion

		#region	Private stuff

		private int	m_FboHandle			= -1;
		private int	m_FboDepthHandle	= -1;

		#endregion

	}
}
