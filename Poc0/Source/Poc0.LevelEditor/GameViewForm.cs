using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Poc0.Core.Objects;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.World;

namespace Poc0.LevelEditor
{
	public partial class GameViewForm : Form
	{
		/// <summary>
		/// Initialises this form
		/// </summary>
		public GameViewForm( GameSetup setup )
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
			using ( System.IO.Stream stream = source.Open( ) )
			{
				return SceneExporter.Open( stream );
			}
		}

		private void GameViewForm_Load( object sender, EventArgs e )
		{
			//	Load the scene
			Scene scene = DeserializeScene( m_Setup.SceneSource );

			//	Load in the game viewer
			LoadParameters loadArgs = new LoadParameters( );
			loadArgs.Properties.Add( "Users", m_Users );
			m_Viewer = ( Viewer )AssetManager.Instance.Load( m_Setup.ViewerSource, loadArgs );
			gameDisplay.AddViewer( m_Viewer );

			//	Get start points
			IEnumerable< PlayerStart > startPoints = scene.Objects.GetAllOfType<PlayerStart>( );

			//	Setup players
			InputContext inputContext = new InputContext( m_Viewer );
			m_Users = new CommandUser[ m_Setup.NumberOfPlayers ];
			for ( int playerIndex = 0; playerIndex < m_Setup.NumberOfPlayers; ++playerIndex )
			{
				PlayerSetup player = m_Setup.Players[ playerIndex ];

				//	Find the start position for this player
				PlayerStart playerStart = null;
				foreach ( PlayerStart startPoint in startPoints )
				{
					if ( startPoint.PlayerIndex == playerIndex )
					{
						playerStart = startPoint;
						break;
					}
				}
				if ( playerStart == null )
				{
					throw new InvalidOperationException( "No player start available for player " + playerIndex );
				}

				//	Load the player's character
				object character = AssetManager.Instance.Load( player.CharacterSource );
				scene.Objects.Add( Guid.NewGuid( ), character );

				//	Load game inputs
				m_Users[ playerIndex ] = new CommandUser( );
				CommandInputTemplateMap gameInputs = ( CommandInputTemplateMap )AssetManager.Instance.Load( player.CommandSource );
				gameInputs.BindToInput( inputContext, m_Users[ playerIndex ] );
			}

			//	Start rendering the scene
			m_Viewer.Renderable = scene;
		}
	}

	/// <summary>
	/// Setup data for a player
	/// </summary>
	public class PlayerSetup
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Player name</param>
		/// <param name="commandSource">Player command source</param>
		/// <param name="characterSource">Player character source</param>
		public PlayerSetup( string name, ISource commandSource, ISource characterSource )
		{
			m_Name = name;
			m_CommandSource = commandSource;
			m_CharacterSource = characterSource;
		}

		/// <summary>
		/// Gets the name of the player
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the source of the command input mappings for the player
		/// </summary>
		public ISource CommandSource
		{
			get { return m_CommandSource; }
		}

		/// <summary>
		/// Gets the source of the player's character
		/// </summary>
		public ISource CharacterSource
		{
			get { return m_CharacterSource; }
		}

		private readonly string m_Name;
		private readonly ISource m_CommandSource;
		private readonly ISource m_CharacterSource;
	}

	/// <summary>
	/// Information for setting up the game
	/// </summary>
	public class GameSetup
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="sceneSource">Location of the scene file</param>
		/// <param name="players">Player setup data</param>
		/// <param name="viewerSource">Location of the viewer file</param>
		public GameSetup( ISource sceneSource, PlayerSetup[] players, ISource viewerSource )
		{
			m_SceneSource = sceneSource;
			m_Players = players;
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
		/// Gets the array of player setup information
		/// </summary>
		public PlayerSetup[] Players
		{
			get { return m_Players; }
		}

		/// <summary>
		/// Gets the number of players
		/// </summary>
		public int NumberOfPlayers
		{
			get { return m_Players.Length; }
		}

		private readonly ISource m_ViewerSource;
		private readonly ISource m_SceneSource;
		private readonly PlayerSetup[] m_Players;
	}

}