using Poc1.Bob.Core.Classes.Biomes.Models;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// Interface for biome list views
	/// </summary>
	public interface IBiomeListView
	{
		/// <summary>
		/// Event raised when the user requests that a new biome be added to the biome list
		/// </summary>
		event ActionDelegates.Action AddNewBiome;

		/// <summary>
		/// Event raised when the user requests the removal of a biome
		/// </summary>
		event ActionDelegates.Action<BiomeModel> RemoveBiome;

		/// <summary>
		/// Event raised when the user selects a biome
		/// </summary>
		event ActionDelegates.Action<BiomeModel> BiomeSelected;
	}
}
