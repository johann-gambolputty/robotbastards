using System;
using System.Collections;
using System.Windows.Forms;
using RbEngine.Rendering;


namespace RbEngine.Scene
{
	/// <summary>
	/// Scene viewer
	/// </summary>
	public class SceneView : Components.Node, Components.IContext
	{
		#region	Public properties

		/// <summary>
		/// Access to the active command list
		/// </summary>
		public Interaction.CommandList	ActiveCommands
		{
			get
			{
				return m_ActiveCommands;
			}
			set
			{
				if ( m_ActiveCommands != null )
				{
					Output.WriteLineCall( Output.SceneInfo, "Unbinding command list \"{0}\"", m_ActiveCommands.Name );
					m_ActiveCommands.UnbindFromView( this );
				}
				m_ActiveCommands = value;
				if ( m_ActiveCommands != null )
				{
					Output.WriteLineCall( Output.SceneInfo, "Binding command list \"{0}\"", m_ActiveCommands.Name );
					m_ActiveCommands.BindToView( this );
				}
			}
		}


		/// <summary>
		/// Sets the ActiveCommands command list by looking up the command list manager with the value string
		/// </summary>
		public string				ActiveCommandListName
		{
			set
			{
				ActiveCommands = Interaction.CommandListManager.Inst.Get( value );
				if ( ActiveCommands == null )
				{
					throw new ApplicationException( string.Format( "Could not find command list \"{0}\" to set scene view active commands", value ) );
				}
			}
		}



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

				if ( m_Scene != null )
				{
					foreach ( object curObject in m_SceneContextObjects )
					{
						m_Scene.AddToContext( curObject );
					}
					m_SceneContextObjects.Clear( );
				}

				//	Create a controller for the camera
				//	TODO: A bit nasty...
				if ( ( m_CameraController == null ) && ( m_Scene != null ) )
				{
					m_CameraController = m_Camera.CreateDefaultController( m_Control, m_Scene );
				}
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

				//	Create a controller for the camera
				//	TODO: A bit nasty...
				if ( m_Scene != null )
				{
					m_CameraController = m_Camera.CreateDefaultController( m_Control, m_Scene );
				}
			}
		}

		/// <summary>
		/// Gets the list of view techniques that this view uses to render the scene
		/// </summary>
		public ArrayList			ViewTechniques
		{
			get
			{
				return m_ViewTechniques;
			}
		}

		/// <summary>
		/// Gets the control associated with this view
		/// </summary>
		public Control				Control
		{
			get
			{
				return m_Control;
			}
		}

		#endregion

		#region	Constructors

		/// <summary>
		/// Empty viewer
		/// </summary>
		public SceneView( Control control )
		{
			m_Control = control;
		}

		/// <summary>
		/// Sets up this view to look at the specified scene
		/// </summary>
		public SceneView( Control control, Scene.SceneDb scene )
		{
			m_Control = control;
			m_Scene	= scene;
		}

		#endregion

		#region	Setup

		/// <summary>
		/// Loads the setup for this view from a resource
		/// </summary>
		public void Load( string path )
		{
			//	TODO: If there's more than one scene view, it makes sense to load all the scene view techniques into a ModelSet and 
			//	reference them
			Resources.ResourceManager.Inst.Load( path, new Resources.LoadParameters( this ) );
		}

		#endregion

		#region	Rendering

		/// <summary>
		/// Renders the view
		/// </summary>
		public void					RenderView( )
		{
			if ( m_Scene == null )
			{
				return;
			}

			Renderer renderer = Renderer.Inst;
			renderer.CurrentControl = m_Control;

			if ( Camera != null )
			{
				Camera.Begin( );
			}

			m_Scene.Rendering.RenderUpdate( );

			if ( m_ViewTechniques.Count == 0 )
			{
				renderer.SetViewport( 0, 0, m_Control.Width, m_Control.Height );
				renderer.ClearDepth( 1.0f );
				renderer.ClearVerticalGradient( System.Drawing.Color.LightSkyBlue, System.Drawing.Color.Black );

				RenderScene( );
			}
			else
			{
				TechniqueRenderDelegate render = new TechniqueRenderDelegate( RenderScene );
				foreach ( ITechnique viewTechnique in m_ViewTechniques )
				{
					viewTechnique.Apply( render );
				}
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

			if ( Camera != null )
			{
				Camera.End( );
			}
		}

		/// <summary>
		/// Renders the associated scene.
		/// </summary>
		private void  RenderScene( )
		{
			if ( Scene != null )
			{
				Scene.Rendering.Render( );
			}
		}

		#endregion

		#region IContext Members

		/// <summary>
		/// Event, invoked when an object is added to this context
		/// </summary>
		public event Components.AddedToContextDelegate AddedToContext;

		/// <summary>
		/// Event, invoked when an object is removed from this context
		/// </summary>
		public event Components.RemovedFromContextDelegate RemovedFromContext;

		/// <summary>
		/// Adds an object to this view
		/// </summary>
		public void AddToContext( Object obj )
		{
			if ( Scene == null )
			{
				m_SceneContextObjects.Add( obj );
			}
			else
			{
				Scene.AddToContext( obj );
			}
			if ( AddedToContext != null )
			{
				AddedToContext( this, obj );
			}
		}

		/// <summary>
		/// Removes an object from this view
		/// </summary>
		public void RemoveFromContext( Object obj )
		{
			m_SceneContextObjects.Remove( obj );
			if ( Scene != null )
			{
				Scene.RemoveFromContext( obj );
			}
			if ( RemovedFromContext != null )
			{
				RemovedFromContext( this, obj );
			}
		}

		#endregion

		#region	Private stuff

		private ArrayList				m_AllObjects			= new ArrayList( );
		private ArrayList				m_SceneContextObjects	= new ArrayList( );
		private ArrayList				m_ViewTechniques		= new ArrayList( );
		private Scene.SceneDb			m_Scene;
		private Cameras.CameraBase		m_Camera;
		private Control					m_Control;
		private Object					m_CameraController;
		private Interaction.CommandList	m_ActiveCommands;

		#endregion

	}
}
