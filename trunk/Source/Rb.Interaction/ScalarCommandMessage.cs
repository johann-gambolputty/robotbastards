using System;

namespace Rb.Interaction
{
    /// <summary>
    /// Scalar value command message
    /// </summary>
    [Serializable]
    public class ScalarCommandMessage : CommandMessage
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ScalarCommandMessage( Command cmd, float value ) :
			base( cmd )
		{
            m_Value = value;
		}

        /// <summary>
        /// Scalar value access
        /// </summary>
        public float Value
        {
            get { return m_Value;  }
            set { m_Value = value; }
        }

        private float m_Value;
    }
}
