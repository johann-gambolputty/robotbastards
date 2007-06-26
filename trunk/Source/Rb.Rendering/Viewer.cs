using Rb.Core.Utils;

namespace Rb.Rendering
{
    /// <summary>
    /// Viewer stores information about how to view an object
    /// </summary>
    public class Viewer
    {
		/// <summary>
		/// The rendering context for the viewer
		/// </summary>
        public IRenderContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
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
		/// Default constructor
		/// </summary>
        public Viewer( )
        {
        }

		/// <summary>
		/// Viewer setup
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="renderable"></param>
        public Viewer( Cameras.CameraBase camera, IRenderable renderable )
        {
            m_Camera = camera;
            m_Renderable = renderable;
        }

		/// <summary>
		/// Renders the view
		/// </summary>
        public void Render( )
        {
			m_Context.RenderTime = TinyTime.CurrentTime;
            m_Camera.Begin( );
            m_Renderable.Render( m_Context );
            m_Camera.End( );
        }

        private Cameras.CameraBase  m_Camera;
        private IRenderContext      m_Context = new RenderContext( );
        private IRenderable         m_Renderable;
    }
}
