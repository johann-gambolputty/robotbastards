using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
    /// <summary>
    /// At the moment, this base class is only used for identifying composites in render assemblies
    /// </summary>
    /// <remarks>
    /// A composite is a complex object, like a mesh, that encapsulates many low-level objects like
    /// render states, materials and so on. It may be more optimal for a rendering assembly to supply
    /// a specific composite implementation than a generic one.
    /// </remarks>
    public abstract class Composite
    {
    }
}
