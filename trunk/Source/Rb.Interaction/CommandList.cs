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
			CommandListManager.Inst.Add( this );
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
		/// Builds a command list from an enum type
		/// </summary>
		/// <param name="enumType">enum's Type</param>
		public static CommandList BuildFromEnum( Type enumType )
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

				Command newCommand = new Command( name, displayName, description, checked( ( byte )commandValues[ commandIndex ] ) );

				commands.Add( newCommand );
			}

			return commands;
		}

		#endregion

		#region Events

		/// <summary>
		/// Called when a command in this list is active
		/// </summary>
		public CommandEventDelegate CommandActive;

		/// <summary>
		/// Called when a command in this list becomes active
		/// </summary>
		public CommandEventDelegate CommandActivated;

		/// <summary>
		/// Fires the CommandActive event
		/// </summary>
		/// <param name="input">Input that kept the command active</param>
		/// <param name="message">Command message</param>
		public void OnCommandActive( IInput input, CommandMessage message )
		{
			if ( CommandActive != null )
			{
				CommandActive( input, message );
			}
		}

		/// <summary>
		/// Fires the CommandActivated event
		/// </summary>
		/// <param name="input">Input that activated command</param>
		/// <param name="message">Command message</param>
		public void OnCommandActivated( IInput input, CommandMessage message )
		{
			if ( CommandActivated != null )
			{
				CommandActivated( input, message );
			}
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

		#region Updates

		/// <summary>
        /// Updates all the commands in the list (calling <see cref="Command.Update"/>)
        /// </summary>
        public void Update( )
        {
            foreach ( Command curCommand in this )
            {
                curCommand.Update( this );
            }
		}

		#endregion


		#region Private stuff

		private string m_Name;

		#endregion
	}
}
