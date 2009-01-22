using Poc1.Bob.Core.Classes.Biomes.Models;

namespace Poc1.Bob.Core.Interfaces.Biomes.Models
{
	/// <summary>
	/// Biome distribution model
	/// </summary>
	interface IBiomeListDistributionModel
	{
		/// <summary>
		/// Gets the distribution used by a specified biome in this model
		/// </summary>
		IBiomeDistribution this[ BiomeModel model ]
		{
			get;
		}
	}
}
