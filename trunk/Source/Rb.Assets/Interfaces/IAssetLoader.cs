
namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// An asset loader is responsible for loading a particular type of asset (e.g. images, MD3 files, etc.)
	/// </summary>
	public interface IAssetLoader
	{
		/// <summary>
		/// Gets the cache associated with this loader
		/// </summary>
		IAssetCache Cache
		{
			get;
		}

		/// <summary>
		/// Gets the name of assets loaded by this object (descriptive, not used for anything important)
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the array of file extensions supported by this loader
		/// </summary>
		/// <remarks>
		/// Should return undecorated extensions, e.g. ["jpg", "gif", "bmp"]
		/// </remarks>
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
