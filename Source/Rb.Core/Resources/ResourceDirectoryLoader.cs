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
            get { return ms_Cache; }
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

        #region Private stuff

        private static ResourceCache ms_Cache = new ResourceCache( );
        
        #endregion
    }
}
