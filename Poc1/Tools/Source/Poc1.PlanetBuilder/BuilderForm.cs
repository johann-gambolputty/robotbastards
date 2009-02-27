using System;
using System.Windows.Forms;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Interaction;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;
using Rb.Interaction.Windows;
using Rb.Log;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.PlanetBuilder
{
	public partial class BuilderForm : Form
	{
		public BuilderForm( )
		{
			InitializeComponent( );
		}

		private TerrainVisualiserForm m_TerrainVisForm;

		/// <summary>
		/// Creates the camera used by the main display
		/// </summary>
		private ICamera CreateCamera( ICommandUser user )
		{
			FirstPersonCamera camera = new FirstPersonCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 15000.0f;

			Units.Metres cameraPos = BuilderState.Instance.SpherePlanet.PlanetModel.Radius;
			if ( BuilderState.Instance.SpherePlanet.PlanetModel.TerrainModel != null )
			{
				cameraPos += BuilderState.Instance.SpherePlanet.PlanetModel.TerrainModel.MaximumHeight;
			}
			else
			{
				cameraPos += new Units.Metres( 1000000 );
			}

			camera.Position = new UniPoint3( cameraPos.ToUniUnits, 0, 0 );
			new FirstPersonCameraController( user, camera );

			testDisplay.OnBeginRender += delegate { InteractionUpdateTimer.Instance.OnUpdate( ); };

			CommandControlInputSource.StartMonitoring( user, testDisplay, FirstPersonCameraCommands.DefaultBindings );

			return camera;
		}

		private void BuilderForm_Shown( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			Viewer viewer = new Viewer( );
			try
			{
				SolarSystem system = new SolarSystem( );
				system.Planets.Add( BuilderState.Instance.Planet );

				viewer.Renderable = new RenderableList( new FpsDisplay( ), system );
			}
			catch ( Exception ex )
			{
				//	TODO: AP: Remove exception handler
				//	This is a bodge of an exception handler. Accessing BuilderState.Instance creates
				//	the planet, which can throw. So this try-catch assumes that this is the first access...
				AppLog.Exception( ex, "Error accessing planet instance" );
				ShowExceptionForm.Display( this, ex, "Error accessing planet instance ({0})", ex.Message );
				Close( );
				return;
			}
			viewer.PreRender +=
				delegate
				{
					DebugText.Write( "Camera(m): {0}", ( ( IUniCamera )viewer.Camera ).Position.ToMetresString( ) );
					DebugText.Write( "Camera Z: {0}", ( ( IUniCamera )viewer.Camera ).Frame.ZAxis );
					DebugText.Write( "Camera 'Z: {0}", ( ( IUniCamera )viewer.Camera ).InverseFrame.ZAxis );
				};

		//	Control focusControl = ActiveControl;
		//	testDisplay.MouseEnter += delegate { testDisplay.Focus( ); };
		//	testDisplay.MouseLeave += delegate { testDisplay.Focus( ); };
			testDisplay.AddViewer( viewer );

			ICommandUser user = CommandUser.Default;
			viewer.Camera = CreateCamera( user );
		}

		private void BuilderForm_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			try
			{
				BuilderState.Instance.Planet = null;	//	Forces dispose of current planet
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Error destroying planet instance" );
				ShowExceptionForm.Display( this, ex, "Error destroying planet instance ({0})", ex.Message );
			}
			Graphics.Renderer.Dispose( );
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void buildToolStripMenuItem_Click( object sender, EventArgs e )
		{
			try
			{
				AppUtils.BuildAssets( );
			}
			catch ( Exception ex )
			{
				ShowExceptionForm.Display( this, ex, "Error running data build step ({0})", ex.Message );
			}
		}

		private void terrainVisualiserToolStripMenuItem_Click( object sender, EventArgs e )
		{
			if ( ( m_TerrainVisForm != null ) && ( !m_TerrainVisForm.IsDisposed ) )
			{
				return;
			}
			m_TerrainVisForm = new TerrainVisualiserForm( );
			m_TerrainVisForm.Show( this );
			m_TerrainVisForm.TerrainModel = BuilderState.Instance.Planet.PlanetModel.TerrainModel;
		}
	}
}