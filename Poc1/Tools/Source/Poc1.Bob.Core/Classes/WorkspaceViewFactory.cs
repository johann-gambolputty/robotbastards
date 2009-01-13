using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Interfaces;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Waves;

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
		public static void ShowCreateTemplateInstanceView( IWorkspace workspace, IViewFactory viewFactory )
		{
			viewFactory.ShowCreateTemplateInstanceView( );
		}

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		public static IBiomeTerrainTextureView CreateBiomeTerrainTextureView( IWorkspace workspace, IViewFactory viewFactory )
		{
		}

		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		public static IWaveAnimatorView CreateWaveAnimatorView( IWorkspace workspace, IViewFactory viewFactory )
		{
		}

		/// <summary>
		/// Creates a biome list view
		/// </summary>
		public static IBiomeListView CreateBiomeListView( IWorkspace workspace, IViewFactory viewFactory )
		{
		}

	}
}
