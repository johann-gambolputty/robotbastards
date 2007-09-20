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

		public Scene GameScene
		{
			set
			{
				m_Viewer.Renderable = value;
			}
		}

		private Viewer m_Viewer;
		private readonly CommandUser m_User = new CommandUser( );

		
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

			//	Get game commands invalidating the view
			foreach ( Command command in gameInputs.Commands )
			{
				m_User.AddActiveListener( command, OnGameCommand );
			}
		}

	}
}
