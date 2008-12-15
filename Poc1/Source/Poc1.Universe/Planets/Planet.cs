using System;
using System.Collections.Generic;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Core.Components;
using Rb.Core.Utils;
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
		/// <summary>
		/// Default constructor
		/// </summary>
		public Planet( )
		{
			m_Models = new ObjectTypeMap<IPlanetEnvironmentModel>( ObjectTypeMap.GetAllInterfaceTypes );
			m_Renderers = new ObjectTypeMap<IPlanetEnvironmentRenderer>( ObjectTypeMap.GetAllInterfaceTypes );
		}

		#region IPlanet Members

		/// <summary>
		/// Called when the planet changes
		/// </summary>
		public event EventHandler PlanetChanged;

		/// <summary>
		/// Gets/sets the planet's orbit
		/// </summary>
		/// <remarks>
		/// Raises the PlanetChanged event on set to a different orbit
		/// </remarks>
		public IOrbit Orbit
		{
			get { return m_Orbit; }
			set
			{
				if ( m_Orbit != value )
				{
					m_Orbit = value;
					OnPlanetChanged( );
				}
			}
		}

		#region Models and Renderers

		/// <summary>
		/// Gets an environment model (e.g. <see cref="IPlanetRingModel"/>) from the planet
		/// </summary>
		/// <typeparam name="T">Environment model</typeparam>
		/// <returns>Returns the specified environment model</returns>
		public T Model<T>( ) where T : class, IPlanetEnvironmentModel
		{
			return m_Models.GetFirstOfType<T>( );
		}

		/// <summary>
		/// Gets an environment renderer (e.g. <see cref="IPlanetMarbleRenderer"/> from the planet
		/// </summary>
		/// <typeparam name="T">Environment renderer</typeparam>
		/// <returns>Returns the specified environment renderer</returns>
		public T Renderer<T>( ) where T : class, IPlanetEnvironmentRenderer
		{
			return m_Renderers.GetFirstOfType<T>( );
		}

		/// <summary>
		/// Adds an environment model to the planet
		/// </summary>
		/// <param name="model">Model to add</param>
		public void AddModel( IPlanetEnvironmentModel model )
		{
			m_Models.Add( model );
			OnModelAssigned( model );
		}

		/// <summary>
		/// Adds an environment renderer to the planet. Can be retrieved by 
		/// </summary>
		/// <param name="renderer">Renderer to add</param>
		/// <param name="renderOrder">Ordinal determining when the renderer is used during rendering.</param>
		/// <param name="deepRenderOrder">Ordinal determining when the renderer is used during deep rendering. 0 is first.</param>
		public void AddRenderer( IPlanetEnvironmentRenderer renderer, PlanetRenderPassOrder renderOrder, PlanetRenderPassOrder deepRenderOrder )
		{
			InsertRenderer( renderOrder, renderer, true );
			InsertRenderer( deepRenderOrder, renderer, false );
			m_Renderers.Add( renderer );
			OnRendererAssigned( renderer );
		}

		#endregion

		#region Models

		/// <summary>
		/// Gets/sets the ring model
		/// </summary>
		public IPlanetRingModel RingModel
		{
			get { return m_RingModel; }
			set
			{
				m_RingModel = value;
				OnModelAssigned( value );
			}
		}

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
		/// Gets/sets the planet marble renderer
		/// </summary>
		public IPlanetMarbleRenderer MarbleRenderer
		{
			get { return m_MarbleRenderer; }
			set
			{
				m_MarbleRenderer = value;
				OnRendererAssigned( value );
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

		/// <summary>
		/// Gets/sets the ring renderer
		/// </summary>
		public IPlanetRingRenderer RingRenderer
		{
			get { return m_RingRenderer; }
			set
			{
				m_RingRenderer = value;
				OnRendererAssigned( value );
			}
		}

		#endregion

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
			foreach ( IPlanetEnvironmentModel model in m_Models )
			{
				DisposableHelper.Dispose( model );
			}
			foreach ( IPlanetEnvironmentRenderer renderer in m_Renderers )
			{
				DisposableHelper.Dispose( renderer );
			}
			m_Models.Clear( );
			m_Renderers.Clear( );

			//DisposableHelper.Dispose( m_AtmosphereModel );
			//DisposableHelper.Dispose( m_CloudModel );
			//DisposableHelper.Dispose( m_OceanModel );
			//DisposableHelper.Dispose( m_TerrainModel );
			//DisposableHelper.Dispose( m_AtmosphereRenderer );
			//DisposableHelper.Dispose( m_CloudRenderer );
			//DisposableHelper.Dispose( m_TerrainRenderer );
			//DisposableHelper.Dispose( m_OceanRenderer );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the PlanetChanged event
		/// </summary>
		protected void OnPlanetChanged( )
		{
			if ( PlanetChanged != null )
			{
				PlanetChanged( this, EventArgs.Empty );
			}
		}

		#endregion

		#region Private Members

		private string						m_Name;
		private UniTransform				m_Transform = new UniTransform( );
		private IOrbit						m_Orbit;
		private readonly ObjectTypeMap<IPlanetEnvironmentModel> m_Models;
		private readonly ObjectTypeMap<IPlanetEnvironmentRenderer> m_Renderers;
		private readonly List<IPlanetEnvironmentRenderer> m_UnorderedRenderers = new List<IPlanetEnvironmentRenderer>( );
		private readonly List<IUniRenderable> m_UnorderedUniRenderers = new List<IUniRenderable>( );
		private readonly List<KeyValuePair<int, IPlanetEnvironmentRenderer>> m_OrderedRenderers = new List<KeyValuePair<int, IPlanetEnvironmentRenderer>>( );
		private readonly List<KeyValuePair<int, IUniRenderable>> m_OrderedUniRenderers = new List<KeyValuePair<int, IUniRenderable>>( );

		private IPlanetAtmosphereModel		m_AtmosphereModel;
		private IPlanetCloudModel			m_CloudModel;
		private IPlanetOceanModel			m_OceanModel;
		private IPlanetTerrainModel			m_TerrainModel;
		private IPlanetRingModel			m_RingModel;

		private IPlanetAtmosphereRenderer	m_AtmosphereRenderer;
		private IPlanetCloudRenderer		m_CloudRenderer;
		private IPlanetTerrainRenderer		m_TerrainRenderer;
		private IPlanetOceanRenderer		m_OceanRenderer;
		private IPlanetMarbleRenderer		m_MarbleRenderer;
		private IPlanetRingRenderer			m_RingRenderer;

		/// <summary>
		/// Inserts a renderer into an ordered list
		/// </summary>
		private void InsertRenderer( PlanetRenderPassOrder order, IPlanetEnvironmentRenderer renderer, bool addToUniList )
		{
			if ( order == PlanetRenderPassOrder.NotRendered )
			{
				return;
			}
			if ( addToUniList && !( renderer is IUniRenderable ) )
			{
				return;
			}
			if ( order == PlanetRenderPassOrder.Unordered )
			{
				if ( addToUniList )
				{
					m_UnorderedUniRenderers.Add( ( IUniRenderable )renderer );
				}
				else
				{
					m_UnorderedRenderers.Add( renderer );
				}
				return;
			}

			int orderValue = ( int )order;	//	Only Unordered and NotRendered symbolic values can be less than 0
			if ( orderValue < 0 )
			{
				throw new ArgumentOutOfRangeException( "order" );
			}
			if ( addToUniList )
			{
				InsertRenderer( orderValue, m_OrderedUniRenderers, ( IUniRenderable )renderer );
			}
			else
			{
				InsertRenderer( orderValue, m_OrderedRenderers, renderer );	
			}
		}

		/// <summary>
		/// Inserts a renderer into an ordered list
		/// </summary>
		private static void InsertRenderer<T>( int order, List<KeyValuePair<int, T>> orderedList, T renderer )
		{
			for ( int index = 0; index < orderedList.Count; ++index )
			{
				if ( orderedList[ index ].Key >= order )
				{
					orderedList.Insert( index, new KeyValuePair<int, T>( order, renderer ) );
					return;
				}
			}
			orderedList.Add( new KeyValuePair<int, T>( order, renderer ) );
		}

		/// <summary>
		///	Renders planet marble
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderMarble( IRenderContext context )
		{
			//	Push marble render transform
			UniCamera.PushAstroRenderTransform( TransformType.LocalToWorld, Transform );

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
			//	Push transform
			UniCamera.PushRenderTransform( TransformType.LocalToWorld, Transform );

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

		#endregion
	}
}
