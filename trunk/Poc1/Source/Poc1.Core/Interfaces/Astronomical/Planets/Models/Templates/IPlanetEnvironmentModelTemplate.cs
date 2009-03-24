
namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Base interface for templates defining planetary environmental features
	/// </summary>
	public interface IPlanetEnvironmentModelTemplate
	{
		/// <summary>
		/// Sets up an instance of the template
		/// </summary>
		void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context );

		/// <summary>
		/// Calls a visitor object
		/// </summary>
		TReturn InvokeVisit<TReturn>( IPlanetEnvironmentModelTemplateVisitor<TReturn> visitor );
	}
}
