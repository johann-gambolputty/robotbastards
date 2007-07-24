

namespace Rb.Interaction
{
    /// <summary>
    /// Base class for inputs that generates a 1d scalar value
    /// </summary>
    public class ScalarInput : Input
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="context">Input context</param>
        public ScalarInput( InputContext context ) :
            base( context )
        {
        }

        /// <summary>
        /// Scalar value access
        /// </summary>
        public float Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        
        /// <summary>
        /// Creates a new command message
        /// </summary>
        public override CommandMessage CreateCommandMessage( Command cmd )
        {
            return new ScalarCommandMessage( cmd, m_Value );
        }

        private float m_Value;
    }
}
