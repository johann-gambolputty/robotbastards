using System;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.Geometry;
using Poc0.LevelEditor.Core.Rendering;
using Poc0.LevelEditor.Properties;
using Rb.Core.Assets;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Log;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Controls.Forms;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.World;
using Rb.World.Services;

namespace Poc0.LevelEditor
{
	public partial class MainForm : EditorForm
	{
		public MainForm( )
		{
			InitializeComponent( );

			EditorState.Instance.AddEditMode( new SelectEditMode( System.Windows.Forms.MouseButtons.Left, new RayCastOptions( RayCastLayers.Entity | RayCastLayers.StaticGeometry ) ) );
		}

		#region Protected members

		protected override void InitializeDockingControls( )
		{
			base.InitializeDockingControls( );

			m_EditorControlsContent = DockingManager.Contents.Add( new EditorControls( ), Properties.Resources.EditorToolbox );
			//	NOTE: AP: Removed game view for now - it was screwing up pick ray calculation in Camera3 (game view viewport used occasionally)
			//m_GameViewContent = DockingManager.Contents.Add( new GameViewControl( ), Properties.Resources.EditorToolbox );
			//DockingManager.AddContentToZone( m_GameViewContent, LogDisplayContent.ParentWindowContent.ParentZone, 0 );

			DockingManager.AddContentToZone( m_EditorControlsContent, SelectionContent.ParentWindowContent.ParentZone, 0 );
		}
		
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
		/// Populates the scene
		/// </summary>
		/// <param name="scene">Populates the scene</param>
		protected override void PopulateNewScene( Scene scene )
		{
			base.PopulateNewScene( scene );

			//	Add raycast service to scene
			IRayCastService rayCaster = new RayCastService( );
			rayCaster.AddIntersector( RayCastLayers.Grid, new Plane3( new Vector3( 0, 1, 0 ), 0 ) );
			scene.AddService( rayCaster );

			//	Add material set service to scene
			ISource materialSetSource = new Location( "Editor/DefaultMaterialSet.components.xml" );
			MaterialSet materials;
			try
			{
				materials = MaterialSet.Load( materialSetSource, false );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to load default material set from \"{0}\"", materialSetSource );
				ErrorMessageBox.Show( this, Resources.FailedToLoadMaterialSet, materialSetSource );
				materials = new MaterialSet( "Default" );
			}
			scene.AddService( materials );

			//	TODO: AP: Fix Z order rendering cheat
			scene.Objects.Add( Guid.NewGuid( ), Graphics.Factory.Create< GroundPlaneGrid >( ) );
			scene.Objects.Add( Guid.NewGuid( ), Graphics.Factory.Create< LevelGeometry >( ) );
		}

		/// <summary>
		/// Populates a runtime scene created by the export process
		/// </summary>
		/// <param name="scene">Scene to populate</param>
		protected override void PopulateRuntimeScene( Scene scene )
		{
			base.PopulateRuntimeScene( scene );

			//	Populate runtime scene
			scene.AddService( new LightingService( ) );
			IUpdateService updater = new UpdateService( );
			updater.AddClock( new Clock( "updateClock", 30, true ) );
			updater.AddClock( new Clock( "animationClock", 60, true ) );
			scene.AddService( updater );

		}

		#endregion

		#region Private members

		private Content m_EditorControlsContent;

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
			Scene runtimeScene = CreateNewRuntimeScene( );

			if ( !SceneExporter.Instance.Export( runtimeScene ) )
			{
				ErrorMessageBox.Show( this, Properties.Resources.ExportFailedCantRunGame );
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
				GameViewForm gameForm = new GameViewForm( setup );
				gameForm.ShowDialog( this );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Game threw an exception" );
				ErrorMessageBox.Show( this, Properties.Resources.GameUnhandledException );
			}

			//	Re-enable continuous rendering on the main display
			display.ContinuousRendering = true;
		}

		#endregion
	}
}