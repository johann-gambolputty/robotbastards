using System.Collections.Generic;
using Rb.Core.Components.Generic;

namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Main planetary model template
	/// </summary>
	public interface IPlanetModelTemplate : IPlanetModelTemplateBase, IComposite<IPlanetEnvironmentModelTemplate>
	{
		/// <summary>
		/// Gets/sets the list of environment model templates
		/// </summary>
		List<IPlanetEnvironmentModelTemplate> EnvironmentModelTemplates
		{
			get; set;
		}

		/// <summary>
		/// Gets a model template of the specified type
		/// </summary>
		/// <typeparam name="T">Model template type to retrieve</typeparam>
		/// <returns>Model template object, or null if no model template of type T exists in this planet template</returns>
		T GetModelTemplate<T>( ) where T : class, IPlanetEnvironmentModelTemplate;

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		void CreateModelInstance( IPlanetModel planetModel, ModelTemplateInstanceContext context );
	}
}
