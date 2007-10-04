using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;

namespace Poc0.LevelEditor
{
	public partial class GameViewControl : UserControl
	{
		/// <summary>
		/// Initialises this control
		/// </summary>
		public GameViewControl( )
		{
			InitializeComponent( );
		}

		private Viewer m_Viewer;
		private readonly CommandUser m_User = new CommandUser( );

		/// <summary>
		/// Called when a new scene is opened
		/// </summary>
		/// <param name="edit">New scene's edit state</param>
		private void OnSceneOpened( SceneEditState edit )
		{
			m_Viewer.Renderable = edit.RuntimeScene;
		}

		/// <summary>
		/// Called when the current scene is closed
		/// </summary>
		/// <param name="edit">Closing scene's edit state</param>
		private void OnSceneClosed( SceneEditState edit )
		{
			m_Viewer.Renderable = null;
		}

		/// <summary>
		/// Invalidates the game display when game command messages received
		/// </summary>
		private void OnGameCommand( CommandMessage msg )
		{
			gameDisplay.Invalidate( );
		}

		private void GameViewControl_Load( object sender, System.EventArgs e )
		{
			//	Load in the game viewer
			LoadParameters loadArgs = new LoadParameters( );
			loadArgs.Properties.Add( "User", m_User );
			m_Viewer = ( Viewer )AssetManager.Instance.Load( "Editor/LevelEditorGameViewer.components.xml", loadArgs );
			gameDisplay.AddViewer( m_Viewer );

			CommandInputTemplateMap gameInputs = ( CommandInputTemplateMap )AssetManager.Instance.Load( "Editor/LevelEditorGameCommandInputs.components.xml" );
			gameInputs.BindToInput( new InputContext( m_Viewer ), m_User );
			
			//	Start rendering the current scene
			if ( EditorState.Instance.CurrentRuntimeScene != null )
			{
				m_Viewer.Renderable = EditorState.Instance.CurrentRuntimeScene;
			}

			//	Listen out for scenes being opened and closed
			EditorState.Instance.SceneOpened += OnSceneOpened;
			EditorState.Instance.SceneClosed += OnSceneClosed;

			//	Get game commands invalidating the view
			foreach ( Command command in gameInputs.Commands )
			{
				m_User.AddActiveListener( command, OnGameCommand );
			}
		}

	}
}
