
using Bob.Core.Ui.Interfaces.Views;

namespace Bob.Core.Workspaces.Interfaces
{
	/// <summary>
	/// View provider interface
	/// </summary>
	public interface IViewProvider
	{
		/// <summary>
		/// Gets the array of views supported by this provider
		/// </summary>
		IViewInfo[] Views
		{
			get;
		}
	}
}
