
using Goo.Core.Environment;

namespace Goo.Core.Workspaces
{
	/// <summary>
	/// Workspace interface
	/// </summary>
	public interface IWorkspace : IServiceProvider
	{
		/// <summary>
		/// Gets the workspace environment (application-level context)
		/// </summary>
		IEnvironment Environment
		{
			get;
		}
	}
}
