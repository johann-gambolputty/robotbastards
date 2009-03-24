using Poc1.Core.Classes.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Models
{
	/// <summary>
	/// Very simple implementation of the planet environment model factory (hard-coded relationship
	/// between template types and model types)
	/// </summary>
	public class SimpleSpherePlanetEnvironmentModelFactory : IPlanetEnvironmentModelFactory
	{
		#region IPlanetEnvironmentModelFactory Members

		/// <summary>
		/// Creates an environment model from an environment model template
		/// </summary>
		/// <param name="modelTemplate">Template to instance</param>
		/// <returns>Returns an environment model based on the specified template, or null if no model is assoiated with the template type</returns>
		public IPlanetEnvironmentModel CreateModel( IPlanetEnvironmentModelTemplate modelTemplate )
		{
			return modelTemplate.InvokeVisit( m_Visitor );
		}

		#endregion

		#region Private Members

		private readonly ModelFactoryVisitor m_Visitor = new ModelFactoryVisitor( );

		/// <summary>
		/// Visitor class used to create environment models
		/// </summary>
		private class ModelFactoryVisitor : IPlanetEnvironmentModelTemplateVisitor<IPlanetEnvironmentModel>
		{
			#region IPlanetEnvironmentModelTemplateVisitor Members

			/// <summary>
			/// Visits a model template that has no explicitly-typed support in this visitor
			/// </summary>
			/// <param name="modelTemplate">Model template to visit</param>
			public IPlanetEnvironmentModel Visit( IPlanetEnvironmentModelTemplate modelTemplate )
			{
				return null;
			}

			/// <summary>
			/// Visits a cloud model template
			/// </summary>
			/// <param name="cloudModelTemplate">Model template to visit</param>
			public IPlanetEnvironmentModel Visit( IPlanetSimpleCloudTemplate cloudModelTemplate )
			{
				return new PlanetSimpleCloudModel( );
			}

			/// <summary>
			/// Visits a terrain model template
			/// </summary>
			/// <param name="terrainModelTemplate">Model template to visit</param>
			public IPlanetEnvironmentModel Visit( IPlanetProcTerrainTemplate terrainModelTemplate )
			{
				return new PlanetProcTerrainModel( );
			}

			/// <summary>
			/// Visits a ring model template
			/// </summary>
			/// <param name="ringModelTemplate">Model template to visit</param>
			public IPlanetEnvironmentModel Visit( IPlanetRingTemplate ringModelTemplate )
			{
				return new SpherePlanetRingModel( );
			}

			/// <summary>
			/// Visits an ocean model template
			/// </summary>
			/// <param name="oceanModelTemplate">Model template to visit</param>
			public IPlanetEnvironmentModel Visit( IPlanetOceanTemplate oceanModelTemplate )
			{
				return new PlanetOceanModel( );
			}

			/// <summary>
			/// Visits an atmosphere model template
			/// </summary>
			/// <param name="atmosphereModelTemplate">Model template to visit</param>
			public IPlanetEnvironmentModel Visit( IPlanetAtmosphereTemplate atmosphereModelTemplate )
			{
				return new PlanetAtmosphereScatteringModel( );
			}

			#endregion
		}

		#endregion
	}
}
