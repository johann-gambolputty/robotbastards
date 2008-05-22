using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.Particles.Classes;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;

namespace Poc1.ParticleSystemBuilder
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			Graphics.InitializeFromConfiguration( );

			InitializeComponent( );

			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = psDisplay;
			m_DockingManager.OuterControl = this;

			IParticleSystem ps = new ParticleSystem( );
			m_ParticleSystems.Add( ps );

			BuilderControl psBuilder = new BuilderControl( );
			psBuilder.ParticleSystem = ps;
			Content psBuilderContent = m_DockingManager.Contents.Add( psBuilder, "Particle System Builder" );
			m_DockingManager.AddContentWithState( psBuilderContent, State.DockLeft );
		}

		private readonly RenderableList m_ParticleSystems = new RenderableList( );
		private readonly DockingManager m_DockingManager;

		private void MainForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Camera = new SphereCamera( Constants.HalfPi, 0, 10 );
			viewer.Renderable = m_ParticleSystems;

			psDisplay.AddViewer( viewer );
		}
	}
}