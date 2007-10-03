using System;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// The state of the editor
	/// </summary>
	public class EditorState
	{
		#region Singleton

		/// <summary>
		/// Gets the singleton instance of this class
		/// </summary>
		public static EditorState Instance
		{
			get { return ms_Singleton; }
		}

		#endregion

		#region Public members

		/// <summary>
		/// Event, raised by <see cref="StartEditingScene"/>, when the editor opens up a new scene
		/// </summary>
		public event Action< SceneEditState > SceneEditStarted;

		/// <summary>
		/// Starts editing a given editor scene
		/// </summary>
		/// <param name="scene">Scene to edit</param>
		public void StartEditingScene( EditorScene scene )
		{
			m_CurrentSceneEditState = new SceneEditState( scene );
			if ( SceneEditStarted != null )
			{
				SceneEditStarted( m_CurrentSceneEditState );
			}
		}

		/// <summary>
		/// Shortcut to get the current scene
		/// </summary>
		public EditorScene CurrentScene
		{
			get { return m_CurrentSceneEditState.EditorScene; }
		}

		/// <summary>
		/// Shortcut to get the current runtime scene
		/// </summary>
		public Scene CurrentRuntimeScene
		{
			get { return CurrentScene.RuntimeScene; }
		}

		/// <summary>
		/// Shortcut to get the current selection set
		/// </summary>
		public SelectionSet CurrentSelection
		{
			get { return m_CurrentSceneEditState.SelectedObjects; }
		}

		/// <summary>
		/// Shortcut to get the current selection set
		/// </summary>
		public UndoStack CurrentUndoStack
		{
			get { return m_CurrentSceneEditState.UndoStack; }
		}

		/// <summary>
		/// Gets the edit state for the current scene
		/// </summary>
		public SceneEditState CurrentSceneEditState
		{
			get { return m_CurrentSceneEditState; }
		}

		/// <summary>
		/// Gets/sets the object editor builder
		/// </summary>
		public IObjectEditorBuilder ObjectEditorBuilder
		{
			set { m_Builder = value; }
			get { return m_Builder; }
		}

		#endregion

		#region Private stuff

		private IObjectEditorBuilder m_Builder = new ObjectEditorBuilder( );
		private SceneEditState m_CurrentSceneEditState;
		private static readonly EditorState ms_Singleton = new EditorState( );

		#endregion
	}
}
