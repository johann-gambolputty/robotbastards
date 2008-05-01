using System.Windows.Forms;
using Rb.Rendering;
using Rb.Rendering.Cameras;

namespace Poc1.TerrainPatchTest
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent( );

			MouseWheel += MainForm_MouseWheel;
		}

		private readonly SphereCamera m_Camera = new SphereCamera( 0, 0.3f, 60 );

		private void MainForm_MouseWheel( object sender, MouseEventArgs e )
		{
			if ( e.Delta > 0 )
			{
				m_Camera.Zoom += 1.0f;
			}
			else
			{
				m_Camera.Zoom -= 1.0f;	
			}
		}

		private void MainForm_Load( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Camera = m_Camera;
			viewer.Renderable = new Patch( new Terrain( 512 ) );
			display1.AddViewer( viewer );
		}
	}
}