using Goo.Core.Ui.Properties;
using Rb.Interaction.Classes;

namespace Goo.Core.Ui.Commands
{
	/// <summary>
	/// Default UI commands
	/// </summary>
	public static class DefaultCommands
	{

		/// <summary>
		/// Default file commands
		/// </summary>
		public static class File
		{
			/// <summary>
			/// Gets the file command group
			/// </summary>
			public static CommandGroup Commands
			{
				get { return s_Commands; }
			}

			/// <summary>
			/// Gets the file open command
			/// </summary>
			public static Command Open
			{
				get { return s_Open; }
			}

			/// <summary>
			/// Gets the file close command
			/// </summary>
			public static Command Close
			{
				get { return s_Close; }
			}

			/// <summary>
			/// Gets the file exit command
			/// </summary>
			public static Command Exit
			{
				get { return s_Exit; }
			}

			#region Private Members

			private readonly static CommandGroup s_Commands = new CommandGroup( "file", Resources.FileCommandGroupText );
			private readonly static Command s_Open = s_Commands.NewCommand( "open", Resources.FileOpenCommandText, Resources.FileOpenCommandDescription );
			private readonly static Command s_Close = s_Commands.NewCommand( "close", Resources.FileCloseCommandText, Resources.FileCloseCommandDescription );
			private readonly static Command s_Exit = s_Commands.NewCommand( "exit", Resources.FileExitCommandText, Resources.FileExitCommandDescription );

			#endregion
		}

		/// <summary>
		/// Default help commands
		/// </summary>
		public static class Help
		{
			/// <summary>
			/// Gets the help command group
			/// </summary>
			public static CommandGroup Commands
			{
				get { return s_Commands; }
			}

			/// <summary>
			/// Gets the about help command
			/// </summary>
			public static Command About
			{
				get { return s_About; }
			}

			#region Private Members

			private readonly static CommandGroup s_Commands = new CommandGroup( "help", Resources.HelpCommandGroupText );
			private readonly static Command s_About = s_Commands.NewCommand( "about", Resources.HelpAboutCommandText, Resources.HelpAboutCommandDescription );

			#endregion
		}
	}
}
