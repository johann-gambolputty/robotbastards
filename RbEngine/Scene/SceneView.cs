using System;
using RbEngine.Rendering;


namespace RbEngine.Scene
{
	/// <summary>
	/// Scene viewer
	/// </summary>
	public class SceneView : Components.Node
	{
		#region	Public properties

		/// <summary>
		/// Access to the scene being viewed
		/// </summary>
		public SceneDb				Scene
		{
			get
			{
				return m_Scene;
			}
			set
			{
				m_Scene = value;
			}
		}

		/// <summary>
		/// Gets the technique used to render the scene
		/// </summary>
		public SelectedTechnique	ViewTechnique
		{
			get
			{
				return m_ViewTechnique;
			}
		}

		/// <summary>
		/// Access to the RenderEffect that the ViewTechnique uses
		/// </summary>
		public RenderEffect			ViewTechniqueEffect
		{
			set
			{
				m_ViewTechnique.Effect = value;
			}
			get
			{
				return m_ViewTechnique.Effect;
			}
		}

		/// <summary>
		/// Access to the name of the technique that the ViewTechnique uses
		/// </summary>
		/// <remarks>
		/// If this is set, then the ViewTechnique must have been assigned an effect
		/// </remarks>
		public string				ViewTechniqueName
		{
			set
			{
				m_ViewTechnique.Technique = m_ViewTechnique.Effect.FindTechnique( value );
			}
			get
			{
				return m_ViewTechnique.Technique == null ? string.Empty : m_ViewTechnique.Technique.Name;
			}
		}

		/// <summary>
		/// Access to the view camera
		/// </summary>
		public Cameras.CameraBase	Camera
		{
			get
			{
				return m_Camera;
			}
			set
			{
				m_Camera = value;
			}
		}

		#endregion

		#region	Constructors

		/// <summary>
		/// Empty viewer
		/// </summary>
		public SceneView( )
		{
		//	m_ViewTechnique.Technique		= CreateCoreViewTechnique( );
			m_ViewTechnique.RenderCallback	= new RenderTechnique.RenderDelegate( RenderScene );
		}

		/// <summary>
		/// Sets up this view to look at the specified scene
		/// </summary>
		public SceneView( Scene.SceneDb scene )
		{
			m_Scene	= scene;

		//	m_ViewTechnique.Technique		= CreateCoreViewTechnique( );
			m_ViewTechnique.RenderCallback	= new RenderTechnique.RenderDelegate( RenderScene );
		}

		#endregion

		#region	Rendering

		/// <summary>
		/// Renders the view
		/// </summary>
		public void					Render( System.Windows.Forms.Control control )
		{
			if ( m_Scene == null )
			{
				return;
			}

			Renderer renderer = Renderer.Inst;
			renderer.CurrentControl = control;

			if ( m_ViewTechnique.Technique == null )
			{
				renderer.SetViewport( 0, 0, control.Width, control.Height );
				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );

				RenderScene( );
			}
			else
			{
				m_ViewTechnique.Apply( );
			}

			//	Render any attachments to this object (e.g. FPS counter, etc.)
			foreach ( Object childObject in Children )
			{
				IRender childRender = ( childObject as IRender );
				if ( childRender != null )
				{
					childRender.Render( );
				}
				else
				{
					ISceneRenderable childRenderable = ( childObject as ISceneRenderable );
					if ( childRenderable != null )
					{
						childRenderable.Render( TinyTime.CurrentTime );
					}
				}
			}
		}

		/// <summary>
		/// Renders the scene
		/// </summary>
		private void				RenderScene( )
		{
			if ( Scene != null )
			{
				Scene.Rendering.Render( );
			}
		}

		#endregion

		/// <summary>
		/// Creates a RenderTechnique that 
		/// </summary>
		/// <returns></returns>
		private RenderTechnique				CreateCoreViewTechnique( )
		{
			//	TODO: Overkill? Wrap up all the passes into a nice Client3Pass object, that sets up the object for 3d rendering, with a bunch of options
			RenderTechnique technique= new RenderTechnique( );
			technique.Add
			(
				new RenderPass
				(
					new ClearTargetDepth( )
					, new ClearTargetColour( System.Drawing.Color.LightSlateGray )
					, m_Camera
				)
			);

			return technique;
		}

		private Scene.SceneDb					m_Scene;
		private SelectedTechnique				m_ViewTechnique	 = new SelectedTechnique( );
		private Cameras.CameraBase				m_Camera;
	}
}
