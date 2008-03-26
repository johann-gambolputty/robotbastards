using System;
using System.ComponentModel;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Base
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
		/// <param name="trackChangesToSource">If true, then changes to the source are tracked</param>
		public AssetHandleT( ISource source, bool trackChangesToSource ) :
			base( source, trackChangesToSource )
		{
		}

		/// <summary>
		/// Asset object
		/// </summary>
		/// <remarks>
		/// If the asset is not yet loaded, the first time this property is accessed, the handle will attempt
		/// to load it.
		/// </remarks>
		[ Browsable( false ) ]
		public new T Asset
		{
			get { return ( T )base.Asset; }
		}
	}
}
