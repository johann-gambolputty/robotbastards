using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Rendering
{
	/// <summary>
	/// Atmosphere renderer base interface
	/// </summary>
	public interface IAtmosphereRenderer : IRenderable
	{
		/// <summary>
		/// Sets up parameters for effects that use atmospheric rendering
		/// </summary>
		void SetupAtmosphereEffectParameters( IEffect effect, bool objectRendering );
	}
}
