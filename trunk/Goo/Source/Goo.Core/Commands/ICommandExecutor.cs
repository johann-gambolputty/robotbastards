using Goo.Core.Environment;
using Rb.Interaction.Classes;

namespace Goo.Core.Commands
{
	/// <summary>
	/// Return by <see cref="ICommandExecutor.Execute"/>
	/// </summary>
	public enum CommandExecutionResult
	{
		/// <summary>
		/// Instructs the <see cref="ICommandHost"/> object to allow other <see cref="ICommandExecutor"/> objects to execute the command
		/// </summary>
		ContinueExecutingCommand,

		/// <summary>
		/// Instructs the <see cref="ICommandHost"/> object exit
		/// </summary>
		StopExecutingCommand
	}

	/// <summary>
	/// Executes commands
	/// </summary>
	public interface ICommandExecutor
	{
		/// <summary>
		/// Returns the visibility state of a command (whether it can be executed, and if it appears in the UI)
		/// </summary>
		void GetCommandAvailability( IEnvironment env, Command command, ref bool available, ref bool visible );

		/// <summary>
		/// Executes a command
		/// </summary>
		CommandExecutionResult Execute( IEnvironment env, Command command, CommandParameters parameters );

	}
}
