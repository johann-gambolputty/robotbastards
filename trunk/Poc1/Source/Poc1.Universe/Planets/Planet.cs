using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using IPlanet=Poc1.Universe.Interfaces.Planets.IPlanet;

namespace Poc1.Universe.Planets
{
	/// <summary>
	/// Planet base implementation
	/// </summary>
	public class Planet : IPlanet
	{

		#region Models

		/// <summary>
		/// Gets/sets the atmosphere model
		/// </summary>
		public IPlanetAtmosphereModel AtmosphereModel
		{
			get { return m_AtmosphereModel; }
			set { m_AtmosphereModel = value; }
		}

		/// <summary>
		/// Gets/sets the planet's ocean model
		/// </summary>
		public IPlanetOceanModel OceanModel
		{
			get { return m_OceanModel; }
			set { m_OceanModel = value; }
		}

		/// <summary>
		/// Gets/sets the cloud model
		/// </summary>
		public IPlanetCloudModel CloudModel
		{
			get { return m_CloudModel; }
			set { m_CloudModel = value; }
		}

		/// <summary>
		/// Gets/sets the terrain model
		/// </summary>
		public IPlanetTerrainModel TerrainModel
		{
			get { return m_TerrainModel; }
			set { m_TerrainModel = value; }
		}

		#endregion

		#region Renderers

		/// <summary>
		/// Gets/sets the atmosphere renderer
		/// </summary>
		public IPlanetAtmosphereRenderer AtmosphereRenderer
		{
			get { return m_AtmosphereRenderer; }
			set { m_AtmosphereRenderer = value; }
		}

		/// <summary>
		/// Gets/sets the planet's ocean renderer
		/// </summary>
		public IPlanetOceanRenderer OceanRenderer
		{
			get { return m_OceanRenderer; }
			set { m_OceanRenderer = value; }
		}

		/// <summary>
		/// Gets/sets the cloud renderer
		/// </summary>
		public IPlanetCloudRenderer CloudRenderer
		{
			get { return m_CloudRenderer; }
			set { m_CloudRenderer = value; }
		}

		/// <summary>
		/// Gets/sets the terrain renderer
		/// </summary>
		public IPlanetTerrainRenderer TerrainRenderer
		{
			get { return m_TerrainRenderer; }
			set { m_TerrainRenderer = value; }
		}

		#endregion


		#region IRenderable Members
		
		/// <summary>
		/// Renders the planet
		/// </summary>
		/// <param name="context">Rendering context</param>
		public virtual void Render( IRenderContext context )
		{
			//	Start profiling
			GameProfiles.Game.Rendering.PlanetRendering.Begin( );

			//	Push transform
			UniCamera.PushRenderTransform( TransformType.LocalToWorld, Transform );

			//	Render planet
			if ( m_TerrainRenderer != null )
			{
				m_TerrainRenderer.Render( context );
			}
			if ( m_CloudRenderer != null )
			{
				m_CloudRenderer.Render( context );
			}

			//	Pop transform
			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );

			//	End profiling
			GameProfiles.Game.Rendering.PlanetRendering.End( );
		}

		#endregion


		#region IBody Members

		/// <summary>
		/// Gets the transform for this planet
		/// </summary>
		public UniTransform Transform
		{
			get { return m_Transform; }
		}

		#endregion

		#region Private Members

		private UniTransform				m_Transform = new UniTransform( );
		private IPlanetAtmosphereModel		m_AtmosphereModel;
		private IPlanetCloudModel			m_CloudModel;
		private IPlanetOceanModel			m_OceanModel;
		private IPlanetTerrainModel			m_TerrainModel;
		private IPlanetAtmosphereRenderer	m_AtmosphereRenderer;
		private IPlanetCloudRenderer		m_CloudRenderer;
		private IPlanetTerrainRenderer		m_TerrainRenderer;
		private IPlanetOceanRenderer		m_OceanRenderer;
		
		#endregion

	}
}
