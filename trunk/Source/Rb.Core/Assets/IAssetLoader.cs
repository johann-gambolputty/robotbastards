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
		/// Gets the asset name
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the asset extension
		/// </summary>
		string Extension
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
