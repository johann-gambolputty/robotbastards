using System;
using System.ComponentModel;

namespace Rb.Core.Assets
{
	/// <summary>
	/// A handle to an asset
	/// </summary>
	/// <remarks>
	/// Useful for serialization (the asset location is serialized, the asset itself is not), and making asset
	/// change management transparent (so long as the asset is accessed through <see cref="Asset"/>, the handle
	/// can track changes to the location and update the asset accordingly).
	/// Use <see cref="AssetProxy"/> to create decorator pattern asset handles that derive from AssetHandle, and
	/// implement asset interfaces.
	/// </remarks>
	[Serializable]
	public class AssetHandle
	{
		/// <summary>
		/// No source - set <see cref="AssetHandle.Source"/> prior to accessing <see cref="AssetHandle.Asset"/>
		/// </summary>
		public AssetHandle( )
		{
		}

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
			set { m_Source = value; }
		}

		/// <summary>
		/// Gets/sets load parameters
		/// </summary>
		public LoadParameters LoadParameters
		{
			get { return m_Params; }
			set { m_Params = value; }
		}

		/// <summary>
		/// Asset object
		/// </summary>
		/// <remarks>
		/// If the asset is not yet loaded, the first time this property is accessed, the handle will attempt
		/// to load it.
		/// </remarks>
		[ ReadOnly( true ) ]
		public object Asset
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

		private LoadParameters m_Params;
		private ISource m_Source;

		[NonSerialized]
		private object m_Asset;

		[NonSerialized]
		private bool m_LoadFailed;

		/// <summary>
		/// Loads the asset
		/// </summary>
		private void LoadAsset( )
		{
			try
			{
				m_Asset = AssetManager.Instance.Load( m_Source, m_Params );
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
