using System;

namespace RbEngine.Scene
{
	/// <summary>
	/// Summary description for SceneComponentFactory.
	/// </summary>
	public class SceneComponentFactory : Components.IBuilder
	{
		/// <summary>
		/// Sets the scene used by this factory
		/// </summary>
		public SceneComponentFactory( Scene.SceneDb scene )
		{
			m_Scene = scene;
		}

		#region IBuilder Members

		/// <summary>
		/// Creates a new object of the specified type
		/// </summary>
		public object	Create( Type type )
		{
			//	Create the object using the activator
			object newObject = System.Activator.CreateInstance( type );
			return Build( newObject );
		}

		/// <summary>
		/// Builds an object
		/// </summary>
		public object	Build( object obj )
		{
			//	If the new object is an ISceneObject, notify it
			Scene.ISceneObject	sceneObject	= obj as Scene.ISceneObject;
			if ( sceneObject != null )
			{
				sceneObject.AddedToScene( m_Scene );
			}

			return sceneObject;
		}

		#endregion

		private Scene.SceneDb	m_Scene;
	}
}
