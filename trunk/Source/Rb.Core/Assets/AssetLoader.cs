using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Simple implementation of the IAssetLoader interface
	/// </summary>
	public abstract class AssetLoader : IAssetLoader
	{
		#region IAssetLoader Members

		/// <summary>
		/// Gets the cache for this loader
		/// </summary>
		public IAssetCache Cache
		{
			get { return m_Cache; }
		}

		/// <summary>
		/// Creates default loading parameters
		/// </summary>
		public LoadParameters CreateDefaultParameters( )
		{
			return new LoadParameters( );
		}

		/// <summary>
		/// Loads an asset
		/// </summary>
		/// <param name="source">Source of the asset</param>
		/// <param name="parameters">Load parameters</param>
		/// <returns>Loaded asset</returns>
		public abstract object Load( ISource source, LoadParameters parameters );

		/// <summary>
		/// Returns true if this loader can load the asset at the specified location
		/// </summary>
		/// <param name="source">Source of the asset</param>
		/// <returns>Returns true if this loader can process the specified source</returns>
		public abstract bool CanLoad( ISource source );

		#endregion

		private readonly IAssetCache m_Cache = new AssetCache( );
	}
}
