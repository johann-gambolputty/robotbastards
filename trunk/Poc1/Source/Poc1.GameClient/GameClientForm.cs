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

			SpherePlanet planet = new SpherePlanet( null, "TEST0", 8.0f );
			SpherePlanet moon = new SpherePlanet( new CircularOrbit( planet, 15.0, TimeSpan.FromSeconds( 6 ) ), "TEST1", 3.0f );
			SpherePlanet moon1 = new SpherePlanet( new CircularOrbit( moon, 5.0, TimeSpan.FromSeconds( 6 ) ), "TEST2", 1.0f );
			moon.Moons.Add( moon1 );
			planet.Moons.Add( moon );

			Viewer viewer = new Viewer( );
			viewer.Camera = new SphereCamera( 0, 0, 70 );
			viewer.ShowFps = true;
			viewer.Control = this;
			viewer.Renderable = planet;
			gameDisplay.AddViewer( viewer );
		}
	}
}