using Poc1.Core.Classes.Astronomical.Planets.Renderers.PackTextures;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers.PackTextures;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.PackTextures
{
	/// <summary>
	/// Supports an underlying effect with additional properties related to spherical planets
	/// </summary>
	public class SpherePlanetPackTextureTechnique : PlanetPackTextureTechnique
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetPackTextureTechnique( ITerrainPackTextureProvider provider ) :
			base( provider, "Effects/Planets/terrestrialPlanetTerrain.cgfx" )
		{
		}

		/// <summary>
		/// Sets up the terrain rendering effect
		/// </summary>
		protected override void SetupTerrainEffect( IUniCamera camera, IEffect effect, IPlanet planet, ITerrainPackTextureProvider textureProvider )
		{
			base.SetupTerrainEffect( camera, effect, planet, textureProvider );
			ISpherePlanet spherePlanet = ( ISpherePlanet )planet;
			effect.Parameters[ "PlanetRadius" ].Set( spherePlanet.Model.Radius.ToRenderUnits );
		}
	}
}
