using System;
using System.Collections;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A list of commands
	/// </summary>
	public class CommandList : Components.Node
	{

		/// <summary>
		/// Setup
		/// </summary>
		public CommandList( )
		{
			m_Children = new ArrayList( );
		}

		/// <summary>
		/// Access the command list (synonym for Components.Node.Children)
		/// </summary>
		public ArrayList	Commands
		{
			get
			{
				return m_Children;
			}
		}

		/// <summary>
		/// Binds all commands to the specified client
		/// </summary>
		/// <param name="client">Client to bind to</param>
		public void	BindToClient( Network.Client client )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.BindToClient( client );
			}
		}

		/// <summary>
		/// Adds a command
		/// </summary>
		/// <param name="cmd">Command to add</param>
		public void AddCommand( Command cmd )
		{
			Commands.Add( cmd );
		}

		/// <summary>
		/// Updates the list of commands. Active commands send their command messages to the specified target
		/// </summary>
		/// <param name="commandTarget">Target to send command messages to</param>
		public void	Update( Components.IMessageHandler commandTarget )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.Update( commandTarget );
			}
		}
	}
}
