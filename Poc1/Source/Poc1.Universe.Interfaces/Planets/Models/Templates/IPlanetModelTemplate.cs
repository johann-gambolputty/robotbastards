
using System.Collections.Generic;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Main planetary model template
	/// </summary>
	public interface IPlanetModelTemplate : IPlanetModelTemplateBase
	{
		/// <summary>
		/// Gets/sets the list of environment model templates
		/// </summary>
		List<IPlanetEnvironmentModelTemplate> EnvironmentModelTemplates
		{
			get; set;
		}

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		IPlanetModel CreateModelInstance( ModelTemplateInstanceContext context );
	}
}
