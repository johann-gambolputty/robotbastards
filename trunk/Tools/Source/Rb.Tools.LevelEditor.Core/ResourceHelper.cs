using System.Windows.Forms;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// Handy resource lookups
	/// </summary>
	public static class ResourceHelper
	{
		/// <summary>
		/// Returns the name of a given mouse button
		/// </summary>
		public static string MouseButtonName( MouseButtons button )
		{
			if ( ( button & MouseButtons.Left ) != 0 )
			{
				return Properties.Resources.LeftMouseButton;
			}
			else if ( ( button & MouseButtons.Middle ) != 0 )
			{
				return Properties.Resources.MiddleMouseButton;
			}
			else if ( ( button & MouseButtons.Right ) != 0 )
			{
				return Properties.Resources.RightMouseButton;
			}

			return "";
		}
	}
}
