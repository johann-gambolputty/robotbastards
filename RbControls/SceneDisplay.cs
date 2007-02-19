using System;

namespace RbControls
{
	/// <summary>
	/// Extends Display to render a scene (RbEngine.Scene.SceneDb)
	/// </summary>
	public class SceneDisplay : Display
	{
		/// <summary>
		/// The scene that this control will render
		/// </summary>
		public RbEngine.Scene.SceneDb					Scene
		{
			get
			{
				return m_Scene;
			}
			set
			{
				m_Scene = value;
				m_CameraController = m_Camera.CreateDefaultController( this, m_Scene );
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SceneDisplay( )
		{
			m_Camera = new RbEngine.Cameras.SphereCamera( );
		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected override void Draw( )
		{
			SetupViewport( );

			RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

			renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );

			m_Camera.Apply( Width, Height );
			m_Scene.Rendering.Render( );

			//	Render the camera controller
			RbEngine.Rendering.IRender camRenderer = m_CameraController as RbEngine.Rendering.IRender;
			if ( camRenderer != null )
			{
				camRenderer.Render( );
			}
		}
		
		private RbEngine.Scene.SceneDb					m_Scene				= null;
		private RbEngine.Cameras.CameraBase				m_Camera			= null;
		private Object									m_CameraController	= null;

	}
}
