using System;
using System.Collections;
using System.Windows.Forms;
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
				if ( m_Scene != null )
				{
					RemoveChildrenFromScene( this, m_Scene );
				}

				m_Scene = value;

				if ( m_Scene != null )
				{
					AddChildrenToScene( this, m_Scene );
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
			Resources.ResourceManager.Inst.Load( path, this );
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

		#region	Child objects

		/// <summary>
		/// Adds a child object to this view. Any children that implement ISceneObject get ISceneObject.AddedToScene() called
		/// </summary>
		/// <param name="childObject"></param>
		public override void AddChild( Object childObject )
		{
			if ( ( Scene != null ) && ( childObject is ISceneObject ) )
			{
				( ( ISceneObject )childObject ).AddedToScene( Scene );
				AddChildrenToScene( childObject, Scene );

				//	TODO: Should add callbacks to track new objects added to childObject
			}

			base.AddChild( childObject );
		}

		/// <summary>
		/// Runs through all children of an object, removing them, and their children, from a scene
		/// </summary>
		private void RemoveChildrenFromScene( Object obj, SceneDb scene )
		{
			Components.IParentObject parentObj = obj as Components.IParentObject;
			if ( parentObj != null )
			{
				foreach ( Object childObj in parentObj.Children )
				{
					if ( childObj is ISceneObject )
					{
						( ( ISceneObject )childObj ).RemovedFromScene( scene );
					}

					RemoveChildrenFromScene( childObj, scene );
				}
			}
		}

		/// <summary>
		/// Runs through all children of an object, adding them, and their children, to a scene
		/// </summary>
		private void AddChildrenToScene( Object obj, SceneDb scene )
		{
			Components.IParentObject parentObj = obj as Components.IParentObject;
			if ( parentObj != null )
			{
				foreach ( Object childObj in parentObj.Children )
				{
					if ( childObj is ISceneObject )
					{
						( ( ISceneObject )childObj ).AddedToScene( scene );
					}
					AddChildrenToScene( childObj, scene );
				}
			}
		}

		#endregion

		private ArrayList			m_ViewTechniques = new ArrayList( );
		private Scene.SceneDb		m_Scene;
		private Cameras.CameraBase	m_Camera;
		private Control				m_Control;
		private Object				m_CameraController;
	}
}
