using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces.Biomes;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Tools.Waves;

namespace Poc1.Bob.Core.Interfaces
{
	/// <summary>
	/// Creates a view and its associated controller from a model
	/// </summary>
	public interface IViewFactory
	{
		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		IBiomeTerrainTextureView CreateBiomeTerrainTextureView( ISelectedBiomeContext context );

		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		IWaveAnimatorView CreateWaveAnimatorView( WaveAnimationParameters model );

		/// <summary>
		/// Creates a biome list view
		/// </summary>
		IBiomeListView CreateBiomeListView( ISelectedBiomeContext context, BiomeListModel model );

	}
}
