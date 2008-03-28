
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Renderable object interface
	/// </summary>
	public interface IRenderable
	{
		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		void Render( IRenderContext context );
	}
}
