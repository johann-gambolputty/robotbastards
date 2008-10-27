using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Renderers
{
	/// <summary>
	/// Planet atmosphere renderer
	/// </summary>
	public interface IPlanetAtmosphereRenderer : IPlanetEnvironmentRenderer, IUniRenderable
	{
		/// <summary>
		/// Sets up parameters for effects that use atmospheric rendering
		/// </summary>
		/// <param name="effect">Effect to set up</param>
		/// <param name="objectRendering">True if the effect is being used to render an object in the atmosphere</param>
		/// <param name="deepRender">If true, distances are passed to the effect in astro render units. Otherwise, render units are used.</param>
		void SetupAtmosphereEffectParameters( IEffect effect, bool objectRendering, bool deepRender );
	}
}
