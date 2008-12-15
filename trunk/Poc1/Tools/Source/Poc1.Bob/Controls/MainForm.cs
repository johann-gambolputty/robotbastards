using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.Bob.Core.Classes;
using Poc1.Bob.Core.Classes.Biomes.Models;
using Poc1.Tools.Waves;
using Rb.Log;
using Rb.Log.Controls.Vs;

namespace Poc1.Bob.Controls
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );

			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = mainDisplay;
			m_DockingManager.OuterControl = this;

			m_LogDisplay = new VsLogListView( );
		}

		private readonly Control m_LogDisplay;
		private readonly DockingManager m_DockingManager;
		private readonly Workspace m_Workspace = new Workspace( );
		private readonly ViewFactory m_ViewFactory = new ViewFactory( );

		/// <summary>
		/// Returns the name of the file used to store layout information
		/// </summary>
		private static string GetLayoutConfigurationPath( )
		{
			Assembly refAsm = Assembly.GetExecutingAssembly( );
			string filename = refAsm.GetName( ).Name + " layout for " + Environment.UserName + ".xml";
			return Path.Combine( Path.GetDirectoryName( refAsm.Location ), filename );
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		/// <summary>
		/// Adds a new view window
		/// </summary>
		private void AddViewWindow( Control control, string title, State? state )
		{
			Content content = m_DockingManager.Contents.Add( control, title.Replace( "&", "" ) );
			if ( state != null )
			{
				m_DockingManager.AddContentWithState( content, state.Value );
			}
			ToolStripItem menuItem = viewToolStripMenuItem.DropDownItems.Add( title );
			menuItem.Click +=
				delegate
				{
					if ( content.Visible )
					{
						m_DockingManager.HideContent( content );
					}
					else
					{
						m_DockingManager.ShowContent( content );	
					}
				};

		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			AddViewWindow( ( Control )m_ViewFactory.CreateBiomeListView( m_Workspace, new BiomeListModel( ) ), "&Biome List", null );
			AddViewWindow( ( Control )m_ViewFactory.CreateBiomeManagerView( m_Workspace ), "&Biome Manager", null );
			AddViewWindow( ( Control )m_ViewFactory.CreateWaveAnimatorView( new WaveAnimationParameters( ) ), "&Wave Animation", null );

			viewToolStripMenuItem.DropDownItems.Add( new ToolStripSeparator( ) );

			AddViewWindow( m_LogDisplay, "&Output", State.DockBottom );

			string layoutFile = GetLayoutConfigurationPath( );
			if ( File.Exists( layoutFile ) )
			{
				AppLog.Info( "Reading layout configuration from \"{0}\"", layoutFile );
				m_DockingManager.LoadConfigFromFile( layoutFile );
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
				m_DockingManager.SaveConfigToFile( filename );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( this, "Error saving layout:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
	}
}