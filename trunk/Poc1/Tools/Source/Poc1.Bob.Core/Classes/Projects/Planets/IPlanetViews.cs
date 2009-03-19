using Bob.Core.Ui.Interfaces.Views;

namespace Poc1.Bob.Core.Classes.Projects.Planets
{
	/// <summary>
	/// Planet component docking views
	/// </summary>
	public interface IPlanetViews
	{
		/// <summary>
		/// Gets all views
		/// </summary>
		IViewInfo[] Views { get; }

		/// <summary>
		/// Gets the cloud view
		/// </summary>
		IViewInfo CloudView { get; }
	}
}