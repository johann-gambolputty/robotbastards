using System;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Handle to an asset
	/// </summary>
	/// <typeparam name="T">Asset type</typeparam>
	[Serializable]
	public class AssetHandleT< T > : AssetHandle
		where T : class
	{
		/// <summary>
		/// No source - set <see cref="AssetHandle.Source"/> prior to accessing <see cref="AssetHandle.Asset"/>
		/// </summary>
		public AssetHandleT( )
		{
		}

		/// <summary>
		/// Sets the source of the asset. Does not load the asset until <see cref="Asset"/> is first accessed
		/// </summary>
		/// <param name="source">Asset source</param>
		public AssetHandleT( ISource source ) :
			base( source )
		{
		}

		/// <summary>
		/// Sets the source of the asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="loadImmediately">If true, the asset is loaded in this constructor. Otherwise, the
		/// asset is loaded on-demand when <see cref="Asset"/> is first accessed</param>
		public AssetHandleT( ISource source, bool loadImmediately ) :
			base( source, loadImmediately )
		{
		}

		/// <summary>
		/// Asset object
		/// </summary>
		/// <remarks>
		/// If the asset is not yet loaded, the first time this property is accessed, the handle will attempt
		/// to load it.
		/// </remarks>
		public new T Asset
		{
			get { return ( T )base.Asset; }
		}


	}
}
