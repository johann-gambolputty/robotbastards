using System;
using System.Collections;
using System.Reflection;

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
		public void					Add( CommandList commands )
		{
			Output.WriteLineCall( Output.InputInfo, "Adding command list \"{0}\" to command list manager", commands.Name );
			m_CommandLists.Add( commands );
		}

		/// <summary>
		/// Gets a named command list
		/// </summary>
		public CommandList			Get( string name )
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
		/// Gets the command list from the hash of its name
		/// </summary>
		public CommandList			GetFromStringHash( int stringHash )
		{
			foreach ( CommandList curList in m_CommandLists )
			{
				if ( curList.Name.GetHashCode( ) == stringHash )
				{
					return curList;
				}
			}
			return null;
		}

		/// <summary>
		/// Creates a CommandList from an enumerated type
		/// </summary>
		public static CommandList	CreateFromEnum( Type enumType )
		{
			Output.WriteLineCall( Output.InputInfo, "Creating command list from enum \"{0}\"", enumType.Name );

			CommandList commands		= new CommandList( enumType.Name );
			string[]	commandNames	= Enum.GetNames( enumType );
			int[]		commandValues	= ( int[] )Enum.GetValues( enumType );

			for ( int commandIndex = 0; commandIndex < commandNames.Length; ++commandIndex )
			{
				string name			= commandNames[ commandIndex ];
				string description	= string.Empty;

				MemberInfo[]	info	= enumType.GetMember( name );
				object[]		attribs	= info[ 0 ].GetCustomAttributes( typeof( CommandEnumDescriptionAttribute ), false );
				if ( attribs.Length > 0 )
				{
					description = ( ( CommandEnumDescriptionAttribute )attribs[ 0 ] ).Description;
				}

				Output.WriteLineCall( Output.InputInfo, "Adding enum command \"{0}\"", name );
				commands.AddCommand( new Command( name, description, ( ushort )commandValues[ commandIndex ] ) );
			}

			return commands;
		}

		/// <summary>
		/// Stored command lists
		/// </summary>
		public ArrayList			CommandLists
		{
			get
			{
				return m_CommandLists;
			}
		}

		private CommandListManager( )
		{
			m_UpdateClock.Subscribe( new Scene.Clock.TickDelegate( UpdateLists ) );
		}

		private void UpdateLists( Scene.Clock clock )
		{
			foreach ( CommandList curList in m_CommandLists )
			{
				curList.Update( );
			}
		}

		private Scene.Clock					m_UpdateClock	= new Scene.Clock( "InputUpdateClock", 1 );
		private ArrayList					m_CommandLists	= new ArrayList( );
		private static CommandListManager	ms_Singleton	= new CommandListManager( );

	}
}
