using Poc1.Fast.Terrain;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Procedural terrain model
	/// </summary>
	public interface IPlanetProcTerrainModel : IPlanetTerrainModel
	{
		/// <summary>
		/// Gets/sets the height function
		/// </summary>
		TerrainFunction HeightFunction
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the ground function
		/// </summary>
		TerrainFunction GroundFunction
		{
			get; set;
		}
	}
}
