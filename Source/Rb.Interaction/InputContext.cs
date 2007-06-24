using Rb.Rendering;

namespace Rb.Interaction
{
    /// <summary>
    /// Input context
    /// </summary>
    public class InputContext
    {
        /// <summary>
        /// Graphical context
        /// </summary>
        public Viewer Viewer
        {
            get { return m_Viewer; }
        }

        /// <summary>
        /// Control context
        /// </summary>
        public object Control
        {
            get { return m_Control; }
        }

        /// <summary>
        /// Input context setup constructor
        /// </summary>
        /// <param name="viewer">Graphical context</param>
        /// <param name="control">Control control</param>
        public InputContext( Viewer viewer, object control )
        {
            m_Viewer = viewer;
            m_Control = control;
        }

        private Viewer m_Viewer;
        private object m_Control;
    }
}
