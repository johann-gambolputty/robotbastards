
namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	/// <summary>
	/// Scattering-based atmosphere model
	/// </summary>
	public interface IPlanetAtmosphereScatteringModel : IPlanetAtmosphereModel
	{
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
		/// Gets/sets the weighting applied to the rayleigh phase function.
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
		/// Gets/sets the weighting applied to the mie phase function.
		/// </summary>
		float MiePhaseWeight
		{
			get; set;
		}
	}
}
