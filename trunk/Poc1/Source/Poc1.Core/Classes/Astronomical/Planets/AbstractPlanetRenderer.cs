using System;
using System.Collections.Generic;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Rb.Core.Components.Generic;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets
{
	/// <summary>
	/// Abstract planet renderer
	/// </summary>
	public abstract class AbstractPlanetRenderer : Composite<IPlanetEnvironmentRenderer>, IPlanetRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Planet that this renderer is attached to</param>
		public AbstractPlanetRenderer( IPlanet planet )
		{
			Arguments.CheckNotNull( planet, "planet" );
			m_Planet = planet;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="planet">Spherical planet that this renderer is attached to</param>
		/// <param name="nearSorter">Render order sorter used to determine the order that near objects are rendered in</param>
		/// <param name="farSorter">Render order sorter used to determine the order that far objects are rendered in</param>
		public AbstractPlanetRenderer( ISpherePlanet planet, IRenderOrderSorter nearSorter, IRenderOrderSorter farSorter )
		{
			Arguments.CheckNotNull( planet, "planet" );
			Arguments.CheckNotNull( nearSorter, "nearSorter" );
			Arguments.CheckNotNull( farSorter, "farSorter" );

			m_Planet = planet;
			m_NearSorter = nearSorter;
			m_FarSorter = farSorter;
		}

		/// <summary>
		/// Adds an environment renderer
		/// </summary>
		public override void Add( IPlanetEnvironmentRenderer renderer )
		{
			base.Add( renderer );
			m_OrderedNearRenderables = m_OrderedFarRenderables = null;
		}

		/// <summary>
		/// Removes an environment renderer
		/// </summary>
		public override void Remove( IPlanetEnvironmentRenderer renderer )
		{
			base.Remove( renderer );
			m_OrderedNearRenderables = m_OrderedFarRenderables = null;
		}

		#region IPlanetRenderer Members

		/// <summary>
		/// Gets the planet that this renderer is attached to
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
		}

		/// <summary>
		/// Shortcut to Planet.PlanetModel
		/// </summary>
		public IPlanetModel PlanetModel
		{
			get
			{
				Debug.Assert( m_Planet.Model != null );
				return m_Planet.Model;
			}
		}

		#endregion


		#region IRenderable<IUniRenderContext> Members

		/// <summary>
		/// Performs rendering
		/// </summary>
		/// <param name="context">Rendering context</param>
		public virtual void Render( IUniRenderContext context )
		{
			if ( m_NearSorter == null )
			{
				m_NearSorter = DefaultNearSorter( );
			}
			if ( m_FarSorter == null )
			{
				m_FarSorter = DefaultFarSorter( );
			}
			if ( m_OrderedNearRenderables == null )
			{
				m_OrderedNearRenderables = m_NearSorter.Sort( GetRenderables( ) );
			}
			if ( m_OrderedFarRenderables == null )
			{
				m_OrderedFarRenderables = m_FarSorter.Sort( GetRenderables( ) );
			}
		}

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
		/// Creates the default near sorter
		/// </summary>
		protected abstract IRenderOrderSorter DefaultNearSorter( );

		/// <summary>
		/// Creates the default far sorter
		/// </summary>
		protected abstract IRenderOrderSorter DefaultFarSorter( );

		#endregion

		#region Private Members

		private IRenderable[] m_OrderedNearRenderables;
		private IRenderable[] m_OrderedFarRenderables;
		private IRenderOrderSorter m_NearSorter;
		private IRenderOrderSorter m_FarSorter;
		private readonly IPlanet m_Planet;

		/// <summary>
		/// Gets an array of the near renderables stored in this object
		/// </summary>
		private IRenderable[] GetRenderables( )
		{
			List<IRenderable> renderables = new List<IRenderable>( Components.Count );
			foreach ( IPlanetEnvironmentRenderer component in Components )
			{
				renderables.Add( component );
			}
			return renderables.ToArray( );
		}

		#endregion
	}

}
