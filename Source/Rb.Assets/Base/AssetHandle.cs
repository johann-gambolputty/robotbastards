using System;
using System.ComponentModel;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Base
{
	/// <summary>
	/// A handle to an asset
	/// </summary>
	/// <remarks>
	/// Useful for serialization (the asset location is serialized, the asset itself is not), and making asset
	/// change management transparent (so long as the asset is accessed through <see cref="Asset"/>, the handle
	/// can track changes to the location and update the asset accordingly).
	/// Use <see cref="AssetHandleProxy"/> to create decorator pattern asset handles that derive from AssetHandle, and
	/// implement asset interfaces.
	/// </remarks>
	[Serializable]
	public class AssetHandle
	{
		/// <summary>
		/// Event, invoked when the asset is reloaded
		/// </summary>
		public event Action<ISource> OnReload
		{
			add { m_OnReload += value; }
			remove { m_OnReload -= value; }
		}

		/// <summary>
		/// No source - call <see cref="AssetHandle.SetSource"/> prior to accessing <see cref="AssetHandle.Asset"/>
		/// </summary>
		public AssetHandle( )
		{
		}

		/// <summary>
		/// Sets the source of the asset. Does not load the asset until <see cref="Asset"/> is first accessed
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="trackChangesToSource">If true, then changes to the source are tracked</param>
		public AssetHandle( ISource source, bool trackChangesToSource )
		{
			SetSource( source, trackChangesToSource );
		}

		/// <summary>
		/// Sets the source of the asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="trackChangesToSource">If true, then changes to the source are caught, and the asset re-loaded</param>
		public void SetSource( ISource source, bool trackChangesToSource )
		{
			if ( m_Source != null )
			{
				m_Source.SourceChanged -= ReloadAsset;
			}
			m_Source = source;
			if ( ( m_Source != null ) && ( trackChangesToSource ) )
			{
				m_Source.SourceChanged += ReloadAsset;
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
			set { m_Asset = value; }
		}

		#region Private stuff

		private LoadParameters m_Params;
		private ISource m_Source;

		[NonSerialized]
		private object m_Asset;

		[NonSerialized]
		private bool m_LoadFailed;

		[NonSerialized]
		private Action<ISource> m_OnReload;

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
		
		/// <summary>
		/// Reloads the asset. Used as a listener to the ISource.SourceChanged event
		/// </summary>
		private void ReloadAsset( ISource source )
		{
			if ( m_Asset != null )
			{
				AssetManager.Instance.RemoveAssetFromCache( source );
			}
			m_Asset = null;
			if ( m_OnReload != null )
			{
				m_OnReload( source );
			}
		}

		#endregion
	}

}
