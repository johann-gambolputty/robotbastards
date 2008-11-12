using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Renderers
{
	/// <summary>
	/// Planet cloud renderer
	/// </summary>
	public interface IPlanetCloudRenderer : IPlanetEnvironmentRenderer
	{
		/// <summary>
		/// Sets up parameters for effects that use cloud rendering
		/// </summary>
		/// <param name="effect">Effect to set up</param>
		void SetupCloudEffectParameters( IEffect effect );
	}
}
