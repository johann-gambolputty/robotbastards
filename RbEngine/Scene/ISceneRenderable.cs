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
		/// <param name="delta">Frame delta (time between updates). Varies from 0 to 1.</param>
		void Render( float delta );
	}
}
