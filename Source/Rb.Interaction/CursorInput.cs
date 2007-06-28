
namespace Rb.Interaction
{
    /// <summary>
    /// Base class for inputs that generate 2d screen coordinates
    /// </summary>
    public abstract class CursorInput : Input
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="context">Input context</param>
        public CursorInput( InputContext context ) :
            base( context )
        {
        }

        /// <summary>
        /// Gets the X coordinate of the cursor
        /// </summary>
        public int X
        {
            get { return m_X; }
        }

        /// <summary>
        /// Gets the Y coordinate of the cursor
        /// </summary>
        public int Y
        {
            get { return m_Y; }
        }

        /// <summary>
        /// Gets the last X coordinate of the cursor
        /// </summary>
        public int LastX
        {
            get { return m_LastX; }
        }

        /// <summary>
        /// Gets the last Y coordinate of the cursor
        /// </summary>
        public int LastY
        {
            get { return m_LastY; }
        }

        /// <summary>
        /// Creates a command message for this input
        /// </summary>
        public override CommandMessage CreateCommandMessage( Command cmd )
        {
            return new CursorCommandMessage( cmd, LastX, LastY, X, Y );
        }

        #region Protected stuff

        protected int m_X;
        protected int m_Y;
        protected int m_LastX;
        protected int m_LastY;

        #endregion
    }
}
