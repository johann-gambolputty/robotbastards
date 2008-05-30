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

			psDisplay.OnBeginPaint += psDisplay_OnBeginPaint;

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

			RenderControl renderControl = new RenderControl( m_Method );
			Content renderControlContent = m_DockingManager.Contents.Add( renderControl, "Rendering" );
			m_DockingManager.AddContentWithState( renderControlContent, State.Floating );
		}

		private readonly RenderMethod m_Method = new RenderMethod( );
		private float m_Angle;
		private Point3 m_RenderPosition;
		private readonly RenderableList<IParticleSystem> m_ParticleSystems = new RenderableList<IParticleSystem>( );
		private readonly DockingManager m_DockingManager;

		private void MainForm_Shown( object sender, System.EventArgs e )
		{
			Viewer viewer = new Viewer( );
			viewer.Camera = new SphereCamera( Constants.HalfPi, 0, 10 );
			viewer.Renderable = m_ParticleSystems;

			psDisplay.AddViewer( viewer );
		}

		private void psDisplay_OnBeginPaint( object sender, System.EventArgs e )
		{
			switch ( m_Method.Method )
			{
				case RenderMethodType.Stationary		:
					{
						m_RenderPosition = Point3.Origin;
						break;
					}
				case RenderMethodType.Circle			:
					{
						m_Angle = Utils.Wrap( m_Angle + 0.1f, 0, Constants.TwoPi );
						m_RenderPosition = new Point3( Functions.Sin( m_Angle ) * m_Method.Radius, 0, Functions.Cos( m_Angle ) * m_Method.Radius );
						break;
					}
				case RenderMethodType.FigureOfEight		:
					{
						m_RenderPosition = Point3.Origin;
						break;
					}
				case RenderMethodType.FollowTheMouse:
					{
						m_RenderPosition = Point3.Origin;
						break;
					}
			}

			foreach ( IParticleSystem ps in m_ParticleSystems )
			{
				ps.Frame.Translation = m_RenderPosition;
			}
		}

	}
}