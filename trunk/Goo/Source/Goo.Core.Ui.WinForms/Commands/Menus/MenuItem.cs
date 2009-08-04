using System.Drawing;
using System.Windows.Forms;
using Goo.Core.Ui.Commands.Menus;
using Rb.Core.Utils;
using Rb.Interaction.Classes;

namespace Goo.Core.Ui.WinForms.Commands.Menus
{
	/// <summary>
	/// Menu item implementation
	/// </summary>
	public class MenuItem : IMenuItem
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="item">Tool strip item that this object is bound to</param>
		/// <param name="itemInfo">Item information</param>
		public MenuItem( ToolStripMenuItem item, MenuItemInfo itemInfo )
		{
			Arguments.CheckNotNull( item, "item" );
			Arguments.CheckNotNull( itemInfo, "itemInfo" );

			item.Name = itemInfo.Name;
			item.Text = itemInfo.Text;
			item.Tag = this;
			m_Item = item;
			m_Ordinal = itemInfo.Ordinal;
			m_Command = itemInfo.Command;
		}

		#region IMenuItem Members

		/// <summary>
		/// Fluent interface for setting images
		/// </summary>
		public IMenuItem SetImage( Image image )
		{
			m_Item.Image = image;
			return this;
		}

		/// <summary>
		/// Fluent interface for setting text
		/// </summary>
		public IMenuItem SetText( string text )
		{
			m_Item.Text = text;
			return this;
		}

		/// <summary>
		/// Gets/sets this item's image
		/// </summary>
		public Image Image
		{
			get { return m_Item.Image; }
			set { m_Item.Image = value; }
		}

		/// <summary>
		/// Gets/sets this item's text
		/// </summary>
		public string Text
		{
			get { return m_Item.Text; }
			set { m_Item.Text = value; }
		}

		/// <summary>
		/// Gets the command associated with this item
		/// </summary>
		public Command Command
		{
			get { return m_Command; }
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

		#endregion

		#region Private Members

		private readonly int m_Ordinal;
		private readonly ToolStripMenuItem m_Item;
		private readonly Command m_Command;

		#endregion
	}
}
