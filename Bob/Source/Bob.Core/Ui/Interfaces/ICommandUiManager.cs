using System.Collections.Generic;
using Rb.Interaction.Classes;

namespace Bob.Core.Ui.Interfaces
{
	/// <summary>
	/// Manages UI for commands
	/// </summary>
	public interface ICommandUiManager
	{
		/// <summary>
		/// Adds a set of commands to the UI
		/// </summary>
		/// <param name="commands">Commands to add</param>
		void AddCommands( IEnumerable<Command> commands );

		/// <summary>
		/// Removes a set of commands to the UI
		/// </summary>
		/// <param name="commands">Commands to remove</param>
		void RemoveCommands( IEnumerable<Command> commands );
	}
}
