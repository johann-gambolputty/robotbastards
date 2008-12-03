using System;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets
{
	/// <summary>
	/// Planet render pass order enumeration
	/// </summary>
	public enum PlanetRenderPassOrder : int
	{
		/// <summary>
		/// Renderer is not rendered at all for the pass
		/// </summary>
		NotRendered = -2,

		/// <summary>
		/// Renderer is not rendered in any particular order
		/// </summary>
		Unordered = -1,

		/// <summary>
		/// Renderer is rendered first in a pass
		/// </summary>
		First = 0
	}

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

		#region Models and Renderers

		/// <summary>
		/// Gets an environment model (e.g. <see cref="IPlanetRingModel"/>) from the planet
		/// </summary>
		/// <typeparam name="T">Environment model</typeparam>
		/// <returns>Returns the specified environment model</returns>
		T Model<T>( ) where T : class, IPlanetEnvironmentModel;

		/// <summary>
		/// Gets an environment renderer (e.g. <see cref="IPlanetMarbleRenderer"/> from the planet
		/// </summary>
		/// <typeparam name="T">Environment renderer</typeparam>
		/// <returns>Returns the specifeid environment renderer</returns>
		T Renderer<T>( ) where T : class, IPlanetEnvironmentRenderer;

		/// <summary>
		/// Adds an environment model to the planet
		/// </summary>
		/// <param name="model">Model to add</param>
		void AddModel( IPlanetEnvironmentModel model );

		/// <summary>
		/// Adds an environment renderer to the planet. Can be retrieved by calling <see cref="Renderer{T}"/>
		/// </summary>
		/// <param name="renderer">Renderer to add</param>
		/// <param name="renderOrder">Ordinal determining when the renderer is used during rendering.</param>
		/// <param name="deepRenderOrder">Ordinal determining when the renderer is used during deep rendering. 0 is first.</param>
		void AddRenderer( IPlanetEnvironmentRenderer renderer, PlanetRenderPassOrder renderOrder, PlanetRenderPassOrder deepRenderOrder );

		#endregion

		#region Models

		/// <summary>
		/// Gets/sets the planet's ring model
		/// </summary>
		IPlanetRingModel RingModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the atmosphere model
		/// </summary>
		IPlanetAtmosphereModel AtmosphereModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the ocean model
		/// </summary>
		IPlanetOceanModel OceanModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the cloud model
		/// </summary>
		IPlanetCloudModel CloudModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the terrain model
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
