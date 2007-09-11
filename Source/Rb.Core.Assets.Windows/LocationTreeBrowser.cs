using System;
using System.Windows.Forms;

namespace Rb.Core.Assets.Windows
{
	public partial class LocationTreeBrowser : UserControl, ILocationBrowser
	{
		public LocationTreeBrowser( )
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
