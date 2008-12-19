using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc1.Particles.Classes;
using Poc1.Particles.Interfaces;
using Rb.Core.Maths;
using Rb.Interaction;
using Rb.Interaction.Classes;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Tools.Cameras;

namespace Poc1.ParticleSystemBuilder
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );

			psDisplay.OnBeginRender += psDisplay_OnBeginPaint;

			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = psDisplay;
			m_DockingManager.OuterControl = this;

			IParticleSystem ps = new ParticleSystem( );
			m_ParticleSystems.Add( new ParticleSystemEmitter( ps ) );

		//	BuilderControl psBuilder = new BuilderControl( );
			ParticleSystemEditor psBuilder = new ParticleSystemEditor( );
			psBuilder.ParticleSystem = ps;
			Content psBuilderContent = m_DockingManager.Contents.Add( psBuilder, "Particle System Builder" );
			m_DockingManager.AddContentWithState( psBuilderContent, State.DockLeft );

			RenderControl renderControl = new RenderControl( m_Method );
			Content renderControlContent = m_DockingManager.Contents.Add( renderControl, "Rendering" );
			m_DockingManager.AddContentWithState( renderControlContent, State.Floating );
		}

		private readonly RenderMethod m_Method = new RenderMethod( );
		private float m_Angle;
		private Point3 m_RenderPosition;
		private readonly RenderableList<ParticleSystemEmitter> m_ParticleSystems = new RenderableList<ParticleSystemEmitter>();
		private readonly DockingManager m_DockingManager;

		private static ICamera CreateCamera( CommandUser user )
		{
			SphereCamera camera = new SphereCamera( Constants.HalfPi, Constants.Pi / 4, 10 );
			camera.AddChild( new SphereCameraController( user ) );
			return camera;
		}

		private void MainForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			psDisplay.AddViewer( viewer );

			viewer.Camera = CreateCamera( new CommandUser( "me", 0 ) );
			viewer.Renderable = new RenderableList( new Grid( 8, 8 ), m_ParticleSystems );
		}

		private const float PsY = 2.0f;

		private void psDisplay_OnBeginPaint( object sender, System.EventArgs e )
		{
			switch ( m_Method.Method )
			{
				case RenderMethodType.Stationary		:
					{
						m_RenderPosition = new Point3( 0, PsY, 0 );
						break;
					}
				case RenderMethodType.Circle			:
					{
						m_Angle = Utils.Wrap( m_Angle + 0.03f, 0, Constants.TwoPi );
						m_RenderPosition = new Point3( Functions.Sin( m_Angle ) * m_Method.Radius, PsY, Functions.Cos( m_Angle ) * m_Method.Radius );
						break;
					}
				case RenderMethodType.FigureOfEight		:
					{
						m_RenderPosition = new Point3( 0, PsY, 0 );
						break;
					}
				case RenderMethodType.FollowTheMouse:
					{
						m_RenderPosition = new Point3( 0, PsY, 0 );
						break;
					}
			}

			foreach ( ParticleSystemEmitter ps in m_ParticleSystems )
			{
				ps.Frame.Translation = m_RenderPosition;
			}
		}

	}
}