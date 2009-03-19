using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Projects.Planets;
using Poc1.Bob.Core.Classes.Projects.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Planets.Spherical.Models;
using Poc1.Universe.Planets.Spherical.Models.Templates;
using Poc1.Universe.Planets.Spherical.Renderers;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc1.Bob.Projects
{
	/// <summary>
	/// Adds dockable views to the SpherePlanetProjectType
	/// </summary>
	public class SpherePlanetDockingProjectType : SpherePlanetProjectType
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SpherePlanetDockingProjectType( DockingViewManager viewManager, IViewFactory viewFactory, IPlanetViews views ) :
			base( views )
		{
			Arguments.CheckNotNull( viewManager, "viewManager" );
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			m_ViewFactory = viewFactory;
			m_Views = 
				new DockingViewInfo[]
				{
					new DockingViewInfo( "Planet Template Composition View", CreatePlanetTemplateCompositionView, true ), 
					new DockingViewInfo( "Biome Distribution View", CreateBiomeDistributionView, true ), 
					new DockingViewInfo( "Biome List View", CreateBiomeListView, true ), 
					new DockingViewInfo( "Biome Terrain Texture View", CreateBiomeTerrainTextureView, true ),
					new DockingViewInfo( "Ocean Template View", CreateOceanTemplateView, true ),
					new DockingViewInfo( "Cloud Template View", CreateCloudTemplateView, true, DockState.Float ),
					new DockingViewInfo( "Procedural Terrain View", CreateProceduralTerrainView, true ), 
					new DockingViewInfo( "Planet Template View", CreatePlanetTemplateView, true, DockState.Float ),
					new DockingViewInfo( "Planet Display", CreatePlanetDisplay, true, DockState.Document )
				};
		}

		/// <summary>
		/// Gets the views associated with this project type
		/// </summary>
		public override IViewInfo[] Views
		{
			get { return m_Views; }
		}

		#region Private Members

		private readonly IViewFactory m_ViewFactory;
		private readonly DockingViewInfo[] m_Views;

		/// <summary>
		/// Helper method for retrieving the current project from a workspace
		/// </summary>
		private static SpherePlanetProject CurrentProject( IWorkspace workspace )
		{
			return ( SpherePlanetProject )( ( WorkspaceEx )workspace ).ProjectContext.CurrentProject;
		}

		/// <summary>
		/// Creates an homogenous procedural terrain view
		/// </summary>
		private Control CreateProceduralTerrainView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );
			IPlanetProcTerrainTemplate template = currentProject.PlanetTemplate.GetModelTemplate<IPlanetProcTerrainTemplate>( );
			if ( template == null )
			{
				//currentProject.PlanetTemplate.EnvironmentModelTemplates.Add( null );
			}
			IPlanetProcTerrainModel model = currentProject.PlanetModel.TerrainModel as IPlanetProcTerrainModel;
			if ( model == null )
			{
				model = new SpherePlanetProcTerrainModel( );
				currentProject.PlanetModel.TerrainModel = model;
			}
			return ( Control )m_ViewFactory.CreateHomogenousProcTerrainTemplateView( template, model );
		}

		/// <summary>
		/// Creates a planet template composition view
		/// </summary>
		private Control CreatePlanetTemplateCompositionView( IWorkspace workspace)
		{
			//SpherePlanetProject currentProject = CurrentProject( workspace );
			//EditableCompositeControl control = new EditableCompositeControl( );
			//IPlanetEnvironmentModelVisitor visitor = new PlanetEnvironmentModelTemplateViewVisitor( workspace, m_ViewManager, m_Views );
			//new EditablePlanetTemplateViewController( m_ViewFactory, control, currentProject.PlanetTemplate, m_ViewVisitor );
			//control.PlanetTemplate = currentProject.PlanetTemplate;
			//return control;
			return null;
		}

		/// <summary>
		/// Creates a biome distribution view
		/// </summary>
		private Control CreateBiomeDistributionView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace ); 
			BiomeListLatitudeDistributionModel distributions = currentProject.BiomeDistributions;
			return ( Control )m_ViewFactory.CreateBiomeDistributionView( distributions );
		}

		/// <summary>
		/// Creates an ocean template view
		/// </summary>
		private Control CreateOceanTemplateView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );
			return ( Control )WorkspaceViewFactory.CreateWaveAnimatorView( workspace, m_ViewFactory, currentProject.WaveAnimationModel );
		}

		/// <summary>
		/// Creates an flat cloud template view
		/// </summary>
		private Control CreateCloudTemplateView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );

			//	TODO: AP: REMOVEME
			IPlanetCloudModelTemplate cloudModelTemplate = currentProject.PlanetTemplate.GetModelTemplate<IPlanetCloudModelTemplate>( );
			if ( cloudModelTemplate == null )
			{
				cloudModelTemplate = new SpherePlanetCloudModelTemplate( );
				currentProject.PlanetTemplate.Add( cloudModelTemplate );
			}

			IPlanetCloudModel cloudModel = currentProject.PlanetModel.CloudModel;
			if ( cloudModel == null )
			{
				cloudModelTemplate.SetupInstance( cloudModel, currentProject.InstanceContext );
				currentProject.Planet.PlanetRenderer.CloudRenderer = new SpherePlanetCloudRenderer( );
			}

			return ( Control )m_ViewFactory.CreateCloudTemplateView( cloudModelTemplate, currentProject.PlanetModel.CloudModel );
		}

		/// <summary>
		/// Creates an atmosphere template view control
		/// </summary>
		private Control CreateBiomeListView( IWorkspace workspace )
		{
			//	Design: 
			//		Planet template
			//			|
			//			+- Biome list template
			//			+- Biome distribution template
			//			+- Terrain model template
			//			+- Cloud template
			//			+- Ocean template
			//
			//	Add Template | Remove Template
			//	Double click opens a view on the model
			//
			SpherePlanetProject currentProject = CurrentProject( workspace );
			return ( Control )WorkspaceViewFactory.CreateBiomeListView( ( WorkspaceEx )workspace, m_ViewFactory, currentProject.AllBiomes, currentProject.CurrentBiomes );
		}

		/// <summary>
		/// Creates an atmosphere template view control
		/// </summary>
		private Control CreateBiomeTerrainTextureView( IWorkspace workspace )
		{
			WorkspaceEx workspaceEx = ( WorkspaceEx )workspace;
			SpherePlanetProject project = ( SpherePlanetProject )( workspaceEx .ProjectContext.CurrentProject );
			return ( Control )WorkspaceViewFactory.CreateBiomeTerrainTextureView( m_ViewFactory, workspaceEx.SelectedBiomeContext, project.CurrentBiomes );
		}

		/// <summary>
		/// Creates a planet template view control
		/// </summary>
		private Control CreatePlanetTemplateView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );
			return ( Control )m_ViewFactory.CreatePlanetTemplateView( currentProject.PlanetTemplate, currentProject.PlanetModel );
		}

		/// <summary>
		/// Creates a planet view control
		/// </summary>
		private Control CreatePlanetDisplay( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject( workspace );
			return ( Control )m_ViewFactory.CreatePlanetView( currentProject.Planet );
		}

		#endregion
	}
}
