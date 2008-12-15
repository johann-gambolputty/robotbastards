using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Context for deserializing command trigger data
	/// </summary>
	public class CommandSerializationContext
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="userRegistry">Registry containing all users</param>
		/// <param name="commandRegistry">Registry containing all commands</param>
		public CommandSerializationContext( CommandUserRegistry userRegistry, CommandRegistry commandRegistry )
		{
			m_Users = userRegistry;
			m_Commands = commandRegistry;
		}

		/// <summary>
		/// Gets the user registry
		/// </summary>
		public CommandUserRegistry Users
		{
			get { return m_Users; }
		}

		/// <summary>
		/// Getrs the command registry
		/// </summary>
		public CommandRegistry Commands
		{
			get { return m_Commands; }
		}

		/// <summary>
		/// Creates a StreamingContext with a CommandSerializationContext as its user context
		/// </summary>
		public static StreamingContext ToStreamingContext( CommandUserRegistry userRegistry, CommandRegistry commandRegistry )
		{
			return ToStreamingContext( StreamingContextStates.Clone, userRegistry, commandRegistry );
		}

		/// <summary>
		/// Creates a StreamingContext with a CommandSerializationContext as its user context
		/// </summary>
		public static StreamingContext ToStreamingContext( StreamingContextStates state, CommandUserRegistry userRegistry, CommandRegistry commandRegistry )
		{
			CommandSerializationContext userContext = new CommandSerializationContext( userRegistry, commandRegistry );
			StreamingContext context = new StreamingContext( state, userContext );
			return context;
		}

		#region Private Members

		private readonly CommandUserRegistry m_Users;
		private readonly CommandRegistry m_Commands;

		#endregion
	}
}
