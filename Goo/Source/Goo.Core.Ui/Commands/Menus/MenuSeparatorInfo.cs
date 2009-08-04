
namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// A separator
	/// </summary>
	public class MenuSeparatorInfo : MenuNodeInfo
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public MenuSeparatorInfo( int ordinal ) :
			base( "seperator", "-----------", ordinal )
		{
		}
	}
}
