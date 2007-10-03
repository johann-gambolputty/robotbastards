using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.EditModes;
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

			AddEditMode( new SelectEditMode( MouseButtons.Left ) );
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

		/// <summary>
		/// Adds an edit mode. If the mode is exclusive, then it replaces the current exclusive mode
		/// </summary>
		/// <param name="mode">Mode to add</param>
		public void AddEditMode( IEditMode mode )
		{
			if ( mode.Exclusive )
			{
				if ( m_ExclusiveMode != null )
				{
					m_ExclusiveMode.Stop( );
				}
				m_ExclusiveMode = mode;
				mode.Start( );
			}
			else if ( !m_SharedModes.Contains( mode ) )
			{
				m_SharedModes.Add( mode );
				mode.Start( );
			}
		}

		/// <summary>
		/// Gets the set of controls registered with this object
		/// </summary>
		/// <remarks>
		/// These control are bound to by new edit modes
		/// </remarks>
		public List< Control > EditModeControls
		{
			get { return m_EditModeControls; }
		}

		#region Private members

		private readonly List< Control >	m_EditModeControls = new List< Control >( );
		private readonly SelectionSet		m_Selection = new SelectionSet( );
		private readonly EditorScene		m_EditorScene;
		private readonly UndoStack			m_UndoStack = new UndoStack( );
		private IEditMode					m_ExclusiveMode;
		private readonly List< IEditMode >	m_SharedModes = new List< IEditMode >( );

		#endregion
	}
}
