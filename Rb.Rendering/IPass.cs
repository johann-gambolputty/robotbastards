using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    /// <summary>
    /// Interface for objects that modify the render state prior to rendering, and restore it after
    /// </summary>
    public interface IPass
    {
        /// <summary>
        /// Begins the pass
        /// </summary>
        void Begin( );

        /// <summary>
        /// Ends the pass
        /// </summary>
        void End( );
    }
}
