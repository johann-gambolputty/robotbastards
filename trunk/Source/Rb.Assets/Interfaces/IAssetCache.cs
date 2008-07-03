
namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// Caches assets
	/// </summary>
	/// <remarks>
	/// If an asset is loaded, and can be cached (<see cref="LoadParameters.CanCache"/>), then the asset
	/// manager will add it to the loader's cache (<see cref="IAssetLoader.Cache"/>).
	/// On subsequent attempts to load from the same location, the asset manager will check to see if
	/// the location is in the cache, and return the asset directly if it is.
	/// Implementors of this interface should maintain weak references to the assets - it's not up to
	/// the cache to determine the lifetime of the associated assets.
	/// All caches can be cleared by calling <see cref="AssetManager.ClearAllCaches"/>.
	/// </remarks>
	public interface IAssetCache
	{
		/// <summary>
		/// Adds an asset to the asset cache
		/// </summary>
		/// <param name="key">Asset key (asset location hash code)</param>
		/// <param name="asset">Loaded asset</param>
		/// <exception cref="System.ArgumentException">Thrown if the key already exists in the cache</exception>
		/// <seealso cref="Rb.Assets.Base.AssetCache.Add"/>
		void Add( int key, object asset );

		/// <summary>
		/// Finds an object in the cache
		/// </summary>
		/// <param name="key">Asset key (asset location hash code)</param>
		/// <returns>Returns the asset that was loaded from the location, or null if it's not in the cache</returns>
		/// <seealso cref="Rb.Assets.Base.AssetCache.Find"/>
		object Find( int key );

		/// <summary>
		/// Removes an asset from the cache
		/// </summary>
		/// <param name="key">Asset key</param>
		/// <returns>Returns true if the cache contained the key, and it was successfully removed</returns>
		bool Remove( int key );

		/// <summary>
		/// Clears the cache
		/// </summary>
		void Clear( );

	}
}
