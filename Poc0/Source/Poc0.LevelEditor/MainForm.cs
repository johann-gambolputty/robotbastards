using System;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.Rendering;
using Rb.Core.Assets;
using Rb.Core.Maths;
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
			EditorState.Instance.ObjectEditorBuilder = new GameObjectEditorBuilder( );
		}

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
		protected override void PopulateNewScene( EditorScene scene )
		{
			base.PopulateNewScene( scene );

			//	Populate runtime scene
			scene.RuntimeScene.AddService( new LightingService( ) );

			//	Populate editor scene
			IRayCastService rayCaster = new RayCastService( );
			scene.AddService( rayCaster );
			rayCaster.AddIntersector( RayCastLayers.Grid, new Plane3( new Vector3( 0, 1, 0 ), 0 ) );

			//	TODO: AP: Fix Z order rendering cheat
			scene.Objects.Add( Guid.NewGuid( ), Graphics.Factory.Create< GroundPlaneGrid >( ) );
			scene.Objects.Add( Guid.NewGuid( ), Graphics.Factory.Create< LevelGeometry >( scene ) );
		}

		private Content m_EditorControlsContent;

		private void MainForm_Load( object sender, EventArgs e )
		{
			viewToolStripMenuItem.DropDownItems.Add( "&Game", null, OnGameClicked );
		}

		private void OnGameClicked( object sender, EventArgs args )
		{
			Exporter.Export( EditorState.Instance.CurrentRuntimeScene );

			ISource sceneSource = new Location( Exporter.LastExportPath );
			ISource viewerSource = new Location( "Editor/LevelEditorGameViewer.components.xml" );

			PlayerSetup[] players = new PlayerSetup[]
				{
					new PlayerSetup
					(
						"test",
						new Location( "Editor/LevelEditorGameCommandInputs.components.xml" ),
						new Location( "Graphics/Entities/TestPlayer0/Full.components.xml" )
					) 	
				};

			GameSetup setup = new GameSetup( sceneSource, players, viewerSource );

			GameViewForm gameForm = new GameViewForm( setup );
			gameForm.Show( this );
		}
	}
}