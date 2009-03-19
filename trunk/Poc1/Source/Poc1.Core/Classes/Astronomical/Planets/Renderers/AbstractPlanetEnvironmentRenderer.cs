using System;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Renderers
{
	/// <summary>
	/// Abstract implementation of <see cref="IPlanetEnvironmentRenderer"/>
	/// </summary>
	public abstract class AbstractPlanetEnvironmentRenderer : IPlanetEnvironmentRenderer
	{
		/// <summary>
		/// Retrieves a planet environment model from the current planet
		/// </summary>
		/// <typeparam name="TModel">Planet environment model to get</typeparam>
		/// <returns>Returns the first planet environment model of the specified type, or null if no such model exists.</returns>
		public TModel GetModel<TModel>( )
			where TModel : class, IPlanetEnvironmentModel
		{
			return Planet == null ? null : Planet.Model.Get<TModel>( );
		}

		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets the planet that this renderer is attached to (via the planet renderer)
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Renderer == null ? null : m_Renderer.Planet; }
		}

		/// <summary>
		/// Gets/sets the planet renderer that this renderer is a part of
		/// </summary>
		public IPlanetRenderer PlanetRenderer
		{
			get { return m_Renderer; }
			set
			{
				if ( m_Renderer == value )
				{
					return;
				}
				if ( m_Renderer != null )
				{
					m_Renderer.Remove( this );
					OnRemovedFromPlanetRenderer( m_Renderer );
				}
				m_Renderer = value;
				if ( m_Renderer != null )
				{
					if ( !m_Renderer.Components.Contains( this ) )
					{
						m_Renderer.Add( this );
					}
					OnAddedToPlanetRenderer( m_Renderer );
				}
			}
		}

		#endregion

		#region IRenderable<IUniRenderContext> Members

		/// <summary>
		/// Performs rendering
		/// </summary>
		/// <param name="context">Rendering context</param>
		public abstract void Render( IUniRenderContext context );

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Performs rendering
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( context is IUniRenderContext )
			{
				Render( ( IUniRenderContext )context );
				return;
			}
			throw new NotSupportedException( "Rendering from standard render context is not supported - use an IUniRenderContext" );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Called after this environment renderer has been removed from the specified planet renderer
		/// </summary>
		/// <param name="renderer">Planet renderer that this environment renderer was removed from</param>
		protected virtual void OnRemovedFromPlanetRenderer( IPlanetRenderer renderer )
		{
		}

		/// <summary>
		/// Called after this environment renderer has been added to the specified planet renderer
		/// </summary>
		/// <param name="renderer">Planet renderer that this environment renderer was added to</param>
		protected virtual void OnAddedToPlanetRenderer( IPlanetRenderer renderer )
		{
		}

		#endregion

		#region Private Members

		private IPlanetRenderer m_Renderer;

		#endregion
	}

}
