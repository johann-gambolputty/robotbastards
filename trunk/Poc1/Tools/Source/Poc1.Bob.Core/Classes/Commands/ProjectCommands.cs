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

		#endregion

		#region Private Members

		private readonly static WorkspaceCommand s_NewProject;

		static ProjectCommands( )
		{
			s_NewProject = WorkspaceCommand.NewCommand( DefaultCommands.FileCommands, "newProject", "&New Project", "Creates a new project" );
		}

		#endregion

	}
}
