using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command group
	/// </summary>
	public class CommandGroup
	{
		/// <summary>
		/// Setup constructor. Sets the registry used to keep track of commands added to this group
		/// </summary>
		public CommandGroup( string commandGroupName, string commandGroupLocName, CommandRegistry registry )
		{
			Arguments.CheckNotNullOrEmpty( commandGroupName, "commandGroupName" );
			Arguments.CheckNotNull( registry, "registry" );
			m_Name = commandGroupName;
			m_LocName = commandGroupLocName;
			m_Registry = registry;
		}

		/// <summary>
		/// Setup constructor. Group is a child of a parent group
		/// </summary>
		/// <param name="parentGroup">Parent group. Can't be null</param>
		/// <param name="commandGroupName">Command group name</param>
		/// <param name="commandGroupLocName">Command group localised name</param>
		public CommandGroup( CommandGroup parentGroup, string commandGroupName, string commandGroupLocName )
		{
			Arguments.CheckNotNull( parentGroup, "parentGroup" );
			Arguments.CheckNotNullOrEmpty( commandGroupName, "commandGroupName" );
			m_Name = commandGroupName;
			m_LocName = commandGroupLocName;
			m_Registry = parentGroup.m_Registry;
			m_ParentGroup = parentGroup;
			m_ParentGroup.m_SubGroups.Add( this );
		}

		/// <summary>
		/// Gets the parent command group. Returns null if this is a root command group
		/// </summary>
		public CommandGroup ParentCommandGroup
		{
			get { return m_ParentGroup; }
		}

		/// <summary>
		/// Gets the command sub-groups
		/// </summary>
		public ReadOnlyCollection<CommandGroup> SubGroups
		{
			get { return m_SubGroups.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the identifying name of this command group
		/// </summary>
		public string NameId
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the localised name of this command group
		/// </summary>
		public string NameUi
		{
			get { return m_LocName; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public Command AddCommand( Command command )
		{
			Arguments.CheckNotNull( command, "command" );
			if ( m_Commands.Contains( command ) )
			{
				throw new ArgumentException( string.Format( "Command \"{0}\" already exists in group \"{1}\"", command.NameId, NameId ) );
			}

			m_Commands.Add( command );
			command.Group = this;

			m_Registry.Register( command );

			return command;
		}

		/// <summary>
		/// Creates a new command and adds it to this group and the associated command registry
		/// </summary>
		/// <param name="commandName">Non-localised command identifier</param>
		/// <param name="commandLocName">Localised command name</param>
		/// <param name="commandLocDescription">Localised command description</param>
		/// <returns>Returns the created command</returns>
		public Command NewCommand( string commandName, string commandLocName, string commandLocDescription )
		{
			Command cmd = CreateCommand( commandName, commandLocName, commandLocDescription );
			return AddCommand( cmd );
		}

		/// <summary>
		/// Gets the list of commands in this group
		/// </summary>
		public ReadOnlyCollection<Command> Commands
		{
			get { return m_Commands.AsReadOnly( ); }
		}

		#region Protected Members

		/// <summary>
		/// Creates a new command
		/// </summary>
		protected virtual Command CreateCommand( string name, string locName, string locDescription )
		{
			return new Command( name, locName, locDescription );
		}

		#endregion

		#region Private Members

		private readonly CommandGroup m_ParentGroup;
		private readonly string m_Name;
		private readonly string m_LocName;
		private readonly CommandRegistry m_Registry;
		private readonly List<Command> m_Commands = new List<Command>( );
		private readonly List<CommandGroup> m_SubGroups = new List<CommandGroup>( );

		#endregion
	}

}
