using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Bob.Core.Commands;
using Bob.Core.Ui.Interfaces;
using Bob.Core.Windows.Forms;
using Bob.Core.Windows.Forms.Ui;
using Bob.Core.Workspaces.Classes;
using Bob.Core.Workspaces.Interfaces;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Bob.Core.Classes.Commands;
using Poc1.Bob.Core.Classes.Templates.Planets.Spherical;
using Poc1.Bob.Core.Interfaces.Templates;
using Poc1.Tools.Waves;
using Rb.Interaction.Classes;
using Rb.Log;
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
							new SpherePlanetTemplate( )
						)
					)
				);

			m_Workspace = new Workspace( this );
			m_Workspace.Services.AddService( new SelectedBiomeContext( ) );
			m_MessageUi = new MessageUiProvider( this );
			m_LogDisplay = new VsLogListView( );
			m_ViewFactory = new ViewFactory( m_MessageUi );
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

			DefaultCommandListener defaultListener = new DefaultCommandListener( );
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
		/// Gets the message UI manager
		/// </summary>
		public IMessageUiProvider MessageUi
		{
			get { return m_MessageUi; }
		}

		#endregion

		#region Private Members

		private readonly MessageUiProvider m_MessageUi;
		private readonly MenuCommandUiManager m_CommandUi;
		private readonly TemplateGroupContainer m_Templates;
		private readonly Control m_LogDisplay;
		private readonly WorkspaceWindowInfo[] m_Windows;
		private readonly IWorkspace m_Workspace;
		private readonly ViewFactory m_ViewFactory;

		/// <summary>
		/// Returns a dock content object from a string that has been persisted to a layout file
		/// </summary>
		private IDockContent DeserializeContent( string persistString )
		{
			foreach ( WorkspaceWindowInfo info in m_Windows )
			{
				if ( info.Control.GetType( ).Name == persistString )
				{
					return info.Content;
				}
			}
			return null;
		}
		/// <summary>
		/// Returns the name of the file used to store layout information
		/// </summary>
		private static string GetLayoutConfigurationPath( )
		{
			Assembly refAsm = Assembly.GetExecutingAssembly( );
			string filename = refAsm.GetName( ).Name + " layout for " + Environment.UserName + ".xml";
			return Path.Combine( Path.GetDirectoryName( refAsm.Location ), filename );
		}


		#region Event Handlers

		private void OnViewMenuItemClicked( object sender, EventArgs args )
		{
			WorkspaceWindowInfo info = ( WorkspaceWindowInfo )( ( ToolStripItem )sender ).Tag;
			DockContent content = info.Content;

			if ( content.Visible )
			{
				content.Hide( );
			}
			else if ( content.VisibleState == DockState.Unknown )
			{
				content.Show( mainDockPanel, info.DefaultDockState );
			}
			else
			{
				content.Show( );
			}
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			//foreach ( WorkspaceWindowInfo window in m_Windows )
			//{
			//    ToolStripMenuItem menu = viewToolStripMenuItem;
			//    if ( !string.IsNullOrEmpty( window.Group ) )
			//    {
			//        ToolStripItem[] foundItems = menu.DropDownItems.Find( window.Group, false );
			//        if ( foundItems.Length == 0 )
			//        {
			//            ToolStripMenuItem subMenu = new ToolStripMenuItem( window.Group );
			//            subMenu.Name = window.Group;
			//            menu.DropDownItems.Add( subMenu );
			//            menu = subMenu;
			//        }
			//        else
			//        {
			//            menu = ( ToolStripMenuItem )foundItems[ 0 ];
			//        }
			//    }

			//    ToolStripItem item = menu.DropDownItems.Add( window.MenuName );
			//    item.Tag = window;
			//    item.Click += OnViewMenuItemClicked;
			//}

			string layoutFile = GetLayoutConfigurationPath( );
			if ( File.Exists( layoutFile ) )
			{
				AppLog.Info( "Reading layout configuration from \"{0}\"", layoutFile );
				mainDockPanel.LoadFromXml( layoutFile, DeserializeContent );
			}
			else
			{
				AppLog.Warning( "No layout file exists at \"{0}\" - using default layout", layoutFile );
			}
		}

		private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			try
			{
				string filename = GetLayoutConfigurationPath( );
				mainDockPanel.SaveAsXml( filename );
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