
namespace Poc1.Universe.Interfaces.Planets.Models
{
	/// <summary>
	/// Planet atmosphere model
	/// </summary>
	public interface IPlanetAtmosphereModel : IPlanetEnvironmentModel
	{
		/// <summary>
		/// Gets/sets the thickness of the atmosphere
		/// </summary>
		Units.Metres AtmosphereThickness
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the atmosphere phase coefficient
		/// </summary>
		/// <remarks>
		/// Good values are -0.75 to -0.99
		/// </remarks>
		float PhaseCoefficient
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the weighting applied to the phase function.
		/// </summary>
		/// <remarks>
		/// A weighting of zero means that the phase function has no effect, and the
		/// displayed colours are from in-scatter/out-scatter only.
		/// </remarks>
		float PhaseWeight
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the weighting applied to the mie phase function
		/// </summary>
		float MiePhaseWeight
		{
			get; set;
		}
	}
}
