using Bob.Core.Workspaces.Interfaces;

namespace Bob.Core.Ui.Interfaces
{
	/// <summary>
	/// View manager interface
	/// </summary>
	public interface IViewManager
	{
		/// <summary>
		/// Creates a view from a view object
		/// </summary>
		/// <param name="workspace">Workspace context</param>
		/// <param name="view">View information</param>
		void Create( IWorkspace workspace, IViewInfo view );
	}
}
