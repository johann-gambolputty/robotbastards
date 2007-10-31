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
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Interaction;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
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

		#region Private ExPropertySpec class

		//	TODO: AP: Make a shared base class (there's an ExPropertySpec that does similar things in ObjectPropertyEditor)

		/// <summary>
		/// Extended property specifier for property bags
		/// </summary>
		private class ExPropertySpec : PropertySpec
		{
			/// <summary>
			/// Creates this property specifier
			/// </summary>
			/// <param name="property">Underlying object property</param>
			public ExPropertySpec( PropertyInfo property ) :
				base( property.Name, property.PropertyType, GetPropertyCategory( property ), GetPropertyDescription( property )  )
			{
				m_Property = property;
			}

			#region Access

			/// <summary>
			/// Sets the value of an extended property
			/// </summary>
			public static void SetValue( object sender, PropertySpecEventArgs e )
			{
				( ( ExPropertySpec )e.Property ).Property.SetValue( null, e.Value, null );
			}

			/// <summary>
			/// Gets the value of an extended property
			/// </summary>
			public static void GetValue( object sender, PropertySpecEventArgs e )
			{
				e.Value = ( ( ExPropertySpec )e.Property ).Property.GetValue( e, null );
			}

			#endregion

			#region Private members

			/// <summary>
			/// The underlying object property
			/// </summary>
			private PropertyInfo Property
			{
				get { return m_Property; }
			}

			private readonly PropertyInfo m_Property;
			
			/// <summary>
			/// Gets the category that a property belongs to
			/// </summary>
			private static string GetPropertyCategory( ICustomAttributeProvider property )
			{
				CategoryAttribute[] categoryAttrs = ( CategoryAttribute[] )property.GetCustomAttributes( typeof( CategoryAttribute ), false );
				return categoryAttrs.Length > 0 ? categoryAttrs[ 0 ].Category : "";
			}

			/// <summary>
			/// Gets the description of a property
			/// </summary>
			private static string GetPropertyDescription( PropertyInfo property )
			{
				DescriptionAttribute[] descriptionAttrs = ( DescriptionAttribute[] )property.GetCustomAttributes( typeof( DescriptionAttribute ), false );
				return descriptionAttrs.Length > 0 ? descriptionAttrs[ 0 ].Description : property.PropertyType.Name;
			}

			#endregion
		}

		#endregion

		private readonly Content m_DebugInfoContent;
		private readonly DockingManager m_DockingManager;
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
			m_Scene = DeserializeScene( m_Setup.SceneSource );

			//	Add a performance display
			m_Scene.Objects.Add( new PerformanceDisplay( ) );
			DebugInfo.ShowFps = true;
			DebugInfo.ShowMemoryWorkingSet = true;
			DebugInfo.ShowMemoryPeakWorkingSet = true;

			//	Load in the game viewer
			LoadParameters loadArgs = new LoadParameters( );
			loadArgs.Properties.Add( "Users", m_Users );
			m_Viewer = ( Viewer )AssetManager.Instance.Load( m_Setup.ViewerSource, loadArgs );
			gameDisplay.AddViewer( m_Viewer );

			//	Get start points
			IEnumerable< PlayerStart > startPoints = m_Scene.Objects.GetAllOfType<PlayerStart>( );

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
				m_Scene.Objects.Add( ( IUnique )character );

				//	Place the character at the start position
				IPlaceable placeable = Rb.Core.Components.Parent.GetType<IPlaceable>( character );
				if ( placeable != null )
				{
					placeable.Position = playerStart.Position;
				}

				//	Bit of a hack... if the viewer camera is a follow camera, force it to look at the player
				if ( ( playerIndex == 0 ) && m_Viewer.Camera is FollowCamera )
				{
					( ( FollowCamera )m_Viewer.Camera ).Target = ( IPlaceable )character;
				}

				( ( IParent )character ).AddChild( new UserController( m_Users[ playerIndex ], ( IMessageHandler )character ) );
			}

			//	Start rendering the scene
			m_Viewer.Renderable = m_Scene;

			//	Kick off the update service... (TODO: AP: Not a very good hack)
			IUpdateService updater = m_Scene.GetService< IUpdateService >( );
			if ( updater != null )
			{
				updater.Start( );
			}
		}

		private void GameViewForm_FormClosing( object sender, FormClosingEventArgs e )
		{
			m_Scene.Dispose( );
			m_Scene = null;
		}

		private Scene m_Scene;
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