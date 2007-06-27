using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Interaction
{
    /// <summary>
    /// Input source base class for commands
    /// </summary>
    public abstract class Input : IInput
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="context">Input context</param>
        public Input( InputContext context )
        {
            m_Context = context;
        }

        /// <summary>
        /// Gets the input context
        /// </summary>
        public InputContext Context
        {
            get { return m_Context; }
        }

        /// <summary>
        /// Returns true if this input is active
        /// </summary>
        public virtual bool IsActive
        {
            get { return m_Active; }
            set { m_Active = value; }
        }

        /// <summary>
        /// Flag that determines if the input should be disabled (IsActive = false) after each update
        /// </summary>
        public virtual bool DeactivateOnUpdate
        {
            get { return false; }
        }

        /// <summary>
        /// Creates a new command message
        /// </summary>
        public virtual CommandMessage CreateCommandMessage( Command cmd )
        {
            return new CommandMessage( cmd );
        }

        #region Private stuff

        private InputContext m_Context;
        private bool m_Active;

        #endregion
    }
}
