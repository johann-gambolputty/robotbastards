using System;
using System.Windows.Forms;
using Tao.OpenGl;
using Gdi = Tao.Platform.Windows.Gdi;
using Wgl = Tao.Platform.Windows.Wgl;
using User = Tao.Platform.Windows.User;


namespace Rb.Rendering.OpenGl.Windows
{
	/// <summary>
	/// Display setup for OpenGL windows controls
	/// </summary>
	public class DisplaySetup : Rb.Rendering.Windows.IWindowsDisplaySetup
	{
		#region IWindowsDisplaySetup Implementation

		/// <summary>
		/// Returns the class styles required by the owner window
		/// </summary>
		public int ClassStyles
		{
			get
			{
				const Int32 CS_VREDRAW	= 0x1;
				const Int32 CS_HREDRAW	= 0x2;
				const Int32 CS_OWNDC	= 0x20;
				return CS_VREDRAW | CS_HREDRAW | CS_OWNDC;
			}
		}

		/// <summary>
		/// Returns control styles that the control should apply
		/// </summary>
		public ControlStyles AddStyles
		{
			get
			{
				return	ControlStyles.AllPaintingInWmPaint |
						ControlStyles.Opaque |
						ControlStyles.ResizeRedraw |
						ControlStyles.UserPaint;
			}
		}

		/// <summary>
		/// Returns control styles that the control should remove
		/// </summary>
		public ControlStyles RemoveStyles
		{
			get
			{
				return ControlStyles.DoubleBuffer;
			}
		}

		/// <summary>
		/// Creates an image that is displayed in this control in design mode
		/// </summary>
		public System.Drawing.Image CreateDesignImage( )
		{
			return null;
		}

		#endregion

		#region IDisplaySetup Implementation

		/// <summary>
		/// Called when the control Load event fires
		/// </summary>
		public void Create( object display, byte colourBits, byte depthBits, byte stencilBits )
		{
			Control control = ( Control )display;

			Gdi.PIXELFORMATDESCRIPTOR descriptor = new Gdi.PIXELFORMATDESCRIPTOR( );

			descriptor.nSize = ( short )System.Runtime.InteropServices.Marshal.SizeOf( descriptor );
			descriptor.nVersion = 1;
			descriptor.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
			descriptor.iPixelType = Gdi.PFD_TYPE_RGBA;
			descriptor.cColorBits = colourBits;
			descriptor.cRedBits = 0;
			descriptor.cRedShift = 0;
			descriptor.cGreenBits = 0;
			descriptor.cGreenShift = 0;
			descriptor.cBlueBits = 0;
			descriptor.cBlueShift = 0;
			descriptor.cAlphaBits = 0;
			descriptor.cAlphaShift = 0;
			descriptor.cAccumBits = 0;
			descriptor.cAccumRedBits = 0;
			descriptor.cAccumGreenBits = 0;
			descriptor.cAccumBlueBits = 0;
			descriptor.cAccumAlphaBits = 0;
			descriptor.cDepthBits = depthBits;
			descriptor.cStencilBits = stencilBits;
			descriptor.cAuxBuffers = 0;
			descriptor.iLayerType = Gdi.PFD_MAIN_PLANE;
			descriptor.bReserved = 0;
			descriptor.dwLayerMask = 0;
			descriptor.dwVisibleMask = 0;
			descriptor.dwDamageMask = 0;

			m_DeviceContext = User.GetDC( control.Handle );

			//	Choose a pixel format
			int pixelFormat = Gdi.ChoosePixelFormat( m_DeviceContext, ref descriptor );
			if ( pixelFormat == 0 )
			{
				throw new ApplicationException( "Failed to choose pixel format" );
			}

			//	Set the chosen pixel format in the current device context
			if ( !Gdi.SetPixelFormat( m_DeviceContext, pixelFormat, ref descriptor ) )
			{
				throw new ApplicationException( "Failed to set pixel format" );
			}

			//	Create the OpenGL rendering context
			m_RenderContext = Wgl.wglCreateContext( m_DeviceContext );
			if ( m_RenderContext == IntPtr.Zero )
			{
				throw new ApplicationException( "Failed to create GL rendering context" );
			}

			//	All contexts share the same display list space
			if ( ms_LastRenderContext == IntPtr.Zero )
			{
				ms_LastRenderContext = m_RenderContext;

				//	TODO: This is a horrible bodge, to ensure that opengl extensions get loaded correctly

				//	Make it current
				Wgl.wglMakeCurrent( m_DeviceContext, m_RenderContext );

				//	Load extensions
				OpenGlRenderer.LoadExtensions( );
			}
			else
			{
				Wgl.wglShareLists( ms_LastRenderContext, m_RenderContext );
			}

			GraphicsLog.Info( "Successfully created GL render context" );
		}

		/// <summary>
		/// Makes this rendering context current. Called by the paint event handler prior to any rendering
		/// </summary>
		public bool BeginPaint( object display )
		{
			//	Make m_RenderContext the current rendering context
			if ( !Wgl.wglMakeCurrent( m_DeviceContext, m_RenderContext ) )
			{
				return false;
			}

			Control control = ( Control )display;
			OpenGlRenderer.Inst.SetViewport( 0, 0, control.Width, control.Height );

			return true;
		}

		/// <summary>
		/// Called when painting has finished
		/// </summary>
		public void EndPaint( object display )
		{
			Gl.glFinish( );
			Gdi.SwapBuffers( m_DeviceContext );
		}

		#endregion

		#region Private stuff

		private IntPtr m_DeviceContext;
		private IntPtr m_RenderContext;
		private static IntPtr ms_LastRenderContext;

		#endregion
	}
}
