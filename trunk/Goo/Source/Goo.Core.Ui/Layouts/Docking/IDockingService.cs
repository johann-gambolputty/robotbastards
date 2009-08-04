using Goo.Core.Mvc;

namespace Goo.Core.Ui.Layouts.Docking
{
	/// <summary>
	/// Docking layouts
	/// </summary>
	public interface IDockingService
	{
		/// <summary>
		/// Creates a docking frame with the specified title
		/// </summary>
		/// <param name="title">Frame title</param>
		/// <returns>Returns a new frame</returns>
		IDockingFrame CreateFrame( string title );

		/// <summary>
		/// Shows a view in a new docking frame
		/// </summary>
		/// <param name="title">Frame title</param>
		/// <param name="view">View to show</param>
		/// <param name="initialDockLocation">Initial docking state of the frame</param>
		/// <returns>Returns the docking frame created to host the view</returns>
		IDockingFrame Show( string title, IView view, DockLocation initialDockLocation );

		/// <summary>
		/// Shows a view in an existing docking frame
		/// </summary>
		/// <param name="frame">Existing frame</param>
		/// <param name="view">View to show</param>
		void Show( IDockingFrame frame, IView view );
	}
}
