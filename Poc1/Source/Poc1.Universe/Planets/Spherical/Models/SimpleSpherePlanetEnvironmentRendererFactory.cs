using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Renderers;
using Poc1.Universe.Planets.Spherical.Renderers;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Very dumb renderer factory implementation for planet environment models
	/// </summary>
	public class SimpleSpherePlanetEnvironmentRendererFactory : IPlanetEnvironmentRendererFactory
	{

		#region IPlanetEnvironmentModelRendererFactory Members

		/// <summary>
		/// Creates a renderer for a model
		/// </summary>
		/// <param name="model">Source model</param>
		/// <returns>Returns a renderer for a model. Returns null if no renderer is associated with the model</returns>
		public IPlanetEnvironmentRenderer CreateModelRenderer( IPlanetEnvironmentModel model )
		{
			return model.InvokeVisit( m_Visitor );
		}

		#endregion

		#region Private Members

		private readonly RendererFactoryVisitor m_Visitor = new RendererFactoryVisitor( );

		/// <summary>
		/// Render factory visitor
		/// </summary>
		private class RendererFactoryVisitor : IPlanetEnvironmentModelVisitor<IPlanetEnvironmentRenderer>
		{
			/// <summary>
			/// Visits a model that is not recognized by the compiler
			/// </summary>
			/// <param name="model">Model to visit</param>
			public IPlanetEnvironmentRenderer Visit( IPlanetEnvironmentModel model )
			{
				return null;
			}

			/// <summary>
			/// Creates a renderer for an ocean model
			/// </summary>
			public IPlanetEnvironmentRenderer Visit( IPlanetOceanModel model )
			{
				return new SpherePlanetOceanRenderer( );
			}

			/// <summary>
			/// Creates a renderer for an atmosphere model
			/// </summary>
			public IPlanetEnvironmentRenderer Visit( IPlanetAtmosphereModel model )
			{
				return new SpherePlanetAtmosphereRenderer( );
			}

			/// <summary>
			/// Creates a renderer for a cloud model
			/// </summary>
			public IPlanetEnvironmentRenderer Visit( IPlanetCloudModel model )
			{
				return new SpherePlanetCloudRenderer( );
			}

		}

		#endregion
	}
}
