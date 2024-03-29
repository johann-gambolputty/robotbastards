using Poc1.Universe.Interfaces.Planets.Renderers;
using Rb.Assets;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Renders a spherical gas giant
	/// </summary>
	public class SphereGasGiantMarbleRenderer : AbstractSpherePlanetEnvironmentRenderer, IPlanetMarbleRenderer
	{
		/// <summary>
		/// Default constructor. Loads the gas giant marble effect
		/// </summary>
		public SphereGasGiantMarbleRenderer( )
		{
			IEffect effect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/marbleSphereGasGiant.cgfx" );
			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );

			TextureLoadParameters loadParams = new TextureLoadParameters( true );
			m_Texture = ( ITexture )AssetManager.Instance.Load( "Planets/Gas Giants/GasGiant0.jpg", loadParams );
		}

		#region IRenderable Members

		/// <summary>
		/// Renders the planet
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			if ( m_Geometry == null )
			{
				float radius = ( float )SpherePlanet.PlanetModel.Radius.ToAstroRenderUnits;
				Graphics.Draw.StartCache( );
				Graphics.Draw.Sphere( null, Point3.Origin, radius, 50, 50 );
				m_Geometry = Graphics.Draw.StopCache( );
			}

			m_Technique.Effect.Parameters[ "MarbleTexture" ].Set( m_Texture );
			context.ApplyTechnique( m_Technique, m_Geometry );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Assigns/unassigns this renderer to/from a planet
		/// </summary>
		protected override void AssignToPlanet( IPlanetRenderer renderer, bool remove )
		{
			renderer.MarbleRenderer = remove ? null : this;
			DisposableHelper.Dispose( m_Geometry );
			m_Geometry = null;
		}

		#endregion

		#region Private Members

		private IRenderable m_Geometry;
		private readonly ITexture m_Texture;
		private readonly TechniqueSelector m_Technique;

		#endregion
	}
}
