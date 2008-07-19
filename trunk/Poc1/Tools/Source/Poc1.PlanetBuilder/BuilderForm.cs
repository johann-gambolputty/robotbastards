using System;
using System.Windows.Forms;
using Poc1.Universe;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Interfaces;
using Rb.Interaction;
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
			camera.Position = new UniPoint3( UniUnits.Metres.ToUniUnits( BuilderState.PlanetRadius + 50 ), 0, 0 );
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
			viewer.Renderable = new RenderableList( new StarBox( ), BuilderState.Instance.Planet );
			viewer.PreRender +=
				delegate
				{
					DebugText.Write( "Camera(m): {0}", ( ( IUniCamera )viewer.Camera ).Position.ToMetresString( ) );
				};

			testDisplay.AddViewer( viewer );

			InputContext context = new InputContext( viewer );
			CommandUser user = new CommandUser( );

			viewer.Camera = CreateCamera( context, user );
		}

		private void BuilderForm_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			BuilderState.Instance.Planet = null;	//	Forces dispose of current planet
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