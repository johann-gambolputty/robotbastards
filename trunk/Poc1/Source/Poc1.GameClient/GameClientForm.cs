using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.GameClient.Properties;
using Poc1.Universe;
using Poc1.Universe.Classes;
using Poc1.Universe.Interfaces;
using Rb.Assets;
using Rb.Core.Components;
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

			PropertyGrid debugInfoProperties = CreateDebugInfoPropertyGrid( );
			m_DebugInfoContent = m_DockingManager.Contents.Add(debugInfoProperties, "Debug Info");
			m_DockingManager.AddContentWithState( m_DebugInfoContent, State.Floating );
			m_DockingManager.HideContent( m_DebugInfoContent );

			m_LogDisplayContent = m_DockingManager.Contents.Add( m_LogDisplay, "Log" );
			m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );

			if ( File.Exists( m_ClientSetupFile ) )
			{
				m_DockingManager.LoadConfigFromFile( m_ClientSetupFile );
			}
		}

		private readonly string m_ClientSetupFile = Path.Combine( Directory.GetCurrentDirectory( ), "ClientSetup.xml" );


		private readonly Control m_LogDisplay = new VsLogListView( );
		private readonly Content m_LogDisplayContent;
		private readonly Content m_ProfileViewer1Content;
		private readonly Content m_ProfileViewer2Content;
		private readonly Content m_DebugInfoContent;
		private readonly DockingManager m_DockingManager;
		private readonly CommandUser m_User = new CommandUser( );
		private SolarSystem m_SolarSystem;
		
		private static PropertyGrid CreateDebugInfoPropertyGrid( )
		{
			//	Can't just bung the a DebugInfo object into a property grid - it's all static properties
			//	Create a property bag containing those properties instead
			PropertyBag debugInfo = new PropertyBag( );

			foreach ( PropertyInfo property in typeof( DebugInfo ).GetProperties( BindingFlags.Static | BindingFlags.Public ) )
			{
				debugInfo.Properties.Add( new ExPropertySpec( property ) );
			}
			debugInfo.SetValue += ExPropertySpec.SetValue;
			debugInfo.GetValue += ExPropertySpec.GetValue;

			PropertyGrid debugInfoPropertyGrid = new PropertyGrid( );
			debugInfoPropertyGrid.SelectedObject = debugInfo;

			return debugInfoPropertyGrid;
		}

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

			UniPoint3 initialViewPos = new UniPoint3( );
			m_SolarSystem = CreateSolarSystem( initialViewPos );

			//	Load the game viewer
			try
			{
				LoadParameters loadParams = new LoadParameters( );
				loadParams.Properties[ "user" ] = m_User;
				Viewer viewer = ( Viewer )AssetManager.Instance.Load( "Viewers/TestGameViewer.components.xml", loadParams );
				viewer.Renderable = m_SolarSystem;
				( ( IUniCamera )viewer.Camera ).Position.Z = initialViewPos.Z;

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
				CommandInputTemplateMap[] gameControlMaps = new CommandInputTemplateMap[]
					{
						( CommandInputTemplateMap )AssetManager.Instance.Load( "Input/TestTrackingCameraInputs.components.xml" ),
						( CommandInputTemplateMap )AssetManager.Instance.Load( "Input/HeadCameraInputs.components.xml" )
					};
				foreach ( Viewer viewer in gameDisplay.Viewers )
				{
					foreach ( CommandInputTemplateMap gameControlMap in gameControlMaps )
					{
						gameControlMap.BindToInput( new InputContext( viewer ), m_User );
					}
				}
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Error occurred creating game controls" );
				ErrorMessageBox.Show( this, Resources.ErrorCreatingGameControls, ex );
			}
		}

		private static SolarSystem CreateSolarSystem( UniPoint3 initialViewPos )
		{
			SolarSystem system = new SolarSystem( );

			SpherePlanet planet = new SpherePlanet( null, "TEST0", 800000.0 );
			initialViewPos.Z = planet.Radius + UniUnits.Metres.ToUniUnits( 10000 );
		//	SpherePlanet moon = new SpherePlanet( new CircularOrbit( planet, 150000.0, TimeSpan.FromSeconds( 60 ) ), "TEST1", 3000.0f );
			//SpherePlanet moon1 = new SpherePlanet( new CircularOrbit( moon, 500000.0, TimeSpan.FromSeconds( 60 ) ), "TEST2", 100000.0f );
			//moon.Moons.Add( moon1 );
		//	planet.Moons.Add( moon );

			system.Planets.Add( planet );
		//	system.Planets.Add( moon );
			//system.Planets.Add( moon1 );

			return system;
		}

		private void GameClientForm_Load( object sender, EventArgs e )
		{
		}

		private void GameClientForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			m_DockingManager.SaveConfigToFile( m_ClientSetupFile, Encoding.ASCII );

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

		private void debugInfoWindowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if ( !m_DebugInfoContent.Visible )
			{
				m_DockingManager.ShowContent( m_DebugInfoContent );
			}
		}
	}
}