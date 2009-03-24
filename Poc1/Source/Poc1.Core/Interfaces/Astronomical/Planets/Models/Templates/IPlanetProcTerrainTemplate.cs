
using Poc1.Fast.Terrain;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Interface for homogenous procedural terrain templates
	/// </summary>
	public interface IPlanetProcTerrainTemplate : IPlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Gets/sets the terrain height function
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
