using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Projects;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Bob.Core.Interfaces.Rendering;
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
		/// Shows a create template instance view as a modal dialog
		/// </summary>
		void ShowCreateProjectView( IWorkspace workspace, ProjectContext context, ProjectGroupContainer rootGroup );

		/// <summary>
		/// Creates a biome distribution view
		/// </summary>
		IBiomeDistributionView CreateBiomeDistributionView( BiomeListLatitudeDistributionModel model );

		/// <summary>
		/// Creates a view with a universe camera
		/// </summary>
		IUniCameraView CreateUniCameraView( );

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		IBiomeTerrainTextureView CreateBiomeTerrainTextureView( SelectedBiomeContext context, BiomeListModel model );

		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		IWaveAnimatorView CreateWaveAnimatorView( WaveAnimationParameters model );

		/// <summary>
		/// Creates a biome list view
		/// </summary>
		IBiomeListView CreateBiomeListView( SelectedBiomeContext context, BiomeListModel model );

	}
}
