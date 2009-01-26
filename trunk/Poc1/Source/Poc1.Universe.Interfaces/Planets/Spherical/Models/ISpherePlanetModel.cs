
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

		/// <summary>
		/// Gets the sphere planet ring model
		/// </summary>
		ISpherePlanetRingModel SphereRingModel
		{
			get;
		}

		/// <summary>
		/// Gets the sphere atmosphere model
		/// </summary>
		ISpherePlanetAtmosphereModel SphereAtmosphereModel
		{
			get;
		}

		/// <summary>
		/// Gets the sphere cloud model
		/// </summary>
		ISpherePlanetCloudModel SphereCloudModel
		{
			get;
		}

		/// <summary>
		/// Gets the sphere terrain model
		/// </summary>
		ISpherePlanetTerrainModel SphereTerrainModel
		{
			get;
		}
	}
}
