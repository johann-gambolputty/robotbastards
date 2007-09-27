using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.Actions;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Log;
using Rb.Rendering;
using Crownwood.Magic.Common;
using Graphics=Rb.Rendering.Graphics;
using Rectangle=System.Drawing.Rectangle;

namespace Poc0.LevelEditor
{
	public partial class MainForm : Form
	{
		private readonly DockingManager m_DockingManager;

		private readonly Content m_LogDisplayContent;
		private readonly Content m_ToolBoxContent;
		private readonly Content m_PropertyEditorContent;
		private readonly Content m_SelectionContent;
		private readonly Content m_GameViewContent;


		public MainForm( )
		{
			LogForm logDisplay = new LogForm( );

			//	Write greeting
			AppLog.Info( "Beginning Poc0.LevelEditor at {0}", DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			//	Load the rendering assembly
			string renderAssemblyName = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				renderAssemblyName = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssemblyName );
			
			//	Load resource settings
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

			CreateEditContext( );

			//	Create the docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = tileGrid2dDisplay;
			m_DockingManager.OuterControl = statusStrip;

			//	Add log, property editor views to the docking manager
			m_LogDisplayContent = m_DockingManager.Contents.Add( logDisplay, "Log" );
			m_ToolBoxContent = m_DockingManager.Contents.Add( new EditorControls( ), "Tool Box" );
			m_PropertyEditorContent = m_DockingManager.Contents.Add( new ObjectPropertyEditor( ), "Property Editor" );
			m_SelectionContent = m_DockingManager.Contents.Add( new SelectionControl( ), "Selection" );
			m_GameViewContent = m_DockingManager.Contents.Add( new GameViewControl( ), "Game View" );

			m_DockingManager.AddContentWithState( m_GameViewContent, State.Floating );
			WindowContent logWindow = m_DockingManager.AddContentWithState( m_LogDisplayContent, State.DockBottom );
			WindowContent selectionWindow = m_DockingManager.AddContentWithState( m_SelectionContent, State.DockRight );
			m_DockingManager.AddContentToZone( m_ToolBoxContent, selectionWindow.ParentZone, 0 );
			m_DockingManager.AddContentToZone( m_GameViewContent, logWindow.ParentZone, 0 );
			m_DockingManager.AddContentWithState( m_PropertyEditorContent, State.DockLeft );

			m_Serializer.LastSavePathChanged += SavePathChanged;
			m_Exporter.LastExportPathChanged += ExportPathChanged;

			Icon = Properties.Resources.AppIcon;
		}

		private void SavePathChanged( string newPath )
		{
			Text = newPath + " => " + m_Exporter.LastExportPath;
		}

		private void ExportPathChanged( string newPath )
		{
			Text = m_Serializer.LastSavePath + " => " + newPath;
		}

		private void CreateEditContext( )
		{
			//	Create the tile grid edit state
			EditModeContext editContext = EditModeContext.CreateNewContext( );
			editContext.AddEditControl( tileGrid2dDisplay );
			editContext.AddEditMode( new SelectEditMode( MouseButtons.Left ) );

			editContext.UndoStack.ActionAdded += UndoStackChanged;
			editContext.UndoStack.ActionUndone += UndoStackChanged;
			editContext.UndoStack.ActionRedone += UndoStackChanged;

		}

		private void UndoStackChanged( IAction action )
		{
			undoToolStripMenuItem.Enabled = EditModeContext.Instance.UndoStack.CanUndo;
			redoToolStripMenuItem.Enabled = EditModeContext.Instance.UndoStack.CanRedo;
		}

