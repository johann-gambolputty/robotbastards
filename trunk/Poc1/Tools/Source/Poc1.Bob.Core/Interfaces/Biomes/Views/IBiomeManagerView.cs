
using Poc1.Bob.Core.Classes.Biomes.Models;

namespace Poc1.Bob.Core.Interfaces.Biomes.Views
{
	/// <summary>
	/// Biome manager view
	/// </summary>
	public interface IBiomeManagerView
	{
		/// <summary>
		/// Gets/sets the biome that this view is displaying. Can set to null (no biome shown)
		/// </summary>
		BiomeModel CurrentBiome
		{
			get; set;
		}

		/// <summary>
		/// Gets the sub-view showing terrain textures
		/// </summary>
		IBiomeTerrainTextureView TerrainTextureView
		{
			get;
		}
	}
}
