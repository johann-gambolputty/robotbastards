using System;
using System.Collections.Generic;

namespace Rb.Interaction
{
	/// <summary>
	/// Stores a dictionary of CommandList objects
	/// </summary>
	public class CommandListManager
	{
		/// <summary>
		/// Gets the singleton instance of CommandListManager
		/// </summary>
		public static CommandListManager Instance
		{
			get { return s_Singleton; }
		}

		/// <summary>
		/// Adds a command list
		/// </summary>
		public void Add( CommandList commands )
		{
			InteractionLog.Info( "Adding command list \"{0}\" to command list manager", commands.Name );
			m_CommandLists.Add( commands );
		}

		/// <summary>
		/// Gets a named command list
		/// </summary>
		public CommandList Get( string name )
		{
			foreach ( CommandList curList in m_CommandLists )
			{
				if ( curList.Name == name )
				{
					return curList;
				}
			}
			return null;
		}
		
		/// <summary>
		/// Returns a CommandList for a given enum type (must be created already)
		/// </summary>
		public CommandList Get( Type enumType )
		{
			return Get( enumType.FullName );
		}

		/// <summary>
		/// Returns a CommandList for a given enum type (must be created already)
		/// </summary>
		public CommandList Get< EnumType >( )
		{
			return Get( typeof( EnumType ) );
		}

		/// <summary>
		/// Finds a command list for a given command list enum. If there isn't one, a new list is created
		/// </summary>
		public CommandList FindOrCreateFromEnum( Type enumType )
		{
			CommandList result = Get( enumType );
			if ( result == null )
			{
				result = CommandList.FromEnum( enumType );
			}
			return result;
		}

		/// <summary>
		/// Stored command lists
		/// </summary>
		public IList< CommandList > CommandLists
		{
			get { return m_CommandLists; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		private CommandListManager( )
		{
		}

		private readonly List< CommandList >		m_CommandLists	= new List< CommandList >( );
		private static readonly CommandListManager	s_Singleton	= new CommandListManager( );
	}
}
