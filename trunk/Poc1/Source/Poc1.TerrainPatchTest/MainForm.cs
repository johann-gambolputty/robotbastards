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

		private PatchGrid m_PatchGrid;
		private readonly SphereCamera m_Camera = new SphereCamera( 0, 0.3f, 250 );

		private void MainForm_MouseWheel( object sender, MouseEventArgs e )
		{
			if ( e.Delta > 0 )
			{
				if ( m_PatchGrid.UseCameraDist )
				{
					m_Camera.Zoom += 2.0f;
				}
				else
				{
					m_PatchGrid.CameraDistanceToPatch += 2.0f;
				}
			}
			else
			{
				if ( m_PatchGrid.UseCameraDist )
				{
					m_Camera.Zoom -= 2.0f;
				}
				else
				{
					m_PatchGrid.CameraDistanceToPatch -= 2.0f;
				}
			}
		}

		private void MainForm_Load( object sender, System.EventArgs e )
		{
			m_Camera.PerspectiveZNear = 0.1f;

			m_PatchGrid = new PatchGrid( new Terrain( 256 ), 5, 5 );
			Viewer viewer = new Viewer( );
			viewer.Camera = m_Camera;
			viewer.Renderable = m_PatchGrid;
			display1.AddViewer( viewer );
		}

		private void display1_KeyUp( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Space )
			{
				m_PatchGrid.UseCameraDist = !m_PatchGrid.UseCameraDist;
			}
		}
	}
}