using System;
using System.Windows.Forms;

namespace Rb.Core.Assets.Windows
{
	public partial class FileLocationBrowser : UserControl, ILocationBrowser
	{
		public FileLocationBrowser( )
		{
			InitializeComponent( );
		}

		#region ILocationBrowser Members

		public ISource[] Sources
		{
			get { throw new Exception( "The method or operation is not implemented." ); }
		}

		#endregion
	}
}
