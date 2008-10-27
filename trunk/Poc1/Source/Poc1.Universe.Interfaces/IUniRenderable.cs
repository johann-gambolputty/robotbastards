using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces
{
	/// <summary>
	/// Uni-renderable object
	/// </summary>
	public interface IUniRenderable
	{
		/// <summary>
		/// Renders the deep version of this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		void DeepRender( IRenderContext context );
	}
}
