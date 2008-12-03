using System.Drawing;
using System.Windows.Forms;

namespace Rb.Common.Controls.Tabs
{
	public partial class PrettyTabControl : TabControl
	{
		public PrettyTabControl( )
		{
			InitializeComponent( );
		}

		private void PrettyTabControl_DrawItem( object sender, DrawItemEventArgs e )
		{
			TabPage page = TabPages[ e.Index ];
			Rectangle tabBounds = GetTabRect( e.Index );
			StringFormat format = new StringFormat( );
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			e.Graphics.DrawString( page.Text, Font, Brushes.Black, tabBounds, format );
		}
	}
}
