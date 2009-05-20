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

			m_ReflectionMatrixDataSource = Graphics.EffectDataSources.CreateValueDataSourceForNamedParameter<Matrix44>( "ReflectionProjectionMatrix" );
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

			Matrix44 reflectionMatrix = Matrix44.MakeReflectionMatrix( Point3.Origin, new Vector3( 0, 1, 0 ) );
			Matrix44 reflectedCameraMatrix = new Matrix44( reflectionMatrix * camera.InverseFrame );
			reflectedCameraMatrix.YAxis = -reflectedCameraMatrix.YAxis; // Restore handedness
			Graphics.Renderer.PushTransform( TransformType.WorldToView );
			Graphics.Renderer.SetTransform( TransformType.WorldToView, reflectedCameraMatrix );

			m_ReflectionMatrixDataSource.Value = Graphics.Renderer.GetTransform( TransformType.ViewToScreen ) * reflectedCameraMatrix;
			
			//	TODO: AP: Reflect camera position
			reflectionsContext.RenderingReflections = true;
			m_Reflections.Begin( );
			Graphics.Renderer.ClearDepth( 1.0f );
			Graphics.Renderer.ClearColour( Color.SteelBlue );
			RenderSceneObjects( reflectionsContext );
			m_Reflections.End( );

			Graphics.Renderer.PopTransform( TransformType.WorldToView );

			//	Render the scene as usual
			reflectionsContext.RenderingReflections = false;
			RenderSceneObjects( reflectionsContext );
		}

		#endregion

		#region Private Members

		private readonly IEffectValueDataSource<Matrix44> m_ReflectionMatrixDataSource;
		private readonly IRenderTarget m_Reflections;
		private readonly IRenderable[] m_SceneObjects;

		private void RenderSceneObjects( IRenderContext context )
		{
			foreach ( IRenderable sceneObject in m_SceneObjects )
			{
				sceneObject.Render( context );
			}
		}

		#endregion
	}
}
