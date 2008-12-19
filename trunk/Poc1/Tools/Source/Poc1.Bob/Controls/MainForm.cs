using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Tools.Waves;
using Rb.Log;
using Rb.Log.Controls.Vs;
using Rb.Rendering.Windows;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc1.Bob.Controls
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );

			m_LogDisplay = new VsLogListView( );
			m_ViewFactory = new ViewFactory( new MessageUiProvider( this ) );
			m_Windows = new WorkspaceWindowInfo[]
				{
					new WorkspaceWindowInfo( "Biomes", "&Biome List", delegate { return ( Control )m_ViewFactory.CreateBiomeListView( m_Workspace, new BiomeListModel( ) ); } ),
					new WorkspaceWindowInfo( "Biomes", "Biome &Terrain Texturing", delegate { return ( Control )m_ViewFactory.CreateBiomeTerrainTextureView( m_Workspace ); } ),
					new WorkspaceWindowInfo( "Oceans", "&Wave Animator", delegate { return ( Control )m_ViewFactory.CreateWaveAnimatorView( new WaveAnimationParameters( ) ); } ),
					new WorkspaceWindowInfo( "", "&Planet View", delegate { return ( Control )new Display( ); }, DockState.Document ),
					new WorkspaceWindowInfo( "", "&Output", delegate { return m_LogDisplay; } )
				};
		}

		#region Private Members

		private readonly Control m_LogDisplay;
		private readonly WorkspaceWindowInfo[] m_Windows;
		private readonly Workspace m_Workspace = new Workspace( );
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

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

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
			foreach ( WorkspaceWindowInfo window in m_Windows )
			{
				ToolStripMenuItem menu = viewToolStripMenuItem;
				if ( !string.IsNullOrEmpty( window.Group ) )
				{
					ToolStripItem[] foundItems = menu.DropDownItems.Find( window.Group, false );
					if ( foundItems.Length == 0 )
					{
						ToolStripMenuItem subMenu = new ToolStripMenuItem( window.Group );
						subMenu.Name = window.Group;
						menu.DropDownItems.Add( subMenu );
						menu = subMenu;
					}
					else
					{
						menu = ( ToolStripMenuItem )foundItems[ 0 ];
					}
				}

				ToolStripItem item = menu.DropDownItems.Add( window.MenuName );
				item.Tag = window;
				item.Click += OnViewMenuItemClicked;
			}

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