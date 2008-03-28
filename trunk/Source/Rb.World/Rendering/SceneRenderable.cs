using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.World.Rendering
{
	/// <summary>
	/// Simple class that wraps up another IRenderable object, and registers self with the scene rendering list
	/// </summary>
	public class SceneRenderable : List< IRenderable >, IRenderable, ISceneObject
	{
		#region IRenderable Members

		/// <summary>
		/// Renders all IRenderable objects in the base list
		/// </summary>
		/// <param name="context">Render context</param>
		public virtual void Render( IRenderContext context )
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
		public void AddedToScene( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		/// <summary>
		/// Detaches this object to the scene render list
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void RemovedFromScene(Scene scene)
		{
			scene.Renderables.Remove( this );
		}

		#endregion
	}
}
