using System;
using Rb.Core.Assets;

namespace Rb.Rendering
{
	/// <summary>
	/// Handle to an asset that implements the IRenderable interface. Handle acts as a proxy
	/// </summary>
	/// <typeparam name="T">IRenderable asset type</typeparam>
	[Serializable]
	public class RenderableAssetHandle< T > : AssetHandleT< T >, IRenderable
		where T : class, IRenderable
	{
		/// <summary>
		/// No source - set <see cref="AssetHandle.Source"/> prior to accessing <see cref="AssetHandle.Asset"/>
		/// </summary>
		public RenderableAssetHandle( )
		{
		}

		/// <summary>
		/// Sets the source of the asset. Does not load the asset until <see cref="AssetHandleT{T}.Asset"/> is first accessed
		/// </summary>
		/// <param name="source">Asset source</param>
		public RenderableAssetHandle( ISource source ) :
			base( source )
		{
		}

		/// <summary>
		/// Sets the source of the asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="loadImmediately">If true, the asset is loaded in this constructor. Otherwise, the
		/// asset is loaded on-demand when <see cref="AssetHandleT{T}.Asset"/> is first accessed</param>
		public RenderableAssetHandle( ISource source, bool loadImmediately ) :
			base( source, loadImmediately )
		{
		}

		#region IRenderable Members

		/// <summary>
		/// Renders the asset
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( Asset != null )
			{
				Asset.Render( context );
			}
		}

		#endregion
	}
}
