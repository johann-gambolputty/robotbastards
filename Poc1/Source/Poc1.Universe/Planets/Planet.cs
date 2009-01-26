using System;
using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using IPlanet=Poc1.Universe.Interfaces.Planets.IPlanet;

namespace Poc1.Universe.Planets
{
	/// <summary>
	/// Planet base implementation
	/// </summary>
	public class Planet : IPlanet, IRenderable, IUniRenderable
	{

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

		/// <summary>
		/// Gets/sets the planet model
		/// </summary>
		public IPlanetModel PlanetModel
		{
			get { return m_Model; }
			set
			{
				m_Model = value;
				if ( m_Model != null )
				{
					//m_Model.Planet = this;
				}
				OnPlanetChanged( );
			}
		}

		/// <summary>
		/// Gets/sets the planet renderer
		/// </summary>
		public IPlanetRenderer PlanetRenderer
		{
			get { return m_Renderer; }
			set
			{
				m_Renderer = value;
				if ( m_Renderer != null )
				{
					m_Renderer.Planet = this;
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
			if ( m_Renderer != null )
			{
				m_Renderer.DeepRender( context );
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
			if ( m_Renderer != null )
			{
				m_Renderer.Render( context );
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
			DisposableHelper.Dispose( m_Model );
			DisposableHelper.Dispose( m_Renderer );
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

		private string			m_Name;
		private UniTransform	m_Transform = new UniTransform( );
		private IOrbit			m_Orbit;
		private IPlanetModel	m_Model;
		private IPlanetRenderer m_Renderer;

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

		#endregion
	}
}
