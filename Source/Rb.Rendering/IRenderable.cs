using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    /// <summary>
    /// An object that can be rendered
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Renders this object in context
        /// </summary>
        void Render( IRenderContext context );
    }
}
