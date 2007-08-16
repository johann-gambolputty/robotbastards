using System;
using System.Reflection;
using System.Collections.Generic;

namespace Rb.Interaction
{
    /// <summary>
    /// List of commands
    /// </summary>
    public class CommandList : List< Command >
	{
		#region Construction

		/// <summary>
		/// Sets the name of this command list, and registers in the CommandListManager
		/// </summary>
		/// <param name="name">Command list name</param>
		public CommandList( string name )
		{
			m_Name = name;
			CommandListManager.Instance.Add( this );
		}

		/// <summary>
		/// Finds a command in the list by its name
		/// </summary>
		/// <param name="name">Command name</param>
		/// <returns>Returns the named command, or null if no command could be find</returns>
		public Command FindByName( string name )
		{
			foreach ( Command cmd in this )
			{
				if ( cmd.Name == name )
				{
					return cmd;
				}
			}
			return null;
		}

		/// <summary>
		/// Builds or retrieves a command list from a given enum type
		/// </summary>
		public static CommandList FromEnum( Type enumType )
		{
			CommandList commands = CommandListManager.Instance.Get( enumType.Name );
			if ( commands == null )
			{
				commands = BuildFromEnum( enumType );
			}
			return commands;
		}

		/// <summary>
		/// Builds a command list from an enum type
		/// </summary>
		/// <param name="enumType">enum's Type</param>
		private static CommandList BuildFromEnum( Type enumType )
		{
			InteractionLog.Info( "Creating command list from enum \"{0}\"", enumType.Name );

			CommandList commands = new CommandList( enumType.Name );

			string[] commandNames = Enum.GetNames( enumType );
			int[] commandValues = ( int[] )Enum.GetValues( enumType );

			for ( int commandIndex = 0; commandIndex < commandNames.Length; ++commandIndex )
			{
				string name = commandNames[ commandIndex ];
				string displayName = string.Empty;
				string description = string.Empty;

				MemberInfo[] info = enumType.GetMember( name );
				object[] attribs = info[ 0 ].GetCustomAttributes( typeof( CommandDescriptionAttribute ), false );
				if ( attribs.Length > 0 )
				{
					displayName = ( ( CommandDescriptionAttribute )attribs[ 0 ] ).Name;
					description = ( ( CommandDescriptionAttribute )attribs[ 0 ] ).Description;
				}

				InteractionLog.Verbose( "Adding enum command \"{0}\"", name );

				Command newCommand = new Command( commands, name, displayName, description, checked( ( byte )commandValues[ commandIndex ] ) );

				commands.Add( newCommand );
			}

			return commands;
		}

		#endregion

		#region Public properties

		/// <summary>
        /// The name of this command list
        /// </summary>
        public string Name
        {
            get { return m_Name; }
		}

		#endregion

		#region Private stuff

		private readonly string m_Name;

		#endregion
	}
}
