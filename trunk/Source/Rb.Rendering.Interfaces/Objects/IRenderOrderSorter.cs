using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Interface for sorting an array of renderable objects
	/// </summary>
	public interface IRenderOrderSorter
	{
		/// <summary>
		/// Sorts an array of renderable objects into the order that they should be rendered in
		/// </summary>
		/// <param name="renderables">Renderable object array to sort</param>
		/// <returns>Returns the same array, sorted in render order</returns>
		IRenderable[] Sort( IRenderable[] renderables );
	}
}
