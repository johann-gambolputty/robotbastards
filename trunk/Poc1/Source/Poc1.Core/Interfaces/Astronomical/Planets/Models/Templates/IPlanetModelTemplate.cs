using Rb.Core.Components.Generic;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Main planetary model template
	/// </summary>
	public interface IPlanetModelTemplate : IComposite<IPlanetEnvironmentModelTemplate>
	{
		/// <summary>
		/// Gets a model template of the specified type
		/// </summary>
		/// <typeparam name="T">Model template type to retrieve</typeparam>
		/// <returns>Model template object, or null if no model template of type T exists in this planet template</returns>
		T GetModelTemplate<T>( ) where T : class, IPlanetEnvironmentModelTemplate;

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		void CreateModelInstance( IPlanetModel planetModel, IPlanetEnvironmentModelFactory modelFactory, ModelTemplateInstanceContext context );
	}
}
