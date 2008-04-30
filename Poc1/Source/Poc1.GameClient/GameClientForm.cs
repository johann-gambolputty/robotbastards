using System;
using System.Reflection;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.GameClient.Properties;
using Poc1.Universe;
using Poc1.Universe.Classes;
using Rb.Assets;
using Rb.Interaction;
using Rb.Log;
using Rb.Log.Controls.Vs;
using Rb.ProfileViewerControls;
using Rb.Rendering;

namespace Poc1.GameClient
{
	public partial class GameClientForm : Form
	{
		public GameClientForm( )
		{
			InitializeComponent( );

			//	Write greeting
			AppLog.Info( "Beginning {0} at {1}", Assembly.GetCallingAssembly( ), DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );
		}

		private readonly Control m_LogDisplay = new VsLogListView( );
		private Content m_LogDisplayContent;
		private Content m_ProfileViewer1Content;
		private Content m_ProfileViewer2Content;
		private DockingManager m_DockingManager;
		private readonly CommandUser m_User = new CommandUser( );
		private SolarSystem m_SolarSystem;

		private void GameClientForm_Shown( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			gameDisplay.OnBeginPaint += delegate { GameProfiles.Game.Rendering.Begin( ); };
			gameDisplay.OnEndPaint += delegate
			                          {
										GameProfiles.Game.Rendering.End( );
										GameProfiles.Game.Rendering.Reset( );
			                          };

			m_SolarSystem = CreateSolarSystem( );

			//	Load the game viewer
			try
			{
				LoadParameters loadParams = new LoadParameters( );
				loadParams.Properties[ "user" ] = m_User;
				Viewer viewer = ( Viewer )AssetManager.Instance.Load( "Viewers/TestGameViewer.components.xml", loadParams );
				viewer.Renderable = m_SolarSystem;

				gameDisplay.AddViewer( viewer );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Error occurred creating game viewer" );
				ErrorMessageBox.Show( this, Resources.ErrorCreatingGameViewer, ex );
			}

			//	Load the game controls
			try
			{
			//	CommandInputTemplateMap gameControlsMap = ( CommandInputTemplateMap )AssetManager.Instance.Load( "Input/TestTrackingCameraInputs.components.xml" );
				CommandInputTemplateMap gameControlsMap = ( CommandInputTemplateMap )AssetManager.Instance.Load( "Input/HeadCameraInputs.components.xml" );

				foreach ( Viewer viewer in gameDisplay.Viewers )
				{
					gameControlsMap.BindToInput( new InputContext( viewer ), m_User );
				}
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Error occurred creating game controls" );
				ErrorMessageBox.Show( this, Resources.ErrorCreatingGameControls, ex );
			}
		}

		private static SolarSystem CreateSolarSystem( )
		{
			SolarSystem system = new SolarSystem( );

			SpherePlanet planet = new SpherePlanet( null, "TEST0", 80000.0 );
		//	SpherePlanet moon = new SpherePlanet( new CircularOrbit( planet, 1500000.0, TimeSpan.FromSeconds( 60 ) ), "TEST1", 300000.0f );
			//SpherePlanet moon1 = new SpherePlanet( new CircularOrbit( moon, 500000.0, TimeSpan.FromSeconds( 60 ) ), "TEST2", 100000.0f );
			//moon.Moons.Add( moon1 );
			//planet.Moons.Add( moon );

			system.Planets.Add( planet );
		//	system.Planets.Add( moon );
			//system.Planets.Add( moon1 );

			return system;
		}

		private void GameClientForm_Load( object sender, EventArgs e )
		{
			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = gameDisplay;
			m_DockingManager.OuterControl = this;

			ProfileViewer profileViewer1 = new ProfileViewer( );
			profileViewer1.RootSection = GameProfiles.Game;
			m_ProfileViewer1Content = m_DockingManager.Contents.Add( profileViewer1, "Profile Viewer 1" );
			m_DockingManager.AddContentWithState( m_ProfileViewer1Content, State.Floating );
			m_DockingManager.HideContent( m_ProfileViewer1Content );

			ProfileViewer profileViewer2 = new ProfileViewer( );
			profileViewer2.RootSection = GameProfiles.Game;
			m_ProfileViewer2Content = m_DockingManager.Contents.Add( profileViewer2, "Profile Viewer 2" );
			m_DockingManager.AddContentWithState( m_ProfileViewer2Content, State.Floating );
			m_DockingManager.HideContent( m_ProfileViewer2Content );

			m_LogDisplayContent = m_DockingManager.Contents.Add( m_LogDisplay, "Log" );
			m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );
		}

		private void GameClientForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			m_SolarSystem.Dispose( );
			Graphics.Renderer.Dispose( );
		}

		private void profileWindow1ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( !m_ProfileViewer1Content.Visible )
			{
				m_DockingManager.ShowContent( m_ProfileViewer1Content );
			}
		}

		private void profileWindow2ToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( !m_ProfileViewer2Content.Visible )
			{
				m_DockingManager.ShowContent( m_ProfileViewer2Content );
			}
		}

		private void logWindowToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( !m_LogDisplayContent.Visible )
			{
				m_DockingManager.ShowContent( m_LogDisplayContent );
			}
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}
	}
}