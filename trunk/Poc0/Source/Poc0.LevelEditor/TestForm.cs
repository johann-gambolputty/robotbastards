using System;
using System.Windows.Forms;
using Rb.Core.Assets.Windows;

namespace Poc0.LevelEditor
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();
		}

		private void TestForm_Load(object sender, EventArgs e)
		{
			Control control = ( Control )( new FileLocationManagerWithUI( ).CreateControl( ) );
			control.Dock = DockStyle.Fill;
			Controls.Add(control);
		}
	}
}