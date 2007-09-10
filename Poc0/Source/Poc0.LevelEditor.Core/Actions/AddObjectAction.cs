using System;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Components;
using Rb.World;

namespace Poc0.LevelEditor.Core.Actions
{
	class AddObjectAction : IAction
	{
		/// <summary>
		/// Action setup constructor
		/// </summary>
		public AddObjectAction( EditorScene scene, object template, float x, float y, Guid id )
		{
			m_Id = id;
			m_Scene = scene;
			m_Instance = new ObjectEditState( scene, x, y, CreateInstance( template ) );

			Redo( );
		}

		#region IAction Members

		/// <summary>
		/// Undoes this action, by removing the object edit state from the level editor scene, and the actual object
		/// from the runtime scene
		/// </summary>
		public void Undo( )
		{
			m_Scene.Objects.Remove( m_Id );
			m_Scene.RuntimeScene.Objects.Remove( m_Id );
		}

		/// <summary>
		/// Redoes this action, by adding the object edit state to the level editor scene, and the actual object
		/// to the runtime scene
		/// </summary>
		public void Redo( )
		{
			m_Scene.Objects.Add( m_Id, m_Instance );
			m_Scene.RuntimeScene.Objects.Add( m_Id, m_Instance.Instance );
		}

		#endregion

		#region Private members

		private readonly EditorScene m_Scene;
		private readonly Guid m_Id;
		private readonly ObjectEditState m_Instance;
		
		private static object CreateInstance( object template )
		{
			//return ( ( ICloneable )template ).Clone( );
			return ( ( IInstanceBuilder )template ).CreateInstance( EditModeContext.Instance.RuntimeScene.Builder );

		}

		#endregion
	}
}
