using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Implementation of <see cref="IRenderOrderSorter"/> that does bugger all
	/// </summary>
	public class UnorderedRenderOrderSorter : IRenderOrderSorter
	{
		/// <summary>
		/// Sorts an array of renderable objects into the order that they should be rendered in
		/// </summary>
		/// <param name="renderables">Renderable object array to sort</param>
		/// <returns>Returns the same array, sorted in render order</returns>
		public IRenderable[] Sort( IRenderable[] renderables )
		{
			return renderables;
		}
	}
}
