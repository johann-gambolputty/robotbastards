using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.PlanetBuilder
{
	public partial class BuilderForm : Form
	{
		public BuilderForm( )
		{
			InitializeComponent( );
		}

		private static ICamera CreateCamera( InputContext context, CommandUser user )
		{
			FlightCamera camera = new FlightCamera( );
			camera.PerspectiveZNear = 1.0f;
			camera.PerspectiveZFar = 4000.0f;
			camera.Position = new Point3( 0, -220, 0 );
			camera.AddChild( new BuilderCameraController( context, user ) );
			return camera;
		}

		private void BuilderForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Renderable = BuilderState.Instance.TerrainMesh;

			testDisplay.AddViewer( viewer );

			InputContext context = new InputContext( viewer );
			CommandUser user = new CommandUser( );

			viewer.Camera = CreateCamera( context, user );
		}
	}
}