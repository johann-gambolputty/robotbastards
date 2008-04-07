using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public abstract class SpherePlanetRenderer : IRenderable
	{
		public SpherePlanetRenderer( SpherePlanet planet )
		{
			m_Planet = planet;
		}

		public SpherePlanet Planet
		{
			get { return m_Planet; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public abstract void Render( IRenderContext context );

		#endregion

		#region Private Members

		private readonly SpherePlanet m_Planet;

		#endregion
	}
}
