using Rb.Assets;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Textures;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Renders the sea for a sphere planet
	/// </summary>
	public class SphereSea
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SphereSea( SpherePlanet planet )
		{
			m_Texture = ( ITexture )AssetManager.Instance.Load( "Ocean/ocean0.jpg", new TextureLoader.TextureLoadParameters( true ) );

			//	Load in sea effect
			IEffect seaEffect = ( IEffect )AssetManager.Instance.Load( "Effects/Planets/Ocean.cgfx" );
			m_SeaTechniques = new TechniqueSelector( seaEffect, "DefaultTechnique" );

			//	Generate cached sphere for rendering the planet
			long seaRadius = planet.Radius + UniUnits.Metres.ToUniUnits( planet.SeaLevel );
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( null, Point3.Origin, ( float )UniUnits.RenderUnits.FromUniUnits( seaRadius ), 40, 40 );
			m_SeaGeometry = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Renders the sea
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_SeaTechniques.Effect.Parameters[ "OceanTexture" ].Set( m_Texture );
			context.ApplyTechnique( m_SeaTechniques, m_SeaGeometry );
		}

		#region Private Members

		private readonly ITexture m_Texture;
		private readonly IRenderable m_SeaGeometry;
		private readonly TechniqueSelector m_SeaTechniques;

		#endregion


	}
}
