using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	public class SceneEditState
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="scene">Editor scene</param>
		public SceneEditState( EditorScene scene )
		{
			m_EditorScene = scene;
		}

		/// <summary>
		/// Gets the current selection set
		/// </summary>
		public SelectionSet SelectedObjects
		{
			get { return m_Selection; }
		}

		/// <summary>
		/// Gets the scene being edited
		/// </summary>
		public EditorScene EditorScene
		{
			get { return m_EditorScene; }
		}

		/// <summary>
		/// Gets the runtime scene being edited
		/// </summary>
		public Scene RuntimeScene
		{
			get { return m_EditorScene.RuntimeScene; }
		}

		/// <summary>
		/// Gets the undo stack
		/// </summary>
		public UndoStack UndoStack
		{
			get { return m_UndoStack; }
		}


		#region Private members

		private readonly SelectionSet	m_Selection = new SelectionSet( );
		private readonly EditorScene	m_EditorScene;
		private readonly UndoStack		m_UndoStack = new UndoStack( );

		#endregion
	}
}
