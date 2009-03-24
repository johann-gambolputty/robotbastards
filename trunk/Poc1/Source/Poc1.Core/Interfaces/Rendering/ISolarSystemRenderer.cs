using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Rendering
{
	/// <summary>
	/// Solar system renderer
	/// </summary>
	public interface ISolarSystemRenderer
	{
		/// <summary>
		/// Renders a solar system
		/// </summary>
		/// <param name="solarSystem">The scene</param>
		/// <param name="context">Rendering context</param>
		void Render( ISolarSystem solarSystem, IUniCamera camera, IRenderContext context );
	}
}
