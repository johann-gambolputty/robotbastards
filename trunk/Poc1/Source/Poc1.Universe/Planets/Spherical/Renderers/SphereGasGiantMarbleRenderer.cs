
using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Interfaces.Planets.Spherical;
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
	public class SphereGasGiantMarbleRenderer : IPlanetMarbleRenderer
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

		#region IPlanetEnvironmentRenderer Members

		/// <summary>
		/// Gets/sets the planet being rendered
		/// </summary>
		public IPlanet Planet
		{
			get { return m_Planet; }
			set
			{
				m_Planet = ( ISpherePlanet )value;
				DisposableHelper.Dispose( m_Geometry );
				m_Geometry = null;
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the planet
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Geometry == null )
			{
				float radius = ( float )m_Planet.Radius.ToAstroRenderUnits;
				Graphics.Draw.StartCache( );
				Graphics.Draw.Sphere( null, Point3.Origin, radius, 50, 50 );
				m_Geometry = Graphics.Draw.StopCache( );
			}

			m_Technique.Effect.Parameters[ "MarbleTexture" ].Set( m_Texture );
			context.ApplyTechnique( m_Technique, m_Geometry );
		}

		#endregion

		#region Private Members

		private ISpherePlanet m_Planet;
		private IRenderable m_Geometry;
		private readonly ITexture m_Texture;
		private readonly TechniqueSelector m_Technique;

		#endregion
	}
}
