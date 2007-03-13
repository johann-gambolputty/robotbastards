using System;
using RbEngine.Scene;

namespace RbControls
{
	/// <summary>
	/// Extends Display to render a scene (RbEngine.Scene.SceneDb)
	/// </summary>
	public class SceneDisplay : Display
	{
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
		}

		/// <summary>
		/// Constructor - sets the scene to display
		/// </summary>
		public SceneDisplay( SceneDb scene )
		{
			Scene = scene;
		}

		private void InitializeComponent()
		{
			// 
			// SceneDisplay
			// 
			this.Name = "SceneDisplay";

		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected override void Draw( )
		{
			if ( m_View != null )
			{
				m_View.Render( this );
			}
			else
			{
				SetupViewport( );

				RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );
			}
		}


		private SceneView m_View = new SceneView( );
	}
}
