using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Components.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets
{
	/// <summary>
	/// Planet renderer
	/// </summary>
	public interface IPlanetRenderer : IComposite<IPlanetEnvironmentRenderer>, IRenderable<IUniRenderContext>
	{
		/// <summary>
		/// Gets the planet that this renderer is attached to
		/// </summary>
		IPlanet Planet
		{
			get;
		}

		/// <summary>
		/// Shortcut to Planet.PlanetModel
		/// </summary>
		IPlanetModel PlanetModel
		{
			get;
		}
	}

}
