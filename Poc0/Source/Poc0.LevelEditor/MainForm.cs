using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
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

		private void OnSelectionChanged( object obj )
		{
			object[] selectedObjects = EditModeContext.Instance.Selection.ToArray( );

			niceComboBox1.Items.Clear( );
			if ( selectedObjects.Length == 0 )
			{
				propertyGrid1.SelectedObject = null;
				return;
			}

			foreach ( object selectedObj in selectedObjects )
			{
				AddObjectToCombo( 0, selectedObj );
				niceComboBox1.AddSeparator( );
			}
			niceComboBox1.SelectedIndex = 0;
			propertyGrid1.SelectedObject = selectedObjects[ 0 ];
		}

		private void AddObjectToCombo( int depth, object obj )
		{
			string str = obj.GetType( ).ToString( );
			str = str.Substring( str.LastIndexOf( '.' ) + 1 );

			NiceComboBox.Item item = new NiceComboBox.Item( depth, str, depth == 0 ? FontStyle.Bold : 0, null, null, obj );
			niceComboBox1.Items.Add( item );

			IParent parent = obj as IParent;
			if ( parent != null )
			{
				foreach ( object childObj in parent.Children )
				{
					AddObjectToCombo( depth + 1, childObj );
				}
			}
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
			Scene scene = new Scene( );

			//	Create the tile grid
			TileGrid grid = new TileGrid( tileTypes );
			grid.SetDimensions( width, height );

			//	Create the tile grid edit state
			EditModeContext editContext = EditModeContext.CreateNewContext( scene, grid, new SelectedObjects( ) );
			editContext.AddEditControl( display1 );
			editContext.AddEditMode( new SelectEditMode( MouseButtons.Left ) );
			editContext.AddEditMode(new PaintTileEditMode(MouseButtons.Right, tileTypes[0]));
			editContext.Selection.ObjectSelected += OnSelectionChanged;
			editContext.Selection.ObjectDeselected += OnSelectionChanged;

			//	Add a renderer for the tile grid to the scene renderables
			scene.Renderables.Add( new OpenGlTileBlock2dRenderer( grid, editContext ) );

			//	Store
			m_EditContext = editContext;
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
				editorControls1.Setup( m_Scene, m_Grid, m_EditContext, m_Templates );

				ComponentLoadParameters loadParams = new ComponentLoadParameters( );
				loadParams.Properties[ "User" ] = m_User;

				//	Load in the scene viewer
				Viewer viewer = ( Viewer )ResourceManager.Instance.Load( "LevelEditorStandardViewer.components.xml", loadParams );
				viewer.Renderable = m_Scene;

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
		private EditModeContext				m_EditContext;

		private void niceComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = niceComboBox1.GetTag( niceComboBox1.SelectedIndex );
		}
	}
}