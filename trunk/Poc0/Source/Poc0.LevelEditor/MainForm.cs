using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Rendering.OpenGl;  
using Rb.Core.Components;
using Rb.Core.Resources;
using Rb.Core.Utils;
using Rb.Interaction;
using Rb.Log;
using Rb.Rendering;
using Rb.World;

namespace Poc0.LevelEditor
{
	public partial class MainForm : Form
	{
		public MainForm( )
		{
			LogForm form = new LogForm( );
			form.Show( );

			//	Write greeting
			AppLog.Info( "Beginning Poc0.LevelEditor at {0}", DateTime.Now );
			AppLog.GetSource( Severity.Info ).WriteEnvironment( );

			//	Load the rendering assembly
			string renderAssembly = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssembly == null )
			{
				renderAssembly = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssembly );
			
			//	Load resource settings
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

			InitializeComponent( );

			Icon = Properties.Resources.AppIcon;
		}

		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close( );
		}

		private void logToolStripMenuItem_Click( object sender, EventArgs e )
		{
			LogForm form = new LogForm( );
			form.Show( );
		}

		/// <summary>
		/// Creates a new scene, including tile grid, renderer and edit state
		/// </summary>
		private void CreateNewScene( TileTypeSet tileTypes, int width, int height)
		{
			//	Create the tile grid
			TileGrid grid = new TileGrid( tileTypes );
			grid.SetDimensions( width, height );

			//	Create the tile grid edit state
			TileGridEditState editState = new TileGridEditState( );
			editState.OnPaint = tileTypes[ 0 ].SetTileToType;

			//	Add a renderer for the tile grid to the scene renderables
			Scene scene = new Scene( );
			scene.Renderables.Add( new OpenGlTileBlock2dRenderer( grid, editState ) );

			//	Store
			m_EditState = editState;
			m_Grid = grid;
			m_Scene = scene;
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( !DesignMode )
			{
				//	Load input bindings
				CommandInputTemplateMap map = ( CommandInputTemplateMap )ResourceManager.Instance.Load( "LevelEditorCommandInputs.components.xml" );
				m_User.InitialiseAllCommandListBindings( );

				//	Create a new scene
				CreateNewScene( TileTypeSet.CreateDefaultTileTypeSet( ), 16, 16 );

				//	Load default templates
				m_Templates.Append( "TestObjectTemplates.components.xml" );

				//	Set up controls
				editorControls1.Setup( m_Scene.Builder, m_Grid, m_EditState, m_Templates );

				ComponentLoadParameters loadParams = new ComponentLoadParameters( );
				loadParams.Properties[ "User" ] = m_User;

				//	Load in the scene viewer
				Viewer viewer = ( Viewer )ResourceManager.Instance.Load( "LevelEditorStandardViewer.components.xml", loadParams );
				viewer.Renderable = m_Scene;

				//	Add a controller to the viewer camera
				viewer.Camera.AddChild( new TileEditCommandHandler( m_User, m_Grid, m_EditState ) );

				display1.Viewers.Add( viewer );

				//	Test load a command list
				try
				{
					//	TODO: AP: May need to move
					map.AddContextInputsToUser( new InputContext( display1.Viewers[ 0 ], display1 ), m_User );
				}
				catch ( Exception ex )
				{
					ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
				}
			}
		}

		private readonly CommandUser		m_User = new CommandUser( );
		private readonly ObjectTemplates	m_Templates = new ObjectTemplates( );
		private Scene						m_Scene;
		private TileGrid					m_Grid;
		private TileGridEditState			m_EditState;
	}
}