using System.Collections.Generic;
using Rb.Rendering;

namespace Rb.World.Rendering
{
	/// <summary>
	/// Simple class that wraps up another IRenderable object, and registers self with the scene rendering list
	/// </summary>
	public class SceneRenderable : List<IRenderable>, IRenderable, ISceneObject
	{
		#region IRenderable Members

		/// <summary>
		/// Renders all IRenderable objects in the base list
		/// </summary>
		/// <param name="context">Render context</param>
		public void Render( IRenderContext context )
		{
			foreach ( IRenderable renderable in this )
			{
				renderable.Render( context );
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Attaches this object to the scene render list
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void SetSceneContext( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		#endregion
	}
}
