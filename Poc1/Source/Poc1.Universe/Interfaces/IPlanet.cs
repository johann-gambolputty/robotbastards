
using System;
using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Planetary body interface
	/// </summary>
	public interface IPlanet : IEntity, IRenderable, IDisposable
	{
		/// <summary>
		/// Gets the description of the orbit of this planet
		/// </summary>
		IOrbit Orbit
		{
			get;
		}

		/// <summary>
		/// Enables/disables the rendering of the terrain of this planet
		/// </summary>
		/// <remarks>
		/// Only one planet should have this flag enabled
		/// </remarks>
		bool EnableTerrainRendering
		{
			get; set;
		}

		/// <summary>
		/// Gets a list of moons that orbit this planet
		/// </summary>
		IList<IPlanet> Moons
		{
			get;
		}
	}
}
