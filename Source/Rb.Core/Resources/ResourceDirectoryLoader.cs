using System;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Loads resources from directories
	/// </summary>
    public abstract class ResourceDirectoryLoader : ResourceLoader
    {
        #region Resource caching

        /// <summary>
        /// Gets the resource cache for this loader. By default, this is a global cache shared by all directory loader objects
        /// </summary>
        public override IResourceCache Cache
        {
            get { return m_Cache; }
        }

        #endregion

		#region	Directory loading

		/// <summary>
		/// Loads resources from a directory
		/// </summary>
		/// <param name="provider">The resource provider that can open files in the directory</param>
        /// <param name="directory">The path to the directory</param>
        /// <param name="canCacheResult">Set by the method. If true, then the return value can be cached in association with the source</param>
		/// <returns>Returns the loaded object</returns>
		public abstract Object Load( ResourceProvider provider, string directory, out bool canCacheResult );

		/// <summary>
		/// Loads resources from a directory
		/// </summary>
		/// <param name="provider">The resource provider that can open files in the directory</param>
        /// <param name="directory">The path to the directory</param>
        /// <param name="canCacheResult">Set by the method. If true, then the return value can be cached in association with the source</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Returns the loaded object</returns>
		public abstract Object Load( ResourceProvider provider, string directory, out bool canCacheResult, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the specified directory
		/// </summary>
		public abstract bool CanLoadDirectory( ResourceProvider provider, string directory );

		#endregion

        #region PreLoadState

        /// <summary>
        /// PreLoadState for the stream loader type
        /// </summary>
        public class DirectoryPreLoadState : ResourcePreLoadState
        {
            /// <summary>
            /// Sets up the preload state
            /// </summary>
            /// <param name="loader">Associated loader</param>
            /// <param name="parameters">Parameters to send to <see cref="ResourceDirectoryLoader.Load"/></param>
            /// <param name="location">Resource location (directory path)</param>
            /// <param name="provider">Provider that can load resources from the directory</param>
            public DirectoryPreLoadState( ResourceLoader loader, LoadParameters parameters, string location, ResourceProvider provider ) :
                base( loader, parameters, location )
            {
                m_Provider = provider;
            }

            /// <summary>
            /// Loads and returns the resource
            /// </summary>
            public override object Load( )
            {
                object result = base.Load( );
                if ( result != null )
                {
                    return result;
                }

                bool canCache;
                if ( Parameters == null )
                {
                    result = ( ( ResourceDirectoryLoader )Loader ).Load( m_Provider, Location, out canCache );
                }
                else
                {
                    result = ( ( ResourceDirectoryLoader )Loader ).Load( m_Provider, Location, out canCache, Parameters );
                }

                return PostLoad( result, canCache );
            }

            private ResourceProvider m_Provider;
        }

        #endregion

        #region Private stuff

        private ResourceCache m_Cache = new ResourceCache( );
        
        #endregion
    }
}
