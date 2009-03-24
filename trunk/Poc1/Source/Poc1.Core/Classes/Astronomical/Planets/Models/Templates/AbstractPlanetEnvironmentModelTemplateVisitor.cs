
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Base class implementation of <see cref="IPlanetEnvironmentModelTemplateVisitor{TReturn}"/>
	/// </summary>
	/// <remarks>
	/// All Visit() models are virtual, and by default call the generic <see cref="Visit(IPlanetEnvironmentModelTemplate)"/>
	/// </remarks>
	public abstract class AbstractPlanetEnvironmentModelTemplateVisitor<TReturn> : IPlanetEnvironmentModelTemplateVisitor<TReturn>
	{
		/// <summary>
		/// Visits a model template that has no explicitly-typed support in this visitor
		/// </summary>
		/// <param name="modelTemplate">Model template to visit</param>
		public abstract TReturn Visit( IPlanetEnvironmentModelTemplate modelTemplate );

		/// <summary>
		/// Visits a cloud model template
		/// </summary>
		/// <param name="cloudModelTemplate">Model template to visit</param>
		public virtual TReturn Visit( IPlanetSimpleCloudTemplate cloudModelTemplate )
		{
			return Visit( ( IPlanetEnvironmentModelTemplate )cloudModelTemplate );
		}

		/// <summary>
		/// Visits a terrain model template
		/// </summary>
		/// <param name="terrainModelTemplate">Model template to visit</param>
		public virtual TReturn Visit( IPlanetProcTerrainTemplate terrainModelTemplate )
		{
			return Visit( ( IPlanetEnvironmentModelTemplate )terrainModelTemplate );
		}

		/// <summary>
		/// Visits a ring model template
		/// </summary>
		/// <param name="ringModelTemplate">Model template to visit</param>
		public virtual TReturn Visit( IPlanetRingTemplate ringModelTemplate )
		{
			return Visit( ( IPlanetEnvironmentModelTemplate )ringModelTemplate );
		}

		/// <summary>
		/// Visits an ocean model template
		/// </summary>
		/// <param name="oceanModelTemplate">Model template to visit</param>
		public virtual TReturn Visit( IPlanetOceanTemplate oceanModelTemplate )
		{
			return Visit( ( IPlanetEnvironmentModelTemplate )oceanModelTemplate );
		}

		/// <summary>
		/// Visits an atmosphere model template
		/// </summary>
		/// <param name="atmosphereModelTemplate">Model template to visit</param>
		public virtual TReturn Visit( IPlanetAtmosphereTemplate atmosphereModelTemplate )
		{
			return Visit( ( IPlanetEnvironmentModelTemplate )atmosphereModelTemplate );
		}
	}
}
