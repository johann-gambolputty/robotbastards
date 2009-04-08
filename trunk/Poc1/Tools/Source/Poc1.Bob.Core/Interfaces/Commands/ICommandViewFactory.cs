using Bob.Core.Workspaces.Interfaces;

namespace Poc1.Bob.Core.Interfaces.Commands
{
	/// <summary>
	/// View factory interface
	/// </summary>
	public interface ICommandViewFactory
	{
		/// <summary>
		/// Shows the render targets view
		/// </summary>
		void ShowRenderTargetViews( IWorkspace workspace );
	}
}
