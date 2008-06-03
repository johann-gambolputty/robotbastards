using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.PlanetBuilder
{
	public partial class BuilderForm : Form
	{
		public BuilderForm( )
		{
			InitializeComponent( );
		}

		private IRenderable m_TerrainMesh;

		private static ICamera CreateCamera( InputContext context, CommandUser user )
		{
			FlightCamera camera = new FlightCamera( );
			camera.Position = new Point3( 0, 0, -5 );
			camera.AddChild( new BuilderCameraController( context, user ) );
			return camera;
		}

		private IRenderable TerrainMesh
		{
			get
			{
				if ( m_TerrainMesh != null )
				{
					return m_TerrainMesh;
				}

				m_TerrainMesh = new TerrainMesh( 2048, 64, 2048 );

				return m_TerrainMesh;
			}
		}

		private void BuilderForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Renderable = TerrainMesh;

			testDisplay.AddViewer( viewer );


			InputContext context = new InputContext( viewer );
			CommandUser user = new CommandUser( );

			viewer.Camera = CreateCamera( context, user );
		}
	}
}