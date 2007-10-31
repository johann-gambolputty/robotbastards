namespace Rb.Core.Assets
{
	/// <summary>
	/// Loads assets
	/// </summary>
	public interface IAssetLoader
	{
		/// <summary>
		/// Gets the cache for this loader
		/// </summary>
		IAssetCache Cache
		{
			get;
		}

		/// <summary>
		/// Gets the asset name
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the asset extensions
		/// </summary>
		string[] Extensions
		{
			get;
		}

		/// <summary>
		/// Creates default loading parameters
		/// </summary>
		/// <param name="addAllProperties">If true, then the parameters object gets all relevant dynamic properties with their default values added</param>
		/// <returns>Returns default loading parameters</returns>
		LoadParameters CreateDefaultParameters( bool addAllProperties );

		/// <summary>
		/// Loads an asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Loaded asset</returns>
		object Load( ISource source, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the asset at the specified location
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <returns>Returns true if this loader can process the specified source</returns>
		bool CanLoad( ISource source );
	}
}
