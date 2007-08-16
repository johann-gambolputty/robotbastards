using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Singleton class that manages all IAssetProvider obejcts
	/// </summary>
	public class AssetProviders : IEnumerable< IAssetProvider >
	{
		#region Public properties

		/// <summary>
		/// Gets the asset providers singleton
		/// </summary>
		public static AssetProviders Instance
		{
			get { return ms_Singleton;  }
		}

		/// <summary>
		/// Adds an asset provider
		/// </summary>
		/// <param name="provider">Provider to add</param>
		public void Add( IAssetProvider provider )
		{
			m_Providers.Add( provider );
		}

		#endregion

		#region IEnumerable<IAssetProvider> Members

		/// <summary>
		/// Returns an enumerator that can step through all the asset providers
		/// </summary>
		public IEnumerator< IAssetProvider > GetEnumerator()
		{
			return m_Providers.GetEnumerator( );
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that can step through all the asset providers
		/// </summary>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator( )
		{
			return m_Providers.GetEnumerator( );
		}

		#endregion

		#region Private stuff

		private readonly List< IAssetProvider > m_Providers		= new List< IAssetProvider >( );
		private static readonly AssetProviders	ms_Singleton	= new AssetProviders( );

		#endregion
	}
}
