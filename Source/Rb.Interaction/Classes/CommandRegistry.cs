using System;
using System.Collections.Generic;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Maintains a registry of commands
	/// </summary>
	public class CommandRegistry
	{
		/// <summary>
		/// Standard singleton instance
		/// </summary>
		public static CommandRegistry Instance
		{
			get { return s_Instance; }
		}

		/// <summary>
		/// Registers a command
		/// </summary>
		/// <param name="cmd">Command to register</param>
		public void Register( Command cmd )
		{
			InteractionLog.Info( "Registering command \"{0}\" ({1})", cmd.NameId, cmd.Id );
			Arguments.CheckNotNull( cmd, "cmd" );
			CheckCommandIdDoesNotExist( cmd );

			m_Commands[ cmd.Id ] = cmd;
		}

		/// <summary>
		/// Finds a command by its unique identifier
		/// </summary>
		public Command FindById( int id )
		{
			return m_Commands[ id ];
		}

		#region Private Members

		private static readonly CommandRegistry s_Instance = new CommandRegistry( );
		private readonly Dictionary<int, Command> m_Commands = new Dictionary<int, Command>( );

		/// <summary>
		/// Checks if a command already exists
		/// </summary>
		private void CheckCommandIdDoesNotExist( Command cmd )
		{
			Command existingCmd;
			if ( m_Commands.TryGetValue( cmd.Id, out existingCmd ) )
			{
				throw new ArgumentException( string.Format( "Command \"{0}\" has the same ID as command \"{1}\" - change name or description in either", cmd.NameId, existingCmd.NameId ), "cmd" );
			}
		}

		#endregion
	}
}
