using System.Collections.Generic;
using System.Collections.ObjectModel;

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
			m_Name = commandListName;
			m_LocName = commandListLocName;
			m_Registry = registry;
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
			Command cmd = new Command( m_Name + '.' + commandName, commandLocName, commandLocDescription );
			m_Commands.Add( cmd );
			m_Registry.Register( cmd );
			return cmd;
		}

		/// <summary>
		/// Gets the command list
		/// </summary>
		public ReadOnlyCollection<Command> Commands
		{
			get { return m_Commands.AsReadOnly( ); }
		}

		#region Private Members

		private readonly string m_Name;
		private readonly string m_LocName;
		private readonly CommandRegistry m_Registry;
		private readonly List<Command> m_Commands = new List<Command>( );

		#endregion
	}

}
