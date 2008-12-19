using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Bob.Core.Projects
{
	/// <summary>
	/// A command list of <see cref="ProjectCommand"/> commands
	/// </summary>
	public class ProjectCommandList : CommandList
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public ProjectCommandList( string commandListName, string commandListLocName, CommandRegistry registry ) : base( commandListName, commandListLocName, registry )
		{
		}

		/// <summary>
		/// Gets the project command list
		/// </summary>
		public IEnumerable<ProjectCommand> ProjectCommands
		{
			get { return EnumerableAdapter<ProjectCommand>.Cast( Commands ); }
		}

		/// <summary>
		/// Creates a new project command and adds it to the command list
		/// </summary>
		public ProjectCommand NewCommand( Icon icon, string commandName, string commandLocName, string commandLocDescription )
		{
			ProjectCommand cmd = new ProjectCommand( icon, commandName, commandLocName, commandLocDescription );
			AddCommand( cmd );
			return cmd;
		}

		#region Protected Members

		/// <summary>
		/// Creates a new project command
		/// </summary>
		protected override Command CreateCommand( string name, string locName, string locDescription )
		{
			return new ProjectCommand( null, name, locName, locDescription );
		}

		#endregion

	}
}
