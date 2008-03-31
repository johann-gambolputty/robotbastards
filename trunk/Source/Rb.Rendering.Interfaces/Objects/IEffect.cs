using System.Collections.Generic;

namespace Rb.Rendering.Interfaces.Objects
{
	public interface IEffect : IPass
	{
		/// <summary>
		/// Gets the techniques supported by this effect
		/// </summary>
		IDictionary<string, ITechnique> Techniques
		{
			get;
		}

		/// <summary>
		/// Gets the parameters supported by this effect
		/// </summary>
		IDictionary<string, IEffectParameter> Parameters
		{
			get;
		}

		/// <summary>
		/// Finds a technique in this effect that can substitute the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>
		/// Returns an ITechnique from this effect that can substitute technique. If none
		/// can be found, technique is returned.
		/// </returns>
		ITechnique SubstituteTechnique( ITechnique technique );
	}
}
