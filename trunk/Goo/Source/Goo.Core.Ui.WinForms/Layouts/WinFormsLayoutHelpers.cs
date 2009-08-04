
using System.Windows.Forms;
using Goo.Core.Mvc;

namespace Goo.Core.Ui.WinForms.Layouts
{
	/// <summary>
	/// Winforms layout helpers
	/// </summary>
	public static class WinFormsLayoutHelpers
	{
		/// <summary>
		/// Gets the control inside a view
		/// </summary>
		public static Control TryGetViewAsControl( IView view )
		{
			return ( view as Control );
		}

		/// <summary>
		/// Gets the control inside a view
		/// </summary>
		public static Control GetViewAsControl( IView view )
		{
			return ( Control )view;
		}
	}
}
