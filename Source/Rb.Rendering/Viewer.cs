using System;
using System.Drawing;
using Rb.Core.Utils;

namespace Rb.Rendering
{
    /// <summary>
    /// Viewer stores information about how to view an object
    /// </summary>
    public class Viewer : IDisposable
    {
		/// <summary>
		/// The underlying control
		/// </summary>
    	public object Control
    	{
			get { return m_Control; }
			set { m_Control = value; }
    	}

		/// <summary>
		/// The rendering context for the viewer
		/// </summary>
        public IRenderContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        /// <summary>
        /// The technique used to display the renderable object (can be null)
        /// </summary>
        public ITechnique Technique
        {
            get { return m_Technique; }
            set { m_Technique = value; }
        }

		/// <summary>
		/// The current camera
		/// </summary>
        public Cameras.CameraBase Camera
        {
            get { return m_Camera;  }
            set { m_Camera = value; }
        }

		/// <summary>
		/// The object rendered in the view
		/// </summary>
        public IRenderable Renderable
        {
            get { return m_Renderable; }
            set { m_Renderable = value; }
        }

        /// <summary>
        /// If set to true, the current fps count is shown in the top left corner of the viewer
        /// </summary>
        public bool ShowFps
        {
            get { return m_ShowFps; }
            set { m_ShowFps = value; }
        }


		/// <summary>
		/// Gets the FPS display
		/// </summary>
    	public FpsDisplay FpsDisplay
    	{
			get { return m_FpsDisplay; }
    	}

		/// <summary>
		/// View rectangle
		/// </summary>
		public RectangleF ViewRect
    	{
			get { return m_ViewRect; }
			set { m_ViewRect = value; }
    	}

		/// <summary>
		/// Default constructor
		/// </summary>
        public Viewer( )
        {
        }

		/// <summary>
		/// Viewer setup
		/// </summary>
        public Viewer( object control, Cameras.CameraBase camera, IRenderable renderable )
        {
			m_Control = control;
            m_Camera = camera;
            m_Renderable = renderable;
        }

		/// <summary>
		/// Gets the window rectangle covered by this viewer
		/// </summary>
		public Rectangle GetWindowRectangle( Rectangle windowRect )
		{
			int x = ( int )( m_ViewRect.Left * windowRect.Width );
			int y = ( int )( m_ViewRect.Top * windowRect.Height );
			int w = ( int )( m_ViewRect.Width * windowRect.Width );
			int h = ( int )( m_ViewRect.Height * windowRect.Height );

			return new Rectangle( x, y, w, h );
		}

		/// <summary>
		/// Renders the view
		/// </summary>
        public void Render( )
        {
			Renderer renderer = Graphics.Renderer;
			Rectangle oldRect = renderer.Viewport;

			int x = ( int )( m_ViewRect.Left * oldRect.Width );
			int y = ( int )( m_ViewRect.Top * oldRect.Height );
			int w = ( int )( m_ViewRect.Width * oldRect.Width );
			int h = ( int )( m_ViewRect.Height * oldRect.Height );
			Graphics.Renderer.SetViewport( x, y, w, h );

			
			renderer.ClearDepth( 1.0f );
			//renderer.ClearColour( Color.DarkSeaGreen );
			renderer.ClearVerticalGradient( Color.DarkSeaGreen, Color.Black );

            m_Context.RenderTime = TinyTime.CurrentTime;

			if ( m_Camera != null )
			{
				m_Camera.Begin( );
			}
            m_Context.ApplyTechnique( m_Technique, m_Renderable );

			if ( m_Camera != null )
			{
				m_Camera.End( );
			}

			if ( m_ShowFps )
			{
				m_FpsDisplay.Render( m_Context );
			}

			Graphics.Renderer.SetViewport( oldRect.Left, oldRect.Top, oldRect.Width, oldRect.Height );
        }

		private object				m_Control;
		private FpsDisplay			m_FpsDisplay = new FpsDisplay( );
        private Cameras.CameraBase  m_Camera;
        private IRenderContext      m_Context = new RenderContext( );
        private IRenderable         m_Renderable;
        private ITechnique          m_Technique;
		private RectangleF			m_ViewRect = new RectangleF( 0, 0, 1, 1 );
        private bool                m_ShowFps;

		#region IDisposable Members

		/// <summary>
		/// Disposes of the viewer and its components
		/// </summary>
		public void Dispose( )
		{
			Dispose( m_FpsDisplay );
			Dispose( m_Camera );
			Dispose( m_Context );
			Dispose( m_Renderable );
			Dispose( m_Technique );

			m_Control		= null;
			m_FpsDisplay	= null;
			m_Camera		= null;
			m_Context		= null;
			m_Renderable	= null;
			m_Technique		= null;
		}

		private static void Dispose( object obj )
		{
			IDisposable disposable = obj as IDisposable;
			if ( disposable != null )
			{
				disposable.Dispose( );
			}
		}

		#endregion
	}
}
