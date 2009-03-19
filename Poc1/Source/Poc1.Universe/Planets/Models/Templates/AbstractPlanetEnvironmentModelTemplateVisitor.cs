using Poc1.Universe.Interfaces.Planets.Models.Templates;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Base class implementation of <see cref="IPlanetEnvironmentModelTemplateVisitor{T}"/>
	/// </summary>
	/// <remarks>
	/// All Visit() models are virtual, and by default call the generic <see cref="Visit(IPlanetEnvironmentModelTemplate)"/>
	/// </remarks>
	public abstract class AbstractPlanetEnvironmentModelTemplateVisitor<T> : IPlanetEnvironmentModelTemplateVisitor<T>
	{
		/// <summary>
		/// Visits a model template that has no explicitly-typed support in this visitor
		/// </summary>
		/// <param name="modelTemplate">Model template to visit</param>
		public abstract T Visit( IPlanetEnvironmentModelTemplate modelTemplate );

		/// <summary>
		/// Visits a cloud model template
		/// </summary>
		/// <param name="cloudModelTemplate">Model template to visit</param>
		public virtual T Visit( IPlanetCloudModelTemplate cloudModelTemplate )
		{
			Visit( ( IPlanetEnvironmentModelTemplate )cloudModelTemplate );
		}

		/// <summary>
		/// Visits a terrain model template
		/// </summary>
		/// <param name="terrainModelTemplate">Model template to visit</param>
		public virtual T Visit( IPlanetProcTerrainModel terrainModelTemplate )
		{
			Visit( ( IPlanetEnvironmentModelTemplate )terrainModelTemplate );
		}

		/// <summary>
		/// Visits a ring model template
		/// </summary>
		/// <param name="ringModelTemplate">Model template to visit</param>
		public virtual T Visit( IPlanetRingModelTemplate ringModelTemplate )
		{
			Visit( ( IPlanetEnvironmentModelTemplate )ringModelTemplate );
		}

		/// <summary>
		/// Visits an ocean model template
		/// </summary>
		/// <param name="oceanModelTemplate">Model template to visit</param>
		public virtual T Visit( IPlanetOceanModelTemplate oceanModelTemplate )
		{
			Visit( ( IPlanetEnvironmentModelTemplate )oceanModelTemplate );
		}

		/// <summary>
		/// Visits an atmosphere model template
		/// </summary>
		/// <param name="atmosphereModelTemplate">Model template to visit</param>
		public virtual T Visit( IPlanetAtmosphereModelTemplate atmosphereModelTemplate )
		{
			Visit( ( IPlanetEnvironmentModelTemplate )atmosphereModelTemplate );
		}
	}
}
