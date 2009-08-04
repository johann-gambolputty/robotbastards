using Goo.Core.Mvc;

namespace Goo.Common.Layouts.Dialogs
{
	/// <summary>
	/// Displays views in modal or modeless dialogs
	/// </summary>
	public interface IDialogService
	{
		/// <summary>
		/// Creates a default dialog frame
		/// </summary>
		/// <param name="frameType">Default frame type</param>
		/// <param name="title">Title text</param>
		/// <returns>Returns a new dialog frame</returns>
		IDialogFrame CreateDefaultDialogFrame( DefaultDialogFrameType frameType, string title );

		/// <summary>
		/// Shows a view in a default modal frame
		/// </summary>
		/// <param name="frameType">Default frame type to use</param>
		/// <param name="title">Frame title text</param>
		/// <param name="view">View to show in the frame</param>
		/// <returns>Returns the result of the dialog</returns>
		ResultOfDialog ShowModal( DefaultDialogFrameType frameType, string title, IView view );

		/// <summary>
		/// Shows a view in a default modeless frame
		/// </summary>
		/// <param name="frameType">Default frame type to use</param>
		/// <param name="title">Frame title text</param>
		/// <param name="view">View to show in the frame</param>
		/// <returns>Returns the new default dialog frame used to show the view</returns>
		IDialogFrame Show( DefaultDialogFrameType frameType, string title, IView view );

	}
}
