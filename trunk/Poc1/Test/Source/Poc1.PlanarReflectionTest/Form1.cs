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
		private TechniqueSelector m_Technique;
		/// <summary>
		/// Creates a camera for the main display
		/// </summary>
		private ICamera CreateCamera( CommandUser user )
		{
			FlightCamera camera = new FlightCamera( );
			camera.Position = new Point3( 0, 10, 20 );
			new FlightCameraController( user, camera );
			CommandControlInputSource.StartMonitoring( CommandUser.Default, display1, FlightCameraController.DefaultBindings );

			return camera;
		}

		/// <summary>
		/// Renders some spheres for reference purposes
		/// </summary>
		private void RenderSpheres( IRenderContext context )
		{
			m_Technique.Apply
			(
				context,
				delegate
				{
					Graphics.Draw.Sphere( null, new Point3( 0, 10, 0 ), 4, 40, 40 );
					Graphics.Draw.Sphere( Graphics.Surfaces.White, new Point3( 0, 3, 0 ), 1, 40, 40 );
					Graphics.Draw.Sphere( Graphics.Surfaces.Red, new Point3( 3, 3, 0 ), 1, 40, 40 );
					Graphics.Draw.Sphere( Graphics.Surfaces.Red, new Point3( 6, 3, 0 ), 1, 40, 40 );
					Graphics.Draw.Sphere( Graphics.Surfaces.Blue, new Point3( 0, 3, 3 ), 1, 40, 40 );
					Graphics.Draw.Sphere( Graphics.Surfaces.Blue, new Point3( 0, 3, 6 ), 1, 40, 40 );
				}
			);
		}

		private void Form1_Load( object sender, System.EventArgs e )
		{
			m_Technique = new TechniqueSelector( "PlanarReflectionTest/diffuseLit.cgfx", true, "DefaultTechnique" );

			m_Camera = CreateCamera( CommandUser.Default );
			m_Scene = new ReflectionScene
				(
					new ReflectionPlane( ),
					new DelegateRenderable( RenderSpheres ),
					new ReflectionTextureDisplay( )
				);
			display1.AddViewer( new Viewer( this, m_Camera, m_Scene ) );
			//	TODO: AP: Horrible bodge to work around InteractionUpdateTimer not working properly without manual intervention
			display1.OnBeginRender += delegate { InteractionUpdateTimer.Instance.OnUpdate( ); };

		}

	}
}