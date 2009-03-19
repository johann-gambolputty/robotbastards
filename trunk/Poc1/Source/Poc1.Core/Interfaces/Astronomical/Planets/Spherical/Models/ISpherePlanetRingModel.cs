using Poc1.Core.Interfaces.Astronomical.Planets.Models;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models
{

	/// <summary>
	/// Sphere planet ring model interface
	/// </summary>
	public interface ISpherePlanetRingModel : IPlanetRingModel, ISpherePlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the distance in metres from the centre of the planet to the inner edge of the rings
		/// </summary>
		Units.Metres InnerRadius
		{
			get; set;
		}
	}

}
