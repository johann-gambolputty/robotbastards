using Goo.Core.Workspaces;

namespace Goo.Core.Services.Workspaces
{
	/// <summary>
	/// Service that maintains a single active workspace
	/// </summary>
	public interface IActiveWorkspaceService
	{
		/// <summary>
		/// Gets/sets the currently active workspace
		/// </summary>
		/// <remarks>
		/// Setting the current environment to a new value will trigger an <see cref="ActiveWorkspaceChangedEvent"/>
		/// event in the event service.
		/// </remarks>
		IWorkspace CurrentWorkspace
		{
			get; set;
		}
	}
}
