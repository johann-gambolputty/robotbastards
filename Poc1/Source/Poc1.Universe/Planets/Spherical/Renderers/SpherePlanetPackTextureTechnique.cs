using Poc1.Universe.Interfaces.Planets;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Planets.Renderers;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Supports an underlying effect with additional properties related to spherical planets
	/// </summary>
	public class SpherePlanetPackTextureTechnique : PlanetPackTextureTechnique
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetPackTextureTechnique( ISpherePlanet planet ) :
			base( planet, "Effects/Planets/terrestrialPlanetTerrain.cgfx" )
		{
		}

		/// <summary>
		/// Sets up the terrain rendering effect
		/// </summary>
		protected override void SetupTerrainEffect( IEffect effect, IPlanet planet )
		{
			base.SetupTerrainEffect( effect, planet );
			ISpherePlanet spherePlanet = ( ISpherePlanet )planet;
			effect.Parameters[ "PlanetRadius" ].Set( spherePlanet.PlanetModel.Radius.ToRenderUnits );
		}
	}
}
