using System;
using System.Collections;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A list of commands
	/// </summary>
	public class CommandList : Components.IParentObject
	{

		/// <summary>
		/// Binds all commands to the specified client
		/// </summary>
		/// <param name="client">Client to bind to</param>
		public void	BindToClient( Network.Client client )
		{
			foreach ( Command curCommand in m_Commands )
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
			m_Commands.Add( cmd );
		}

		/// <summary>
		/// Updates the list of commands. Active commands send their command messages to the specified target
		/// </summary>
		/// <param name="commandTarget">Target to send command messages to</param>
		public void	Update( Components.IMessageHandler commandTarget )
		{
			foreach ( Command curCommand in m_Commands )
			{
				curCommand.Update( commandTarget );
			}
		}


		#region IParentObject Members

		/// <summary>
		/// Adds the child to this command list
		/// </summary>
		/// <param name="childObject">Child object. Must be of type Command</param>
		public void AddChild( Object childObject )
		{
			AddCommand( ( Command )childObject );
		}

		/// <summary>
		/// Visits all commands in this command list
		/// </summary>
		/// <param name="visitor">Visitor delegate</param>
		public void VisitChildren( RbEngine.Components.ChildVisitorDelegate visitor )
		{
			foreach ( Command curCommand in m_Commands )
			{
				if ( !visitor( curCommand ) )
				{
					return;
				}
			}
		}

		#endregion


		private ArrayList m_Commands = new ArrayList( );
	}
}
