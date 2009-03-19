
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Template for planetary atmosphere model instances
	/// </summary>
	public abstract class PlanetAtmosphereModelTemplate : PlanetEnvironmentModelTemplate
	{

		/// <summary>
		/// Atmosphere model creation
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			IPlanetAtmosphereModel atmosphereModel = ( IPlanetAtmosphereModel )model;
		}
	}
}
