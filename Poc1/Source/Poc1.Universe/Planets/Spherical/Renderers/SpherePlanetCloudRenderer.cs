using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renderer for sphere planet clouds
	/// </summary>
	public class SpherePlanetCloudRenderer : ISpherePlanetCloudRenderer
	{
		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets/sets the planet associated with this renderer
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the cloud model
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( Planet == null || Planet.CloudModel == null )
			{
				return;
			}
		}

		#endregion

		#region Private Members

		private IPlanet m_Planet;

		#endregion
	}
}
