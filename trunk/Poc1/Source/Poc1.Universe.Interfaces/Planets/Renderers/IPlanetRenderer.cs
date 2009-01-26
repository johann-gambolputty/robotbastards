
using System;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Renderers
{
	/// <summary>
	/// Planet renderer interface
	/// </summary>
	public interface IPlanetRenderer: IRenderable, IUniRenderable, IDisposable
	{
		/// <summary>
		/// Gets/sets the planet associated with this renderer
		/// </summary>
		IPlanet Planet
		{
			get; set;
		}

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

		/// <summary>
		/// Gets/sets the ring renderer
		/// </summary>
		IPlanetRingRenderer RingRenderer
		{
			get; set;
		}

		#endregion
	}
}
