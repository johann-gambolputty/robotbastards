using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Renders the sea for a sphere planet
	/// </summary>
	public class SphereOceanRenderer : OceanRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SphereOceanRenderer( SpherePlanet planet ) :
			base( "Effects/Planets/sphereOcean.cgfx" )
		{
			//	Generate cached sphere for rendering the planet
			long seaRadius = planet.Radius + UniUnits.Metres.ToUniUnits( planet.SeaLevel );
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, ( float )UniUnits.RenderUnits.FromUniUnits( seaRadius ), 40, 40 );
			m_OceanGeometry = Graphics.Draw.StopCache( );
		}

		#region Protected Members

		/// <summary>
		/// Renders the ocean geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		protected override void RenderOcean( IRenderContext context )
		{
			m_OceanGeometry.Render( context );
		}

		#endregion

		#region Private Members

		private readonly IRenderable m_OceanGeometry;

		#endregion
	}
}
