using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command list
	/// </summary>
	public class CommandList
	{
		/// <summary>
		/// Setup constructor. Sets the registry used to keep track of commands added to this list
		/// </summary>
		public CommandList( string commandListName, string commandListLocName, CommandRegistry registry )
		{
			Arguments.CheckNotNullOrEmpty( commandListName, "commandListName" );
			Arguments.CheckNotNull( registry, "registry" );
			m_Name = commandListName;
			m_LocName = commandListLocName;
			m_Registry = registry;
		}

		/// <summary>
		/// Setup constructor. List is a child of a parent list
		/// </summary>
		/// <param name="parentList">Parent list. Can't be null</param>
		/// <param name="commandListName">Command list name</param>
		/// <param name="commandListLocName">Command list localised name</param>
		public CommandList( CommandList parentList, string commandListName, string commandListLocName )
		{
			Arguments.CheckNotNull( parentList, "parentList" );
			Arguments.CheckNotNullOrEmpty( commandListName, "commandListName" );
			m_Name = commandListName;
			m_LocName = commandListLocName;
			m_Registry = parentList.m_Registry;
			m_ParentList = parentList;
			m_ParentList.m_ChildLists.Add( this );
		}

		/// <summary>
		/// Gets the parent command list. Returns null if this is a root command list
		/// </summary>
		public CommandList ParentCommandList
		{
			get { return m_ParentList; }
		}

		/// <summary>
		/// Gets the child command lists
		/// </summary>
		public ReadOnlyCollection<CommandList> ChildCommandLists
		{
			get { return m_ChildLists.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the identifying name of this command list
		/// </summary>
		public string NameId
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the localised name of this command list
		/// </summary>
		public string NameUi
		{
			get { return m_LocName; }
		}

		/// <summary>
		/// Creates a new command and adds it to this list and the associated command registry
		/// </summary>
		/// <param name="commandName">Non-localised command identifier</param>
		/// <param name="commandLocName">Localised command name</param>
		/// <param name="commandLocDescription">Localised command description</param>
		/// <returns>Returns the created command</returns>
		public Command NewCommand( string commandName, string commandLocName, string commandLocDescription )
		{
			Command cmd = CreateCommand( m_Name + '.' + commandName, commandLocName, commandLocDescription );
			AddCommand( cmd );
			return cmd;
		}

		/// <summary>
		/// Gets the command list
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

		/// <summary>
		/// Adds a command to the list
		/// </summary>
		protected void AddCommand( Command cmd )
		{
			Arguments.CheckNotNull( cmd, "cmd" );
			m_Commands.Add( cmd );
			m_Registry.Register( cmd );
		}

		#endregion

		#region Private Members

		private readonly CommandList m_ParentList;
		private readonly string m_Name;
		private readonly string m_LocName;
		private readonly CommandRegistry m_Registry;
		private readonly List<Command> m_Commands = new List<Command>( );
		private readonly List<CommandList> m_ChildLists = new List<CommandList>( );

		#endregion
	}

}
