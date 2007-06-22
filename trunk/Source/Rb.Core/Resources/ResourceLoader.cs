using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Resources
{
    /// <summary>
    /// Base class for resource loader classes
    /// </summary>
    public abstract class ResourceLoader
    {
        #region Resource caching

        /// <summary>
        /// Gets the resource cache for this loader
        /// </summary>
        public abstract IResourceCache Cache
        {
            get;
        }

        #endregion

        #region Load parameters

        /// <summary>
        /// Creates default loading parameters for this loader
        /// </summary>
        public virtual LoadParameters CreateDefaultLoadParameters( )
        {
            return new LoadParameters( );
        }

        #endregion

    }
}
