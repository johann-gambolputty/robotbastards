
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Poc1.Universe.Planets.Models.Templates;

namespace Poc1.Universe.Planets.Spherical.Models.Templates
{
	/// <summary>
	/// Planetary atmosphere model template
	/// </summary>
	public class SpherePlanetAtmosphereModelTemplate : PlanetAtmosphereModelTemplate
	{
		/// <summary>
		/// Atmosphere model creation
		/// </summary>
		public override IPlanetEnvironmentModel CreateInstance( IPlanetModel planetModel, ModelTemplateInstanceContext context )
		{
			SpherePlanetAtmosphereModel model = new SpherePlanetAtmosphereModel( );
			planetModel.Atmosphere = model;
			return model;
		}
	}
}
