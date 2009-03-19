using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Renderers
{
	/// <summary>
	/// Abstract base class for planet renderers
	/// </summary>
	public abstract class AbstractPlanetEnvironmentRenderer : IPlanetEnvironmentRenderer
	{

		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets/sets the planet renderer composite that contains this renderer
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
					AssignToPlanet( m_Renderer, true );
				}
				m_Renderer = value;
				if ( m_Renderer != null )
				{
					AssignToPlanet( m_Renderer, false );
				}
			}

		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public abstract void Render( IRenderContext context );

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the planet that the planet renderer is attached to
		/// </summary>
		protected IPlanet Planet
		{
			get { return m_Renderer == null ? null : m_Renderer.Planet; }
		}

		/// <summary>
		/// Assigns/unassigns this renderer to/from a planet
		/// </summary>
		protected abstract void AssignToPlanet( IPlanetRenderer renderer, bool remove );

		#endregion

		#region Private Members

		private IPlanetRenderer m_Renderer;

		#endregion
	}
}
