using Poc1.Bob.Core.Classes.Biomes.Models;

namespace Poc1.Bob.Core.Interfaces.Biomes.Models
{
	/// <summary>
	/// Interface for biome distributions
	/// </summary>
	public interface IBiomeDistribution
	{
		/// <summary>
		/// Gets the biome that this distribution is associated with
		/// </summary>
		BiomeModel Biome
		{
			get;
		}

		/// <summary>
		/// Splits this distribution in two, changing this distribution in place and returning
		/// a new distribution representing the other area, associated with the specified biome
		/// </summary>
		IBiomeDistribution Split( BiomeModel biome );
	}
}
