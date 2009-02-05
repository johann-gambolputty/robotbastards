
namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Base interface for templates defining planetary environmental features
	/// </summary>
	public interface IPlanetEnvironmentModelTemplate : IPlanetModelTemplateBase
	{
		/// <summary>
		/// Creates and sets up an instance of the template
		/// </summary>
		void CreateInstance( IPlanetModel planetModel, ModelTemplateInstanceContext context );
	}
}
