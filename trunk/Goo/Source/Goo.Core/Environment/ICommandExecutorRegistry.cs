using Goo.Core.Commands;

namespace Goo.Core.Environment
{
	/// <summary>
	/// Keeps a registry of all available command executors
	/// </summary>
	public interface ICommandExecutorRegistry
	{
		/// <summary>
		/// Returns all the executors in this registry
		/// </summary>
		ICommandExecutor[] Executors
		{
			get;
		}

		/// <summary>
		/// Registers a command executor
		/// </summary>
		void RegisterExecutor( ICommandExecutor executor );

		/// <summary>
		/// Unregisters a command executor
		/// </summary>
		void UnregisterExecutor( ICommandExecutor executor );
	}
}
