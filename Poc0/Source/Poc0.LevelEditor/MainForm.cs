using System;
using Crownwood.Magic.Docking;
using Poc0.LevelEditor.Core;
using Rb.Core.Assets;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Controls.Forms;
using Rb.World;

namespace Poc0.LevelEditor
{
	public class MainForm : EditorForm
	{
		/// <summary>
		/// Initialises the form
		/// </summary>
		public MainForm( )
		{
			EditorState.Instance.ObjectEditorBuilder = new GameObjectEditorBuilder( );

			m_EditorControlsContent = DockingManager.Contents.Add( new EditorControls( ), Properties.Resources.EditorToolbox );
			m_GameViewContent = DockingManager.Contents.Add( new GameViewControl( ), Properties.Resources.EditorToolbox );

			DockingManager.AddContentToZone( m_EditorControlsContent, SelectionContent.ParentWindowContent.ParentZone, 0 );
			DockingManager.AddContentToZone( m_GameViewContent, LogDisplayContent.ParentWindowContent.ParentZone, 0 );
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
		protected override ISource  InputTemplatePath
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
			scene.RuntimeScene.AddService( new LightingManager( ) );

			//	Populate editor scene
			scene.Objects.Add( Guid.NewGuid( ), new LevelGeometry( scene ) );

			IRayCaster rayCaster = new RayCaster( );
			scene.AddService( rayCaster );
			rayCaster.AddIntersector( GridLayer, new Plane3( new Vector3( 0, 1, 0 ), 0 ) );
		}

		public const ulong GridLayer = 0x1;
		public const ulong StaticGeometryLayer = 0x2;
		public const ulong EntityLayer = 0x2;

		private readonly Content m_EditorControlsContent;
		private readonly Content m_GameViewContent;
	}
}
