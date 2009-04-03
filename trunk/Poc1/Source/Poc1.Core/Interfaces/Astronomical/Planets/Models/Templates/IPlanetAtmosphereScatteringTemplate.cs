
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Interface for scattering atmosphere templates
	/// </summary>
	public interface IPlanetAtmosphereScatteringTemplate : IPlanetAtmosphereTemplate
	{
		/// <summary>
		/// Gets/sets the name of the atmosphere
		/// </summary>
		/// <remarks>
		/// The atmosphere name maps to the texture files used for in-game rendering, and the 
		/// data files used for editor build tasks.
		/// </remarks>
		string AtmosphereName
		{
			get; set;
		}
		
		/// <summary>
		/// Gets the scattering lookup texture data
		/// </summary>
		ITexture3d ScatteringTexture
		{
			get; set;
		}

		/// <summary>
		/// Gets the optical depth lookup texture data
		/// </summary>
		ITexture2d OpticalDepthTexture
		{
			get; set;
		}
	}
}
