using System.Windows.Forms;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class LogForm : Form
	{
		public LogForm( )
		{
			InitializeComponent( );
		}

		private void LogForm_Load( object sender, System.EventArgs e )
		{
			vsLogListView1.RefreshView( );
		}
	}
}