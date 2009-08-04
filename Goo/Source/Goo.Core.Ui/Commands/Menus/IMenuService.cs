
namespace Goo.Core.Ui.Commands.Menus
{
	/// <summary>
	/// Main menu service interface
	/// </summary>
	public interface IMenuService
	{
		/// <summary>
		/// Finds or creates a root menu group
		/// </summary>
		IMenuGroup this[ MenuGroupInfo info ]
		{
			get;
		}
	}
}
