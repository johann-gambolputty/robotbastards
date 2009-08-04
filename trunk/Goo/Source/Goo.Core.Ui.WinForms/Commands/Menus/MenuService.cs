using System.Windows.Forms;
using Goo.Core.Commands;
using Goo.Core.Ui.Commands.Menus;
using Rb.Core.Utils;
using GooMenuItem = Goo.Core.Ui.Commands.Menus.MenuItemInfo;
using WinFormsMenuItem = System.Windows.Forms.MenuItem;

namespace Goo.Core.Ui.WinForms.Commands.Menus
{
	/// <summary>
	/// Main menu service winforms implementation
	/// </summary>
	public class MenuService : IMenuService
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainForm">The form to add a menu to</param>
		/// <param name="commandHost">This command host is used to execute commands for all menu items managed by this service</param>
		public MenuService( Form mainForm, ICommandHost commandHost )
		{
			Arguments.CheckNotNull( mainForm, "mainForm" );
			Arguments.CheckNotNull( commandHost, "commandHost" );
			m_Form = mainForm;
			m_CommandHost = commandHost;
		}

		#region IMenuService Members

		/// <summary>
		/// Finds or creates a root menu group
		/// </summary>
		public IMenuGroup this[ MenuGroupInfo info ]
		{
			get
			{
				ToolStripItem item = MainMenu.Items[ info.Name ];
				if ( item != null )
				{
					return ( IMenuGroup )item.Tag;
				}

				item = MainMenu.Items.Add( info.Text );
				return new MenuGroup( m_CommandHost, ( ToolStripMenuItem )item, info );
			}
		}

		#endregion

		#region Private Members

		private readonly Form m_Form;
		private MenuStrip m_Menu;
		private readonly ICommandHost m_CommandHost;

		/// <summary>
		/// Gets the main menu attached to the form
		/// </summary>
		private MenuStrip MainMenu
		{
			get
			{
				if ( m_Menu != null )
				{
					return m_Menu;
				}
				m_Menu = new MenuStrip( );
				m_Form.Controls.Add( m_Menu );
				return m_Menu;
			}
		}

		#endregion

	}
}
