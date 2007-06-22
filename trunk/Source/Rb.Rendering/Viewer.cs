using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    /// <summary>
    /// Viewer stores information about how to view an object
    /// </summary>
    public class Viewer
    {
        public IRenderContext Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }

        public Cameras.CameraBase Camera
        {
            get { return m_Camera;  }
            set { m_Camera = value; }
        }

        public IRenderable Renderable
        {
            get { return m_Renderable; }
            set { m_Renderable = value; }
        }

        public Viewer( )
        {
            
        }

        public Viewer( Cameras.CameraBase camera, IRenderable renderable )
        {
            m_Camera = camera;
            m_Renderable = renderable;
        }

        public void Render( )
        {
            m_Camera.Begin( );
            m_Renderable.Render( m_Context );
            m_Camera.End( );
        }

        private Cameras.CameraBase  m_Camera;
        private IRenderContext      m_Context = new RenderContext( );
        private IRenderable         m_Renderable;
    }
}
