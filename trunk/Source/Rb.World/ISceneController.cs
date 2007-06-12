using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.World
{
    /// <summary>
    /// Scene controller interface
    /// </summary>
    public interface ISceneController
    {
        /// <summary>
        /// Gets the control associated with this controller
        /// </summary>
        object Control { get; }
    }
}
