using System;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Caches assets
	/// </summary>
	/// <remarks>
	/// If an asset is loaded, and can be cached, then the asset manager will add it to the loader's
	/// cache (<see cref="IAssetLoader.Cache"/>).
	/// On subsequent attempts to load from the same location, the asset manager will check to see if
	/// the location is in the cache, and return the asset directly if it is.
	/// 
	/// Implementors of this interface should maintain weak references to the assets - it's not up to
	/// the cache to determine the lifetime of the associated assets.
	/// 
	/// </remarks>
	public interface IAssetCache
	{
		/// <summary>
		/// Adds an asset to the asset cache
		/// </summary>
		/// <param name="key">Asset key (asset location hash code)</param>
		/// <param name="asset">Loaded asset</param>
		/// <exception cref="ArgumentException">Thrown if the key already exists in the cache</exception>
		/// <seealso cref="AssetCache.Add(IAssetCache,string,object)"/>
		void Add( int key, object asset );

		/// <summary>
		/// Finds an object in the cache
		/// </summary>
		/// <param name="key">Asset key (asset location hash code)</param>
		/// <returns>Returns the asset that was loaded from the location, or null if it's not in the cache</returns>
		/// <seealso cref="AssetCache.Find(IAssetCache,string)"/>
		object Find( int key );

		/// <summary>
		/// Clears the cache
		/// </summary>
		void Clear( );

	}
}
