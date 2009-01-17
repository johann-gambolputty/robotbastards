using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Templates;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Tools.Waves;

namespace Poc1.Bob.Core.Classes
{
	/// <summary>
	/// Creates views from a <see cref="IViewFactory"/>. Retrieves required data from a workspace
	/// </summary>
	public class WorkspaceViewFactory
	{
		/// <summary>
		/// Shows a create template instance view as a modal dialog
		/// </summary>
		public static void ShowCreateTemplateInstanceView( WorkspaceEx workspace, IViewFactory viewFactory )
		{
			ShowCreateTemplateInstanceView( workspace, viewFactory, workspace.TemplateRootGroup );
		}

		/// <summary>
		/// Shows a create template instance view, for a specific templat group, as a modal dialog
		/// </summary>
		public static void ShowCreateTemplateInstanceView( WorkspaceEx workspace, IViewFactory viewFactory, TemplateGroupContainer rootGroup )
		{
			viewFactory.ShowCreateTemplateInstanceView( workspace, workspace.TemplateInstanceContext, rootGroup );
		}

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		public static IBiomeTerrainTextureView CreateBiomeTerrainTextureView( WorkspaceEx workspace, IViewFactory viewFactory )
		{
			return viewFactory.CreateBiomeTerrainTextureView( workspace.SelectedBiomeContext );
		}

		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		public static IWaveAnimatorView CreateWaveAnimatorView( IWorkspace workspace, IViewFactory viewFactory, WaveAnimationParameters waveAnimParams )
		{
			return viewFactory.CreateWaveAnimatorView( waveAnimParams );
		}

		/// <summary>
		/// Creates a biome list view
		/// </summary>
		public static IBiomeListView CreateBiomeListView( WorkspaceEx workspace, IViewFactory viewFactory, BiomeListModel model )
		{
			//	TODO: AP: ...
			return viewFactory.CreateBiomeListView( workspace.SelectedBiomeContext, model );
		}

	}
}
