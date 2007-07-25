using System;
using System.Windows.Forms;

namespace Rb.LogViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openDialog = new OpenFileDialog( );
			openDialog.Filter = "Text files (*.txt)|*.txt|All Files (*.*)|*.*";
			if ( openDialog.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}
			m_logView.OpenFile( openDialog.FileName, false );
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close( );
		}
    }
}