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
		/// Raised after an edit mode is registered (<see cref="ActivateEditMode"/>)
		/// </summary>
		public event Action<IEditMode> EditModeRegistered;

		/// <summary>
		/// Raised after an edit mode is activated (<see cref="ActivateEditMode"/>)
		/// </summary>
		public event Action< IEditMode > EditModeActivated;


		/// <summary>
		/// Gets the list of registered edit modes
		/// </summary>
		public IEnumerable< IEditMode > RegisteredEditModes
		{
			get { return m_RegisteredModes; }
		}


		/// <summary>
		/// Returns an enumerator to the current set edit modes
		/// </summary>
		public IEnumerable< IEditMode > ActiveEditModes
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
		/// Registers an edit mode. This does nothing more that put it in the registered edit mode list
		///  (<see cref="RegisteredEditModes"/>), and call the register event (<see cref="EditModeRegistered"/>)
		/// </summary>
		/// <param name="mode">Edit mode to register</param>
		/// <remarks>
		/// This method will not register the same mode twice (will return without exception)
		/// </remarks>
		public void RegisterEditMode( IEditMode mode )
		{
			if ( m_RegisteredModes.Contains( mode ) )
			{
				return;
			}
			m_RegisteredModes.Add( mode );
			if ( EditModeRegistered != null )
			{
				EditModeRegistered( mode );
			}
		}

		/// <summary>
		/// Adds an edit mode. If the mode is exclusive, then it replaces the current exclusive mode
		/// </summary>
		/// <param name="mode">Mode to add</param>
		public void ActivateEditMode( IEditMode mode )
		{
			//	Make sure that the mode is registered...
			RegisterEditMode( mode );

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

			if ( EditModeActivated != null )
			{
				EditModeActivated( mode );
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
		public void OpenScene( Scene scene )
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
		public Scene CurrentScene
		{
			get { return m_CurrentSceneEditState == null ? null : m_CurrentSceneEditState.EditorScene; }
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

		#endregion

		#region Private stuff

		private SceneEditState				m_CurrentSceneEditState;
		private static readonly EditorState	ms_Singleton = new EditorState( );
		private readonly List< Control >	m_EditModeControls = new List< Control >( );
		private IEditMode					m_ExclusiveMode;
		private readonly List< IEditMode >	m_SharedModes = new List< IEditMode >( );
		private readonly List< IEditMode >	m_RegisteredModes = new List< IEditMode >( );

		#endregion
	}
}
