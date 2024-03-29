using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;

namespace Poc1.Universe.Interfaces.Planets.Spherical
{
	/// <summary>
	/// Spherical planet interface
	/// </summary>
	public interface ISpherePlanet : IPlanet
	{
		/// <summary>
		/// Gets the current sphere planet model
		/// </summary>
		new ISpherePlanetModel PlanetModel
		{
			get; set;
		}

		/// <summary>
		/// Gets the current sphere planet renderer
		/// </summary>
		new ISpherePlanetRenderer PlanetRenderer
		{
			get; set;
		}

	}
}
