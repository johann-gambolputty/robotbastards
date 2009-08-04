using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Goo.Core.Commands;
using Goo.Core.Ui.Commands.Menus;
using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Goo.Core.Ui.WinForms.Commands.Menus
{
	/// <summary>
	/// Menu group implementation
	/// </summary>
	public class MenuGroup : IMenuGroup
	{

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="commandHost">Command host. Used to execute commands from items in this group and sub groups</param>
		/// <param name="item">Tool strip item that this group is bound to</param>
		/// <param name="groupInfo">Group information</param>
		public MenuGroup( ICommandHost commandHost, ToolStripMenuItem item, MenuGroupInfo groupInfo )
		{
			Arguments.CheckNotNull( commandHost, "commandHost" );
			Arguments.CheckNotNull( item, "item" );
			Arguments.CheckNotNull( groupInfo, "groupInfo" );

			m_CommandHost = commandHost;
			m_Ordinal = groupInfo.Ordinal;
			m_Item = item;
			item.Tag = this;
			item.Text = groupInfo.Text;
			item.Name = groupInfo.Name;
		}

		#region IMenuGroup Members

		/// <summary>
		/// Gets all menu nodes (items, sub-groups and separators) making up this group
		/// </summary>
		public IMenuNode[] Nodes
		{
			get { return m_Nodes.ToArray( ); }
		}

		/// <summary>
		/// Fluent interface for setting icons
		/// </summary>
		public IMenuGroup SetImage( Image image )
		{
			m_Item.Image = image;
			return this;
		}

		/// <summary>
		/// Fluent interface for setting text
		/// </summary>
		public IMenuGroup SetText( string text )
		{
			m_Item.Text = text;
			return this;
		}

		/// <summary>
		/// Gets/sets this group's image
		/// </summary>
		public Image Image
		{
			get { return m_Item.Image; }
			set { m_Item.Image = value; }
		}

		/// <summary>
		/// Gets/sets this group's text
		/// </summary>
		public string Text
		{
			get { return m_Item.Text; }
			set { m_Item.Text = value; }
		}

		#region IMenuNode Members

		/// <summary>
		/// Sets the name of this node
		/// </summary>
		public string Name
		{
			get { return m_Item.Name; }
		}

		/// <summary>
		/// Gets the ordinal value of this node
		/// </summary>
		public int Ordinal
		{
			get { return m_Ordinal; }
		}

		#endregion

		/// <summary>
		/// Finds or adds a new menu item matching the specified information
		/// </summary>
		public IMenuItem AddItem( MenuItemInfo itemInfo )
		{
			ToolStripItem item = m_Item.DropDownItems[ itemInfo.Name ];
			if ( item != null )
			{
				return ( IMenuItem )item.Tag;
			}
			ToolStripMenuItem menuItem = new ToolStripMenuItem( );
			InsertMenuItem( itemInfo.Ordinal, menuItem );
			if ( itemInfo.Command != null )
			{
				menuItem.Click += OnMenuItemClicked;
			}
			return new MenuItem( menuItem, itemInfo );
		}

		/// <summary>
		/// Finds or adds a new menu group matching the specified information
		/// </summary>
		public IMenuGroup AddGroup( MenuGroupInfo groupInfo )
		{
			ToolStripItem item = m_Item.DropDownItems[ groupInfo.Name ];
			if ( item != null )
			{
				return ( IMenuGroup )item.Tag;
			}

			ToolStripMenuItem groupItem = new ToolStripMenuItem( );
			InsertMenuItem( groupInfo.Ordinal, groupItem );
			return new MenuGroup( m_CommandHost, groupItem, groupInfo );
		}

		/// <summary>
		/// Finds or adds a new menu separator matching the specified information
		/// </summary>
		public IMenuSeparator AddSeparator( MenuSeparatorInfo separatorInfo )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Finds or adds a new menu item matching the specified information
		/// </summary>
		public IMenuItem this[ MenuItemInfo itemInfo ]
		{
			get { return AddItem( itemInfo ); }
		}

		/// <summary>
		/// Finds or adds a new menu group matching the specified information
		/// </summary>
		public IMenuGroup this[ MenuGroupInfo groupInfo ]
		{
			get { return AddGroup( groupInfo ); }
		}

		/// <summary>
		/// Finds or adds a new menu separator matching the specified information
		/// </summary>
		public IMenuSeparator this[ MenuSeparatorInfo separatorInfo ]
		{
			get { return AddSeparator( separatorInfo ); }
		}

		#endregion

		#region Private Members

		private readonly ICommandHost m_CommandHost;
		private readonly int m_Ordinal;
		private readonly ToolStripMenuItem m_Item;
		private readonly List<IMenuNode> m_Nodes = new List<IMenuNode>( );

		/// <summary>
		/// Inserts a menu item into this group
		/// </summary>
		private void InsertMenuItem( int ordinal, ToolStripItem item )
		{
			for ( int index = 0; index < m_Item.DropDownItems.Count; ++index )
			{
				IMenuNode node = m_Item.DropDownItems[ index ].Tag as IMenuNode;
				if ( node == null )
				{
					continue;
				}
				if ( ordinal <= node.Ordinal )
				{
					m_Item.DropDownItems.Insert( index, item );
					return;
				}
			}
			m_Item.DropDownItems.Add( item );
		}

		/// <summary>
		/// Handle menu item clicks. Farms off commands to the execution context
		/// </summary>
		private void OnMenuItemClicked( object sender, EventArgs args )
		{
			IMenuItem item = ( ( ToolStripItem )sender ).Tag as IMenuItem;
			if ( ( item == null ) || ( item.Command == null ) )
			{
				return;
			}
			m_CommandHost.Execute( item.Command, new CommandParameters( CommandUser.Default, item.Command, null ) );
		}


		#endregion
	}
}
