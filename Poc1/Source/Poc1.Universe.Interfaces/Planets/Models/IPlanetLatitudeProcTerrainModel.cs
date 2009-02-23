using Poc1.Fast.Terrain;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// A procedural terrain model, where parameters to a terrain function vary depending
	/// on latitude
	/// </summary>
	public interface IPlanetLatitudeProcTerrainModel : IPlanetTerrainModel
	{
		/// <summary>
		/// Sets up the terrain functions
		/// </summary>
		/// <param name="heightFunction">The terrain height function</param>
		/// <param name="groundFunction">The terrain ground offset function</param>
		void SetupTerrain( TerrainFunction heightFunction, TerrainFunction groundFunction );
	}
}
