using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Interaction;
using Rb.Rendering;
using Rb.World;

namespace Poc0.LevelEditor
{
	public partial class GameViewControl : UserControl
	{
		public GameViewControl( )
		{
			InitializeComponent( );
		}

		public void Setup( Scene gameScene, CommandUser user )
		{
			m_User = user;
			
			CommandInputTemplateMap gameMap = ( CommandInputTemplateMap )AssetManager.Instance.Load( "LevelEditorGameCommandInputs.components.xml" );

			//	Load in the game viewer
			LoadParameters loadArgs = new LoadParameters( );
			loadArgs.Properties.Add( "User", m_User );
			m_Viewer = ( Viewer )AssetManager.Instance.Load( "LevelEditorGameViewer.components.xml", loadArgs );
			gameDisplay.AddViewer( m_Viewer );

			//	Bind the input map to the viewer
			gameMap.BindToInput( new InputContext( m_Viewer ), m_User );
			
			//	Get the viewer to render the scene
			m_Viewer.Renderable = gameScene;

			//	Get game commands invalidating the view
			foreach ( Command command in gameMap.Commands )
			{
				m_User.AddActiveListener( command, OnGameCommand );
			}
		}

		private Viewer m_Viewer;
		private CommandUser m_User;
		
		/// <summary>
		/// Invalidates the game display when game command messages received
		/// </summary>
		private void OnGameCommand( CommandMessage msg )
		{
			gameDisplay.Invalidate( );
		}

	}
}
