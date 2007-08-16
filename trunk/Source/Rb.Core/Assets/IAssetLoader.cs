using System;
using System.Collections.Generic;
using System.Text;

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
		/// Creates default loading parameters
		/// </summary>
		LoadParameters CreateDefaultParameters( );

		/// <summary>
		/// Loads an asset
		/// </summary>
		/// <param name="location">Location of the asset</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Loaded asset</returns>
		object Load( Location location, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the asset at the specified location
		/// </summary>
		/// <param name="location">Asset location</param>
		/// <returns>Returns the </returns>
		bool CanLoad( Location location );
	}
}
