using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Resources
{
    /// <summary>
    /// Resource cache interface
    /// </summary>
    public interface IResourceCache
    {
        /// <summary>
        /// Adds a resource from a given source
        /// </summary>
        /// <param name="key">Resource source</param>
        /// <param name="resource">Resource object</param>
        void Add( string key, object resource );

        /// <summary>
        /// Finds a resource
        /// </summary>
        /// <param name="key">Resource key</param>
        /// <returns>Resource object (null if key could not be found)</returns>
        object Find( string key );

        /// <summary>
        /// Clears the cache
        /// </summary>
        void Clear( );
    }
}
