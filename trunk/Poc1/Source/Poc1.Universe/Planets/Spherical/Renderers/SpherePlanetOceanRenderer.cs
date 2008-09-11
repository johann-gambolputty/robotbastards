using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Renders the sea for a sphere planet
	/// </summary>
	public class SpherePlanetOceanRenderer : IPlanetOceanRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetOceanRenderer( )
		{
			m_Effect = new EffectAssetHandle( "Effects/Planets/sphereOcean.cgfx", true );
			m_Effect.OnReload += Effect_OnReload;

			//	Generate cached sphere for rendering the planet
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, 10.0f, 40, 40 );
			m_OceanGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Gets/sets the planet this renderer belongs to
		/// </summary>
		public ISpherePlanet Planet
		{
			get { return m_Planet; }
			set { m_Planet = value; }
		}

		/// <summary>
		/// Renders the ocean
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			float seaLevel = m_Planet.Radius.ToRenderUnits;
			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			Graphics.Renderer.Scale( TransformType.LocalToWorld, seaLevel, seaLevel, seaLevel );

			context.ApplyTechnique( null, m_OceanGeometry );

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}

		#region Private Members

		private ISpherePlanet m_Planet;
		private readonly EffectAssetHandle m_Effect;
		private TechniqueSelector m_Technique;
		private readonly IRenderable m_OceanGeometry;

		/// <summary>
		/// Handles reloading ocean renderer effect
		/// </summary>
		private void Effect_OnReload( ISource source )
		{
			m_Technique = new TechniqueSelector( m_Effect.Asset, m_Technique.Name );
		}

		#endregion
	}
}
