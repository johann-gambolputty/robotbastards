using System.Windows.Forms;

namespace Poc1.PlanetBuilder
{
	public partial class BuilderControls : UserControl
	{
		public BuilderControls( )
		{
			InitializeComponent( );
		}

		private void BuilderControls_Load( object sender, System.EventArgs e )
		{
			atmosphereTabPage.Select( );
		}
	}
}
