using Rb.Interaction.Classes;
using Rb.Interaction.Classes.Generic;

namespace Bob.Core.Commands
{
	/// <summary>
	/// A command that supports workspace data stored in command trigger
	/// </summary>
	public class WorkspaceCommand : Command<WorkspaceCommandTriggerData>
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public WorkspaceCommand( string uniqueName, string locName, string locDescription ) :
			base( uniqueName, locName, locDescription )
		{
		}

		/// <summary>
		/// Creates a new <see cref="WorkspaceCommand"/> and adds its to the specified command group
		/// </summary>
		public static WorkspaceCommand NewCommand( CommandGroup group, string name, string uiName, string uiDescription )
		{
			WorkspaceCommand command = new WorkspaceCommand( name, uiName, uiDescription );
			group.AddCommand( command );
			return command;
		}
	}
}
