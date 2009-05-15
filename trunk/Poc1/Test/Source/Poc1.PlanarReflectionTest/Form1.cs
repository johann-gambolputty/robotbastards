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

namespace Poc1.PlanarReflectionTest
{
	public partial class Form1 : Form
	{
		public Form1( )
		{
			InitializeComponent( );
		}

		private ICamera m_Camera;
		private IRenderable m_Scene;

		private ICamera CreateCamera( CommandUser user )
		{
			FlightCamera camera = new FlightCamera( );
			camera.Position = new Point3( 0, 10, 20 );
			new FlightCameraController( CommandUser.Default, camera );
			CommandControlInputSource.StartMonitoring( CommandUser.Default, display1, FlightCameraController.DefaultBindings );
		//	SphereCamera camera = new SphereCamera( Constants.HalfPi, Constants.Pi / 4, 30 );
		//	new SphereCameraController( user, camera );
		//	CommandControlInputSource.StartMonitoring( CommandUser.Default, display1, SphereCameraController.DefaultBindings );

			return camera;
		}

		private static void RenderSphere( IRenderContext context )
		{
			Graphics.Draw.Sphere( Graphics.Surfaces.Red, new Point3( 0, 10, 0 ), 4, 40, 40 );
		}

		private void Form1_Load( object sender, System.EventArgs e )
		{
			m_Camera = CreateCamera( CommandUser.Default );
			m_Scene = new ReflectionScene
				(
					new ReflectionPlane( ),
					new DelegateRenderable( RenderSphere ),
					new ReflectionTextureDisplay( )
				);
			display1.AddViewer( new Viewer( this, m_Camera, m_Scene ) );
			//	TODO: AP: Horrible bodge to work around InteractionUpdateTimer not working properly without manual intervention
			display1.OnBeginRender += delegate { InteractionUpdateTimer.Instance.OnUpdate( ); };

		}

	}
}