using Poc1.Universe.Planets.Renderers;

namespace Poc1.Universe.Planets.Flat.Renderers
{
	/// <summary>
	/// Terrain patch renderer for flat planets
	/// </summary>
	public class FlatPlanetTerrainPatchRenderer : PlanetTerrainPatchRenderer
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public FlatPlanetTerrainPatchRenderer( ) :
			base( new PlanetPackTextureTechnique( "..." ) )
		{
		}
	}
}
