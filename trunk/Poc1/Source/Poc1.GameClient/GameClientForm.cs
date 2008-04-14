using System;
using System.Reflection;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.GameClient.Properties;
using Poc1.Universe.Classes;
using Rb.Assets;
using Rb.Interaction;
using Rb.Log;
using Rb.Log.Controls.Vs;
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
		private DockingManager m_DockingManager;
		private readonly CommandUser m_User = new CommandUser( );

		private void GameClientForm_Shown( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			//	Load the game viewer
			try
			{
				LoadParameters loadParams = new LoadParameters( );
				loadParams.Properties[ "user" ] = m_User;
				Viewer viewer = ( Viewer )AssetManager.Instance.Load( "Viewers/TestGameViewer.components.xml", loadParams );
				viewer.Renderable = CreateSolarSystem( );
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
				CommandInputTemplateMap gameControlsMap = ( CommandInputTemplateMap )AssetManager.Instance.Load( "Input/TestTrackingCameraInputs.components.xml" );

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

			SpherePlanet planet = new SpherePlanet( null, "TEST0", 800000.0 );
			//SpherePlanet moon = new SpherePlanet( new CircularOrbit( planet, 1500000.0, TimeSpan.FromSeconds( 60 ) ), "TEST1", 300000.0f );
			//SpherePlanet moon1 = new SpherePlanet( new CircularOrbit( moon, 500000.0, TimeSpan.FromSeconds( 60 ) ), "TEST2", 100000.0f );
			//moon.Moons.Add( moon1 );
			//planet.Moons.Add( moon );

			system.Planets.Add( planet );
			//system.Planets.Add( moon );
			//system.Planets.Add( moon1 );

			return system;
		}

		private void GameClientForm_Load( object sender, EventArgs e )
		{
			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = gameDisplay;
			m_DockingManager.OuterControl = this;

			m_LogDisplayContent = m_DockingManager.Contents.Add( m_LogDisplay, "Log" );
			m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );
		}

	}
}