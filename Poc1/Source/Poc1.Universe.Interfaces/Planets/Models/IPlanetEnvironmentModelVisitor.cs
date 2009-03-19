
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Visitor pattern interface for planet models
	/// </summary>
	public interface IPlanetEnvironmentModelVisitor<ReturnType>
	{
		/// <summary>
		/// Visits a model that is not recognized by the compiler
		/// </summary>
		/// <param name="model">Model to visit</param>
		ReturnType Visit( IPlanetEnvironmentModel model );

		/// <summary>
		/// Visits an atmosphere model
		/// </summary>
		/// <param name="model">Model to visit</param>
		ReturnType Visit( IPlanetAtmosphereModel model );

		/// <summary>
		/// Visits a cloud model
		/// </summary>
		/// <param name="model">Model to visit</param>
		ReturnType Visit( IPlanetCloudModel model );

	}
}
