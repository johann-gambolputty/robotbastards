using Bob.Core.Ui.Interfaces.Views;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

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
		/// Gets a view for a specified environment model template
		/// </summary>
		IViewInfo GetTemplateView( IPlanetEnvironmentModelTemplate template );
	}
}