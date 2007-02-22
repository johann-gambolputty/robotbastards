using System;
using RbEngine.Rendering;

namespace RbEngine.Network
{
	/*
	 * Local client-server model
	 * 
	 * Client <n:1> Server
	 * 
	 * Remote client-server model
	 * 
	 * Client <n:1> ServerProxy <1:1> Server
	 *
	 * Clients are associated with individual controls
	 */

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

			RenderTechnique defaultTechnique= new RenderTechnique( );
			defaultTechnique.Add
			(
				new RenderPass
				(
					new SetupControlTarget( control ),
					new ClearTargetColour( ),
					new ClearTargetDepth( )
				)
			);
			m_SceneRenderEffect = new RenderEffect( defaultTechnique );
		}


		/// <summary>
		/// The control this client is associated with
		/// </summary>
		public System.Windows.Forms.Control Control
		{
			get { return m_Control; }
		}

		/// <summary>
		/// Server connection
		/// </summary>
		public IServer	Server
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
				}
			}
		}

		public Scene.SceneDb Scene
		{
			get
			{
				return ( m_Server != null ) ? m_Server.Scene : null;
			}
		}

		/// <summary>
		/// Renders the scene
		/// </summary>
		public void Render( )
		{
			if ( m_SceneRenderTechnique == null )
			{
				RbEngine.Rendering.Renderer renderer = RbEngine.Rendering.Renderer.Inst;

				renderer.SetViewport( 0, 0, m_Control.Width, m_Control.Height );
				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );
			}

			if ( Scene != null )
			{
				if ( m_SceneRenderTechnique == null )
				{
				//	Scene.Rendering.Render( );
				}
				else
				{
					m_SceneRenderTechnique.Apply( new RenderTechnique.RenderDelegate( Scene.Rendering.Render ) );
				}
			}
		}

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
		private IServer							m_Server;
		private RenderEffect					m_SceneRenderEffect;
		private SelectedTechnique				m_SceneRenderTechnique;
	}
}
