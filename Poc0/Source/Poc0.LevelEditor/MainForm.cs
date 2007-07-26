using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Rendering.OpenGl;  
using Rb.Core.Components;
using Rb.Core.Resources;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Log;
using Rb.Rendering;

namespace Poc0.LevelEditor
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			LogForm form = new LogForm( );
			form.Show( );

			//	Write greeting
			AppLog.Info( "Beginning Poc0.LevelEditor at {0}", DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			//	Load the rendering assembly
			string renderAssembly = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssembly == null )
			{
				renderAssembly = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssembly );
			
			//	Load resource settings
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

			TileTransitionMaskGenerator.Mask[] masks = new TileTransitionMaskGenerator( ).Generate( 32, 32 );

			System.IO.Directory.CreateDirectory( "Masks" );
			for ( int maskIndex = 0; maskIndex < masks.Length; ++maskIndex )
			{
				masks[ maskIndex ].ToBitmap( ).Save( "Masks/Mask" + maskIndex + ".png" );
			}

			InitializeComponent( );

			Icon = Properties.Resources.AppIcon;
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void logToolStripMenuItem_Click( object sender, EventArgs e )
		{
			LogForm form = new LogForm( );
			form.Show( );
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( !DesignMode )
			{
				//	Load input bindings
				CommandInputTemplateMap map = ( CommandInputTemplateMap )ResourceManager.Instance.Load( "LevelEditorCommandInputs.components.xml" );
				m_User.InitialiseAllCommandListBindings( );

				m_Grid = new TileGrid( TileTypeSet.CreateDefaultTileTypeSet( ) );
				m_EditState = new TileGridEditState( );
				m_EditState.TilePaintType = m_Grid.Set[ 1 ];
				m_GridRenderer = new OpenGlTileGrid2dRenderer( m_Grid, m_EditState );

				tileTypeSetListView1.TileTypes = m_Grid.Set;

				ComponentLoadParameters loadParams = new ComponentLoadParameters( );
				loadParams.Properties[ "User" ] = m_User;

				Viewer viewer = ( Viewer )ResourceManager.Instance.Load( "LevelEditorStandardViewer.components.xml", loadParams );
				viewer.Renderable = m_GridRenderer;

				viewer.Camera.AddChild( new TileEditCommandHandler( m_User, m_Grid, m_EditState ) );

				display1.Viewers.Add( viewer );

				//	TODO: AP: naughty (there should be some user service, or something, that does updates)
				new Clock( "updateClock", 10 ).Subscribe( UpdateUser );

				//	Test load a command list
				try
				{
					//	TODO: AP: May need to move
					map.AddContextInputsToUser( new InputContext( display1.Viewers[ 0 ], display1 ), m_User );
				}
				catch ( Exception ex )
				{
					ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
				}
			}
		}

		/// <summary>
		/// Updates the user every tick of the inputClock
		/// </summary>
		private void UpdateUser( Clock clock )
		{
			//	TODO: AP: Kludge
			m_User.Update( );
		}

		private CommandUser m_User = new CommandUser( );
		private TileGrid m_Grid;
		private TileGridEditState m_EditState;
		private TileGridRenderer m_GridRenderer;

		private void tileTypeSetListView1_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( tileTypeSetListView1.SelectedItems.Count == 0 )
			{
				return;
			}

			TileType type = ( TileType )tileTypeSetListView1.SelectedItems[ 0 ].Tag;
			m_EditState.TilePaintType = type;
		}
	}
}