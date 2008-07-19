
using System;
using System.Collections.Generic;
using Poc1.Fast.Terrain;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Planetary body interface
	/// </summary>
	public interface IPlanet : IEntity, IRenderable, IDisposable
	{
		#region Models

		/// <summary>
		/// Gets the description of the orbit of this planet
		/// </summary>
		IOrbit Orbit
		{
			get;
		}

		/// <summary>
		/// Gets the planetary atmosphere model
		/// </summary>
		IPlanetAtmosphereModel Atmosphere
		{
			get;
		}

		/// <summary>
		/// Gets the planetary terrain model
		/// </summary>
		IPlanetTerrainModel Terrain
		{
			get;
		}

		/// <summary>
		/// Regenerates the planet's terrain mesh.
		/// </summary>
		void RegenerateTerrain( );

		/// <summary>
		/// Gets/sets the sea level of this planet. If this is zero, the planet has no seas. Otherwise, it's measured
		/// as an offset, in metres, from the planet's radius
		/// </summary>
		float SeaLevel
		{
			get; set;
		}

		/// <summary>
		/// Gets the range of heights of the planet's terrain
		/// </summary>
		float TerrainHeightRange
		{
			get;
		}


		/// <summary>
		/// Gets a list of moons that orbit this planet
		/// </summary>
		IList<IPlanet> Moons
		{
			get;
		}

		#endregion

		#region Rendering

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
		/// Gets the planet atmosphere renderer
		/// </summary>
		IAtmosphereRenderer AtmosphereRenderer
		{
			get;
		}

		#endregion
	}
}
