
namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Visitor pattern interface for planet models
	/// </summary>
	public interface IPlanetEnvironmentModelVisitor<TReturn>
	{
		/// <summary>
		/// Visits a model that is not recognized by the compiler
		/// </summary>
		/// <param name="model">Model to visit</param>
		TReturn Visit( IPlanetEnvironmentModel model );

		/// <summary>
		/// Visits an ocean model
		/// </summary>
		/// <param name="model">Model to visit</param>
		TReturn Visit( IPlanetOceanModel model );

		/// <summary>
		/// Visits an atmosphere model
		/// </summary>
		/// <param name="model">Model to visit</param>
		TReturn Visit( IPlanetAtmosphereScatteringModel model );

		/// <summary>
		/// Visits a cloud model
		/// </summary>
		/// <param name="model">Model to visit</param>
		TReturn Visit( IPlanetSimpleCloudModel model );

		/// <summary>
		/// Visits a ring model
		/// </summary>
		/// <param name="model">Model to visit</param>
		TReturn Visit( IPlanetRingModel model );

		/// <summary>
		/// Visits an homogenous procedural terrain model
		/// </summary>
		/// <param name="model">Model to visit</param>
		TReturn Visit( IPlanetHomogenousProceduralTerrainModel model );

	}
}
