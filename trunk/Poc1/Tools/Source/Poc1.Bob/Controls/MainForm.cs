using System;
using System.Windows.Forms;
using Bob.Core.Commands;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms;
using Bob.Core.Windows.Forms.Ui;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Commands;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Commands;
using Poc1.Bob.Core.Classes.Projects.Planets;
using Poc1.Bob.Core.Classes.Projects.Planets.Spherical;
using Poc1.Bob.Core.Interfaces.Commands;
using Poc1.Bob.Core.Interfaces.Projects;
using Poc1.Bob.Projects;
using Rb.Interaction.Classes;

namespace Poc1.Bob.Controls
{
	public partial class MainForm : Form, IMainApplicationDisplay
	{
		public MainForm( )
		{
			InitializeComponent( );

		//	m_ViewManager = new DockingViewManager( mainDockPanel );
			m_ViewManager = new DockedHostPaneViewManager( mainDockPanel, "Properties" );
			m_MessageUi = new MessageUiProvider( this );
			m_ViewFactory = new ViewFactory( m_MessageUi );
			m_CommandViewFactory = new CommandDockingViewFactory( m_ViewManager );
			IPlanetViews planetViews = new SpherePlanetDockingViews( m_ViewManager, m_ViewFactory );
			m_Projects = new ProjectGroupContainer
				(
					"All Project Types", "All Project Types",
					new ProjectGroupContainer
					(
						"Planets", "Planet Project Types",
						new ProjectGroup
						(
							"Spherical Planets", "Spherical Planet Project Types",
							//new ProjectGroup
							//(
							//    "Spherical Planet Environments", "Spherical Planet Environment Project Types",
							//    new SpherePlanetAtmosphereProjectType( )
							//),
							new TerrestrialSpherePlanetProjectType( planetViews ),
							new SpherePlanetProjectType( planetViews )
						)
					)
				);
			m_Workspace = new WorkspaceEx( this, m_Projects );

			m_CommandUi = new MenuCommandUiManager( mainMenu, new WorkspaceCommandTriggerDataFactory( m_Workspace ) );
			m_CommandUi.AddCommands( DefaultCommands.HelpCommands.Commands );
			m_CommandUi.AddCommands( DefaultCommands.FileCommands.Commands );

			m_CommandUi.AddCommands( new Command[] { ProjectCommands.NewProject } );

			DefaultCommandListener defaultListener = new DefaultCommandListener( m_CommandViewFactory );
			defaultListener.StartListening( );

			ProjectCommandListener listener = new ProjectCommandListener( m_ViewFactory );
			listener.StartListening( );

			//DockContent content = new DockContent( );

			//content.Text = "test";
			//BiomeDistributionDisplay control = new BiomeDistributionDisplay( );
			//BiomeListModel biomes = new BiomeListModel( );
			//biomes.Models.Add( new BiomeModel( "arctic" ) );
			//biomes.Models.Add( new BiomeModel( "temperate" ) );
			//biomes.Models.Add( new BiomeModel( "desert" ) );
			//control.Distributions = new BiomeListLatitudeDistributionModel( biomes );
			//content.Controls.Add( control );
			//content.AutoScroll = true;
			//content.HideOnClose = true;

			//control.Dock = DockStyle.Fill;

			//content.Show( mainDockPanel, DockState.Float );
		}

		#region IMainApplicationDisplay Members

		/// <summary>
		/// Gets the command UI manager
		/// </summary>
		public ICommandUiManager CommandUi
		{
			get { return m_CommandUi; }
		}

		/// <summary>
		/// Gets the view manager
		/// </summary>
		public IViewManager Views
		{
			get { return m_ViewManager; }
		}

		/// <summary>
		/// Gets the message UI manager
		/// </summary>
		public IMessageUiProvider MessageUi
		{
			get { return m_MessageUi; }
		}

		#endregion

		#region Private Members

		private readonly MessageUiProvider m_MessageUi;
		private readonly IViewManager m_ViewManager;
		private readonly MenuCommandUiManager m_CommandUi;
		private readonly ProjectGroupContainer m_Projects;
		private readonly IWorkspace m_Workspace;
		private readonly ViewFactory m_ViewFactory;
		private readonly ICommandViewFactory m_CommandViewFactory;

		#region Event Handlers

		private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			try
			{
				m_ViewManager.SaveLayout( );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( this, "Error saving layout:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}

		#endregion

		#endregion
	}
}