using System;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;

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
		/// <param name="trackChangesToSource">If true, then changes to the texture source are tracked</param>
		public RenderableAssetHandle( ISource source, bool trackChangesToSource ) :
			base( source, trackChangesToSource )
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

	/// <summary>
	/// Non-generic version of <see cref="RenderableAssetHandle{T}"/> 
	/// </summary>
	[Serializable]
	public class RenderableAssetHandle : RenderableAssetHandle< IRenderable >
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
		/// <param name="trackChangesToSource">If true, then changes to the texture source are tracked</param>
		public RenderableAssetHandle( ISource source, bool trackChangesToSource ) :
			base( source, trackChangesToSource )
		{
		}

	}
}
