
namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Menu node interface (base interface for groups, items and separators)
	/// </summary>
	public interface IMenuNode
	{
		/// <summary>
		/// Sets the name of this node
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the ordinal value of this node
		/// </summary>
		int Ordinal
		{
			get;
		}
	}
}
