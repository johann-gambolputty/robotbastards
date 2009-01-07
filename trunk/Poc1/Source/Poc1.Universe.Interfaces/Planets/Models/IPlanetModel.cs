
using System;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planetary model
	/// </summary>
	public interface IPlanetModel
	{
		/// <summary>
		/// Raised when the planet model is changed
		/// </summary>
		event EventHandler ModelChanged;

		/// <summary>
		/// Gets/sets the planet's ring model
		/// </summary>
		IPlanetRingModel Rings
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet's atmosphere model
		/// </summary>
		IPlanetAtmosphereModel Atmosphere
		{
			get; set;
		}
	}
}
