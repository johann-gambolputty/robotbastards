using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    /// <summary>
    /// Interface for objects that can modify the rendering of an object
    /// </summary>
    public interface IAppliance
    {
        /// <summary>
        /// Applies some render state changes before calling a given renderable object
        /// </summary>
        /// <param name="renderObj">Renderable object</param>
        void Apply( IRenderable renderObj );
    }
}