		/// <summary>
		/// Creates a new scene, including tile grid, renderer and edit state
		/// </summary>
		private void CreateNewScene( TileTypeSet tileTypes, int width, int height)
		{
			EditorScene scene = new EditorScene( );

			//	Create the tile grid
			TileGrid grid = new TileGrid( tileTypes );
			grid.SetDimensions( width, height );

			//	Add a renderer for the tile grid to the scene renderables
			scene.Objects.Add( Guid.NewGuid( ), grid );

			scene.Renderables.Add( Graphics.Factory.Create< TileBlock2dRenderer >( grid, EditModeContext.Instance ) );

			scene.LevelGeometry = new LevelGeometry( scene );

			Scene = scene;
		}

		/// <summary>
		/// Access to the main scene
		/// </summary>
		public EditorScene Scene
		{
			get { return m_Scene; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value", "Scene cannot be null" );
				}

				m_Scene = value;

				//	Force all viewers to show the new scene
				foreach ( Viewer viewer in tileGrid2dDisplay.Viewers )
				{
					viewer.Renderable = m_Scene;
				}
				( ( GameViewControl )m_GameViewContent.Control ).GameScene = m_Scene.RuntimeScene;

				//	Set the scene in the edit context
				EditModeContext.Instance.Setup( m_Scene );
			}
		}

		private readonly CommandUser		m_User = new CommandUser( );
		private readonly SceneSerializer	m_Serializer = new SceneSerializer( );
		private readonly SceneExporter		m_Exporter = new SceneExporter( );
		private EditorScene					m_Scene;
		
		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( !DesignMode )
			{
				CommandInputTemplateMap editorMap = ( CommandInputTemplateMap )AssetManager.Instance.Load( "Editor/LevelEditorCommandInputs.components.xml" );

				//	Load input bindings
				ComponentLoadParameters loadParams = new ComponentLoadParameters( );
				loadParams.Properties[ "User" ] = m_User;

				//	Load in the scene viewer
				Viewer viewer = ( Viewer )AssetManager.Instance.Load( "Editor/LevelEditorStandardViewer.components.xml", loadParams );
				viewer.Control = tileGrid2dDisplay;
				tileGrid2dDisplay.AddViewer( viewer );

				//	Test load a command list
				try
				{
					editorMap.BindToInput( new InputContext( viewer ), m_User );
				}
				catch ( Exception ex )
				{
					ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
				}

				//	Create a new scene
				CreateNewScene( TileTypeSet.CreateDefaultTileTypeSet( ), 16, 16 );

				( ( GameViewControl )m_GameViewContent.Control ).GameScene = m_Scene.RuntimeScene;
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

		private void toolBoxToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_DockingManager.ShowContent( m_ToolBoxContent );
		}

		private void objectPropertiesToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_DockingManager.ShowContent( m_PropertyEditorContent );
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_Serializer.SaveAs( m_Scene );
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			m_Serializer.Save( m_Scene );
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditorScene scene = m_Serializer.Open( );
			if ( scene != null )
			{
				Scene = scene;
			}
		}

		private void undoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditModeContext.Instance.UndoStack.Undo( );
		}

		private void redoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			EditModeContext.Instance.UndoStack.Redo( );
		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_Exporter.Export( m_Scene.RuntimeScene );
		}

		private void eToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_Exporter.ExportAs( m_Scene.RuntimeScene );
		}

		private void tileGrid2dDisplay_MouseMove( object sender, MouseEventArgs e )
		{
			//	Display the world position that the cursor is at in the status bar
			foreach ( Viewer viewer in tileGrid2dDisplay.Viewers )
			{
				Rectangle winRect = viewer.GetWindowRectangle( tileGrid2dDisplay.Bounds );
				if ( winRect.Contains( e.Location ) )
				{
					ITilePicker picker = viewer.Camera as ITilePicker;
					if ( picker == null )
					{
						continue;
					}
					Point2 pt = picker.CursorToWorld( e.Location.X, e.Location.Y );
					
					string ptString = string.Format( "X: {0}, Z: {1}", pt.X, pt.Y );

					posStatusLabel.Text = ptString;

					return;
				}
			}
			//	No valid viewer found
			posStatusLabel.Text = "";
		}

	}
}