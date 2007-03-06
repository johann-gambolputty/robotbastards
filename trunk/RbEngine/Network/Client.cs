using System;
using RbEngine.Rendering;

namespace RbEngine.Network
{
	/*
	 * Client setup file
	 * 
	 *	<rb>
	 *		<modelSet name="clientEffects">
	 *			<object type="RenderEffect" name="shadowEffect">
	 *				<
	 *			</object>
	 *		</modelSet>
	 *		<object type="RbEngine.Network.Client" name="client" server="server0">
	 *			<
	 *		</client>
	 *	</rb>
	 *
	 * Server setup file (local)
	 *	<rb>
	 *		<object type="RbEngine.Network.Server" name="server0">
	 * 			<object type="RbEngine.Scene.SceneDb" property="Scene">
	 * 				<object type="RbEngine.Components.Simple.GroundPlane"/>
	 * 				<object type="RbEngine.Components.Simple.Ball">
	 * 					<position x="0" y="10" z="0"/>
	 * 					<radius value="10"/>
	 * 				</object>
	 * 			</object>
	 *		</object>
	 *	</rb>
	 *
	 * Server setup file (remote)
	 *	<rb>
	 * 		<object type="RbEngine.Network.ServerProxy" name="server0">
	 *			<connection></connection>
	 * 		</object>
	 *	</rb>
	 *
	 */

	public class OverrideTechnique : IAppliance
	{
		#region	Setup

		/// <summary>
		/// Constructor. When applied as is, this appliance will revert the global technique override
		/// </summary>
		public OverrideTechnique( )
		{
		}

		/// <summary>
		/// Constructor. When applied as is, this appliance will set the global technique override to overrideTechnique
		/// </summary>
		public OverrideTechnique( RenderTechnique overrideTechnique )
		{
			m_Technique = overrideTechnique;
		}

		/// <summary>
		/// Technique
		/// </summary>
		public RenderTechnique	Technique
		{
			get
			{
				return m_Technique;
			}
			set
			{
				m_Technique = value;
			}
		}

		#endregion

		#region IAppliance Members

		/// <summary>
		/// Sets the attached technique as the global override (RenderTechnique.Override)
		/// </summary>
		public void Begin( )
		{
			RenderTechnique.Override = m_Technique;
		}

		/// <summary>
		/// Disables the override
		/// </summary>
		public void End( )
		{
			RenderTechnique.Override = null;
		}

		#endregion

		private RenderTechnique	m_Technique;
	}

	/// <summary>
	/// Contains view dependent stuff and interaction. Communicates with server or serverproxy via IServer interface.
	/// </summary>
	public class Client
	{
		/// <summary>
		/// Client constructor
		/// </summary>
		public Client( System.Windows.Forms.Control control )
		{
			m_Control = control;

			m_Camera = new RbEngine.Cameras.SphereCamera( );

			//	TODO: Overkill? Wrap up all the passes into a nice Client3Pass object, that sets up the object for 3d rendering, with a bunch of options
			RenderTechnique defaultTechnique= new RenderTechnique( );
			defaultTechnique.Add
			(
				new RenderPass
				(
					new ClearTargetDepth( )
					, new ClearTargetColour( System.Drawing.Color.LightSlateGray )
					, m_Camera
				/*
					, new OverrideTechnique
					(
						new RenderTechnique
						(
							"testOverrideTechnique",
							new RenderPass
							(
								RenderFactory.Inst.NewRenderState( ).SetColour( System.Drawing.Color.Brown )
							)
						)
					)
					*/
				)
			);

			m_SceneRenderEffect		= new RenderEffect( defaultTechnique );
			m_SceneRenderTechnique	= new SelectedTechnique( defaultTechnique );
		}

		/// <summary>
		/// The control this client is associated with
		/// </summary>
		public System.Windows.Forms.Control Control
		{
			get
			{
				return m_Control;
			}
		}

		/// <summary>
		/// Server connection
		/// </summary>
		public ServerBase	Server
		{
			get
			{
				return m_Server;
			}
			set
			{
				if ( m_Server != null )
				{
					m_Server.RemoveClient( this );
				}
				m_Server = value;
				if ( m_Server != null )
				{
					m_Server.AddClient( this );
					
					//	Kill the current camera controller
					if ( m_CameraController is IDisposable )
					{
						( ( IDisposable )m_CameraController ).Dispose( );
					}
					//	Create a new one
					m_CameraController = m_Camera.CreateDefaultController( Control, m_Server.Scene );
				}
			}
		}

		/// <summary>
		/// Access to the connected server's scene
		/// </summary>
		public Scene.SceneDb Scene
		{
			get
			{
				return ( m_Server != null ) ? m_Server.Scene : null;
			}
		}

		// TODO: REMOVEME
		/// <summary>
		/// Client's active camera
		/// </summary>
		public RbEngine.Cameras.CameraBase Camera
		{
			get
			{
				return m_Camera;
			}
		}

		private long m_LastRenderTime = TinyTime.CurrentTime;

		/// <summary>
		/// Renders the scene
		/// </summary>
		public void Render( )
		{
			RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

			//	If there's no selected scene render technique, then set up the viewport, and do a default clear of colour and depth buffers 
			if ( m_SceneRenderTechnique == null )
			{
				renderer.SetViewport( 0, 0, m_Control.Width, m_Control.Height );
				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );
			}

			//	If there's a scene on the server, then render it
			if ( Scene != null )
			{
				if ( m_SceneRenderTechnique == null )
				{
					Scene.Rendering.Render( );
				}
				else
				{
					m_SceneRenderTechnique.Apply( new RenderTechnique.RenderDelegate( Scene.Rendering.Render ) );
				}
			}

			if ( m_CameraController is Rendering.IRender )
			{
				( ( Rendering.IRender )m_CameraController ).Render( );
			}

			ShowFps( );
		}

		private void		ShowFps( )
		{
			if ( m_Font == null )
			{
				m_Font = RenderFactory.Inst.NewFont( ).Setup( new System.Drawing.Font( "courier", 14 ) );
			}

			long curTime = TinyTime.CurrentTime;
			float fps = 1.0f / ( float )TinyTime.ToSeconds( m_LastRenderTime, curTime );
			m_Fps[ m_FpsIndex++ ] = fps;
			m_FpsIndex = m_FpsIndex % m_Fps.Length;

			float avgFps = 0;
			for ( int fpsIndex = 0; fpsIndex < m_Fps.Length; ++fpsIndex )
			{
				avgFps += m_Fps[ fpsIndex ];
			}
			avgFps /= ( float )m_Fps.Length;
			m_Font.DrawText( 0, 0, System.Drawing.Color.White, "FPS: {0}", avgFps.ToString( "G4" ) );
			m_LastRenderTime = curTime;
		}

		private int								m_FpsIndex = 0;
		private float[]							m_Fps = new float[ 32 ];
		private RenderFont						m_Font;

		//
		//	Shadow render technique
		//
		//		- Lighting manager
		//			- Determine shadow lights, active lights
		//		- Run through shadow lights
		//			- Setup render target for light
		//			- Apply transform for light
		//			- Render scene (forced shadow technique)
		//		- Apply camera transform
		//		- Render scene (shadow technique)
		//

		private System.Windows.Forms.Control	m_Control;
		private ServerBase						m_Server;
		private RenderEffect					m_SceneRenderEffect;
		private SelectedTechnique				m_SceneRenderTechnique;
		private RbEngine.Cameras.SphereCamera	m_Camera;
		private Object							m_CameraController;
	}
}
