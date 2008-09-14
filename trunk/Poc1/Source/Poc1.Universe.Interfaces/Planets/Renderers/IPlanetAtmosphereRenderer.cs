using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Renderers
{
	/// <summary>
	/// Planet atmosphere renderer
	/// </summary>
	public interface IPlanetAtmosphereRenderer : IPlanetEnvironmentRenderer
	{
		/// <summary>
		/// Sets up parameters for effects that use atmospheric rendering
		/// </summary>
		void SetupAtmosphereEffectParameters( IEffect effect, bool objectRendering );
	}
}
