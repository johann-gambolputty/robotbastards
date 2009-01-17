using System;
using System.Windows.Forms;
using Bob.Core.Commands;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Ui.Interfaces.Views;
using Bob.Core.Windows.Forms;
using Bob.Core.Windows.Forms.Ui;
using Bob.Core.Windows.Forms.Ui.Docking;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Commands;
using Poc1.Bob.Core.Classes.Templates.Planets.Spherical;
using Poc1.Bob.Core.Interfaces.Templates;
using Poc1.Bob.Templates;
using Poc1.Tools.Waves;
using Rb.Interaction.Classes;
using Rb.Log.Controls.Vs;
using Rb.Rendering.Windows;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc1.Bob.Controls
{
	public partial class MainForm : Form, IMainApplicationDisplay
	{
		public MainForm( )
		{
			InitializeComponent( );

			m_ViewManager = new DockingViewManager( mainDockPanel );
			m_MessageUi = new MessageUiProvider( this );
			m_ViewFactory = new ViewFactory( m_MessageUi );
			m_Templates = new TemplateGroupContainer
				(
					"All Templates", "All Templates",
					new TemplateGroupContainer
					(
						"Planets", "Planet Templates",
						new TemplateGroup
						(
							"Spherical Planets", "Spherical Planet Templates",
							new TemplateGroup
							(
								"Spherical Planet Environments", "Spherical Planet Environment Templates",
								new SpherePlanetAtmosphereTemplate( )
								//new Template( "Biomes", "Spherical Planet Biome Template" ),
								//new Template( "Clouds", "Spherical Planet Cloud Template" ),
								//new Template( "Oceans", "Spherical Planet Ocean Template" )
							),
							new SpherePlanetDockingTemplate( m_ViewFactory )
						)
					)
				);

			m_Workspace = new WorkspaceEx( this, m_Templates );
			m_LogDisplay = new VsLogListView( );
			m_Windows = new WorkspaceWindowInfo[]
				{
					new WorkspaceWindowInfo( "Biomes", "&Biome List", delegate { return ( Control )m_ViewFactory.CreateBiomeListView( m_Workspace.Services.SafeService<SelectedBiomeContext>( ), new BiomeListModel( ) ); } ),
					new WorkspaceWindowInfo( "Biomes", "Biome &Terrain Texturing", delegate { return ( Control )m_ViewFactory.CreateBiomeTerrainTextureView( m_Workspace.Services.SafeService<SelectedBiomeContext>( ) ); } ),
					new WorkspaceWindowInfo( "Oceans", "&Wave Animator", delegate { return ( Control )m_ViewFactory.CreateWaveAnimatorView( new WaveAnimationParameters( ) ); } ),
					new WorkspaceWindowInfo( "", "&Planet View", delegate { return ( Control )new Display( ); }, DockState.Document ),
					new WorkspaceWindowInfo( "", "&Output", delegate { return m_LogDisplay; } ),
					new WorkspaceWindowInfo( "", "&Templates", delegate { return ( Control )m_ViewFactory.CreateTemplateSelectorView( m_Templates ); } )
				};
			m_CommandUi = new MenuCommandUiManager( mainMenu, new WorkspaceCommandTriggerDataFactory( m_Workspace ) );
			m_CommandUi.AddCommands( DefaultCommands.HelpCommands.Commands );
			m_CommandUi.AddCommands( DefaultCommands.FileCommands.Commands );

			m_CommandUi.AddCommands( new Command[] { TemplateCommands.NewFromTemplate } );

			DefaultCommandListener defaultListener = new DefaultCommandListener( m_ViewFactory );
			defaultListener.StartListening( );

			TemplateCommandListener listener = new TemplateCommandListener( m_ViewFactory, m_Templates );
			listener.StartListening( );
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
		private readonly DockingViewManager m_ViewManager;
		private readonly MenuCommandUiManager m_CommandUi;
		private readonly TemplateGroupContainer m_Templates;
		private readonly Control m_LogDisplay;
		private readonly WorkspaceWindowInfo[] m_Windows;
		private readonly IWorkspace m_Workspace;
		private readonly ViewFactory m_ViewFactory;

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