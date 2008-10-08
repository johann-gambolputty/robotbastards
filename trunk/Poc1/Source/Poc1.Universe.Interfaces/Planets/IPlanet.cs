using System;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets
{
	/// <summary>
	/// Plane interface
	/// </summary>
	public interface IPlanet : IRenderable, IBody, IDisposable
	{
		/// <summary>
		/// Gets/sets the planet's orbit
		/// </summary>
		IOrbit Orbit
		{
			get; set;
		}

		#region Models

		/// <summary>
		/// Gets the atmosphere model
		/// </summary>
		IPlanetAtmosphereModel AtmosphereModel
		{
			get; set;
		}

		/// <summary>
		/// Gets the ocean model
		/// </summary>
		IPlanetOceanModel OceanModel
		{
			get; set;
		}

		/// <summary>
		/// Gets the cloud model
		/// </summary>
		IPlanetCloudModel CloudModel
		{
			get; set;
		}

		/// <summary>
		/// Gets the terrain model
		/// </summary>
		IPlanetTerrainModel TerrainModel
		{
			get; set;
		}

		#endregion

		#region Renderers

		/// <summary>
		/// Gets/sets the planet marble renderer
		/// </summary>
		IPlanetMarbleRenderer MarbleRenderer
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the atmosphere renderer
		/// </summary>
		IPlanetAtmosphereRenderer AtmosphereRenderer
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the ocean renderer
		/// </summary>
		IPlanetOceanRenderer OceanRenderer
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the cloud renderer
		/// </summary>
		IPlanetCloudRenderer CloudRenderer
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the terrain renderer
		/// </summary>
		IPlanetTerrainRenderer TerrainRenderer
		{
			get; set;
		}

		#endregion
	}
}
