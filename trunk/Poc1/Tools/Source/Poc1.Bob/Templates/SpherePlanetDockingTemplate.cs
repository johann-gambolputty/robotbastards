using System.Windows.Forms;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Templates.Planets.Spherical;
using Poc1.Bob.Core.Interfaces;
using Poc1.Tools.Waves;
using Rb.Core.Utils;

namespace Poc1.Bob.Templates
{
	/// <summary>
	/// Adds dockable views to the SpherePlanetTemplate
	/// </summary>
	public class SpherePlanetDockingTemplate : SpherePlanetTemplate
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="viewFactory">View factory</param>
		public SpherePlanetDockingTemplate( IViewFactory viewFactory )
		{
			Arguments.CheckNotNull( viewFactory, "viewFactory" );
			m_ViewFactory = viewFactory;
			m_Views = new DockingViewInfo[]
				{
					new DockingViewInfo( "Biome List View", CreateBiomeListView ),
					new DockingViewInfo( "Biome Terrain Texture View", CreateBiomeTerrainTextureView ),
					new DockingViewInfo( "Ocean Template View", CreateOceanTemplateView ), 
				};
		}

		/// <summary>
		/// Gets the views associated with this template
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
			return ( Control )WorkspaceViewFactory.CreateBiomeTerrainTextureView( ( WorkspaceEx )workspace, m_ViewFactory );
		}


		#endregion
	}
}