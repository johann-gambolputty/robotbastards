using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Renderers
{
	/// <summary>
	/// Base interface for planet environment renderers
	/// </summary>
	public interface IPlanetEnvironmentRenderer : IRenderable
	{
		/// <summary>
		/// Gets/sets the planet associated with this renderer
		/// </summary>
		IPlanet Planet
		{
			get; set;
		}
	}
}
