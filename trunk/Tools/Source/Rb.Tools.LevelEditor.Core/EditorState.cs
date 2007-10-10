using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.EditModes;
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

		#region Controls

		/// <summary>
		/// Called when a control is added to the control list
		/// </summary>
		public event Action< Control > ControlAdded;
		
		/// <summary>
		/// Adds a control to the list of edit mode controls
		/// </summary>
		/// <param name="control">Control to add</param>
		public void AddEditModeControl( Control control )
		{
			m_EditModeControls.Add( control );
			if ( ControlAdded != null )
			{
				ControlAdded( control );
			}
		}
		
		/// <summary>
		/// Gets the set of controls registered with this object
		/// </summary>
		/// <remarks>
		/// These control are bound to by new edit modes
		/// </remarks>
		public IEnumerable< Control > EditModeControls
		{
			get { return m_EditModeControls; }
		}

		#endregion

		#region Edit modes

		/// <summary>
		/// Raised after an edit mode is added
		/// </summary>
		public event EventHandler EditModeAdded;

		/// <summary>
		/// Returns an enumerator to the current set edit modes
		/// </summary>
		public IEnumerable< IEditMode > EditModes
		{
			get
			{
				foreach ( IEditMode sharedMode in m_SharedModes )
				{
					yield return sharedMode;
				}
				if ( m_ExclusiveMode != null )
				{
					yield return m_ExclusiveMode;
				}
			}
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
					m_ExclusiveMode.Stopped -= ExclusiveModeStopped;
				}

				//	Stop any shared modes that have the same inputs
				foreach ( IEditMode sharedMode in m_SharedModes )
				{
					if ( ( sharedMode.Buttons & mode.Buttons ) != 0 )
					{
						//	Buttons used by both modes - they can't co-exist, so stop the shared mode
						sharedMode.Stop( );
					}
				}

				m_ExclusiveMode = mode;
				mode.Start( );
				mode.Stopped += ExclusiveModeStopped;
			}
			else if ( !m_SharedModes.Contains( mode ) )
			{
				m_SharedModes.Add( mode );
				mode.Start( );
			}

			if ( EditModeAdded != null )
			{
				EditModeAdded( this, null );
			}
		}

		/// <summary>
		/// Called when an exclusive mode is stopped. Restarts all shared modes
		/// </summary>
		private void ExclusiveModeStopped( object sender, EventArgs args )
		{
			foreach ( IEditMode sharedMode in m_SharedModes )
			{
				if ( !sharedMode.IsRunning )
				{
					sharedMode.Start( );
				}
			}
		}

		#endregion

		#region Current scene

		/// <summary>
		/// Event, raised by <see cref="OpenScene"/>, when the editor opens up a new scene
		/// </summary>
		public event Action< SceneEditState > SceneOpened;

		/// <summary>
		/// Event, raised by <see cref="OpenScene"/>, when the editor closes the old scene
		/// </summary>
		public event Action< SceneEditState > SceneClosed;

		/// <summary>
		/// Starts editing a given editor scene
		/// </summary>
		/// <param name="scene">Scene to edit</param>
		public void OpenScene( EditorScene scene )
		{
			if ( ( m_CurrentSceneEditState != null ) && ( SceneClosed != null ) )
			{
				SceneClosed( m_CurrentSceneEditState );
			}
			m_CurrentSceneEditState = new SceneEditState( scene );
			if ( SceneOpened != null )
			{
				SceneOpened( m_CurrentSceneEditState );
			}
		}
		
		/// <summary>
		/// Shortcut to get the current scene
		/// </summary>
		public EditorScene CurrentScene
		{
			get { return m_CurrentSceneEditState == null ? null : m_CurrentSceneEditState.EditorScene; }
		}

		/// <summary>
		/// Shortcut to get the current runtime scene
		/// </summary>
		public Scene CurrentRuntimeScene
		{
			get { return CurrentScene == null ? null : CurrentScene.RuntimeScene; }
		}

		/// <summary>
		/// Shortcut to get the current selection set
		/// </summary>
		public SelectionSet CurrentSelection
		{
			get { return m_CurrentSceneEditState == null ? null : m_CurrentSceneEditState.SelectedObjects; }
		}

		/// <summary>
		/// Shortcut to get the current selection set
		/// </summary>
		public UndoStack CurrentUndoStack
		{
			get { return m_CurrentSceneEditState == null ? null : m_CurrentSceneEditState.UndoStack; }
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

		private IObjectEditorBuilder		m_Builder = new ObjectEditorBuilder( );
		private SceneEditState				m_CurrentSceneEditState;
		private static readonly EditorState	ms_Singleton = new EditorState( );
		private readonly List< Control >	m_EditModeControls = new List< Control >( );
		private IEditMode					m_ExclusiveMode;
		private readonly List< IEditMode >	m_SharedModes = new List< IEditMode >( );

		#endregion
	}
}
