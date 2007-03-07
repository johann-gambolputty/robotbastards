using System;

namespace RbControls
{
	/// <summary>
	/// Extends Display to render a scene (RbEngine.Scene.SceneDb)
	/// </summary>
	public class ClientDisplay : Display
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ClientDisplay( )
		{
			m_Client = new RbEngine.Network.Client( this );
		}

		private void InitializeComponent()
		{
			// 
			// ClientDisplay
			// 
			this.Name = "ClientDisplay";

		}

		public RbEngine.Network.Client Client
		{
			get { return m_Client; }
			set { m_Client = value; }
		}

		/// <summary>
		/// Draws the contents of the control
		/// </summary>
		protected override void Draw( )
		{
			if ( m_Client != null )
			{
				m_Client.Render( );
			}
			else
			{
				SetupViewport( );

				RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );
			}
		}

		private RbEngine.Network.Client		m_Client;

	}
}
