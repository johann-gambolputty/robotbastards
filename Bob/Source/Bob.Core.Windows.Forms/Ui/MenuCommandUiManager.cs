using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bob.Core.Commands;
using Bob.Core.Ui.Interfaces;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Bob.Core.Windows.Forms
{
	/// <summary>
	/// Adds commands to menus
	/// </summary>
	public class MenuCommandUiManager : ICommandUiManager
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="menu">Target menu. Commands are added/removed from here</param>
		/// <param name="triggerDataFactory">Factory used to create trigger data</param>
		/// <exception cref="ArgumentNullException">Thrown if menu or triggerDataFactory are null</exception>
		public MenuCommandUiManager( MenuStrip menu, ICommandTriggerDataFactory triggerDataFactory )
		{
			Arguments.CheckNotNull( menu, "menu" );
			Arguments.CheckNotNull( triggerDataFactory, "triggerDataFactory" );
			m_Menu = menu;
			m_TriggerDataFactory = triggerDataFactory;
		}

		#region ICommandUiManager Members

		/// <summary>
		/// Adds a set of commands to the UI
		/// </summary>
		/// <param name="commands">Commands to add</param>
		public void AddCommands( IEnumerable<Command> commands )
		{
			foreach ( Command command in commands )
			{
				if ( command == null )
				{
					throw new ArgumentException( "Command list contained null command", "commands" );
				}
				AddCommand( GetParentCommandGroups( command ), command );
			}
		}

		/// <summary>
		/// Rmoves a set of commands to the UI
		/// </summary>
		/// <param name="commands">Commands to remove</param>
		public void RemoveCommands( IEnumerable<Command> commands )
		{
			foreach ( Command command in commands )
			{
				RemoveCommand( command );
			}
		}

		#endregion

		#region Private Members

		private readonly ICommandTriggerDataFactory m_TriggerDataFactory;
		private readonly MenuStrip m_Menu;
		private readonly Dictionary<Command, ToolStripItem> m_CommandMenuMap = new Dictionary<Command, ToolStripItem>( );

		/// <summary>
		/// Adds a command to the menu
		/// </summary>
		private void AddCommand( CommandGroup[] groups, Command command )
		{
			if ( m_CommandMenuMap.ContainsKey( command ) )
			{
				return;
			}
			ToolStripItemCollection menuItems = m_Menu.Items;
			for ( int groupIndex = 0; groupIndex < groups.Length; ++groupIndex )
			{
				ToolStripMenuItem subMenu = null;
				foreach ( ToolStripItem item in menuItems )
				{
					if ( item.Tag == groups[ groupIndex ] )
					{
						subMenu = ( ToolStripMenuItem )item;
						break;
					}
				}
				if ( subMenu == null )
				{


					subMenu = new ToolStripMenuItem( groups[ groupIndex ].NameUi );
					subMenu.Tag = groups[ groupIndex ];
					menuItems.Insert( GetCommandGroupInsertPosition( groups[ groupIndex ], menuItems ), subMenu );
				}
				menuItems = subMenu.DropDownItems;
			}
			ToolStripItem commandItem = new ToolStripMenuItem( command.NameUi );
			commandItem.Tag = command;
			commandItem.Click += OnCommandItemClicked;
			menuItems.Insert( 0, commandItem );
			m_CommandMenuMap.Add( command, commandItem );
		}

		/// <summary>
		/// Returns the position in a menu item set that a command group should be inserted at
		/// </summary>
		private static int GetCommandGroupInsertPosition( CommandGroup group, ToolStripItemCollection menuItems )
		{
			int ordinal = GetCommandGroupOrdinal( group );
			int index = 0;
			foreach ( ToolStripMenuItem menuItem in menuItems )
			{
				if ( !( menuItem.Tag is CommandGroup ) )
				{
					//	TODO: AP: Commands should have ordinals too
					return index; 
				}
				if ( GetCommandGroupOrdinal( ( CommandGroup )menuItem.Tag ) >= ordinal )
				{
					return index;
				}
				++index;
			}
			return index;
		}

		/// <summary>
		/// Returns the sortable value of a command group
		/// </summary>
		private static int GetCommandGroupOrdinal( CommandGroup group )
		{
			WorkspaceCommandGroup workspaceGroup = group as WorkspaceCommandGroup;
			return workspaceGroup == null ? WorkspaceCommandGroup.LastOrdinal : workspaceGroup.Ordinal;
		}

		/// <summary>
		/// Removes a command from the menu
		/// </summary>
		private void RemoveCommand( Command command )
		{
			ToolStripItem item;
			if ( !m_CommandMenuMap.TryGetValue( command, out item ) )
			{
				return;
			}
			item.Owner.Items.Remove( item );
			m_CommandMenuMap.Remove( command );
		}

		/// <summary>
		/// Gets the list of command groups containing a command, from the root group to the immediate parent group
		/// </summary>
		private static CommandGroup[] GetParentCommandGroups( Command command )
		{
			List<CommandGroup> groups = new List<CommandGroup>( );
			for ( CommandGroup group = command.Group; group != null; group = group.ParentCommandGroup )
			{
				groups.Add( group );
			}
			groups.Reverse( );
			return groups.ToArray( );
		}

		/// <summary>
		/// Handles click event on a command tool strip item
		/// </summary>
		private void OnCommandItemClicked( object sender, EventArgs args )
		{
			Command command = ( Command )( ( ToolStripItem )sender ).Tag;
			command.Trigger( m_TriggerDataFactory.Create( CommandUser.Default, command, null ) );
		}

		#endregion
	}
}
