using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// The edit state of the current scene
	/// </summary>
	public class SceneEditState
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="scene">Editor scene</param>
		public SceneEditState( Scene scene )
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
		public Scene EditorScene
		{
			get { return m_EditorScene; }
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
		private readonly Scene			m_EditorScene;
		private readonly UndoStack		m_UndoStack = new UndoStack( );

		#endregion
	}
}
