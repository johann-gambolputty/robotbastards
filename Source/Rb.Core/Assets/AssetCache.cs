using System;
using System.Collections.Generic;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Simple implementation of IAssetCache. Also contains helper functions for IAssetCache
	/// </summary>
	public class AssetCache : IAssetCache
	{
		#region IAssetCache Members

		/// <summary>
        /// Adds an asset from a given location
        /// </summary>
        /// <param name="key">Asset location</param>
        /// <param name="asset">Asset object</param>
        public void Add( int key, object asset )
        {
            m_Cache[ key ] = new WeakReference( asset );
        }

        /// <summary>
        /// Finds a asset
        /// </summary>
        /// <param name="key">Asset location key</param>
        /// <returns>Asset object (null if key could not be found)</returns>
        public object Find( int key )
        {
            WeakReference weakRef;
			if ( !m_Cache.TryGetValue( key, out weakRef ) )
			{
                return null;
            }
            if ( weakRef.Target == null )
            {
                //  Asset location key is in the cache, but the asset being referenced has been destroyed
                //  Remove the key and return null
                m_Cache.Remove( key );
                return null;
            }
            return weakRef.Target;
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear( )
        {
            m_Cache.Clear( );
        }

        #endregion

		#region IAssetCache Helpers

		/// <summary>
		/// Helper function that wraps <see cref="IAssetCache.Add"/>
		/// </summary>
		/// <param name="cache">Asset cache to add asset to</param>
		/// <param name="location">Asset location</param>
		/// <param name="asset">Asset object</param>
		public static void Add( IAssetCache cache, string location, object asset )
		{
			cache.Add( Location.GetLocationKey( location ), asset );
		}

		/// <summary>
		/// Helper function that wraps <see cref="IAssetCache.Find"/>
		/// </summary>
		/// <param name="cache">Asset cache to add asset to</param>
		/// <param name="location">Asset location</param>
		/// <returns>Asset object (null if location was not in cache)</returns>
		public static object Find( IAssetCache cache, string location )
		{
			return cache.Find( Location.GetLocationKey( location ) );
		}

		#endregion

		#region Private stuff

		private readonly Dictionary< long, WeakReference > m_Cache = new Dictionary< long, WeakReference >( );

        #endregion
	}
}