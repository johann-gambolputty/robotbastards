
namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Interface for planetary atmosphere
	/// </summary>
	public interface IPlanetAtmosphereModel
	{
		/// <summary>
		/// Gets/sets the coefficient for the atmosphere phase function (Heyney-Greenstein)
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
	}
}
