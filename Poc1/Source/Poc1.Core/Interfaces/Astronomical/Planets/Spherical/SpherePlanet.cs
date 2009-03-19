using System;
using System.Collections.Generic;
using System.Text;
using Poc1.Core.Classes.Astronomical.Planets.Generic;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical
{
	/// <summary>
	/// Spherical planet implementation
	/// </summary>
	public class SpherePlanet : GenericPlanet<ISpherePlanetModel, SpherePlanetModel, ISpherePlanetRenderer, SpherePlanetRenderer>, ISpherePlanet
	{
	}
}
