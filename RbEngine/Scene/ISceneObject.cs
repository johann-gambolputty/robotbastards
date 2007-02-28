using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Interface for objects that know about the scene that they have been added to
	/// </summary>
	public interface ISceneObject
	{
		/// <summary>
		/// Invoked when this object is added to a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was added to</param>
		void	AddedToScene( SceneDb db );

		/// <summary>
		/// Invoked when this object is removed from a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was removed from</param>
		void	RemovedFromScene( SceneDb db );
	}
}
