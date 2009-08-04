
namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// A group of menu items and sub-groups
	/// </summary>
	public class MenuGroupInfo : MenuNodeInfo
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="text">Group text. Use an amperand to designate the group's hot-key</param>
		/// <param name="ordinal">Ordinal value for this group</param>
		public MenuGroupInfo( string text, int ordinal ) :
			this( text.Replace( "&", "" ), text, ordinal )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Node name. Used for finding nodes</param>
		/// <param name="text">Group text. Use an amperand to designate the group's hot-key</param>
		/// <param name="ordinal">Ordinal value for this group</param>
		public MenuGroupInfo( string name, string text, int ordinal ) :
			base( name, text, ordinal )
		{
		}
	}
}
