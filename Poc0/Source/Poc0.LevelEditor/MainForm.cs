using System;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.EditModes;
using Poc0.LevelEditor.Properties;
using Rb.Core.Assets;
using Rb.Log;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Controls.Forms;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.World;

namespace Poc0.LevelEditor
{
	public partial class MainForm : EditorForm
	{
		public MainForm( )
		{
			InitializeComponent( );

			EditorState.Instance.ActivateEditMode( new SelectEditMode( System.Windows.Forms.MouseButtons.Left, new RayCastOptions( RayCastLayers.Entity | RayCastLayers.StaticGeometry ) ) );

			EditorState.Instance.RegisterEditMode( new ObjectEditMode( SelectionPickOptions ) );
			EditorState.Instance.RegisterEditMode( new LevelGeometryEditMode( ) );
		}

		#region Protected members

		//protected override void InitializeDockingControls( )
		//{
		//    base.InitializeDockingControls( );

		//    //	NOTE: AP: Removed game view for now - it was screwing up pick ray calculation in Camera3 (game view viewport used occasionally)
		//    //m_GameViewContent = DockingManager.Contents.Add( new GameViewControl( ), Properties.Resources.EditorToolbox );
		//    //DockingManager.AddContentToZone( m_GameViewContent, LogDisplayContent.ParentWindowContent.ParentZone, 0 );
		//}
		
		/// <summary>
		/// Gets the location of the standard level editor viewer
		/// </summary>
		protected override ISource ViewerSetupPath
		{
			get
			{
				return new Location( "Editor/LevelEditorStandardViewer.components.xml" );
			}
		}

		/// <summary>
		/// Gets the location of the standard level editor viewer
		/// </summary>
		protected override ISource InputTemplatePath
		{
			get
			{
				return new Location( "Editor/LevelEditorCommandInputs.components.xml" );
			}
		}

		/// <summary>
		/// Creates a new scene for the editor to use
		/// </summary>
		protected override Scene CreateNewScene( )
		{
			return EditorSceneBuilder.CreateScene( );
		}

		/// <summary>
		/// Creates a new runtime scene
		/// </summary>
		protected override Scene CreateNewRuntimeScene( )
		{
			return RuntimeSceneBuilder.CreateScene( );
		}

		#endregion


		#region Event handlers

		private void MainForm_Load( object sender, EventArgs e )
		{
			viewToolStripMenuItem.DropDownItems.Add( "&Game", null, OnGameClicked );
			editToolStripMenuItem.DropDownItems.Add( "&Materials", null, OnMaterialsClicked );

			//	Load in a given level if one is specified in the program arguments
			string[] cmdArgs = Environment.GetCommandLineArgs( );
			if ( cmdArgs.Length > 1 )
			{
				string sceneToLoad = cmdArgs[ 1 ];

				Scene scene = SceneSerializer.Instance.Open( sceneToLoad );
				if ( scene != null )
				{
					EditorState.Instance.OpenScene( scene );
				}
				if ( cmdArgs.Length > 2 )
				{
					SceneExporter.Instance.LastExportPath = System.IO.Path.GetFullPath( cmdArgs[ 2 ] );
				}
			}
		}

		private void OnMaterialsClicked( object sender, EventArgs args )
		{
			MaterialsEditorForm form = new MaterialsEditorForm( );
			form.Show( this );
		}

		private void OnGameClicked( object sender, EventArgs args )
		{
			Scene runtimeScene;
			try
			{
				runtimeScene = CreateNewRuntimeScene( );
				BuildRuntimeScene( EditorState.Instance.CurrentScene, runtimeScene );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Error creating runtime scene" );
				ErrorMessageBox.Show( this, Resources.FailedToBuildSceneCantRunGame );
				return;
			}

			if ( !SceneExporter.Instance.Export( runtimeScene ) )
			{
				ErrorMessageBox.Show( this, Resources.ExportFailedCantRunGame );
				return;
			}

			ISource sceneSource = new Location( SceneExporter.Instance.LastExportPath );
			ISource viewerSource = new Location( "Editor/LevelEditorGameViewer.components.xml" );

			PlayerSetup[] players = new PlayerSetup[]
				{
					new PlayerSetup
					(
						"Test Player",
						new Location( "Input/DefaultGameInputs.components.xml" ),
						new Location( "Objects/DefaultPC.components.xml" )
					) 	
				};

			//	Disable continuous rendering on the main display
			display.ContinuousRendering = false;

			GameSetup setup = new GameSetup( sceneSource, players, viewerSource );

			try
			{
				//	Make sure that all assets are loaded afresh
				AssetManager.Instance.ClearAllCaches( );

				GameViewForm gameForm = new GameViewForm( setup );
				gameForm.ShowDialog( this );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Game threw an unhandled exception" );
				ErrorMessageBox.Show( this, Resources.GameUnhandledException );
			}

			//	Re-enable continuous rendering on the main display
			display.ContinuousRendering = true;
		}

		#endregion
	}
}