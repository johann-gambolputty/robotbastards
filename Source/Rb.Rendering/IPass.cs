using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    /// <summary>
    /// Passes modify the rendering of objects in parallel (unlike ITechnique, which modifies serially)
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
