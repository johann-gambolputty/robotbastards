using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Resources
{
    /// <summary>
    /// Stores state prior to loading
    /// </summary>
    public class ResourcePreLoadState
    {
        /// <summary>
        /// Load parameters
        /// </summary>
        public LoadParameters Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }

        /// <summary>
        /// Resource loader
        /// </summary>
        public ResourceLoader Loader
        {
            get { return m_Loader; }
        }

        /// <summary>
        /// Resource location
        /// </summary>
        public string Location
        {
            get { return m_Location; }
        }

        /// <summary>
        /// Loads the resource
        /// </summary>
        public virtual object Load()
        {
            ResourcesLog.Info( "Loading \"{0}\"", m_Location );
            object result = m_Loader.Cache.Find( m_Location );
            if (result == null)
            {
                return null;
            }
            ResourcesLog.Verbose( "Retrieved \"{0}\" from resource cache", m_Location );
            return result;
        }
        
        /// <summary>
        /// Sets up the preload state
        /// </summary>
        protected ResourcePreLoadState( ResourceLoader loader, LoadParameters parameters, string location )
        {
            m_Loader        = loader;
            m_Parameters    = parameters;
            m_Location      = location;
        }

        /// <summary>
        /// Adds the resource to the resource cache
        /// </summary>
        protected object PostLoad( object result, bool canCache )
        {
            //  Cache the result
            if ( canCache && ( result != null ) )
            {
                ResourcesLog.Verbose( "Adding \"{0}\" to resource cache", m_Location );
                m_Loader.Cache.Add( m_Location, result );
            }
            return result;
        }

        private string          m_Location;
        private LoadParameters  m_Parameters;
        private ResourceLoader  m_Loader;
    }
}
