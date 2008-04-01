using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
		}
	}
}