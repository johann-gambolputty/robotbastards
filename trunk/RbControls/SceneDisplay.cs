using System;
using System.Windows.Forms;
using System.ComponentModel;
using RbEngine.Scene;

namespace RbControls
{
	/// <summary>
	/// Extends Display to render a scene (RbEngine.Scene.SceneDb)
	/// </summary>
	public class SceneDisplay : Display
	{
		/// <summary>
		/// Sets the number of depth bits used by the control
		/// </summary>
		[ Category( "Scene rendering properties" ), Description( "Scene view setup file path" ) ]
		public string SceneViewSetupFile
		{
			get
			{
				return m_SetupFile;
			}
			set
			{
				m_SetupFile = value;
			}
		}

		/// <summary>
		/// Sets the scene to be rendered (shortcut to access the scene in the SceneView object)
		/// </summary>
		public SceneDb		Scene
		{
			get
			{
				return m_View.Scene;
			}
			set
			{
				m_View.Scene = value;
			}
		}

		/// <summary>
		/// Gets the scene view associated with this control
		/// </summary>
		public SceneView	SceneView
		{
			get
			{
				return m_View;
			}
		}


		/// <summary>
		/// Constructor. Scene property must be set before this will display anything
		/// </summary>
		public SceneDisplay( )
		{
			m_View = new SceneView( this );
			InitializeComponent( );
		}

		/// <summary>
		/// Constructor - sets the scene to display
		/// </summary>
		public SceneDisplay( SceneDb scene )
		{
			m_View = new SceneView( this );
			Scene = scene;
			InitializeComponent( );
		}

		private void InitializeComponent()
		{
			// 
			// SceneDisplay
			// 
			this.Name = "SceneDisplay";
			this.Load += new System.EventHandler(this.SceneDisplay_Load);

		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected override void Draw( )
		{
			if ( m_View != null )
			{
				m_View.RenderView( );
			}
			else
			{
				SetupViewport( );

				RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );
			}
		}


		private SceneView	m_View;
		private string		m_SetupFile	= string.Empty;

		/// <summary>
		/// Loads the scene view description from the setup file
		/// </summary>
		private void SceneDisplay_Load(object sender, System.EventArgs e)
		{
			if ( !DesignMode )
			{
				if ( m_SetupFile != string.Empty )
				{
					m_View.Load( m_SetupFile );
				}
			}
		}
	}
}
