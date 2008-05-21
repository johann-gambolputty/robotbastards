using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.Particles.Classes;
using Rb.Rendering;
using Rb.Rendering.Cameras;

namespace Poc1.ParticleSystemBuilder
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );
			
			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = psDisplay;
			m_DockingManager.OuterControl = this;

			BuilderControl psBuilder = new BuilderControl( );
			Content psBuilderContent = m_DockingManager.Contents.Add( psBuilder, "Particle System Builder" );
			m_DockingManager.AddContentWithState( psBuilderContent, State.DockLeft );
		}

		private readonly DockingManager m_DockingManager;

		private void MainForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Camera = new SphereCamera( );
			viewer.Renderable = new ParticleSystem( );

			psDisplay.AddViewer( viewer );
		}
	}
}