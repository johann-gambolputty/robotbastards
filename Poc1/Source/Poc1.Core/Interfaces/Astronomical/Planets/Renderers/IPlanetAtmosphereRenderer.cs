
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers
{
	/// <summary>
	/// Interface for planetary atmosphere renderers
	/// </summary>
	public interface IPlanetAtmosphereRenderer
	{
		/// <summary>
		/// Sets up an effect used to render objects as seen through this atmosphere
		/// </summary>
		/// <param name="camera">Current camera</param>
		/// <param name="effect">Effect to set up</param>
		/// <param name="farObject">Effect is set up for a far away object</param>
		/// <remarks>
		/// Will expect certain variables to be available in the effect
		/// </remarks>
		void SetupObjectEffect( IUniCamera camera, IEffect effect, bool farObject );
	}
}
