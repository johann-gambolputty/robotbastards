using System;
using System.Windows.Forms;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Interaction;
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

		/// <summary>
		/// Creates the camera used by the main display
		/// </summary>
		private static ICamera CreateCamera( InputContext context, CommandUser user )
		{
		//	FlightCamera camera = new FlightCamera( );
			UniCamera camera = new HeadCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 10000.0f;

			Units.Metres cameraPos = BuilderState.Instance.SpherePlanet.Radius;
			cameraPos += BuilderState.Instance.SpherePlanet.TerrainModel.MaximumHeight;

			camera.Position = new UniPoint3( cameraPos.ToUniUnits, 0, 0 );
			camera.AddChild( new BuilderCameraController( context, user ) );
		//	camera.AddChild( new HeadCameraController( context, user ) );
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
				viewer.Renderable = new RenderableList( new StarBox( ), BuilderState.Instance.Planet );;
			}
			catch ( Exception ex )
			{
				//	TODO: AP: Remove exception handler
				//	This is a bodge of an exception handler. Accessing BuilderState.Instance creates
				//	the planet, which can throw. So this try-catch assumes that this is the first access...
				AppLog.Exception( ex, "Error accessing planet instance" );
				MessageBox.Show( this, string.Format( "Error accessing planet instance ({0})", ex.Message ) );
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

			testDisplay.AddViewer( viewer );

			InputContext context = new InputContext( viewer );
			CommandUser user = new CommandUser( );

			viewer.Camera = CreateCamera( context, user );
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
				MessageBox.Show( this, string.Format( "Error destroying planet instance ({0})", ex.Message ) );
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
				MessageBox.Show( this, string.Format( "Error running data build step ({0})", ex.Message ) );
			}
		}
	}
}