using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Interface for scene objects that can be rendered
	/// </summary>
	public interface ISceneRenderable
	{
		/// <summary>
		/// Renders the object
		/// </summary>
		/// <param name="renderTime">Frame time (in TinyTime ticks)</param>
		void Render( long renderTime );
	}
}
