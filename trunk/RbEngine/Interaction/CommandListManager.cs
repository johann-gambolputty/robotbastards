using System;
using System.Collections;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Stores a dictionary of CommandList objects
	/// </summary>
	public class CommandListManager
	{
		/// <summary>
		/// Gets the singleton instance of CommandListManager
		/// </summary>
		public static CommandListManager	Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		/// <summary>
		/// Adds a command list
		/// </summary>
		public void			Add( CommandList commands )
		{
			m_CommandLists[ commands.Name ] = commands;
		}

		/// <summary>
		/// Gets a named command list
		/// </summary>
		public CommandList	Get( string name )
		{
			return m_CommandLists[ name ];
		}

		/// <summary>
		/// Stored command lists
		/// </summary>
		public Hashtable	CommandLists
		{
			get
			{
				return m_CommandLists;
			}
		}

		private Hashtable	m_CommandLists = new Hashtable( );

	}
}
