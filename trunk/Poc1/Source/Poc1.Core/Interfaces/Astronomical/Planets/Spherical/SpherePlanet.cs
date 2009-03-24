using Poc1.Core.Classes.Astronomical.Planets.Generic;
using Poc1.Core.Classes.Astronomical.Planets.Spherical;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Spherical planet implementation
	/// </summary>
	public class SpherePlanet : GenericPlanet<ISpherePlanetModel, SpherePlanetModel, ISpherePlanetRenderer, SpherePlanetRenderer>, ISpherePlanet
	{
	}
}
