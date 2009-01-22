using Bob.Core.Commands;
using Poc1.Bob.Core.Interfaces.Commands;
using Rb.Interaction.Classes;

namespace Poc1.Bob.Core.Classes.Commands
{
	/// <summary>
	/// Project commands
	/// </summary>
	public class ProjectCommands : ICommandProvider
	{
		#region ICommandProvider Members

		/// <summary>
		/// Gets the list of command groups supported by this provider
		/// </summary>
		public Command[] Commands
		{
			get
			{
				return new Command[]
					{
						CloseProject,
						NewProject
					};
			}
		}

		/// <summary>
		/// Gets the NewProject command
		/// </summary>
		public static WorkspaceCommand NewProject
		{
			get { return s_NewProject; }
		}

		/// <summary>
		/// Gets the CloseProject command
		/// </summary>
		public static WorkspaceCommand CloseProject
		{
			get { return s_CloseProject; }
		}

		#endregion

		#region Private Members

		private readonly static WorkspaceCommand s_NewProject;
		private readonly static WorkspaceCommand s_CloseProject;

		static ProjectCommands( )
		{
			s_NewProject = WorkspaceCommand.NewCommand( DefaultCommands.FileCommands, "newProject", "&New Project", "Creates a new project" );
			s_CloseProject = WorkspaceCommand.NewCommand( DefaultCommands.FileCommands, "closeProject", "&Close Project", "Closes the current project" );
		}

		#endregion

	}
}
