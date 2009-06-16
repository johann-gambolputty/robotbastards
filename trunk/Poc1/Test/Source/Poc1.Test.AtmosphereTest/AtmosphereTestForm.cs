using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Interaction;
using Rb.Interaction.Classes;
using Rb.Interaction.Windows;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Tools.Cameras;

namespace Poc1.AtmosphereTest
{
	public partial class AtmosphereTestForm : Form
	{
		public AtmosphereTestForm( )
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
			camera.Position = new Point3( 0, 0, 120  );
			FlightCameraController controller = new FlightCameraController( user, camera );
			controller.MaxForwardSpeed = 20;
			controller.MaxSlipSpeed = 20;
			

			CommandControlInputSource.StartMonitoring( CommandUser.Default, display, FlightCameraController.DefaultBindings );

			return camera;
		}

		#region Event Handlers

		private void AtmosphereTestForm_Load( object sender, System.EventArgs e )
		{
			float planetRadius = 100.0f;
			float atmosphereThickness = 10.0f;

			IRenderable scene = new RenderableList( new PlanetSurface( planetRadius ), new AtmosphereSurface( planetRadius + atmosphereThickness ) );

			display.AddViewer( new Viewer( this, CreateCamera( CommandUser.Default ), scene ) );

			//	TODO: AP: Horrible bodge to work around InteractionUpdateTimer not working properly without manual intervention
			display.OnBeginRender += delegate { InteractionUpdateTimer.Instance.OnUpdate( ); };
		}

		#endregion

		#endregion

	}
}