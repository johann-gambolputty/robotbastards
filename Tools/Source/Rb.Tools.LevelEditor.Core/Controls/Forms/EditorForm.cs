using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Common;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Log;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;
using Rectangle=System.Drawing.Rectangle;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	public partial class EditorForm : Form
	{
		/// <summary>
		/// Sets up the form
		/// </summary>
		public EditorForm( )
		{
			EditorState.Instance.AddEditMode( new SelectEditMode( MouseButtons.Left ) );
			EditorState.Instance.SceneOpened += OnSceneOpened;
			EditorState.Instance.SceneClosed += OnSceneClosed;

			//	Create the log display - it will start caching log entries immediately
			Control logDisplay = new Log.Controls.Vs.VsLogListView( );

			//	Write greeting
			AppLog.Info( "Beginning {0} at {1}", Assembly.GetCallingAssembly( ), DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			//	Load the rendering assembly
			string renderAssemblyName = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				renderAssemblyName = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssemblyName );
			
			//	Load asset setup
			string assetSetupPath = ConfigurationManager.AppSettings[ "assetSetupPath" ];
			if ( assetSetupPath == null )
			{
				assetSetupPath = "../assetSetup.xml";
			}
			AssetXmlSetup.Setup( assetSetupPath, AssetManager.Instance, LocationManagers.Instance );

			//	Load all assemblies that support the chosen graphics API
			Rb.AssemblySelector.IdentifierMap.Instance.AddAssemblyIdentifiers( Directory.GetCurrentDirectory( ), SearchOption.TopDirectoryOnly );
			Rb.AssemblySelector.IdentifierMap.Instance.LoadAll( "GraphicsApi=" + Graphics.Factory.ApiName );

			InitializeComponent( );

			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = display;
			m_DockingManager.OuterControl = statusStrip;

			//	Add log, property editor views to the docking manager
			m_LogDisplayContent = m_DockingManager.Contents.Add( logDisplay, "Log" );
			m_PropertyEditorContent = m_DockingManager.Contents.Add( new ObjectPropertyEditor( ), "Property Editor" );
			m_SelectionContent = m_DockingManager.Contents.Add( new SelectionControl( ), "Selection" );

			m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );
			m_DockingManager.AddContentWithState( m_SelectionContent, State.DockRight );
			m_DockingManager.AddContentWithState( m_PropertyEditorContent, State.DockLeft );

			m_Serializer.LastSavePathChanged += SavePathChanged;
			m_Exporter.LastExportPathChanged += ExportPathChanged;
		}

		#region Properties

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

		/// <summary>
		/// Gets the current scene serializer
		/// </summary>
		public SceneSerializer Serializer
		{
			get { return m_Serializer; }
		}

		/// <summary>
		/// Gets the current scene exporter
		/// </summary>
		public SceneExporter Exporter
		{
			get { return m_Exporter; }
		}

		#endregion

		#region Event handlers

		/// <summary>
		/// Called when the save path of the current scene has changed
		/// </summary>
		/// <param name="newPath">The new save path</param>
		protected virtual void SavePathChanged( string newPath )
		{
			Text = newPath + " => " + m_Exporter.LastExportPath;
		}

		/// <summary>
		/// Called when the export path of the current scene has changed
		/// </summary>
		/// <param name="newPath">The new export path</param>
		protected virtual void ExportPathChanged( string newPath )
		{
			Text = m_Serializer.LastSavePath + " => " + newPath;
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

		#region Public methods

		/// <summary>
		/// Creates a new scene, populates it, and opens it up for editing
		/// </summary>
		public void NewScene( )
		{
			EditorScene scene = CreateNewScene( );
			PopulateNewScene( scene );
			EditorState.Instance.OpenScene( scene );
		}

		/// <summary>
		/// Creates a new scene
		/// </summary>
		protected virtual EditorScene CreateNewScene( )
		{
			EditorScene newScene = new EditorScene( new Scene( ) );
			return newScene;
		}

		/// <summary>
		/// Populates a scene created by CreateNewScene()
		/// </summary>
		/// <param name="scene">Scene to populate</param>
		protected virtual void PopulateNewScene( EditorScene scene )
		{
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
					AppLog.Error( "Failed to load viewer from \"{0}\"", viewerSource );
					ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
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
				AppLog.Error( "Failed to load editor control inputs from \"{0}\"", editorMapSource );
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
			}
			return editorMap;
		}

		#endregion

		#region Private fields

		private readonly CommandUser		m_User = new CommandUser( );
		private readonly SceneSerializer	m_Serializer = new SceneSerializer( );
		private readonly SceneExporter		m_Exporter = new SceneExporter( );
		private readonly DockingManager		m_DockingManager;

		private readonly Content 			m_LogDisplayContent;
		private readonly Content 			m_PropertyEditorContent;
		private readonly Content 			m_SelectionContent;

		private static readonly RayCastOptions ms_PickOptions = new RayCastOptions( );

		#endregion

		#region Form event handlers

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( !DesignMode )
			{
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
			m_Serializer.SaveAs( EditorState.Instance.CurrentScene );
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_Serializer.Save( EditorState.Instance.CurrentScene );
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditorScene scene = m_Serializer.Open( );
			if ( scene != null )
			{
				EditorState.Instance.OpenScene( scene );
			}
		}

		private static void undoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditorState.Instance.CurrentUndoStack.Undo( );
		}

		private static void redoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditorState.Instance.CurrentUndoStack.Redo( );
		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_Exporter.Export( EditorState.Instance.CurrentRuntimeScene );
		}

		private void exportAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_Exporter.ExportAs( EditorState.Instance.CurrentRuntimeScene );
		}

		private void display_MouseMove( object sender, MouseEventArgs e )
		{
			//	Display the world position that the cursor is at in the status bar
			ILineIntersection pick = display.FirstPick( e.X, e.Y, ms_PickOptions );
			if ( pick != null )
			{
				posStatusLabel.Text = pick.ToString( );
			}
			else
			{
				posStatusLabel.Text = "";
			}
		}

		#endregion

	}
}