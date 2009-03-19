
using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Generic.Models
{

	/// <summary>
	/// Generic planet environment model interface
	/// </summary>
	/// <typeparam name="TPlanet">Planet type</typeparam>
	/// <typeparam name="TPlanetModel">Planet model</typeparam>
	public interface IGenericPlanetEnvironmentModel<TPlanet, TPlanetModel> : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets the planet that this model is attached to (via the planet model)
		/// </summary>
		new TPlanet Planet
		{
			get;
		}

		/// <summary>
		/// Gets the planet model that this model is a part of
		/// </summary>
		new TPlanetModel PlanetModel
		{
			get;
			set;
		}
	}
}
