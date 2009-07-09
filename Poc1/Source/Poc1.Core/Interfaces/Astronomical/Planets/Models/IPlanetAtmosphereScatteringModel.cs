
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models
{
	public enum ScatteringAtmosphereObjectColourOutput
	{		
		//	Must match constants in atmosphereBase.cg
		FullColour = 0,
		ColourOnly = 1,
		ScatteredColour = 2,
		OpticalDepthOnly = 3,
		ColourOpticalDepth = 4
	}

	/// <summary>
	/// Scattering-based atmosphere model
	/// </summary>
	public interface IPlanetAtmosphereScatteringModel : IPlanetAtmosphereModel
	{
		/// <summary>
		/// Gets/sets object colour output
		/// </summary>
		ScatteringAtmosphereObjectColourOutput ObjectColourOutput
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

		//	TODO: AP: These should go in a derived interface specific to scattering models based
		//	on 3d precomputed texture lookups (and also probably specific for spherical planets)

		/// <summary>
		/// Gets/sets the scattering lookup texture data
		/// </summary>
		ITexture3d ScatteringTexture
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the optical depth lookup texture data
		/// </summary>
		ITexture2d OpticalDepthTexture
		{
			get; set;
		}
	}
}
