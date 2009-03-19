
using Poc1.Universe.Interfaces.Planets.Models.Templates;

namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Creates environment models from templates
	/// </summary>
	public interface IPlanetEnvironmentModelFactory
	{
		/// <summary>
		/// Creates an environment model from an environment model template
		/// </summary>
		/// <param name="modelTemplate">Template to instance</param>
		/// <returns>Returns an environment model based on the specified template, or null if no model is assoiated with the template type</returns>
		IPlanetEnvironmentModel CreateModel( IPlanetEnvironmentModelTemplate modelTemplate );
	}
}
