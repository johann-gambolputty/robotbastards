using System;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets
{
	/// <summary>
	/// Plane interface
	/// </summary>
	public interface IPlanet : IRenderable, IUniRenderable, IBody, IDisposable
	{
		/// <summary>
		/// Called when the planet changes (including changes to the underlying models)
		/// </summary>
		event EventHandler PlanetChanged;

		/// <summary>
		/// Gets/sets the planet's orbit
		/// </summary>
		/// <remarks>
		/// Raises the PlanetChanged event on set to a different orbit
		/// </remarks>
		IOrbit Orbit
		{
			get; set;
		}

		/// <summary>
		/// Gets the planet model
		/// </summary>
		IPlanetModel PlanetModel
		{
			get; set;
		}

		/// <summary>
		/// Gets the planet renderer
		/// </summary>
		IPlanetRenderer PlanetRenderer
		{
			get; set;
		}
	}
}
