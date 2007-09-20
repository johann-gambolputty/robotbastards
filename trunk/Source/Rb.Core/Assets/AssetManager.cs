using System;
using System.Collections.Generic;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Singleton that manages asset loading
	/// </summary>
	public class AssetManager
	{
		/// <summary>
		/// Gets the AssetManager singleton
		/// </summary>
		public static AssetManager Instance
		{
			get { return ms_Singleton;  }
		}

		//	TODO: AP: Asynchronous asset loading

		#region Loaders

		/// <summary>
		/// Gets the currently available loaders
		/// </summary>
		public IEnumerable< IAssetLoader > Loaders
		{
			get { return m_Loaders; }
		}

		/// <summary>
		/// Adds an asset loader
		/// </summary>
		/// <param name="loader">Loader to add</param>
		public void AddLoader( IAssetLoader loader )
		{
			m_Loaders.Add( loader );
		}

		/// <summary>
		/// Finds an <see cref="IAssetLoader"/> that can load a specified source
		/// </summary>
		public IAssetLoader FindLoaderForAsset( ISource source )
		{
			foreach ( IAssetLoader loader in m_Loaders )
			{
				if ( loader.CanLoad( source ) )
				{
					return loader;
				}
			}
			return null;
		}

		#endregion

		#region Synchronous asset loading

		/// <summary>
		/// Creates a load state that can load an asset from a given source
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Asset loading parameters</param>
		/// <returns>Returns a new LoadState that can load the asset at source</returns>
		public LoadState CreateLoadState( ISource source, LoadParameters parameters )
		{
			IAssetLoader loader = FindLoaderForAsset( source );
			if ( loader == null )
			{
				throw new ArgumentException( string.Format( "No loader could load asset {0}", source ) );
			}

			return new LoadState( loader, source, parameters );
		}

		/// <summary>
		/// Loads an asset at a given location path
		/// </summary>
		/// <param name="path">Location path</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( string path )
		{
			return Load( new Location( path ) );
		}

		/// <summary>
		/// Loads an asset at a given location path, with specified parameters
		/// </summary>
		/// <param name="path">Location path</param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( string path, LoadParameters parameters )
		{
			return Load( new Location( path ), parameters );
		}

		/// <summary>
		/// Loads an asset from a given source
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( ISource source )
		{
			return Load( source, null );
		}
		
		/// <summary>
		/// Loads an asset at a given location, with specified parameters
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( ISource source, LoadParameters parameters )
		{
			if ( !source.Exists )
			{
				throw new ArgumentException( string.Format( "Invalid asset source {0}", source ) );
			}

			foreach ( IAssetLoader loader in m_Loaders )
			{
				if ( loader.CanLoad( source ) )
				{
					LoadState loadState = new LoadState( loader, source, parameters );
					return loadState.Load( );
				}
			}

			throw new ArgumentException( string.Format( "No loader could load asset {0}", source ) );
		}

		#endregion

		#region Private stuff

		private readonly List< IAssetLoader >	m_Loaders		= new List< IAssetLoader >( );
		private static readonly AssetManager	ms_Singleton	= new AssetManager( );
		
		#endregion

	}
}
