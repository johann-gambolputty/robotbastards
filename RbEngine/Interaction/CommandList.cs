using System;
using System.Collections;

namespace RbEngine.Interaction
{
	/// <summary>
	/// A list of commands
	/// </summary>
	public class CommandList : Components.Node, Components.INamedObject
	{
		/// <summary>
		/// Access the command list (synonym for Components.Node.Children)
		/// </summary>
		public ArrayList	Commands
		{
			get
			{
				return Children;
			}
		}

		/// <summary>
		/// Binds all commands to the specified scene view
		/// </summary>
		/// <param name="client">View to bind to</param>
		public void	BindToView( Scene.SceneView view )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.BindToView( view );
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
		/// Updates the list of commands
		/// </summary>
		public void	Update( )
		{
			foreach ( Command curCommand in Commands )
			{
				curCommand.Update( );
			}
		}

		#region INamedObject Members

		/// <summary>
		/// Access to the name of this object
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		/// <summary>
		/// Event, invoked when the name of this object changes
		/// </summary>
		public event Components.NameChangedDelegate NameChanged;

		#endregion

		private string	m_Name;
	}
}
