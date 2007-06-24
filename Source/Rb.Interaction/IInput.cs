using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Interaction
{
    /// <summary>
    /// Input interface for commands
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Gets the context that this input was created for
        /// </summary>
        InputContext Context
        {
            get;
        }

        /// <summary>
        /// True if the input is active
        /// </summary>
        bool IsActive
        {
            get;
        }

        /// <summary>
        /// Creates a new command message
        /// </summary>
        CommandMessage CreateCommandMessage( Command cmd );
    }
}
