using Poc1.Fast.Terrain;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Procedural terrain model
	/// </summary>
	public interface IPlanetProcTerrainModel
	{
		/// <summary>
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="maxHeight">Maximum height of the terrain</param>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		void SetupTerrain( float maxHeight, TerrainFunction heightFunction, TerrainFunction groundFunction );
	}
}
