using System;
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
		/// Called when the planet changes. This does not occur when a model changes.
		/// </summary>
		event EventHandler PlanetChanged;

		/// <summary>
		/// Gets/sets the radius of this planet in metres
		/// </summary>
		Units.Metres Radius
		{
			get; set;
		}

		#region Models

		/// <summary>
		/// Gets the atmosphere model
		/// </summary>
		ISpherePlanetAtmosphereModel SphereAtmosphereModel
		{
			get;
		}

		/// <summary>
		/// Gets the cloud model
		/// </summary>
		ISpherePlanetCloudModel SphereCloudModel
		{
			get;
		}

		/// <summary>
		/// Gets the terrain model
		/// </summary>
		ISpherePlanetTerrainModel SphereTerrainModel
		{
			get;
		}

		#endregion

		#region Renderers

		/// <summary>
		/// Gets the atmosphere renderer
		/// </summary>
		ISpherePlanetAtmosphereRenderer SphereAtmosphereRenderer
		{
			get;
		}

		/// <summary>
		/// Gets the cloud renderer
		/// </summary>
		ISpherePlanetCloudRenderer SphereCloudRenderer
		{
			get;
		}

		/// <summary>
		/// Gets the terrain renderer
		/// </summary>
		ISpherePlanetTerrainRenderer SphereTerrainRenderer
		{
			get;
		}

		#endregion

	}
}
