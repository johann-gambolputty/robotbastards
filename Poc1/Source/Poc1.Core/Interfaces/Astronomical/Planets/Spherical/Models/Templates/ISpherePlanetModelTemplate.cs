using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models
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