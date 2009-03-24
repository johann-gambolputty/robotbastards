using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

namespace Poc1.Bob.Core.Interfaces.Planets
{
	/// <summary>
	/// Planet template/instance view. Shows parameters of a planet template and
	/// the currently displayed planet instance
	/// </summary>
	public interface IPlanetModelTemplateView
	{

		/// <summary>
		/// Gets/sets the planet model displayed by this view
		/// </summary>
		IPlanetModel PlanetModel
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the planet model template displayed by this view
		/// </summary>
		IPlanetModelTemplate PlanetModelTemplate
		{
			get; set;
		}
	}
}
