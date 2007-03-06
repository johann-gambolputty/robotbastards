using System;
using RbEngine;
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
			Gl.glBindFramebufferEXT( Gl.GL_RENDERBUFFER_EXT, m_FboHandle );

			if ( colourFormat != System.Drawing.Imaging.PixelFormat.Undefined )
			{
				//	Create a texture
			}

			if ( depthBits != 0 )
			{
				Gl.glGenFramebuffersEXT( 1, out m_FboDepthHandle );
				Gl.glBindFramebufferEXT( Gl.GL_RENDERBUFFER_EXT, m_FboDepthHandle );
				Gl.glRenderbufferStorageEXT( Gl.GL_RENDERBUFFER_EXT, Gl.GL_DEPTH_COMPONENT, width, height );
				Gl.glFramebufferRenderbufferEXT( Gl.GL_FRAMEBUFFER_EXT, Gl.GL_DEPTH_ATTACHMENT_EXT, Gl.GL_RENDERBUFFER_EXT, m_FboDepthHandle );
			}

			if ( stencilBits != 0 )
			{
				throw new ApplicationException( "OpenGL does not support render targets with stencil buffers - please set stencilBits to 0" );
			}

			//	Check if we've succesfully created the frame buffer object
			int status = Gl.glCheckFramebufferStatusEXT( Gl.GL_FRAMEBUFFER_EXT );
			if ( status != Gl.GL_FRAMEBUFFER_COMPLETE_EXT )
			{
				throw new ApplicationException( string.Format( "Failed to create render target ({0}x{1} at {2}, {3} depth bits, {4} stencil bits). GL status = {5}", width, height, colourFormat, depthBits, stencilBits, status ) );
			}
			else
			{
				Output.WriteLineCall( Output.RenderingInfo, "Created render target ({0}x{1} at {2}, {3} depth bits, {4} stencil bits)", width, height, colourFormat, depthBits, stencilBits );
			}
		}

		#region IAppliance Members

		/// <summary>
		/// Sets the render target as the current target
		/// </summary>
		public override void Begin( )
		{
			if ( m_FboHandle != -1 )
			{
				Gl.glBindFramebufferEXT( Gl.GL_RENDERBUFFER_EXT, m_FboHandle );
			}
		}

		/// <summary>
		/// Sets the frame buffer as the current target
		/// </summary>
		public override void End( )
		{
			if ( m_FboHandle != -1 )
			{
				Gl.glBindFramebufferEXT( Gl.GL_RENDERBUFFER_EXT, 0 );
			}
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
				Gl.glDeleteFramebuffersEXT( 1, ref m_FboHandle );
				m_FboHandle = -1;
			}
			if ( m_FboDepthHandle != -1 )
			{
				Gl.glDeleteRenderbuffersEXT( 1, ref m_FboDepthHandle );
				m_FboDepthHandle = -1;
			}
		}

		#endregion

		#region	Private stuff

		private int	m_FboHandle			= -1;
		private int	m_FboColourHandle	= -1;
		private int	m_FboDepthHandle	= -1;

		#endregion

	}
}
