using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Sphere planet model template interface
	/// </summary>
	public interface ISpherePlanetModelTemplate : IPlanetModelTemplate
	{
		/// <summary>
		/// Get/sets the range of valid radii for a spherical planet instance
		/// </summary>
		Range<Units.Metres> Radius { get; set; }
	}
}