
using System.Net;
using System.Windows.Forms;

namespace Rb.TestApp
{
	public partial class HostAddressForm : Form
	{
		public HostAddressForm( RemoteHostAddress address )
		{
			InitializeComponent( );

			ipAddressTextBox.Text = address.Address;
			portTextBox.Text = address.Port.ToString( );
		}

		/// <summary>
		/// Returns the IP end point
		/// </summary>
		public RemoteHostAddress Address
		{
			get
			{
				return new RemoteHostAddress( ipAddressTextBox.Text, int.Parse( portTextBox.Text ) );
			}
		}
	}
}