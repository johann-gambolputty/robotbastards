
using Poc1.Universe.Interfaces.Planets.Models;

namespace Poc1.Universe.Interfaces.Planets.Spherical.Models
{
	/// <summary>
	/// Sphere planet model interface
	/// </summary>
	public interface ISpherePlanetModel : IPlanetModel
	{
		/// <summary>
		/// Gets/sets the planet's radius
		/// </summary>
		Units.Metres Radius
		{
			get; set;
		}
	}
}
