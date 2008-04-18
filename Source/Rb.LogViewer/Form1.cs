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

		private void Form1_Load(object sender, EventArgs e)
		{
			string[] args = Environment.GetCommandLineArgs( );
			if ( args.Length > 1 )
			{
				m_logView.OpenFile( args[ 1 ], false );
			}
		}
    }
}