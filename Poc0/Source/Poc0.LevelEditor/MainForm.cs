using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
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
			string renderAssemblyName = ConfigurationManager.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				renderAssemblyName = "Rb.Rendering.OpenGl.Windows";
			}
			RenderFactory.Load( renderAssemblyName );
			
			//	Load resource settings
			string resourceSetupPath = ConfigurationManager.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath == null )
			{
				resourceSetupPath = "../resourceSetup.xml";
			}
			ResourceManager.Instance.Setup( resourceSetupPath );

			//	Load all assemblies that support the chosen graphics API 
			Rb.AssemblySelector.IdentifierMap.Instance.AddAssemblyIdentifiers( Directory.GetCurrentDirectory( ), SearchOption.TopDirectoryOnly );
			Rb.AssemblySelector.IdentifierMap.Instance.LoadAll( "GraphicsApi=" + RenderFactory.Instance.ApiName );

			InitializeComponent( );

			CreateEditContext( );

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

		private void CreateEditContext( )
		{
			//	Create the tile grid edit state
			EditModeContext editContext = EditModeContext.CreateNewContext( );
			editContext.AddEditControl( display1 );
			editContext.AddEditMode( new SelectEditMode( MouseButtons.Left ) );
			editContext.Selection.ObjectSelected += OnSelectionChanged;
			editContext.Selection.ObjectDeselected += OnSelectionChanged;
			m_EditContext = editContext;
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

			//	Add a renderer for the tile grid to the scene renderables
			scene.Objects.Add( Guid.NewGuid( ), grid );
			scene.Renderables.Add( RenderFactory.Instance.Create< TileBlock2dRenderer >( grid, m_EditContext ) );

			Scene = scene;
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			if ( !DesignMode )
			{
				//	Load input bindings
				CommandInputTemplateMap map = ( CommandInputTemplateMap )ResourceManager.Instance.Load( "LevelEditorCommandInputs.components.xml" );
				m_User.InitialiseAllCommandListBindings( );

				//	Load default templates
				m_Templates.Append( "TestObjectTemplates.components.xml" );

				ComponentLoadParameters loadParams = new ComponentLoadParameters( );
				loadParams.Properties[ "User" ] = m_User;

				//	Load in the scene viewer
				Viewer viewer = ( Viewer )ResourceManager.Instance.Load( "LevelEditorStandardViewer.components.xml", loadParams );
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


				//	Create a new scene
				CreateNewScene( TileTypeSet.CreateDefaultTileTypeSet( ), 16, 16 );
			}
		}

		public Scene Scene
		{
			get { return m_Scene; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value", "Scene cannot be null" );
				}

				TileGrid grid = value.Objects.GetFirstOfType< TileGrid >( );
				if ( grid == null )
				{
					throw new ArgumentException( "Scene did not contain a TileGrid object" );
				}

				m_Scene = value;
				m_Grid = grid;

				foreach ( Viewer viewer in display1.Viewers )
				{
					viewer.Renderable = m_Scene;
				}

				m_EditContext.Setup( m_Scene, m_Grid );
				editorControls1.Setup( m_Scene, m_Grid, m_EditContext, m_Templates );
			}
		}

		private readonly CommandUser		m_User = new CommandUser( );
		private readonly ObjectTemplates	m_Templates = new ObjectTemplates( );
		private readonly SceneSerializer	m_Serializer = new SceneSerializer( );
		private Scene						m_Scene;
		private TileGrid					m_Grid;
		private EditModeContext				m_EditContext;

		private void niceComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = niceComboBox1.GetTag( niceComboBox1.SelectedIndex );
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
			Scene scene = m_Serializer.Open( );
			if ( scene != null )
			{
				Scene = scene;
			}
		}
	}
}