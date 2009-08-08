using Goo.Core.Ui.WinForms.Mvc;
using Rb.Core.Utils;

namespace Goo.Test
{
	public partial class TestView : ViewControl, ITestView
	{
		public TestView( )
		{
			InitializeComponent( );
		}

		#region ITestView Members

		public event ActionDelegates.Action Button1Clicked;

		#endregion

		private void button1_Click( object sender, System.EventArgs e )
		{
			if ( Button1Clicked != null )
			{
				Button1Clicked( );
			}
		}
	}
}
