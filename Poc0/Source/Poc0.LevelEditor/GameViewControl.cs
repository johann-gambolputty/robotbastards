using System.Windows.Forms;
using Rb.Core.Assets;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.World;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// Information for setting up the game
	/// </summary>
	public class GameSetup
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="sceneSource">Location of the scene file</param>
		/// <param name="commandSources">Locations of the user commands file</param>
		/// <param name="viewerSource">Location of the viewer file</param>
		public GameSetup( ISource sceneSource, ISource[] commandSources, ISource viewerSource )
		{
			m_SceneSource = sceneSource;
			m_CommandSources = commandSources;
			m_ViewerSource = viewerSource;
		}

		/// <summary>
		/// Gets the viewer source
		/// </summary>
		public ISource ViewerSource
		{
			get { return m_ViewerSource; }
		}

		/// <summary>
		/// Gets the scene source
		/// </summary>
		public ISource SceneSource
		{
			get { return m_SceneSource; }
		}

		/// <summary>
		/// Gets the source of the user commands
		/// </summary>
		public ISource[] CommandSources
		{
			get { return m_CommandSources; }
		}

		/// <summary>
		/// Gets the number of players
		/// </summary>
		public int NumberOfPlayers
		{
			get { return m_CommandSources.Length; }
		}

		private readonly ISource m_ViewerSource;
		private readonly ISource m_SceneSource;
		private readonly ISource[] m_CommandSources;
	}

	public partial class GameViewControl : UserControl
	{
		/// <summary>
		/// Initialises this control
		/// </summary>
		public GameViewControl( GameSetup setup )
		{
			InitializeComponent( );

			m_Setup = setup;
		}

		private readonly GameSetup m_Setup;
		private CommandUser[] m_Users;
		private Viewer m_Viewer;

		/// <summary>
		/// Deserializes a Scene object from an ISource
		/// </summary>
		private static Scene DeserializeScene( ISource source )
		{
			using( System.IO.Stream stream = source.Open( ) )
			{
				return SceneExporter.Open( stream );
			}
		}

		private void GameViewControl_Load( object sender, System.EventArgs e )
		{
			//	Load the scene
			Scene scene = DeserializeScene( m_Setup.SceneSource );

			//	Load in the game viewer
			LoadParameters loadArgs = new LoadParameters( );
			loadArgs.Properties.Add( "Users", m_Users );
			m_Viewer = ( Viewer )AssetManager.Instance.Load( m_Setup.ViewerSource, loadArgs );
			gameDisplay.AddViewer( m_Viewer );

			//	Load game inputs
			InputContext inputContext = new InputContext( m_Viewer );
			m_Users = new CommandUser[ m_Setup.NumberOfPlayers ];
			for ( int userIndex = 0; userIndex < m_Setup.NumberOfPlayers; ++userIndex )
			{
				m_Users[ userIndex ] = new CommandUser( );

				//	Load game inputs
				CommandInputTemplateMap gameInputs = ( CommandInputTemplateMap )AssetManager.Instance.Load( m_Setup.CommandSources[ userIndex ] );
				gameInputs.BindToInput( inputContext, m_Users[ userIndex ] );
			}

			//	Start rendering the scene
			m_Viewer.Renderable = scene;
		}

	}
}
