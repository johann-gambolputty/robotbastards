
using System.Drawing;

namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Menu group interface
	/// </summary>
	public interface IMenuGroup : IMenuNode
	{
		/// <summary>
		/// Gets all menu nodes (items, sub-groups and separators) making up this group
		/// </summary>
		IMenuNode[] Nodes
		{
			get;
		}

		/// <summary>
		/// Fluent interface for setting images
		/// </summary>
		IMenuGroup SetImage( Image image );

		/// <summary>
		/// Fluent interface for setting text
		/// </summary>
		IMenuGroup SetText( string text );

		/// <summary>
		/// Gets/sets this group's image
		/// </summary>
		Image Image
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets this group's text
		/// </summary>
		string Text
		{
			get; set;
		}

		/// <summary>
		/// Finds or adds a new menu item matching the specified information
		/// </summary>
		IMenuItem AddItem( MenuItemInfo itemInfo );

		/// <summary>
		/// Finds or adds a new menu group matching the specified information
		/// </summary>
		IMenuGroup AddGroup( MenuGroupInfo groupInfo );

		/// <summary>
		/// Finds or adds a new menu separator matching the specified information
		/// </summary>
		IMenuSeparator AddSeparator( MenuSeparatorInfo separatorInfo );

		/// <summary>
		/// Finds or adds a new menu item matching the specified information
		/// </summary>
		IMenuItem this[ MenuItemInfo itemInfo ]
		{
			get;
		}

		/// <summary>
		/// Finds or adds a new menu group matching the specified information
		/// </summary>
		IMenuGroup this[ MenuGroupInfo groupInfo ]
		{
			get;
		}

		/// <summary>
		/// Finds or adds a new menu separator matching the specified information
		/// </summary>
		IMenuSeparator this[ MenuSeparatorInfo separatorInfo ]
		{
			get;
		}
	}
}
