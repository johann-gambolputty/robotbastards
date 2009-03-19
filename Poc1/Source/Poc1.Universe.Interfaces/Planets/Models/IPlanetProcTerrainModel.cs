using Poc1.Fast.Terrain;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Procedural terrain model
	/// </summary>
	public interface IPlanetProcTerrainModel : IPlanetTerrainModel
	{
		/// <summary>
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		void SetupTerrain( TerrainFunction heightFunction, TerrainFunction groundFunction );
	}
}
