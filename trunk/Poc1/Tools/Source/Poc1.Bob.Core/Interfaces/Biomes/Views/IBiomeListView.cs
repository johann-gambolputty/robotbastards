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
		/// Event raised when the user requests that a new biome be created
		/// </summary>
		event ActionDelegates.Action OnCreateBiome;

		/// <summary>
		/// Event raised when the user requests that the currently selected biome be added to the biome list
		/// </summary>
		event ActionDelegates.Action<BiomeModel> OnAddBiome;

		/// <summary>
		/// Event raised when the user selects a biome
		/// </summary>
		event ActionDelegates.Action<BiomeModel> BiomeSelected;

		/// <summary>
		/// Event raised when the user requests that the currently selected biome be removed from the biome list
		/// </summary>
		event ActionDelegates.Action<BiomeModel> OnRemoveBiome;

		/// <summary>
		/// Event raised when the user requests that the currently selected biome be deleted
		/// </summary>
		event ActionDelegates.Action<BiomeModel> OnDeleteBiome;


		/// <summary>
		/// Adds a biome to the view
		/// </summary>
		void AddBiome( BiomeModel model, bool selected );

		/// <summary>
		/// Removes a biome from the view
		/// </summary>
		void RemoveBiome( BiomeModel model );

		/// <summary>
		/// Selects/deselects a biome
		/// </summary>
		/// <param name="model">Biome to select</param>
		/// <param name="selected">Selection/deselection flag</param>
		void SelectBiome( BiomeModel model, bool selected );

	}
}
