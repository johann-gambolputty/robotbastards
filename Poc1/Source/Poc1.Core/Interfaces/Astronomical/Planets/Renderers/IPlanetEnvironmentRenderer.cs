using Poc1.Core.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers
{

	/// <summary>
	/// Planet environment renderer interface
	/// </summary>
	public interface IPlanetEnvironmentRenderer : IRenderable<IUniRenderContext>
	{
		/// <summary>
		/// Gets the planet that this renderer is attached to (via the planet renderer)
		/// </summary>
		IPlanet Planet
		{
			get;
		}

		/// <summary>
		/// Gets/sets the planet renderer that this renderer is a part of
		/// </summary>
		IPlanetRenderer PlanetRenderer
		{
			get; set;
		}
	}

}
