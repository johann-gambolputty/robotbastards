using System;
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
		#region IPlanet Members

		/// <summary>
		/// Gets/sets the planet's orbit
		/// </summary>
		public IOrbit Orbit
		{
			get { return m_Orbit; }
			set { m_Orbit = value; }
		}

		#region Models

		/// <summary>
		/// Gets/sets the atmosphere model
		/// </summary>
		public IPlanetAtmosphereModel AtmosphereModel
		{
			get { return m_AtmosphereModel; }
			set
			{
				 m_AtmosphereModel = value;
				 OnModelAssigned( value );
			}
		}

		/// <summary>
		/// Gets/sets the planet's ocean model
		/// </summary>
		public IPlanetOceanModel OceanModel
		{
			get { return m_OceanModel; }
			set
			{
				m_OceanModel = value;
				OnModelAssigned( value );
			}
		}

		/// <summary>
		/// Gets/sets the cloud model
		/// </summary>
		public IPlanetCloudModel CloudModel
		{
			get { return m_CloudModel; }
			set
			{
				m_CloudModel = value;
				OnModelAssigned( value );
			}
		}

		/// <summary>
		/// Gets/sets the terrain model
		/// </summary>
		public IPlanetTerrainModel TerrainModel
		{
			get { return m_TerrainModel; }
			set
			{
				m_TerrainModel = value;
				OnModelAssigned( value );
			}
		}

		#endregion

		#region Renderers

		/// <summary>
		/// Gets/sets the atmosphere renderer
		/// </summary>
		public IPlanetAtmosphereRenderer AtmosphereRenderer
		{
			get { return m_AtmosphereRenderer; }
			set
			{
				m_AtmosphereRenderer = value;
				OnRendererAssigned( value );
			}
		}

		/// <summary>
		/// Gets/sets the planet's ocean renderer
		/// </summary>
		public IPlanetOceanRenderer OceanRenderer
		{
			get { return m_OceanRenderer; }
			set
			{
				m_OceanRenderer = value;
				OnRendererAssigned( value );
			}
		}

		/// <summary>
		/// Gets/sets the cloud renderer
		/// </summary>
		public IPlanetCloudRenderer CloudRenderer
		{
			get { return m_CloudRenderer; }
			set
			{
				m_CloudRenderer = value;
				OnRendererAssigned( value );
			}
		}

		/// <summary>
		/// Gets/sets the terrain renderer
		/// </summary>
		public IPlanetTerrainRenderer TerrainRenderer
		{
			get { return m_TerrainRenderer; }
			set
			{
				m_TerrainRenderer = value;
				OnRendererAssigned( value );
			}
		}

		#endregion

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
			if ( m_OceanRenderer != null )
			{
				m_OceanRenderer.Render( context );
			}
			if ( m_TerrainRenderer != null )
			{
				m_TerrainRenderer.Render( context );
			}
			if ( m_CloudRenderer != null )
			{
				m_CloudRenderer.Render( context );
			}
			if ( m_AtmosphereRenderer != null )
			{
				m_AtmosphereRenderer.Render( context );
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

		/// <summary>
		/// Gets/sets the name of this planet
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes this object
		/// </summary>
		public void Dispose( )
		{
			Dispose( m_AtmosphereModel as IDisposable );
			Dispose( m_CloudModel as IDisposable );
			Dispose( m_OceanModel as IDisposable );
			Dispose( m_TerrainModel as IDisposable );
			Dispose( m_AtmosphereRenderer as IDisposable );
			Dispose( m_CloudRenderer as IDisposable );
			Dispose( m_TerrainRenderer as IDisposable );
			Dispose( m_OceanRenderer as IDisposable );
		}

		#endregion

		#region Private Members

		private string						m_Name;
		private UniTransform				m_Transform = new UniTransform( );
		private IOrbit						m_Orbit;
		private IPlanetAtmosphereModel		m_AtmosphereModel;
		private IPlanetCloudModel			m_CloudModel;
		private IPlanetOceanModel			m_OceanModel;
		private IPlanetTerrainModel			m_TerrainModel;
		private IPlanetAtmosphereRenderer	m_AtmosphereRenderer;
		private IPlanetCloudRenderer		m_CloudRenderer;
		private IPlanetTerrainRenderer		m_TerrainRenderer;
		private IPlanetOceanRenderer		m_OceanRenderer;

		/// <summary>
		/// Called when a model is assigned to this planet
		/// </summary>
		private void OnModelAssigned( IPlanetEnvironmentModel model )
		{
			if ( model != null )
			{
				model.Planet = this;
			}
		}

		/// <summary>
		/// Called when a renderer is assigned to this planet
		/// </summary>
		private void OnRendererAssigned( IPlanetEnvironmentRenderer renderer )
		{
			if ( renderer != null )
			{
				renderer.Planet = this;
			}
		}

		/// <summary>
		/// Disposes of the specified object, if it is not null
		/// </summary>
		private static void Dispose( IDisposable disposable )
		{
			if ( disposable != null )
			{
				disposable.Dispose( );
			}
		}

		#endregion

	}
}
