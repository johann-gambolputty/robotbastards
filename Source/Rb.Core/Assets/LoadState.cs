using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Manages asset loading states
	/// </summary>
	public class LoadState
	{
		#region Public properties

		/// <summary>
		/// The loader that will load the asset
		/// </summary>
		public IAssetLoader Loader
		{
			get { return m_Loader; }
		}

		/// <summary>
		/// The location of the asset
		/// </summary>
		public Location Location
		{
			get { return m_Location; }
		}

		/// <summary>
		/// Access to the loading parameters
		/// </summary>
		public LoadParameters Parameters
		{
			get { return m_Parameters; }
			set { m_Parameters = value; }
		}

		/// <summary>
		/// Gets the loaded asset
		/// </summary>
		public object Asset
		{
			get { return m_Asset; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="loader">Asset loader</param>
		/// <param name="location">Asset location</param>
		/// <param name="parameters">Loading parameters</param>
		public LoadState( IAssetLoader loader, Location location, LoadParameters parameters )
		{
			m_Loader		= loader;
			m_Location		= location;
			m_Parameters	= parameters;
		}

		/// <summary>
		/// Load step. Loads the object
		/// </summary>
		public virtual object Load( )
		{
			AssetsLog.Info( "Loading asset {0}", m_Location );
			m_Asset = m_Loader.Cache.Find( m_Location.Key );
			if ( m_Asset != null )
			{
				AssetsLog.Verbose( "Retrieved cached asset {0}", m_Location );
			}
			else
			{
				m_Asset = m_Loader.Load( m_Location, m_Parameters );
				if ( ( m_Asset != null ) && ( m_Parameters.AddToCache ) )
				{
					AssetsLog.Verbose( "Caching asset {0}", m_Location );
					m_Loader.Cache.Add( m_Location.Key, m_Asset );
				}
			}
			return m_Asset;
		}

		#endregion

		#region Private members

		private readonly IAssetLoader	m_Loader;
		private readonly Location		m_Location;
		private object					m_Asset;
		private LoadParameters			m_Parameters;

		#endregion
	}
}
