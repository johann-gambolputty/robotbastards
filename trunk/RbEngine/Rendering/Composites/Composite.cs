using System;

namespace RbEngine.Rendering.Composites
{
	/// <summary>
	/// Base class for composite rendering objects
	/// </summary>
	public abstract class Composite
	{
		/// <summary>
		/// Renders this object
		/// </summary>
		public abstract void Render( );
	}
}
