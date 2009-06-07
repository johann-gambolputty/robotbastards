using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.PlanarReflectionTest
{
	/// <summary>
	/// Scene object for the reflection test
	/// </summary>
	public class ReflectionScene : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="sceneObjects">Scene objects</param>
		public ReflectionScene( params IRenderable[] sceneObjects )
		{
			Arguments.CheckNotNullAndContainsNoNulls( sceneObjects, "sceneObjects" );
			m_SceneObjects = sceneObjects;
			m_Reflections = Graphics.Factory.CreateRenderTarget( );
			m_Reflections.Create( "Reflections", 512, 512, TextureFormat.R8G8B8A8, 24, 0, false );

			m_ReflectionMatrixDataSource = Graphics.EffectDataSources.CreateValueDataSourceForNamedParameter<InvariantMatrix44>( "ReflectionProjectionMatrix" );
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			ICamera3 camera = Graphics.Renderer.Camera as ICamera3;
			if ( camera == null )
			{
				throw new InvalidOperationException( "Expected ICamera3 to be attached to renderer" );
			}

			ReflectionsRenderContext reflectionsContext = new ReflectionsRenderContext( context, m_Reflections );
			RenderPlanarReflections( reflectionsContext, camera );

			//	Render the scene as usual
			reflectionsContext.RenderingReflections = false;
			RenderSceneObjects( reflectionsContext );
		}

		#endregion

		#region Private Members

		private readonly IEffectValueDataSource<InvariantMatrix44> m_ReflectionMatrixDataSource;
		private readonly IRenderTarget m_Reflections;
		private readonly IRenderable[] m_SceneObjects;

		/// <summary>
		/// Gets the first reflection plane from the scene, returning null if one can't be found
		/// </summary>
		private ReflectionPlane ReflectionPlane
		{
			get
			{
				foreach ( IRenderable renderable in m_SceneObjects )
				{
					if ( renderable is ReflectionPlane )
					{
						return ( ReflectionPlane )renderable;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Renders all the scene objects
		/// </summary>
		/// <param name="context">Rendering context</param>
		private void RenderSceneObjects( IRenderContext context )
		{
			foreach ( IRenderable sceneObject in m_SceneObjects )
			{
				sceneObject.Render( context );
			}
		}

		/// <summary>
		/// Renders reflections on the first <see cref="ReflectionPlane"/> found in the scene
		/// </summary>
		private void RenderPlanarReflections( ReflectionsRenderContext reflectionsContext, ICamera3 camera )
		{
			ReflectionPlane plane = ReflectionPlane;
			if ( plane == null )
			{
				return;
			}

		//	Matrix44 reflectionMatrix = Matrix44.MakeReflectionMatrix( new Point3( 0, -10, 0 ), new Vector3( 0, 1, 0 ) );
			Matrix44 reflectionMatrix = plane.CreateReflectionMatrix( );
			Matrix44 reflectedCameraMatrix = new Matrix44( camera.InverseFrame * reflectionMatrix );
		//	reflectedCameraMatrix.YAxis = -reflectedCameraMatrix.YAxis; // Restore handedness
			Graphics.Renderer.PushTransform( TransformType.WorldToView );
			Graphics.Renderer.SetTransform( TransformType.WorldToView, reflectedCameraMatrix );
			Graphics.Renderer.PushTransform( TransformType.ViewToScreen );
			IProjectionCamera projCamera = ( IProjectionCamera )camera;
			Graphics.Renderer.SetPerspectiveProjectionTransform( projCamera.PerspectiveFovDegrees, 1, projCamera.PerspectiveZNear, projCamera.PerspectiveZFar );

			m_ReflectionMatrixDataSource.Value = Graphics.Renderer.GetTransform( TransformType.ViewToScreen ) * reflectedCameraMatrix;

			//	TODO: AP: Reflect camera position
			System.Drawing.Rectangle viewport = Graphics.Renderer.Viewport;
			Graphics.Renderer.SetViewport( 0, 0, m_Reflections.Width, m_Reflections.Height );
			reflectionsContext.RenderingReflections = true;
			m_Reflections.Begin( );
			Graphics.Renderer.ClearDepth( 1.0f );
			Graphics.Renderer.ClearColour( Color.SteelBlue );
			RenderSceneObjects( reflectionsContext );
			m_Reflections.End( );
			Graphics.Renderer.SetViewport( viewport.X, viewport.Y, viewport.Width, viewport.Height );
			Graphics.Renderer.PopTransform( TransformType.WorldToView );
			Graphics.Renderer.PopTransform( TransformType.ViewToScreen );
		}

		#endregion
	}
}
