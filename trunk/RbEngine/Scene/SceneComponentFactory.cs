using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Summary description for SceneComponentFactory.
	/// </summary>
	public class SceneComponentFactory : Components.IComponentFactory
	{
		/// <summary>
		/// Sets the scene used by this factory
		/// </summary>
		public SceneComponentFactory( Scene.SceneDb scene )
		{
			m_Scene = scene;
		}

		#region IComponentFactory Members

		/// <summary>
		/// Creates a new object of the specified type
		/// </summary>
		public object Create( Type type )
		{
			//	Create the object using the activator
			object newObject = System.Activator.CreateInstance( type );

			//	If the new object is an ISceneObject, notify it
			Scene.ISceneObject	newSceneObject	= newObject as Scene.ISceneObject;
			if ( newSceneObject != null )
			{
				newSceneObject.AddedToScene( m_Scene );
			}

			return newObject;
		}

		#endregion

		private Scene.SceneDb	m_Scene;
	}
}
