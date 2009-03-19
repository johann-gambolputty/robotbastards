
namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Base interface for templates defining planetary environmental features
	/// </summary>
	public interface IPlanetEnvironmentModelTemplate : IPlanetModelTemplateBase
	{
		/// <summary>
		/// Sets up an instance of the template
		/// </summary>
		void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context );

		/// <summary>
		/// Calls a visitor object
		/// </summary>
		T InvokeVisit<T>( IPlanetEnvironmentModelTemplateVisitor<T> visitor );
	}
}
