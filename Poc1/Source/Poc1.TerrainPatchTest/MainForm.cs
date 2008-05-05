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

	//	private PatchGrid m_Patches;
		private QuadPatchTree m_Patches;
		private readonly SphereCamera m_Camera = new SphereCamera( 0, 0.3f, 250 );

		private void MainForm_MouseWheel( object sender, MouseEventArgs e )
		{
			if ( e.Delta > 0 )
			{
				if ( m_Patches.UseCameraDist )
				{
					m_Camera.Zoom += 2.0f;
				}
				else
				{
					m_Patches.CameraDistanceToPatch += 2.0f;
				}
			}
			else
			{
				if ( m_Patches.UseCameraDist )
				{
					m_Camera.Zoom -= 2.0f;
				}
				else
				{
					m_Patches.CameraDistanceToPatch -= 2.0f;
				}
			}
		}

		private void MainForm_Load( object sender, System.EventArgs e )
		{
			m_Camera.PerspectiveZNear = 0.1f;

		//	m_Patches = new PatchGrid( new Terrain( 256 ), 5, 5 );
			m_Patches = new QuadPatchTree( new Terrain( 256 ) );
			Viewer viewer = new Viewer( );
			viewer.Camera = m_Camera;
			viewer.Renderable = m_Patches;
			display1.AddViewer( viewer );
		}

		private void display1_KeyUp( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Space )
			{
				m_Patches.UseCameraDist = !m_Patches.UseCameraDist;
			}
		}
	}
}