using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Projects.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Rb.Core.Utils;
using WeifenLuo.WinFormsUI.Docking;
using Poc1.Bob.Core.Interfaces.Projects;

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
		/// <param name="viewFactory">View factory</param>
		public SpherePlanetDockingProjectType( IViewFactory viewFactory )
		{
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			m_ViewFactory = viewFactory;
			m_Views = new DockingViewInfo[]
				{
					new DockingViewInfo( "Biome Distribution View", CreateBiomeDistributionView ), 
					new DockingViewInfo( "Biome List View", CreateBiomeListView ), 
					new DockingViewInfo( "Biome Terrain Texture View", CreateBiomeTerrainTextureView ),
					new DockingViewInfo( "Ocean Template View", CreateOceanTemplateView ), 
					new DockingViewInfo( "Planet Template View", CreatePlanetTemplateView, DockState.Document ),
					new DockingViewInfo( "Planet Display", CreatePlanetDisplay, DockState.Document )
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
		private static T CurrentProject<T>( IWorkspace workspace ) where T : Project
		{
			return ( T )( ( WorkspaceEx )workspace ).ProjectContext.CurrentProject;
		}

		/// <summary>
		/// Creates a biome distribution view
		/// </summary>
		private Control CreateBiomeDistributionView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject < SpherePlanetProject>( workspace ); 
			BiomeListLatitudeDistributionModel distributions = currentProject.BiomeDistributions;
			return ( Control )m_ViewFactory.CreateBiomeDistributionView( distributions );
		}

		/// <summary>
		/// Creates an ocean template view
		/// </summary>
		private Control CreateOceanTemplateView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject<SpherePlanetProject>( workspace );
			return ( Control )WorkspaceViewFactory.CreateWaveAnimatorView( workspace, m_ViewFactory, currentProject.WaveAnimationModel );
		}

		/// <summary>
		/// Creates an atmosphere template view control
		/// </summary>
		private Control CreateBiomeListView( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject<SpherePlanetProject>( workspace );
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
			SpherePlanetProject currentProject = CurrentProject<SpherePlanetProject>( workspace );
			return ( Control )m_ViewFactory.CreatePlanetTemplateView( currentProject.PlanetTemplate, currentProject.PlanetModel );
		}
		/// <summary>
		/// Creates a planet view control
		/// </summary>
		private Control CreatePlanetDisplay( IWorkspace workspace )
		{
			SpherePlanetProject currentProject = CurrentProject<SpherePlanetProject>( workspace );
			return ( Control )m_ViewFactory.CreatePlanetView( currentProject.Planet );
		}

		#endregion
	}
}
