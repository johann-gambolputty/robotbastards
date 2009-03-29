using Poc1.Fast.Terrain;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Procedural terrain model, with a uniformally applied terrain function
	/// </summary>
	public interface IPlanetHomogenousProceduralTerrainModel : IPlanetTerrainModel
	{
		/// <summary>
		/// Gets/sets the height function
		/// </summary>
		TerrainFunction HeightFunction
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the ground offset function
		/// </summary>
		TerrainFunction GroundOffsetFunction
		{
			get; set;
		}
	}
}
