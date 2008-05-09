using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Poc0.Core;
using Poc0.Core.Cameras;
using Poc0.Core.Controllers;
using Poc0.Core.Objects;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Rendering.Shadows;
using Rb.World;
using Rb.World.Services;

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

			//	Create a property editor for the DebugInfo class

			//	Can't just bung the a DebugInfo object into a property grid - it's all static properties
			//	Create a property bag containing those properties instead
			PropertyBag debugInfo = new PropertyBag( );

			foreach ( PropertyInfo property in typeof( DebugInfo ).GetProperties( BindingFlags.Static | BindingFlags.Public ) )
			{
				debugInfo.Properties.Add( new ExPropertySpec( property ) );
			}
			debugInfo.SetValue += ExPropertySpec.SetValue;
			debugInfo.GetValue += ExPropertySpec.GetValue;

			PropertyGrid debugInfoProperties = new PropertyGrid( );
			debugInfoProperties.SelectedObject = debugInfo;

			//	Set up a docking manager
			m_DockingManager = new DockingManager( this, VisualStyle.IDE );
			m_DockingManager.InnerControl = gameDisplay;

			//	Add the property grid to the docking manager
			m_DebugInfoContent = m_DockingManager.Contents.Add( debugInfoProperties, "Debug Info" );
			m_DockingManager.AddContentWithState( m_DebugInfoContent, State.DockLeft );
		}

		private readonly Content m_DebugInfoContent;
		private readonly DockingManager m_DockingManager;
		private readonly GameSetup m_Setup;
		private CommandUser[] m_Users;
		private Viewer m_Viewer;

		private Scene Scene
		{
			get { return m_Setup.Scene; }
		}

		private void GameViewForm_Load( object sender, EventArgs e )
		{
			//	Add a performance display
			Scene.Objects.Add( new PerformanceDisplay( ) );
			DebugInfo.ShowFps = true;
			DebugInfo.ShowMemoryWorkingSet = true;
			DebugInfo.ShowMemoryPeakWorkingSet = true;

			//	Load in the game viewer
			LoadParameters loadArgs = new LoadParameters( );
			loadArgs.Properties.Add( "Users", m_Users );
			m_Viewer = ( Viewer )AssetManager.Instance.Load( m_Setup.ViewerSource, loadArgs );
			gameDisplay.AddViewer( m_Viewer );

			//	Get start points
			IEnumerable< PlayerStart > startPoints = Scene.Objects.GetAllOfType<PlayerStart>( );

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

				//	Load game inputs
				m_Users[ playerIndex ] = new CommandUser( );
				CommandInputTemplateMap gameInputs = ( CommandInputTemplateMap )AssetManager.Instance.Load( player.CommandSource );
				gameInputs.BindToInput( inputContext, m_Users[ playerIndex ] );
				
				//	Load the player's character
				object character = AssetManager.Instance.Load( player.CharacterSource );
				Scene.Objects.Add( ( IUnique )character );

				//	Place the character at the start position
				IPlaceable placeable = Rb.Core.Components.Parent.GetType<IPlaceable>( character );
				if ( placeable != null )
				{
					placeable.Position = playerStart.Position;
				}

				//	Hack... if the viewer camera is a follow camera, force it to look at the player
				if ( ( m_Setup.NumberOfPlayers == 1 ) && ( m_Viewer.Camera is FollowCamera ) )
				{
					FollowCamera camera = ( FollowCamera )m_Viewer.Camera;

					//	Follow the player
					camera.Target = ( IPlaceable )character;

					//	Add a camera controller
					FollowCameraControl cameraControl = new FollowCameraControl( camera, m_Users[ 0 ] );
					m_Viewer.Camera.AddChild( cameraControl );
				}

				IParent characterParent = ( IParent )character;
				characterParent.AddChild( new UserController( m_Users[ playerIndex ], ( IMessageHandler )character ) );
			}


			//	Start rendering the scene
			m_Viewer.Renderable = Scene;

			//	Kick off the update service... (TODO: AP: Not a very good hack)
			IUpdateService updater = Scene.GetService< IUpdateService >( );
			if ( updater != null )
			{
				updater.Start( );
			}
			playButton.Enabled = false;
		}

		private void GameViewForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			Scene.Dispose( );

			//	Collect game garbage
			GC.Collect( );
			GC.WaitForPendingFinalizers( );
			GC.Collect( );
		}

		private void shadowBufferToolStripMenuItem_Click( object sender, EventArgs e )
		{
			ShadowBufferTechnique.DumpShadowBuffer = true;
		}

		private void playButton_Click( object sender, EventArgs e )
		{
			IUpdateService updater = Scene.GetService<IUpdateService>( );
			if ( updater != null )
			{
				updater.Start( );
			}
			pauseButton.Enabled = true;
			playButton.Enabled = false;
		}

		private void pauseButton_Click( object sender, EventArgs e )
		{
			IUpdateService updater = Scene.GetService<IUpdateService>( );
			if ( updater != null )
			{
				updater.Stop( );
			}
			pauseButton.Enabled = false;
			playButton.Enabled = true;
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
		/// <param name="scene">Game scene</param>
		/// <param name="players">Player setup data</param>
		/// <param name="viewerSource">Location of the viewer file</param>
		public GameSetup( Scene scene, PlayerSetup[] players, ISource viewerSource )
		{
			m_Scene = scene;
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
		public Scene Scene
		{
			get { return m_Scene; }
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
		private readonly Scene m_Scene;
		private readonly PlayerSetup[] m_Players;
	}

}