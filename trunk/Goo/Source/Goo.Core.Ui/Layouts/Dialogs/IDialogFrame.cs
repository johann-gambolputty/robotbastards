using System;
using Goo.Core.Mvc;
using Goo.Core.Workspaces;

namespace Goo.Common.Layouts.Dialogs
{
	/// <summary>
	/// Interface for dialog-based frames that can host a view
	/// </summary>
	public interface IDialogFrame
	{
		/// <summary>
		/// Event raised when the frame is closed
		/// </summary>
		event Action<ResultOfDialog> FrameClosed;

		/// <summary>
		/// The dialog result, valid if this frame has been close
		/// </summary>
		ResultOfDialog FrameResult
		{
			get;
		}

		/// <summary>
		/// Shows this frame as a modeless frame containing the specified view
		/// </summary>
		void ShowModal( IWorkspace workspace, IView view );

		/// <summary>
		/// Shows this frame as a modal frame containing the specified view
		/// </summary>
		void ShowModeless( IWorkspace workspace, IView view );
	}
}
