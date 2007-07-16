using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Poc0.LevelEditor
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			InitializeComponent( );
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void logToolStripMenuItem_Click( object sender, EventArgs e )
		{
		}

	}
}