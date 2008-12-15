using Poc1.Bob.Core.Classes.Biomes.Models;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Biomes
{
	/// <summary>
	/// Controller context for an active biome
	/// </summary>
	public interface ISelectedBiomeContext
	{
		/// <summary>
		/// Event raised when the selected biome is changed
		/// </summary>
		event ActionDelegates.Action<BiomeModel> BiomeSelected;

		/// <summary>
		/// Gets/sets the currently selected biome
		/// </summary>
		BiomeModel SelectedBiome
		{
			get; set;
		}
	}
}
