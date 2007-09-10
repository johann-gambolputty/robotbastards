using System;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Handle to an asset
	/// </summary>
	/// <typeparam name="T">Asset type</typeparam>
	[Serializable]
	public class AssetHandle< T > where T : class
	{
		/// <summary>
		/// Sets the source of the asset. Does not load the asset until <see cref="Asset"/> is first accessed
		/// </summary>
		/// <param name="source">Asset source</param>
		public AssetHandle( ISource source )
		{
			m_Source = source;
		}

		/// <summary>
		/// Sets the source of the asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="loadImmediately">If true, the asset is loaded in this constructor. Otherwise, the
		/// asset is loaded on-demand when <see cref="Asset"/> is first accessed</param>
		public AssetHandle( ISource source, bool loadImmediately )
		{
			m_Source = source;
			if ( loadImmediately )
			{
				LoadAsset( );
			}
		}

		/// <summary>
		/// Asset source
		/// </summary>
		public ISource Source
		{
			get { return m_Source; }
		}

		/// <summary>
		/// Asset object
		/// </summary>
		/// <remarks>
		/// If the asset is not yet loaded, the first time this property is accessed, the handle will attempt
		/// to load it.
		/// </remarks>
		public T Asset
		{
			get
			{
				if ( ( m_Asset == null ) && ( !m_LoadFailed ) )
				{
					LoadAsset( );
				}

				return m_Asset;
			}
		}


		#region Private stuff

		private readonly ISource m_Source;

		[NonSerialized]
		private T m_Asset;

		[NonSerialized]
		private bool m_LoadFailed;

		private void LoadAsset( )
		{
			try
			{
				m_Asset = ( T )AssetManager.Instance.Load( m_Source );
				m_LoadFailed = false;
			}
			catch ( Exception ex )
			{
				m_LoadFailed = true;
				throw new InvalidOperationException( string.Format( "Failed to load asset handle \"{0}\"", m_Source ), ex );
			}
		}

		#endregion
	}
}
