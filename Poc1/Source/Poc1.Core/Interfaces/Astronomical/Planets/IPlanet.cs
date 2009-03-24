
using Poc1.Core.Interfaces.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets
{
	/// <summary>
	/// Planet interface
	/// </summary>
	public interface IPlanet : IAstronomicalBody, IRenderable<IUniRenderContext>
	{
		/// <summary>
		/// Gets the planet model
		/// </summary>
		IPlanetModel Model
		{
			get;
		}

		/// <summary>
		/// Gets the planet renderer
		/// </summary>
		IPlanetRenderer Renderer
		{
			get;
		}
	}
}
