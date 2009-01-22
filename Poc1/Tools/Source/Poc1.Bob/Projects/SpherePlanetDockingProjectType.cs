using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Projects.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Poc1.Tools.Waves;
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
					new DockingViewInfo( "Distribution Distribution View", CreateBiomeDistributionView ), 
					new DockingViewInfo( "Distribution List View", CreateBiomeListView ), 
					new DockingViewInfo( "Distribution Terrain Texture View", CreateBiomeTerrainTextureView ),
					new DockingViewInfo( "Ocean Template View", CreateOceanTemplateView ), 
					new DockingViewInfo( "Planet View", CreatePlanetView, DockState.Document )
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
		private readonly BiomeListModel m_BiomeListModel = new BiomeListModel( );
		private readonly WaveAnimationParameters m_WaveAnimationModel = new WaveAnimationParameters( );

		/// <summary>
		/// Creates a biome distribution view
		/// </summary>
		private Control CreateBiomeDistributionView( IWorkspace workspace )
		{
			Project currentProject = ( ( WorkspaceEx )workspace ).ProjectContext.CurrentProject; 
			BiomeListLatitudeDistributionModel distributions = ( ( SpherePlanetProject )currentProject ).BiomeDistributions;
			return ( Control )m_ViewFactory.CreateBiomeDistributionView( distributions );
		}

		/// <summary>
		/// Creates an ocean template view
		/// </summary>
		private Control CreateOceanTemplateView( IWorkspace workspace )
		{
			return ( Control )WorkspaceViewFactory.CreateWaveAnimatorView( workspace, m_ViewFactory, m_WaveAnimationModel );
		}

		/// <summary>
		/// Creates an atmosphere template view control
		/// </summary>
		private Control CreateBiomeListView( IWorkspace workspace )
		{
			return ( Control )WorkspaceViewFactory.CreateBiomeListView( ( WorkspaceEx )workspace, m_ViewFactory, m_BiomeListModel );
		}

		/// <summary>
		/// Creates an atmosphere template view control
		/// </summary>
		private Control CreateBiomeTerrainTextureView( IWorkspace workspace )
		{
			WorkspaceEx workspaceEx = ( WorkspaceEx )workspace;
			SpherePlanetProject project = ( SpherePlanetProject )( workspaceEx .ProjectContext.CurrentProject );
			return ( Control )WorkspaceViewFactory.CreateBiomeTerrainTextureView( m_ViewFactory, workspaceEx.SelectedBiomeContext, project.Biomes );
		}

		/// <summary>
		/// Creates a planet view control
		/// </summary>
		private Control CreatePlanetView( IWorkspace workspace )
		{
			return ( Control )m_ViewFactory.CreateUniCameraView( );
		}

		#endregion
	}
}
