using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
			scene.Renderables.Add( new OpenGlTileBlock2dRenderer( grid, m_EditContext ) );

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
		private Scene						m_Scene;
		private TileGrid					m_Grid;
		private EditModeContext				m_EditContext;
		private string						m_LastSavePath;

		private void niceComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = niceComboBox1.GetTag( niceComboBox1.SelectedIndex );
		}

		private void SaveAs( )
		{
			SaveFileDialog saveDialog = new SaveFileDialog( );
			saveDialog.DefaultExt = "scene";
			saveDialog.AddExtension = true;
			if ( saveDialog.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			m_LastSavePath = saveDialog.FileName;
			SaveTo( m_LastSavePath );
		}

		private void Save( )
		{
			if ( m_LastSavePath == null )
			{
				SaveAs( );
			}
			else
			{
				SaveTo( m_LastSavePath );
			}
		}

		private void SaveTo( string path )
		{
			MemoryStream outStream = new MemoryStream( );

			//	TODO: AP: Muesli formatter doesn't work!
			//IFormatter formatter = new Rb.Muesli.BinaryFormatter( );
			IFormatter formatter = CreateFormatter( );
			formatter.Serialize( outStream, m_Scene );

			using ( Stream fileStream = File.OpenWrite( path ) )
			{
				fileStream.Write( outStream.ToArray( ), 0, ( int )outStream.Length );
			}
		}

		private static ISurrogateSelector CreateSurrogateSelector( )
		{
			SurrogateSelector selector = new SurrogateSelector( );
			selector.AddSurrogate( typeof( OpenGlTileBlock2dRenderer ), new StreamingContext( ), new GraphicsSurrogate( ) );
			return selector;
		}

		private static IFormatter CreateFormatter( )
		{
			return new BinaryFormatter( CreateSurrogateSelector( ), new StreamingContext( ) );
		}

		/// <summary>
		/// Test class for remapping graphics types
		/// </summary>
		private class GraphicsSurrogate : ISerializationSurrogate
		{
			#region ISerializationSurrogate Members

			public void GetObjectData( object obj, SerializationInfo info, StreamingContext context )
			{
				TileBlockRenderer renderer = ( ( TileBlockRenderer )obj );
				info.AddValue( "grid", renderer.Grid );
			}

			public object SetObjectData( object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
			{
				TileGrid grid = ( TileGrid )info.GetValue( "grid", typeof( TileGrid ) );
				return new OpenGlTileBlock2dRenderer( grid, EditModeContext.Instance );
			}

			#endregion
		}

		private void Open( string path )
		{
			Scene scene;
			try
			{
				using ( Stream fileStream = File.OpenRead( path ) )
				{
					IFormatter formatter = CreateFormatter( );
					scene = ( Scene )formatter.Deserialize( fileStream );
				}
			}
			catch ( Exception ex )
			{
				string msg = string.Format( Properties.Resources.FailedToOpenScene, path );
				AppLog.Error( msg );
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
				MessageBox.Show( msg, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			Scene = scene;
		}

		private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
		{
			SaveAs( );
		}

		private void saveToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Save( );
		}

		private void openToolStripMenuItem_Click( object sender, EventArgs e )
		{
			OpenFileDialog openDialog = new OpenFileDialog( );
			openDialog.Filter = "Scene Files (*.scene)|*.scene|All Files (*.*)|*.*";
			if ( openDialog.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			Open( openDialog.FileName );
		}
	}
}