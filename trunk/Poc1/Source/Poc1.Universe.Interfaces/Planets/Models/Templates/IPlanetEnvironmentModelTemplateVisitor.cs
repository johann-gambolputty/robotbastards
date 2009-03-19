
namespace Poc1.Universe.Interfaces.Planets.Models.Templates
{
	/// <summary>
	/// Visitor interface for planet environment templates
	/// </summary>
	/// <remarks>
	/// Visit() methods should rarely be called directly. Instead, call
	///  <see cref="IPlanetEnvironmentModelTemplate.InvokeVisit"/>. This will call the correctly
	/// typed method in the visitor object.
	/// 
	/// For example:
	/// <code>
	/// IPlanetEnvironmentModelTemplate template = new PlanetCloudModelTemplate();
	/// IPlanetEnvironmentModelTemplateVisitor visitor = new MockPlanetEnvironmentModelTemplateVisitor();
	/// 
	/// visitor.Visit(template); // This will just call Visit(IPlanetEnvironmentModelTemplate)
	/// template.InvokeVisit(visitor); // This will call Visit(IPlanetCloudModelTemplate)
	/// </code>
	/// 
	/// </remarks>
	public interface IPlanetEnvironmentModelTemplateVisitor<T>
	{
		/// <summary>
		/// Visits a model template that has no explicitly-typed support in this visitor
		/// </summary>
		/// <param name="modelTemplate">Model template to visit</param>
		T Visit( IPlanetEnvironmentModelTemplate modelTemplate );

		/// <summary>
		/// Visits a cloud model template
		/// </summary>
		/// <param name="cloudModelTemplate">Model template to visit</param>
		T Visit( IPlanetCloudModelTemplate cloudModelTemplate );

		/// <summary>
		/// Visits a terrain model template
		/// </summary>
		/// <param name="terrainModelTemplate">Model template to visit</param>
		T Visit( IPlanetProcTerrainModel terrainModelTemplate );

		/// <summary>
		/// Visits a ring model template
		/// </summary>
		/// <param name="ringModelTemplate">Model template to visit</param>
		T Visit( IPlanetRingModelTemplate ringModelTemplate );

		/// <summary>
		/// Visits an ocean model template
		/// </summary>
		/// <param name="oceanModelTemplate">Model template to visit</param>
		T Visit( IPlanetOceanModelTemplate oceanModelTemplate );

		/// <summary>
		/// Visits an atmosphere model template
		/// </summary>
		/// <param name="atmosphereModelTemplate">Model template to visit</param>
		T Visit( IPlanetAtmosphereModelTemplate atmosphereModelTemplate );
	}
}
