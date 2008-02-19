using System.Windows.Forms;
using Rb.Tools.LevelEditor.Core.Properties;

namespace Rb.Tools.LevelEditor.Core
{
	public static class ErrorMessageBox
	{
		public static void Show( string msg )
		{
			MessageBox.Show( msg, Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
		}

		public static void Show( string msg, params object[] args )
		{
			msg = string.Format( msg, args );
			Show( msg );
		}

		public static void Show( IWin32Window parent, string msg )
		{
			MessageBox.Show( parent, msg, Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
		}

		public static void Show( IWin32Window parent, string msg, params object[] args )
		{
			msg = string.Format( msg, args );
			Show( parent, msg );
		}
	}
}
