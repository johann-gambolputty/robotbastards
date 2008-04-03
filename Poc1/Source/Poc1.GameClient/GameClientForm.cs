using System;
using System.Windows.Forms;
using Poc1.Universe;
using Poc1.Universe.Classes;
using Rb.Rendering;
using Rb.Rendering.Cameras;

namespace Poc1.GameClient
{
	public partial class GameClientForm : Form
	{
		public GameClientForm( )
		{
			InitializeComponent( );
		}

		private void GameClientForm_Load( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			SpherePlanet planet = new SpherePlanet( null, "TEST", 8.0f );
			SpherePlanet moon = new SpherePlanet( null, "TEST", 3.0f );
			moon.Transform.Position = new UniPoint3( 13, 0, 0 );
			planet.Moons.Add( moon );

			Viewer viewer = new Viewer( );
			viewer.Camera = new SphereCamera( 0, 0, 50 );
			viewer.ShowFps = true;
			viewer.Control = this;
			viewer.Renderable = planet;
			gameDisplay.AddViewer( viewer );
		}
	}
}