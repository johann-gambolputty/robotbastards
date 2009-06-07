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
		IViewInfo[] Views
		{
			get;
		}

		/// <summary>
		/// Gets the default view
		/// </summary>
		IViewInfo DefaultView
		{
			get;
		}

		/// <summary>
		/// Gets a view on the properties of the  specified object
		/// </summary>
		IViewInfo GetPropertiesView( object obj );
	}
}