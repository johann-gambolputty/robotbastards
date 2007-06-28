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

            //	Display the FPS
            RenderFont font = RenderFonts.GetDefaultFont( DefaultFont.Debug );
            double fps = 1.0 / TinyTime.ToSeconds( m_Context.RenderTime - m_LastRenderTime );
            font.DrawText( 0, 0, System.Drawing.Color.Black, "FPS: {0}", fps.ToString( "G4" ) );

            m_Camera.Begin( );
            m_Context.ApplyTechnique( m_Technique, m_Renderable );
            m_Camera.End( );

		    m_LastRenderTime = m_Context.RenderTime;
        }

        private long                m_LastRenderTime = TinyTime.CurrentTime;
        private Cameras.CameraBase  m_Camera;
        private IRenderContext      m_Context = new RenderContext( );
        private IRenderable         m_Renderable;
        private ITechnique          m_Technique;
        private bool                m_ShowFps;
    }
}
