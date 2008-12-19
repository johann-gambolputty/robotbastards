using System.Windows.Forms;

namespace Poc1.Bob.Controls.Atmosphere
{
	public partial class AtmosphereBuildControl : UserControl
	{
		public AtmosphereBuildControl( )
		{
			InitializeComponent( );

			//	Populate texture resolution combo boxes
			for ( int i = 4; i <= 1024; i *= 2 )
			{
				opticalDepthResolutionComboBox.Items.Add( i );
				scatteringResolutionComboBox.Items.Add( i );
			}
			scatteringResolutionComboBox.SelectedItem = 16;
			opticalDepthResolutionComboBox.SelectedItem = 256;
		}
	}
}
