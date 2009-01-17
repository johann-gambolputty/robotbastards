using Bob.Core.Commands;
using Rb.Interaction.Classes;

namespace Poc1.Bob.Core.Classes.Commands
{
	/// <summary>
	/// Default commands shared by all applications
	/// </summary>
	public class DefaultCommands
	{
		/// <summary>
		/// Gets the group of file commands
		/// </summary>
		public static CommandGroup FileCommands
		{
			get { return s_FileCommands; }
		}

		/// <summary>
		/// Gets the group of view commands
		/// </summary>
		public static CommandGroup ViewCommands
		{
			get { return s_ViewCommands; }
		}

		/// <summary>
		/// Gets the group of help commands
		/// </summary>
		public static CommandGroup HelpCommands
		{
			get { return s_HelpCommands; }
		}

		#region Commands

		/// <summary>
		/// Gets the file-exit command
		/// </summary>
		public static WorkspaceCommand FileExit
		{
			get { return s_FileExit; }
		}

		/// <summary>
		/// Gets the view-output command
		/// </summary>
		public static WorkspaceCommand ViewOutput
		{
			get { return s_ViewOutput; }
		}

		/// <summary>
		/// Gets the help-about command
		/// </summary>
		public static WorkspaceCommand HelpAbout
		{
			get { return s_HelpAbout; }
		}

		#endregion

		#region Private Members

		private readonly static CommandGroup s_FileCommands;
		private readonly static CommandGroup s_ViewCommands;
		private readonly static CommandGroup s_HelpCommands;

		private readonly static WorkspaceCommand s_FileExit;
		private readonly static WorkspaceCommand s_ViewOutput;
		private readonly static WorkspaceCommand s_HelpAbout;

		static DefaultCommands( )
		{
			s_FileCommands = new WorkspaceCommandGroup( 0, "file", "&File", CommandRegistry.Instance );
			s_ViewCommands = new WorkspaceCommandGroup( "view", "&View", CommandRegistry.Instance );
			s_HelpCommands = new WorkspaceCommandGroup( "help", "&Help", CommandRegistry.Instance );

			s_FileExit = WorkspaceCommand.NewCommand( s_FileCommands, "exit", "E&xit", "Exits this application" );
			s_ViewOutput = WorkspaceCommand.NewCommand( s_ViewCommands, "output", "&Output", "Output view" );
			s_HelpAbout = WorkspaceCommand.NewCommand( s_HelpCommands, "about", "&About", "Information about this application" );
		}

		#endregion
	}
}
