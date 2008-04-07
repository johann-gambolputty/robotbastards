using Poc1.Universe.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes
{
	public class SpherePlanet : Planet
	{
		public SpherePlanet( IOrbit orbit, string name, float radius ) :
			base( orbit, name )
		{
			m_Radius = radius;
			m_Renderer = Graphics.Factory.CustomTypes.Create<SpherePlanetRenderer>( this );
		}

		public float Radius
		{
			get { return m_Radius; }
		}

		#region Private Members

		private readonly float m_Radius;
		private SpherePlanetRenderer m_Renderer;

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
