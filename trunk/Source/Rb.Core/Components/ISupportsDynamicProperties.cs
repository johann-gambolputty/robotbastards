using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Components
{
    /// <summary>
    /// Interface for classes that ... support dynamic properties. doh.
    /// </summary>
    public interface ISupportsDynamicProperties
    {
        /// <summary>
        /// Gets the dynamic property set
        /// </summary>
        IDynamicProperties Properties { get; }
    }
}
