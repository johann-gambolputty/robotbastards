using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.AtmosphereTest
{
	/// <summary>
	/// Renders the planet's surface in this test
	/// </summary>
	public class PlanetSurface : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public PlanetSurface( float radius )
		{
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( Graphics.Surfaces.Blue, Point3.Origin, radius, 40, 40 );
			m_Geometry = Graphics.Draw.StopCache( );

			m_Technique = new TechniqueSelector( "Shared/diffuseLit.cgfx", true, "DefaultTechnique" );
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			context.ApplyTechnique( m_Technique, m_Geometry );
		}

		#endregion

		#region Private Members

		private readonly TechniqueSelector m_Technique;
		private readonly IRenderable m_Geometry;

		#endregion
	}
}
