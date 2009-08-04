
using Rb.Core.Utils;

namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Menu node (base class for menu groups and menu items)
	/// </summary>
	public class MenuNodeInfo
	{
		/// <summary>
		/// Ordinal value for nodes that appear first in a group
		/// </summary>
		public const int OrderFirst = 0;

		/// <summary>
		/// Ordinal value for nodes that appear last in a group
		/// </summary>
		public const int OrderLast = 100000;

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Node name</param>
		/// <param name="text">Menu item or group text (including ampersand for designating hot-key)</param>
		/// <param name="ordinal">Ordinal value of this node </param>
		public MenuNodeInfo( string name, string text, int ordinal )
		{
			Arguments.CheckNotNull( name, "name" );
			m_Name = name;
			m_Text = text;
			m_Ordinal = ordinal;
		}

		/// <summary>
		/// Gets the text of this node
		/// </summary>
		public string Text
		{
			get { return m_Text; }
		}

		/// <summary>
		/// Gets the name of this node
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the order value of this node
		/// </summary>
		public int Ordinal
		{
			get { return m_Ordinal; }
		}

		#region Private Members

		private readonly int m_Ordinal;
		private readonly string m_Name;
		private readonly string m_Text;

		#endregion
	}
}
