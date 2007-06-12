using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Resources
{
    /// <summary>
    /// Simple implementation of IResourceCache using a of Dictionary of WeakReference objects
    /// </summary>
    internal class ResourceCache : IResourceCache
    {
        #region IResourceCache Members

        /// <summary>
        /// Adds a resource from a given source
        /// </summary>
        /// <param name="key">Resource source</param>
        /// <param name="resource">Resource object</param>
        public void Add( string key, object resource )
        {
            m_Cache[ key ] = new WeakReference( resource );
        }

        /// <summary>
        /// Finds a resource
        /// </summary>
        /// <param name="key">Resource key</param>
        /// <returns>Resource object (null if key could not be found)</returns>
        public object Find( string key )
        {
            WeakReference weakRef = m_Cache[ key ];
            if ( weakRef == null )
            {
                return null;
            }
            if ( weakRef.Target == null )
            {
                //  Resource key is in the cache, but the resource being referenced has been destroyed
                //  Remove the key and return null
                m_Cache.Remove( key );
                return null;
            }
            return weakRef.Target;
        }

        /// <summary>
        /// Clears the resource cache
        /// </summary>
        public void Clear( )
        {
            m_Cache.Clear( );
        }

        #endregion

        #region Private stuff

        private Dictionary< string, WeakReference > m_Cache = new Dictionary< string, WeakReference >( );

        #endregion
    }
}
