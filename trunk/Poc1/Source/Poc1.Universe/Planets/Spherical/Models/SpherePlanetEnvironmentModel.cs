using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Planets.Models;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Spherical planet environment model base class
	/// </summary>
	public abstract class SpherePlanetEnvironmentModel : PlanetEnvironmentModel
	{
		/// <summary>
		/// Gets the current sphere planet that the planet model is attached to
		/// </summary>
		public ISpherePlanet SpherePlanet
		{
			get { return ( ISpherePlanet )Planet; }
		}

		/// <summary>
		/// Gets the current sphere planet model that contains this environment model
		/// </summary>
		public ISpherePlanetModel SpherePlanetModel
		{
			get { return ( ISpherePlanetModel )PlanetModel; }
		}
	}
}
