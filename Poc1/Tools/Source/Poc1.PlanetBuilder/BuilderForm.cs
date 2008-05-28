using System.Windows.Forms;
using Poc1.Universe.Classes.Cameras;
using Poc1.Universe.Classes.Rendering;
using Rb.Rendering;
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

		private static ICamera CreateCamera( )
		{
			HeadCamera camera = new HeadCamera( );
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

				m_TerrainMesh = null;

				return m_TerrainMesh;
			}
		}

		private void BuilderForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Camera = CreateCamera( );
			viewer.Renderable = TerrainMesh;

			testDisplay.AddViewer( viewer );
		}
	}
}