using Bob.Core.Ui.Interfaces;
using Rb.Core.Sets.Interfaces;

namespace Bob.Core.Workspaces.Interfaces
{
	/// <summary>
	/// Workspace interface
	/// </summary>
	public interface IWorkspace
	{
		/// <summary>
		/// Gets the main display
		/// </summary>
		IMainApplicationDisplay MainDisplay
		{
			get;
		}

		/// <summary>
		/// Gets workspace services
		/// </summary>
		IObjectSetServiceMap Services
		{
			get;
		}
	}
}
