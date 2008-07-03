using System;
using System.Collections.Generic;
using System.Configuration;
using Rb.Assets.Interfaces;

namespace Rb.Assets
{
	public class AssetManager
	{
		//	TODO: AP: Asynchronous asset loading

		#region Singleton

		/// <summary>
		/// Gets the class singleton
		/// </summary>
		public static AssetManager Instance
		{
			get { return ms_Singleton; }
		}
 
		#endregion

		#region Setup

		/// <summary>
		/// Initializes the asset manager from the asset setup file specified in the application configuration file
		/// </summary>
		public static void InitializeFromConfiguration( )
		{
			//	Load asset setup
			string assetSetupPath = ConfigurationManager.AppSettings[ "assetSetupPath" ];
			if ( assetSetupPath == null )
			{
				assetSetupPath = "../assetSetup.xml";
			}
			try
			{
				AssetUtils.Setup( assetSetupPath );
			}
			catch ( Exception ex )
			{
				AssetsLog.Exception( ex, string.Format( "Failed to setup asset manager from asset setup file \"{0}\"", assetSetupPath ) );
				throw;
			}
		}

		#endregion

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

		#region Caching

		/// <summary>
		/// Removes an asset from a given source from the cache
		/// </summary>
		public void RemoveAssetFromCache( ISource asset )
		{
			foreach ( IAssetLoader loader in Loaders )
			{
				if ( ( loader.Cache != null ) && ( loader.Cache.Remove( asset.GetHashCode( ) ) ) )
				{
					return;
				}
			}
		}

		/// <summary>
		/// Clears all asset caches
		/// </summary>
		public void ClearAllCaches( )
		{
			foreach ( IAssetLoader loader in Loaders )
			{
				if ( loader.Cache != null )
				{
					loader.Cache.Clear( );
				}
			}
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
			ISource loc = Locations.NewLocation( path );
			return Load( loc );
		}

		/// <summary>
		/// Loads an asset at a given location path, with specified parameters
		/// </summary>
		/// <param name="path">Location path</param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( string path, LoadParameters parameters )
		{
			ISource loc = Locations.NewLocation( path );
			return Load( loc, parameters );
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
			if ( source == null )
			{
				throw new ArgumentNullException( "source", "Asset source was null - check that location exists" );
			}

			foreach ( IAssetLoader loader in m_Loaders )
			{
				if ( loader.CanLoad( source ) )
				{
					LoadState loadState = new LoadState( loader, source, parameters );
					return loadState.Load( );
				}
			}

			throw new ArgumentException( string.Format( "No loader could load asset \"{0}\"", source ) );
		}

		#endregion

		#region Private stuff

		private readonly List< IAssetLoader > m_Loaders = new List< IAssetLoader >( );
		private readonly static AssetManager ms_Singleton = new AssetManager( );
		
		#endregion
	}
}
