using System;
using Rb.Core.Components;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	class AddObjectAction : IAction
	{
		/// <summary>
		/// Action setup constructor
		/// </summary>
		public AddObjectAction( object template, PickInfoCursor pick, Guid id )
		{
			m_Id = id;
			m_Instance = EditorState.Instance.ObjectEditorBuilder.Create( pick, CreateInstance( template ) );

			Redo( );
		}

		#region IAction Members

		/// <summary>
		/// Undoes this action, by removing the object edit state from the level editor scene, and the actual object
		/// from the runtime scene
		/// </summary>
		public void Undo( )
		{
			EditorState.Instance.CurrentScene.Objects.Remove( m_Id );
			EditorState.Instance.CurrentRuntimeScene.Objects.Remove( m_Id );
		}

		/// <summary>
		/// Redoes this action, by adding the object edit state to the level editor scene, and the actual object
		/// to the runtime scene
		/// </summary>
		public void Redo( )
		{
			EditorState.Instance.CurrentScene.Objects.Add( m_Id, m_Instance );
			EditorState.Instance.CurrentRuntimeScene.Objects.Add( m_Id, m_Instance.Instance );
		}

		#endregion

		#region Private members

		private readonly Guid m_Id;
		private readonly IObjectEditor m_Instance;
		
		private static object CreateInstance( object template )
		{
			if ( template is IInstanceBuilder )
			{
				return ( ( IInstanceBuilder )template ).CreateInstance( EditorState.Instance.CurrentRuntimeScene.Builder );
			}

			if ( template is ICloneable )
			{
				return ( ( ICloneable )template ).Clone( );
			}

			throw new ArgumentException( "Template must implement ICloneable or IInstanceBuilder", "template" );
		}

		#endregion
	}
}
