using Bob.Core.Workspaces.Interfaces;

namespace Bob.Core.Ui.Interfaces.Views
{
	/// <summary>
	/// View manager interface
	/// </summary>
	public interface IViewManager
	{
		/// <summary>
		/// Starts a layout
		/// </summary>
		/// <param name="workspace">Current workspace</param>
		/// <param name="name">Layout name</param>
		/// <param name="views">All views that can be shown in this layout</param>
		void BeginLayout( IWorkspace workspace, string name, IViewInfo[] views );

		/// <summary>
		/// Creates a view from a view object
		/// </summary>
		/// <param name="workspace">Workspace context</param>
		/// <param name="view">View information</param>
		void Show( IWorkspace workspace, IViewInfo view );

		/// <summary>
		/// Saves the current layout
		/// </summary>
		void SaveLayout( );

		/// <summary>
		/// Closes the current layout
		/// </summary>
		void EndLayout( IWorkspace workspace );
	}
}
