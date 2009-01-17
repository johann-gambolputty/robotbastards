using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Templates;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Templates;
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
		void ShowCreateTemplateInstanceView( IWorkspace workspace, TemplateInstanceContext instanceContext, TemplateGroupContainer rootGroup );

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		IBiomeTerrainTextureView CreateBiomeTerrainTextureView( SelectedBiomeContext context );

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
