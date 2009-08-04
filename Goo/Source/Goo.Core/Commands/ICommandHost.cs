using Rb.Interaction.Classes;

namespace Goo.Core.Commands
{
	/// <summary>
	/// Command hosts are responsible for executing commands
	/// </summary>
	public interface ICommandHost
	{
		/// <summary>
		/// Executes a command
		/// </summary>
		/// <param name="command">Command to execute</param>
		/// <param name="parameters">Command parameters</param>
		void Execute( Command command, CommandParameters parameters );
	}
}
