using System.Windows.Forms;
using Rb.Common.Cameras;
using Rb.Common.Cameras.Controllers;
using Rb.Core.Maths;
using Rb.Interaction.Classes;
using Rb.Interaction.Windows;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Test.LocalTexturePatch
{
	public partial class LocalTexturePatchTestForm : Form
	{
		public LocalTexturePatchTestForm( )
		{
			InitializeComponent( );
		}

		#region Private Members

		/// <summary>
		/// Creates a camera for the main display
		/// </summary>
		private ICamera CreateCamera( CommandUser user )
		{
			FlightCamera camera = new FlightCamera( );
			camera.PerspectiveZNear = 2.0f;
			camera.PerspectiveZFar = 2000.0f;
			camera.Position = new Point3( 0, 0, 1001 );
			FlightCameraController controller = new FlightCameraController( user, camera );
			controller.MaxForwardSpeed = 20;
			controller.MaxSlipSpeed = 20;

			CommandControlInputSource.StartMonitoring( CommandUser.Default, display1, FlightCameraController.DefaultBindings );

			return camera;
		}

		private void LocalTexturePatchTestForm_Load( object sender, System.EventArgs e )
		{
			IRenderable scene = new RenderableList( new PlanetSurface( planetRadius ), new AtmosphereSurface( model ) );

			Viewer viewer = new Viewer( this, CreateCamera( CommandUser.Default ), scene );
			display1.AddViewer( viewer );
		}

		#endregion
	}
}