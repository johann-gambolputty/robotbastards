using System;
using System.Windows.Forms;
using RbEngine;
using RbEngine.Rendering;
using Tao.OpenGl;

using Gdi	= Tao.Platform.Windows.Gdi;
using Wgl	= Tao.Platform.Windows.Wgl;
using User	= Tao.Platform.Windows.User;


namespace RbOpenGlRendering
{
	/// <summary>
	/// Summary description for OpenGlControlRenderContext.
	/// </summary>
	public class OpenGlControlRenderContext : RbEngine.Rendering.ControlRenderContext
	{
		/// <summary>
		/// Creates an image that is displayed in this control in design mode
		/// </summary>
		/// <returns></returns>
		public override System.Drawing.Image	CreateDesignImage( )
		{
			return null;
		}

		/// <summary>
		/// Called when the control Load event fires
		/// </summary>
		public override void					Create( Control control, byte colourBits, byte depthBits, byte stencilBits )
		{
			Gdi.PIXELFORMATDESCRIPTOR descriptor = new Gdi.PIXELFORMATDESCRIPTOR( );

			descriptor.nSize			= ( short )System.Runtime.InteropServices.Marshal.SizeOf( descriptor );
			descriptor.nVersion			= 1;
			descriptor.dwFlags			=	Gdi.PFD_DRAW_TO_WINDOW	|
											Gdi.PFD_SUPPORT_OPENGL	|
											Gdi.PFD_DOUBLEBUFFER;
			descriptor.iPixelType		= Gdi.PFD_TYPE_RGBA;
			descriptor.cColorBits		= colourBits;
			descriptor.cRedBits			= 0; 
			descriptor.cRedShift		= 0; 
			descriptor.cGreenBits		= 0; 
			descriptor.cGreenShift		= 0; 
			descriptor.cBlueBits		= 0; 
			descriptor.cBlueShift		= 0; 
			descriptor.cAlphaBits		= 0; 
			descriptor.cAlphaShift		= 0; 
			descriptor.cAccumBits		= 0; 
			descriptor.cAccumRedBits	= 0; 
			descriptor.cAccumGreenBits	= 0; 
			descriptor.cAccumBlueBits	= 0; 
			descriptor.cAccumAlphaBits	= 0; 
			descriptor.cDepthBits		= depthBits; 
			descriptor.cStencilBits		= stencilBits; 
			descriptor.cAuxBuffers		= 0; 
			descriptor.iLayerType		= Gdi.PFD_MAIN_PLANE; 
			descriptor.bReserved		= 0; 
			descriptor.dwLayerMask		= 0; 
			descriptor.dwVisibleMask	= 0; 
			descriptor.dwDamageMask		= 0;

			m_DeviceContext = User.GetDC( control.Handle );

			//	Choose a pixel format
			int pixelFormat = Gdi.ChoosePixelFormat( m_DeviceContext, ref descriptor );
			if ( pixelFormat == 0 )
			{
				throw new System.ApplicationException( "Failed to choose pixel format" );
			}

			//	Set the chosen pixel format in the current device context
			if ( !Gdi.SetPixelFormat( m_DeviceContext, pixelFormat, ref descriptor ) )
			{
				throw new System.ApplicationException( "Failed to set pixel format" );
			}

			//	Create the OpenGL rendering context
			m_RenderContext = Wgl.wglCreateContext( m_DeviceContext );
			if ( m_RenderContext == IntPtr.Zero )
			{
				throw new System.ApplicationException( "Failed to create GL rendering context" );
			}

			MakeCurrent( );

			Output.WriteLineCall( Output.RenderingInfo, "Successfully created GL render context" );
		}

		/// <summary>
		/// Makes this rendering context current. Called by the paint event handler prior to any rendering
		/// </summary>
		public override bool					MakeCurrent( )
		{
			//	Make m_RenderContext the current rendering context
			return Wgl.wglMakeCurrent( m_DeviceContext, m_RenderContext );
		}

		/// <summary>
		/// Called when painting has finished
		/// </summary>
		public override void					EndPaint( )
		{
			Gl.glFinish( );
			Gdi.SwapBuffers( m_DeviceContext );
		}

		IntPtr	m_DeviceContext;
		IntPtr	m_RenderContext;
	}
}
