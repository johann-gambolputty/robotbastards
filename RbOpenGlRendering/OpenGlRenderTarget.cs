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

		/// <summary>
		/// Creates the render target
		/// </summary>
		/// <param name="width">Width of the render target</param>
		/// <param name="height">Height of the render target</param>
		/// <param name="colourFormat">Format of the render target colour buffer. If this is Undefined, then no colour buffer is created</param>
		/// <param name="depthBits">Number of bits per element in the depth buffer (0 for no depth buffer)</param>
		/// <param name="stencilBits">Number of bits per element in the stencil buffer (0 for no stencil buffer)</param>
		public override void	Create( int width, int height, System.Drawing.Imaging.PixelFormat colourFormat, int depthBits, int stencilBits )
		{
			Gl.glGenFramebuffersEXT( 1, out m_FboHandle );
			Gl.glBindFramebufferEXT( Gl.GL_FRAMEBUFFER_EXT, m_FboHandle );

			if ( depthBits != 0 )
			{
				Gl.glGenFramebuffersEXT( 1, out m_FboDepthHandle );
				Gl.glBindRenderbufferEXT( Gl.GL_RENDERBUFFER_EXT, m_FboDepthHandle );
				Gl.glRenderbufferStorageEXT( Gl.GL_RENDERBUFFER_EXT, Gl.GL_DEPTH_COMPONENT24, width, height );
				Gl.glFramebufferRenderbufferEXT( Gl.GL_FRAMEBUFFER_EXT, Gl.GL_DEPTH_ATTACHMENT_EXT, Gl.GL_RENDERBUFFER_EXT, m_FboDepthHandle );
				OpenGlRenderer.CheckErrors( );
			}

			if ( colourFormat != System.Drawing.Imaging.PixelFormat.Undefined )
			{
				//	Create a texture
				OpenGlTexture2d texture = ( OpenGlTexture2d )RenderFactory.Inst.NewTexture2d( );
				texture.Create( width, height, colourFormat );
				
				//	Bind the texture to the frame buffer (creating it has already bound the texture)
				Gl.glFramebufferTexture2DEXT( Gl.GL_FRAMEBUFFER_EXT, Gl.GL_COLOR_ATTACHMENT0_EXT, Gl.GL_TEXTURE_2D, texture.TextureHandle, 0 );
				OpenGlRenderer.CheckErrors( );

				m_Texture = texture;
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
				}

				throw new ApplicationException( string.Format( "Failed to create render target ({0}x{1} at {2}, {3} depth bits, {4} stencil bits). GL status = {5} ({6})", width, height, colourFormat, depthBits, stencilBits, status, problem ) );
			}
			else
			{
				Output.WriteLineCall( Output.RenderingInfo, "Created render target ({0}x{1} at {2}, {3} depth bits, {4} stencil bits)", width, height, colourFormat, depthBits, stencilBits );
			}

			m_Width		= width;
			m_Height	= height;
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

			Renderer.Inst.ClearVerticalGradient( System.Drawing.Color.BlanchedAlmond, System.Drawing.Color.BurlyWood );
		}

		/// <summary>
		/// Sets the frame buffer as the current target
		/// </summary>
		public override void End( )
		{
			Gl.glFlush( );

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
