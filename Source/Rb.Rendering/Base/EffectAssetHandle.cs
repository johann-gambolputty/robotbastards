using System;
using System.Collections.Generic;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Base
{
	/// <summary>
	/// An implementation of <see cref="IEffect"/> that wraps up an effect asset
	/// </summary>
	[Serializable]
	public class EffectAssetHandle : AssetHandleT< IEffect >, IEffect
	{
		#region Construction
		
		/// <summary>
		/// No source - set <see cref="AssetHandle.Source"/> prior to accessing <see cref="AssetHandle.Asset"/>, or calling any IEffect members
		/// </summary>
		public EffectAssetHandle( )
		{
		}

		/// <summary>
		/// Sets the source of the asset. Does not load the asset until <see cref="AssetHandle.Asset"/> is first accessed
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="trackChangesToSource">If true, then changes to the texture source are tracked</param>
		public EffectAssetHandle( ISource source, bool trackChangesToSource ) :
			base( source, trackChangesToSource )
		{
		}

		#endregion

		#region IEffect Members

		/// <summary>
		/// Gets the techniques supported by this effect
		/// </summary>
		public IDictionary<string, ITechnique> Techniques
		{
			get { return Asset.Techniques; }
		}

		/// <summary>
		/// Gets the parameters supported by this effect
		/// </summary>
		public IDictionary<string, IEffectParameter> Parameters
		{
			get { return Asset.Parameters; }
		}

		/// <summary>
		/// Finds a technique in this effect that can substitute the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>
		/// Returns an ITechnique from this effect that can substitute technique. If none
		/// can be found, technique is returned.
		/// </returns>
		public ITechnique SubstituteTechnique(ITechnique technique)
		{
			return Asset.SubstituteTechnique( technique );
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Begins the pass
		/// </summary>
		public void Begin( )
		{
			Asset.Begin( );
		}

		/// <summary>
		/// Ends the pass
		/// </summary>
		public void End( )
		{
			Asset.End( );
		}

		#endregion
	}
}
