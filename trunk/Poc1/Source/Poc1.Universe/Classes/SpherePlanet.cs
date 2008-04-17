using Poc1.Universe.Classes.Rendering;
using Poc1.Universe.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public class SpherePlanet : Planet
	{
		/// <summary>
		/// Creates a spherical planet, with a given orbit, name and radius, in metres
		/// </summary>
		public SpherePlanet( IOrbit orbit, string name, double radius ) :
			base( orbit, name )
		{
			m_Radius = UniUnits.FromMetres( radius );
			m_Renderer = new SpherePlanetRenderer( this );
		}

		/// <summary>
		/// Gets the radius of this planet, in universe units
		/// </summary>
		public long Radius
		{
			get { return m_Radius; }
		}

		#region Private Members

		private readonly long m_Radius;
		private readonly SpherePlanetRenderer m_Renderer;

		#endregion

		#region IRenderable Members

		public override void Render( IRenderContext context )
		{
			base.Render( context );
			m_Renderer.Render( context );
		}

		#endregion
	}
}
