using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;

namespace Poc0.LevelEditor
{
	public partial class LogForm : Form
	{
		public LogForm( )
		{
			InitializeComponent( );

			Icon = Properties.Resources.AppIcon;
		}
	}
}