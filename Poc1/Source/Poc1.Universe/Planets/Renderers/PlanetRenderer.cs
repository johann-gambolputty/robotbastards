using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Renderers
{
	/// <summary>
	/// Planet renderer implementation
	/// </summary>
	public class PlanetRenderer : IPlanetRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Planet associated with this renderer</param>
		public PlanetRenderer( IPlanet planet )
		{
			Arguments.CheckNotNull( planet, "planet" );
			m_Planet = planet;
			planet.PlanetRenderer = this;
		}

		/// <summary>
		/// Gets/sets the planet associated with this renderer
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				if ( m_Planet == value )
				{
					return;
				}
				m_Planet = value;
				foreach ( IPlanetEnvironmentRenderer renderer in m_Renderers )
				{
					renderer.Planet = m_Planet;
				}
			}
		}

		#region Renderers

		/// <summary>
		/// Gets/sets the planet marble renderer
		/// </summary>
		public IPlanetMarbleRenderer MarbleRenderer
		{
			get { return m_MarbleRenderer; }
			set
			{
				if ( m_MarbleRenderer != value )
				{
					UnlinkRenderer( m_MarbleRenderer );
					m_MarbleRenderer = value;
					LinkRenderer( m_MarbleRenderer );
				}
			}
		}

		/// <summary>
		/// Gets/sets the atmosphere renderer
		/// </summary>
		public IPlanetAtmosphereRenderer AtmosphereRenderer
		{
			get { return m_AtmosphereRenderer; }
			set
			{
				if ( m_AtmosphereRenderer != value )
				{
					UnlinkRenderer( m_AtmosphereRenderer );
					m_AtmosphereRenderer = value;
					LinkRenderer( m_AtmosphereRenderer );
				}
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
				if ( m_MarbleRenderer != value )
				{
					UnlinkRenderer( m_OceanRenderer );
					m_OceanRenderer = value;
					LinkRenderer( m_OceanRenderer );
				}
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
				if ( m_CloudRenderer != value )
				{
					UnlinkRenderer( m_CloudRenderer );
					m_CloudRenderer = value;
					LinkRenderer( m_CloudRenderer );
				}
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
				if ( m_TerrainRenderer != value )
				{
					UnlinkRenderer( m_TerrainRenderer );
					m_TerrainRenderer = value;
					LinkRenderer( m_TerrainRenderer );
				}
			}
		}

		/// <summary>
		/// Gets/sets the ring renderer
		/// </summary>
		public IPlanetRingRenderer RingRenderer
		{
			get { return m_RingRenderer; }
			set
			{
				if ( m_RingRenderer != value )
				{
					UnlinkRenderer( m_RingRenderer );
					m_RingRenderer = value;
					LinkRenderer( m_RingRenderer );
				}
			}
		}

		#endregion

		#region IUniRenderable Members

		/// <summary>
		/// Renders the deep version of this planet
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void DeepRender( IRenderContext context )
		{
			//	Start profiling
			using ( GameProfiles.Game.Rendering.PlanetRendering.Enter( ) )
			{
				RenderMarble( context );
			}
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
			using ( GameProfiles.Game.Rendering.PlanetRendering.Enter( ) )
			{
				RenderDetail( context );
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Disposes of this renderer
		/// </summary>
		public void Dispose( )
		{
			foreach ( IPlanetEnvironmentRenderer renderer in m_Renderers )
			{
				DisposableHelper.Dispose( renderer );
			}
			m_Renderers.Clear( );
		}

		#endregion

		#region Private Members
		
		private IPlanet						m_Planet;
		private IPlanetAtmosphereRenderer	m_AtmosphereRenderer;
		private IPlanetCloudRenderer		m_CloudRenderer;
		private IPlanetTerrainRenderer		m_TerrainRenderer;
		private IPlanetOceanRenderer		m_OceanRenderer;
		private IPlanetMarbleRenderer		m_MarbleRenderer;
		private IPlanetRingRenderer			m_RingRenderer;
		private readonly List<IPlanetEnvironmentRenderer> m_Renderers = new List<IPlanetEnvironmentRenderer>( );

		/// <summary>
		///	Renders planet marble
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderMarble( IRenderContext context )
		{
			if ( Planet == null )
			{
				throw new InvalidOperationException( "Cannot render planet without planet reference" );
			}
			//	Push marble render transform
			UniCamera.PushAstroRenderTransform( TransformType.LocalToWorld, Planet.Transform );

			if ( MarbleRenderer != null )
			{
				MarbleRenderer.Render( context );
			}
			if ( RingRenderer != null )
			{
				RingRenderer.Render( context );
			}
			if ( AtmosphereRenderer != null )
			{
				AtmosphereRenderer.DeepRender( context );
			}

			//foreach ( KeyValuePair<int, IUniRenderable> kvp in m_OrderedUniRenderers )
			//{
			//    kvp.Value.DeepRender( context );
			//}

			//	Pop transform
			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}

		/// <summary>
		///	Renders planetary detail
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderDetail( IRenderContext context )
		{
			if ( Planet == null )
			{
				throw new InvalidOperationException( "Cannot render planet without planet reference" );
			}
			//	Push transform
			UniCamera.PushRenderTransform( TransformType.LocalToWorld, Planet.Transform );

			//	Render planet
			if ( m_TerrainRenderer != null )
			{
				m_TerrainRenderer.Render( context );
			}
			if ( m_OceanRenderer != null )
			{
				m_OceanRenderer.Render( context );
			}
			if ( m_CloudRenderer != null )
			{
				m_CloudRenderer.Render( context );
			}

			//	Pop transform
			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}

		/// <summary>
		/// Called when a renderer is unassigned from this planet renderer
		/// </summary>
		private void UnlinkRenderer( IPlanetEnvironmentRenderer renderer )
		{
			if ( renderer != null )
			{
				m_Renderers.Remove( renderer );
			}
		}

		/// <summary>
		/// Called when a renderer is assigned to this planet renderer
		/// </summary>
		private void LinkRenderer( IPlanetEnvironmentRenderer renderer )
		{
			if ( renderer != null )
			{
				renderer.Planet = Planet;
				m_Renderers.Add( renderer );
			}
		}

		#endregion

	}
}
