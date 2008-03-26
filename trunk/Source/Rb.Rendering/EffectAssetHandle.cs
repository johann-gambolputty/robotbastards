using System;
using System.Collections.Generic;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;

namespace Rb.Rendering
{
	/// <summary>
	/// An implementation of <see cref="IEffect"/> that wraps up an effect asset
	/// </summary>
	[Serializable]
	public class EffectAssetHandle : AssetHandleT< IEffect >, IEffect
	{
		#region Construction
		
		/// <summary>
		/// No source - set <see cref="AssetHandle.Source"/> prior to accessing <see cref="AssetHandle.Asset"/>
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
		/// Gets the techniques making up this effect
		/// </summary>
		public ICollection<ITechnique> Techniques
		{
			get { return Asset.Techniques; }
		}

		/// <summary>
		/// Gets a shader parameter by its name
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <returns>Returns the named shader parameter, or null if it doesn't exist</returns>
		public ShaderParameter GetParameter( string name )
		{
			return Asset.GetParameter( name );
		}

		/// <summary>
		/// Gets a technique from its name
		/// </summary>
		/// <param name="name">Name of the technique</param>
		/// <returns>Returns the named technique</returns>
		/// <exception cref="System.ArgumentException">Thrown if name does not correspond to a technique in the current effect</exception>
		public ITechnique GetTechnique(string name)
		{
			return Asset.GetTechnique( name );
		}

		/// <summary>
		/// Finds a technique in this effect that can substitute the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>
		/// Returns an ITechnique from this effect that can substitute technique. If none
		/// can be found, technique is returned.
		/// </returns>
		public ITechnique SubstituteTechnique( ITechnique technique )
		{
			return Asset.SubstituteTechnique( technique );
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Begins the pass
		/// </summary>
		public void Begin()
		{
			Asset.Begin( );
		}

		/// <summary>
		/// Ends the pass
		/// </summary>
		public void End()
		{
			Asset.End( );
		}

		#endregion
	}
}
