using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Projects;
using Poc1.Bob.Core.Interfaces.Biomes.Views;
using Poc1.Bob.Core.Interfaces.Planets;
using Poc1.Bob.Core.Interfaces.Planets.Clouds;
using Poc1.Bob.Core.Interfaces.Planets.Terrain;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Bob.Core.Interfaces.Rendering;
using Poc1.Bob.Core.Interfaces.Waves;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Tools.Waves;
using Rb.Common.Controls.Components;
using Rb.Core.Components;

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
		/// Shows a composite object editor as a model dialog
		/// </summary>
		void ShowEditCompositeView( IComposite composite, ComponentTypeCategory[] categories, ComponentType[] componentTypes );

		/// <summary>
		/// Creates a view used to edit an homogenous procedural terrain model
		/// </summary>
		IHomogenousProcTerrainView CreateHomogenousProcTerrainTemplateView( IPlanetHomogenousProceduralTerrainTemplate template, IPlanetHomogenousProceduralTerrainModel model );

		/// <summary>
		/// Creates a view used to edit flat cloud model templates
		/// </summary>
		IFlatCloudModelTemplateView CreateCloudTemplateView( IPlanetSimpleCloudTemplate template, IPlanetSimpleCloudModel model );

		/// <summary>
		/// Creates a view used to edit the parameters of a planet template
		/// </summary>
		IPlanetModelTemplateView CreatePlanetTemplateView( IPlanetModelTemplate template, IPlanetModel instance );

		/// <summary>
		/// Create a new biome view
		/// </summary>
		INewBiomeView CreateNewBiomeView( );

		/// <summary>
		/// Creates a biome distribution view
		/// </summary>
		IBiomeDistributionView CreateBiomeDistributionView( BiomeListLatitudeDistributionModel model );

		/// <summary>
		/// Creates a universe view on a planet
		/// </summary>
		IUniCameraView CreatePlanetView( IPlanet planet );

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
		IBiomeListView CreateBiomeListView( SelectedBiomeContext context, BiomeListModel allBiomes, BiomeListModel currentBiomes );

	}
}
