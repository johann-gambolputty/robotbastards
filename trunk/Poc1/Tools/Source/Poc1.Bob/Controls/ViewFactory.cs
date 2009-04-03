using System;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Controls.Biomes;
using Poc1.Bob.Controls.Planet;
using Poc1.Bob.Controls.Planet.Clouds;
using Poc1.Bob.Controls.Planet.Terrain;
using Poc1.Bob.Controls.Projects;
using Poc1.Bob.Controls.Rendering;
using Poc1.Bob.Controls.Waves;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Controllers;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Planets;
using Poc1.Bob.Core.Classes.Planets.Clouds;
using Poc1.Bob.Core.Classes.Projects;
using Poc1.Bob.Core.Classes.Rendering;
using Poc1.Bob.Core.Classes.Waves;
using Poc1.Bob.Core.Interfaces;
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
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Poc1.Tools.Waves;
using Rb.Common.Controls.Components;
using Rb.Common.Controls.Forms.Components;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Poc1.Bob.Controls
{
	/// <summary>
	/// Creates views
	/// </summary>
	public class ViewFactory : IViewFactory
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="uiProvider">Message UI provider. If null, no messages are displayed to the user</param>
		/// <remarks>
		/// All views created by this factory use the specified UI provider to display simple
		/// messages to the user.
		/// </remarks>
		public ViewFactory( IMessageUiProvider uiProvider )
		{
			m_UiProvider = uiProvider;
		}

		/// <summary>
		/// Shows a create project view as a modal dialog
		/// </summary>
		public void ShowCreateProjectView( IWorkspace workspace, ProjectContext context, ProjectGroupContainer rootGroup )
		{
			ICreateProjectView view = new CreateProjectForm( );
			new CreateProjectController( workspace, context, view, rootGroup ).Show( );
		}

		/// <summary>
		/// Shows a composite object editor as a model dialog
		/// </summary>
		public void ShowEditCompositeView( IComposite composite, ComponentTypeCategory[] categories, ComponentType[] componentTypes )
		{
			CompositeEditorForm form = new CompositeEditorForm( );
			form.CompositeView.Composite = composite;
			form.Categories = categories;
			new ComponentCompositionEditorController( form, categories, componentTypes, composite );
			form.ShowDialog( );
		}

		/// <summary>
		/// Creates a view used to edit an homogenous procedural terrain model
		/// </summary>
		public IHomogenousProceduralTerrainView CreateHomogenousProcTerrainTemplateView( IPlanetHomogenousProceduralTerrainTemplate template, IPlanetHomogenousProceduralTerrainModel model )
		{
			Arguments.CheckNotNull( model, "model" );
			HomogenousProcTerrainTemplateControl control = new HomogenousProcTerrainTemplateControl( );
			control.Template = template;
			return control;
		}

		/// <summary>
		/// Creates a view used to edit flat cloud model templates
		/// </summary>
		public IFlatCloudModelTemplateView CreateCloudTemplateView( IPlanetSimpleCloudTemplate template, IPlanetSimpleCloudModel model )
		{
			FlatCloudModelTemplateControl view = new FlatCloudModelTemplateControl( );
			new CloudModelTemplateViewController( view, template, model );
			return view;
		}

		/// <summary>
		/// Creates a view used to edit the parameters of a planet template
		/// </summary>
		public IPlanetModelTemplateView CreatePlanetTemplateView( IPlanetModelTemplate template, IPlanetModel instance )
		{
			//	HACK: AP: Create a view and a controller that are dependent on the template type
			IPlanetModelTemplateView view;
			if ( template is ISpherePlanetModelTemplate )
			{
				view = new SpherePlanetModelTemplateViewControl( );
				new SpherePlanetTemplateViewController( ( ISpherePlanetModelTemplateView )view, ( ISpherePlanetModelTemplate )template, ( ISpherePlanetModel )instance );
			}
			else
			{
				throw new NotSupportedException( "Unsupported planet template type " + template.GetType( ) );
			}
			return view;
		}

		/// <summary>
		/// Create a new biome view
		/// </summary>
		public INewBiomeView CreateNewBiomeView( )
		{
			return new NewBiomeForm( );
		}

		/// <summary>
		/// Creates a biome distribution view
		/// </summary>
		public IBiomeDistributionView CreateBiomeDistributionView( BiomeListLatitudeDistributionModel model )
		{
			BiomeLatitudeDistributionDisplay view = new BiomeLatitudeDistributionDisplay( );
			new BiomeDistributionController( view, model );
			return view;
		}

		/// <summary>
		/// Creates a view with a universe camera
		/// </summary>
		public IUniCameraView CreatePlanetView( IPlanet planet )
		{
			IUniCameraView view = new UniCameraViewControl( );

			//	HACK: AP: Create a controller appropriate to the planet type
			if ( planet is ISpherePlanet )
			{
				new SpherePlanetViewController( view, ( ISpherePlanet )planet );
			}
			else
			{
				throw new NotSupportedException( "No view controller available for planet of type " + planet.GetType( ) );
			}
			return view;
		}

		/// <summary>
		/// Creates a project type selector view
		/// </summary>
		public IProjectTypeSelectorView CreateProjectTypeSelectorView( ProjectGroupContainer rootGroup )
		{
			IProjectTypeSelectorView view = new ProjectTypeSelectorView( );
			new ProjectTypeSelectorController( view, rootGroup );
			return view;
		}

		/// <summary>
		/// Creates a biome terrain texture view
		/// </summary>
		public IBiomeTerrainTextureView CreateBiomeTerrainTextureView( SelectedBiomeContext context, BiomeListModel model )
		{
			IBiomeTerrainTextureView view = new BiomeTerrainTextureViewControl( );
			new BiomeTerrainTextureController( context, view );
			return view;
		}

		/// <summary>
		/// Creates an wave animator view
		/// </summary>
		public IWaveAnimatorView CreateWaveAnimatorView( WaveAnimationParameters model )
		{
			IWaveAnimatorView view = new WaveAnimatorControl( );
			new WaveAnimatorController( m_UiProvider, view, model );
			return view;
		}

		/// <summary>
		/// Creates a biome list view
		/// </summary>
		public IBiomeListView CreateBiomeListView( SelectedBiomeContext context, BiomeListModel allBiomes, BiomeListModel currentBiomes )
		{
			IBiomeListView view = new BiomeListControl( );
			new BiomeListController( this, context, allBiomes, currentBiomes, view );
			return view;
		}

		#region Private Members

		private readonly IMessageUiProvider m_UiProvider;

		#endregion

	}
}
