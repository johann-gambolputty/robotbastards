using Bob.Core.Commands;
using Poc1.Bob.Core.Interfaces.Commands;
using Rb.Interaction.Classes;

namespace Poc1.Bob.Core.Classes.Commands
{
	/// <summary>
	/// Template commands
	/// </summary>
	public class TemplateCommands : ICommandProvider
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
						NewFromTemplate
					};
			}
		}

		/// <summary>
		/// Gets the NewFromTemplate command
		/// </summary>
		public static WorkspaceCommand NewFromTemplate
		{
			get { return s_NewFromTemplate; }
		}

		#endregion

		#region Private Members

		private readonly static WorkspaceCommand s_NewFromTemplate;

		static TemplateCommands( )
		{
			s_NewFromTemplate = WorkspaceCommand.NewCommand( DefaultCommands.FileCommands, "new", "&New From Template", "Creates an instance from a template" );
		}

		#endregion

	}
}
