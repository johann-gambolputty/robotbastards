using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Manages lighting in a scene
	/// </summary>
	public class LightingManager : ISceneObject
	{
		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void			AddedToScene( SceneDb db )
		{
			m_Scene = db;
			m_Scene.ObjectAddedToScene += new Components.ChildAddedDelegate( OnObjectAddedToScene );
		}

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void			RemovedFromScene( SceneDb db )
		{
			m_Scene = null;
		}

		#endregion

		private void		OnObjectAddedToScene( Object parentObject, Object childObject )
		{
		}

		#region	Private stuff

		private ArrayList	m_LightingGroups = new ArrayList( );
		private SceneDb		m_Scene;

		#endregion
	}
}
