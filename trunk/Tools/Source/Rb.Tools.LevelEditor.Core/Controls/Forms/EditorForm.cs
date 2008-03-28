using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Common;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.ComponentXmlLoader;
using Rb.Core.Maths;
using Rb.Interaction;
using Rb.Log;
using Rb.Rendering.Base;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.Tools.LevelEditor.Core.Properties;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;


namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public abstract partial class EditorForm : Form
	{
		/// <summary>
		/// Gets the editor form instance
		/// </summary>
		public static EditorForm Instance
		{
			get { return ms_Instance; }
		}


		/// <summary>
		/// Sets up the form
		/// </summary>
		public EditorForm( )
		{
			System.Diagnostics.Trace.Assert( ms_Instance == null );
			ms_Instance = this;

			//	Create the log display - it will start caching log entries immediately
			m_LogDisplay = new Log.Controls.Vs.VsLogListView( );

			//	Write greeting
			AppLog.Info( "Beginning {0} at {1}", Assembly.GetCallingAssembly( ), DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			InitializeComponent( );

			EditorState.Instance.SceneOpened += OnSceneOpened;
			EditorState.Instance.SceneClosed += OnSceneClosed;

			SceneSerializer.Instance.LastSavePathChanged += SavePathChanged;
			SceneExporter.Instance.LastExportPathChanged += ExportPathChanged;

			UpdateInputsStatusLabel( );
			EditorState.Instance.EditModeActivated += EditModeAdded;
		}

		private readonly Control m_LogDisplay;

		protected virtual void InitializeDockingControls( )
		{
			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = display;
			m_DockingManager.OuterControl = statusStrip;

			//	Add log, property editor, and edit mode controls to the docking manager
			m_LogDisplayContent = m_DockingManager.Contents.Add( m_LogDisplay, "Log" );
			m_PropertyEditorContent = m_DockingManager.Contents.Add( new ObjectPropertyEditor( ), "Property Editor" );
			m_SelectionContent = m_DockingManager.Contents.Add( new SelectionControl( ), "Selection" );

			m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );
			m_DockingManager.AddContentWithState( m_SelectionContent, State.DockRight );
			m_DockingManager.AddContentWithState( m_PropertyEditorContent, State.DockLeft );
	
			m_EditModesContent = m_DockingManager.Contents.Add( new EditModesControl( ), Resources.EditModes );
			m_DockingManager.AddContentToZone( m_EditModesContent, m_SelectionContent.ParentWindowContent.ParentZone, 0 );

		}

		#region Properties

		/// <summary>
		/// Returns raycasting options used for standard selection
		/// </summary>
		public RayCastOptions SelectionPickOptions
		{
			get { return ms_PickOptions; }
		}

		/// <summary>
		/// Gets the log display content (docking object)
		/// </summary>
		public Content LogDisplayContent
		{
			get { return m_LogDisplayContent; }
		}

		/// <summary>
		/// Gets the property editor content (docking object)
		/// </summary>
		public Content PropertyEditorContent
		{
			get { return m_PropertyEditorContent; }
		}

		/// <summary>
		/// Gets the selection content (docking object)
		/// </summary>
		public Content SelectionContent
		{
			get { return m_SelectionContent; }
		}

		/// <summary>
		/// Gets the main command user (the user that sends command inputs to the main viewer)
		/// </summary>
		public CommandUser User
		{
			get { return m_User; }
		}

		/// <summary>
		/// Gets the path to the input template file
		/// </summary>
		protected virtual ISource InputTemplatePath
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the path to the viewer setup file
		/// </summary>
		protected virtual ISource ViewerSetupPath
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the docking manager
		/// </summary>
		public DockingManager DockingManager
		{
			get { return m_DockingManager; }
		}

		#endregion

		#region Event handlers

		/// <summary>
		/// Updates the text displayed in the inputs status label
		/// </summary>
		private void UpdateInputsStatusLabel( )
		{
			StringBuilder sb = new StringBuilder( );

			IEnumerator< IEditMode > modePos = EditorState.Instance.ActiveEditModes.GetEnumerator( );
			if ( modePos.MoveNext( ) )
			{
				sb.Append( modePos.Current.InputDescription );
				while ( modePos.MoveNext( ) )
				{
					sb.Append( ", " );
					sb.Append( modePos.Current.InputDescription );
				}
			}

			inputsStatusLabel.Text = sb.ToString( );
		}

		/// <summary>
		/// Called when an edit mode is added
		/// </summary>
		protected virtual void EditModeAdded( IEditMode editMode )
		{
			UpdateInputsStatusLabel( );
		}

		/// <summary>
		/// Called when the save path of the current scene has changed
		/// </summary>
		/// <param name="newPath">The new save path</param>
		protected virtual void SavePathChanged( string newPath )
		{
			Text = newPath + " => " + SceneExporter.Instance.LastExportPath;
		}

		/// <summary>
		/// Called when the export path of the current scene has changed
		/// </summary>
		/// <param name="newPath">The new export path</param>
		protected virtual void ExportPathChanged( string newPath )
		{
			Text = SceneSerializer.Instance.LastSavePath + " => " + newPath;
		}

		/// <summary>
		/// Called when a scene is opened for editing
		/// </summary>
		/// <param name="state">Edit state of the new scene</param>
		protected virtual void OnSceneOpened( SceneEditState state )
		{
			state.UndoStack.ActionAdded += UndoStackChanged;
			state.UndoStack.ActionUndone += UndoStackChanged;
			state.UndoStack.ActionRedone += UndoStackChanged;

			//	Get all viewers to show the new scene
			foreach ( Viewer viewer in display.Viewers )
			{
				viewer.Renderable = state.EditorScene;
			}
		}

		/// <summary>
		/// Called prior to a scene being closed for editing
		/// </summary>
		/// <param name="state">Edit state of the closing scene</param>
		protected virtual void OnSceneClosed( SceneEditState state )
		{
			state.UndoStack.ActionAdded -= UndoStackChanged;
			state.UndoStack.ActionUndone -= UndoStackChanged;
			state.UndoStack.ActionRedone -= UndoStackChanged;

			//	Get all viewers to stop showing stuff
			foreach ( Viewer viewer in display.Viewers )
			{
				viewer.Renderable = null;
			}
		}

		/// <summary>
		/// Called when the undo stack changes
		/// </summary>
		/// <param name="action">Action that has been undone/redone</param>
		private void UndoStackChanged( IAction action )
		{
			undoToolStripMenuItem.Enabled = EditorState.Instance.CurrentUndoStack.CanUndo;
			redoToolStripMenuItem.Enabled = EditorState.Instance.CurrentUndoStack.CanRedo;
		}

		#endregion

		#region Public members

		/// <summary>
		/// Creates a new scene, populates it, and opens it up for editing
		/// </summary>
		public void NewScene( )
		{
			try
			{
				Scene scene = CreateNewScene( );
				EditorState.Instance.OpenScene( scene );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to create new scene" );
				ErrorMessageBox.Show( this, Resources.EditorSceneCreationFailed );
			}
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Creates a new scene
		/// </summary>
		protected abstract Scene CreateNewScene( );

		/// <summary>
		/// Creates a new runtime scene
		/// </summary>
		protected abstract Scene CreateNewRuntimeScene( );
		
		/// <summary>
		/// Builds a runtime scene from the contents of an editor scene
		/// </summary>
		protected static void BuildRuntimeScene( Scene editorScene, Scene runtimeScene )
		{
			IEnumerable< IObjectEditor > editors = editorScene.Objects.GetAllOfType< IObjectEditor >( );
			foreach ( IObjectEditor editor in editors )
			{
				try
				{
					editor.Build( runtimeScene );
				}
				catch ( Exception ex )
				{
					throw new ApplicationException( "Failed to populate runtime scene", ex );	
				}
			}

		}

		/// <summary>
		/// Called to create the default main viewer, if CreateMainViewer() fails
		/// </summary>
		/// <returns>Returns a default main viewer</returns>
		protected virtual Viewer CreateDefaultMainViewer( )
		{
			return new Viewer( );
		}
		
		/// <summary>
		/// Called from the form Load handler. Creates the main viewer, loading it from the source specified by ViewerSetupPath
		/// </summary>
		/// <returns>Return the main viewer object</returns>
		protected virtual Viewer CreateMainViewer( )
		{
			Viewer viewer = null;
			ISource viewerSource = ViewerSetupPath;
			if ( viewerSource != null )
			{
				try
				{
					//	Load viewer
					ComponentLoadParameters loadParams = new ComponentLoadParameters( );
					loadParams.Properties[ "User" ] = m_User;
					viewer = ( Viewer )AssetManager.Instance.Load( viewerSource, loadParams );
				}
				catch ( Exception ex )
				{
					AppLog.Exception( ex, "Failed to load viewer from \"{0}\"", viewerSource );
				}
			}
			if ( viewer == null )
			{
				AppLog.Info( "Creating default viewer" );
				viewer = new Viewer( );
			}
			return viewer;
		}

		/// <summary>
		/// Called from the form Load handler. Creates an input template map from InputTemplatePath
		/// </summary>
		/// <returns>Returns the input template map for the main viewer (null if there are no controls)</returns>
		protected virtual CommandInputTemplateMap CreateMainViewerControls( )
		{
			CommandInputTemplateMap editorMap = null;
			ISource editorMapSource = InputTemplatePath;
			try
			{
				if ( editorMapSource != null )
				{
					editorMap = ( CommandInputTemplateMap )AssetManager.Instance.Load( editorMapSource );
				}
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to load editor control inputs from \"{0}\"", editorMapSource );
			}
			return editorMap;
		}

		#endregion

		#region Private fields

		private readonly CommandUser			m_User = new CommandUser( );

		private DockingManager					m_DockingManager;
		private Content							m_LogDisplayContent;
		private Content							m_PropertyEditorContent;
		private Content							m_SelectionContent;
		private Content							m_EditModesContent;

		private static readonly RayCastOptions	ms_PickOptions = new RayCastOptions( );

		private static EditorForm				ms_Instance;

		#endregion

		#region Form event handlers

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( DesignMode )
			{
				return;
			}

			InitializeDockingControls( );

			EditorState.Instance.AddEditModeControl( display );

			Viewer viewer = CreateMainViewer( );

			if ( viewer != null )
			{
				display.AddViewer( viewer );

				CommandInputTemplateMap editorMap = CreateMainViewerControls( );
				if ( editorMap != null )
				{
					editorMap.BindToInput( new InputContext( viewer ), m_User );
				}
			}

			//	Create a new scene
			NewScene( );
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void logToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_DockingManager.ShowContent( m_LogDisplayContent );
		}

		private void selectionToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_DockingManager.ShowContent( m_SelectionContent );
		}

		private void objectPropertiesToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_DockingManager.ShowContent( m_PropertyEditorContent );
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SceneSerializer.Instance.SaveAs( EditorState.Instance.CurrentScene );
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SceneSerializer.Instance.Save( EditorState.Instance.CurrentScene );
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewScene( );
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Scene scene = SceneSerializer.Instance.Open( );
			if ( scene != null )
			{
				EditorState.Instance.OpenScene( scene );
			}
		}

		private void undoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditorState.Instance.CurrentUndoStack.Undo( );
		}

		private void redoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditorState.Instance.CurrentUndoStack.Redo( );
		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Scene runtimeScene;
			try
			{
				runtimeScene = CreateNewRuntimeScene( );
				BuildRuntimeScene( EditorState.Instance.CurrentScene, runtimeScene );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to build runtime scene" );
				ErrorMessageBox.Show( this, Resources.BuildSceneFailed );
				return;
			}
			SceneExporter.Instance.Export( runtimeScene );
		}

		private void exportAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Scene runtimeScene = CreateNewRuntimeScene( );
			SceneExporter.Instance.ExportAs( runtimeScene );
		}

		private void display_MouseMove( object sender, MouseEventArgs e )
		{
			//	Display the world position that the cursor is at in the status bar
			ILineIntersection pick = display.FirstPick( e.X, e.Y, ms_PickOptions );
			if ( pick != null )
			{
				posStatusLabel.Text = pick.ToString( );
			}
		}

		private void runGameToolStripMenuItem_Click( object sender, EventArgs e )
		{
		}

		private void sceneServicesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ServicesEditorForm servicesForm = new ServicesEditorForm();
			servicesForm.Show( this );
		}

		#endregion

	}
}