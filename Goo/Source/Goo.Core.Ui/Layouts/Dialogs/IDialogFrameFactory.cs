using Goo.Core.Workspaces;

namespace Goo.Common.Layouts.Dialogs
{
	/// <summary>
	/// Dialog frame factory
	/// </summary>
	public interface IDialogFrameFactory
	{
		/// <summary>
		/// Creates a dialog frame
		/// </summary>
		IDialogFrame Create( IWorkspace workspace );
	}
}
