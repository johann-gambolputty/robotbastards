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
            get { return m_Viewer.Control; }
        }

        /// <summary>
        /// Input context setup constructor
        /// </summary>
        /// <param name="viewer">Graphical context</param>
        public InputContext( Viewer viewer )
        {
            m_Viewer = viewer;
        }

        private readonly Viewer m_Viewer;
    }
}
