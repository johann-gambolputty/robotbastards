
using System;
using Rb.Core.Components.Generic;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planetary model
	/// </summary>
	public interface IPlanetModel : IComposite<IPlanetEnvironmentModel>, IDisposable
	{
		/// <summary>
		/// Raised when the planet model is changed
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet associated with this model
		/// </summary>
		IPlanet Planet
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet's ring model
		/// </summary>
		IPlanetRingModel RingModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet's atmosphere model
		/// </summary>
		IPlanetAtmosphereModel AtmosphereModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet's cloud model
		/// </summary>
		IPlanetCloudModel CloudModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet ocean model
		/// </summary>
		IPlanetOceanModel OceanModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet terrain model
		/// </summary>
		IPlanetTerrainModel TerrainModel
		{
			get; set;
		}
	}
}
