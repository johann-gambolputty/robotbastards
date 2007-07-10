using System.Collections.Generic;
using Rb.Core.Utils;

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
		public static CommandListManager Inst
		{
			get { return ms_Singleton; }
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
		public CommandList Get< EnumType >( )
		{
			return Get( typeof( EnumType ).Name );
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

		private List< CommandList >			m_CommandLists	= new List< CommandList >( );
		private static CommandListManager	ms_Singleton	= new CommandListManager( );
	}
}
