using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
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
			m_Planet = planet;

			//	Generate cached sphere for rendering the planet
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, 10.0f, 40, 40 );
			m_OceanGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Renders the ocean
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			float seaLevel = ( float )UniUnits.RenderUnits.FromUniUnits( m_Planet.Radius ) + UniUnits.RenderUnits.FromMetres( m_Planet.SeaLevel );
			seaLevel /= 10.0f;
			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			Graphics.Renderer.Scale( TransformType.LocalToWorld, seaLevel, seaLevel, seaLevel );

			base.Render( context );

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
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

		private readonly SpherePlanet m_Planet;
		private readonly IRenderable m_OceanGeometry;

		#endregion
	}
}
