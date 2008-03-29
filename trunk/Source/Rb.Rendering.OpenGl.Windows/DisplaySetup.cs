using System;
using System.Text;
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
	public class DisplaySetup : Rendering.Windows.IWindowsDisplaySetup
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

		#endregion

		#region IDisplaySetup Implementation

		/// <summary>
		/// Writes information about the GL renderer
		/// </summary>
		private static void WriteInfo( )
		{
			GraphicsLog.Info( "Successfully created GL render context:" );

			StringBuilder sb = new StringBuilder( );
			sb.AppendFormat( "Vendor: {0}\n", Gl.glGetString( Gl.GL_VENDOR ) );
			sb.AppendFormat( "Renderer: {0}\n", Gl.glGetString( Gl.GL_RENDERER ) );
			sb.AppendFormat( "Version: {0}", Gl.glGetString( Gl.GL_VERSION ) );

			GraphicsLog.Info( sb.ToString( ) );
		}

		/// <summary>
		/// Called when the control Load event fires
		/// </summary>
		public void Create( object display, int colourBits, int depthBits, int stencilBits )
		{
			Dispose( );

			Control control = ( Control )display;

			Gdi.PIXELFORMATDESCRIPTOR descriptor = new Gdi.PIXELFORMATDESCRIPTOR( );

			descriptor.nSize = ( short )System.Runtime.InteropServices.Marshal.SizeOf( descriptor );
			descriptor.nVersion = 1;
			descriptor.dwFlags = Gdi.PFD_DRAW_TO_WINDOW | Gdi.PFD_SUPPORT_OPENGL | Gdi.PFD_DOUBLEBUFFER;
			descriptor.iPixelType = Gdi.PFD_TYPE_RGBA;
			descriptor.cColorBits = ( byte )colourBits;
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
			descriptor.cDepthBits = ( byte )depthBits;
			descriptor.cStencilBits = ( byte )stencilBits;
			descriptor.cAuxBuffers = 0;
			descriptor.iLayerType = Gdi.PFD_MAIN_PLANE;
			descriptor.bReserved = 0;
			descriptor.dwLayerMask = 0;
			descriptor.dwVisibleMask = 0;
			descriptor.dwDamageMask = 0;

			m_WindowHandle = control.Handle;
			m_DeviceContext = User.GetDC( m_WindowHandle );

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

				//	Make it current
				Wgl.wglMakeCurrent( m_DeviceContext, m_RenderContext );

				// Force A Reset On The Working Set Size
				//Kernel.SetProcessWorkingSetSize( Process.GetCurrentProcess( ).Handle, -1, -1 );

				//	Load extensions
				//OpenGlRenderer.LoadExtensions( );
			}
			else
			{
				Wgl.wglShareLists( ms_LastRenderContext, m_RenderContext );
			}

			WriteInfo( );

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
			Graphics.Renderer.SetViewport( 0, 0, control.Width, control.Height );

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

		private IntPtr			m_WindowHandle;
		private IntPtr			m_DeviceContext;
		private IntPtr			m_RenderContext;
		private static IntPtr	ms_LastRenderContext;

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Releases device context and render context
		/// </summary>
		public void Dispose( )
		{
			if ( m_RenderContext != IntPtr.Zero )
			{
				Wgl.wglMakeCurrent( IntPtr.Zero, IntPtr.Zero );
				Wgl.wglDeleteContext( m_RenderContext );
				m_RenderContext = IntPtr.Zero;
			}

			if ( ( m_WindowHandle != IntPtr.Zero ) && ( m_DeviceContext != IntPtr.Zero ) )
			{
				User.ReleaseDC( m_WindowHandle, m_DeviceContext );
				m_WindowHandle = IntPtr.Zero;
				m_DeviceContext = IntPtr.Zero;
			}
		}

		#endregion
	}
}
