using System.Windows.Forms;

namespace Poc0.LevelEditor
{
	public partial class LogForm : Form
	{
		public LogForm( )
		{
			InitializeComponent( );

			Icon = Properties.Resources.AppIcon;
		}

		private void LogForm_Load( object sender, System.EventArgs e )
		{
			vsLogListView1.RefreshView( );
		}
	}
}