
using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Menu item
	/// </summary>
	public class MenuItemInfo : MenuNodeInfo
	{
		/// <summary>
		/// Setup constructor. Menu text is the command name, prefixed with '&'
		/// </summary>
		/// <param name="command">Command</param>
		/// <param name="ordinal">Ordinal value for this item</param>
		public MenuItemInfo( Command command, int ordinal ) :
			this( command, command.NameUi, ordinal )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="command">Command</param>
		/// <param name="text">Menu item text. Use an amperand to designate the hot-key.</param>
		/// <param name="ordinal">Ordinal value for this item</param>
		public MenuItemInfo( Command command, string text, int ordinal ) :
			this( text.Replace( "&", "" ), command, text, ordinal )
		{
		}
		
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Menu item name</param>
		/// <param name="command">Command triggered by the menu item.</param>
		/// <param name="text">Menu item text. Use an amperand to designate the hot-key.</param>
		/// <param name="ordinal">Ordinal value for this item</param>
		public MenuItemInfo( string name, Command command, string text, int ordinal ) :
			base( name, text, ordinal )
		{
			Arguments.CheckNotNull( command, "command" );
			m_Command = command;
		}

		/// <summary>
		/// Gets the command associated with this menu item
		/// </summary>
		public Command Command
		{
			get { return m_Command; }
		}

		#region Private Members

		private readonly Command m_Command;

		#endregion
	}
}
