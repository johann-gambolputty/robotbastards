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
		/// Adds a group of commands to the UI
		/// </summary>
		/// <param name="group">Group of commands to add</param>
		void AddCommands( CommandGroup group );

		/// <summary>
		/// Adds a set of commands to the UI
		/// </summary>
		/// <param name="commands">Commands to add</param>
		void AddCommands( IEnumerable<Command> commands );

		/// <summary>
		/// Removes a group of commands from the UI
		/// </summary>
		/// <param name="group">Group of commands to remove</param>
		void RemoveCommands( CommandGroup group );

		/// <summary>
		/// Removes a set of commands to the UI
		/// </summary>
		/// <param name="commands">Commands to remove</param>
		void RemoveCommands( IEnumerable<Command> commands );
	}
}
