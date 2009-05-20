using Poc1.Core.Classes.Rendering.Cameras;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using System.Drawing;
using RbGraphics=Rb.Rendering.Graphics;

namespace Poc1.Core.Classes.Rendering
{
	/// <summary>
	/// Solar system rendering implementation
	/// </summary>
	/// <remarks>
	/// Supports multiple passes for shadows, reflections, and post-process effects.
	/// </remarks>
	public class SolarSystemRenderer : BaseSolarSystemRenderer, ISolarSystemRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public SolarSystemRenderer( bool enableReflections )
		{
			EnableReflections = enableReflections;
		}

		/// <summary>
		/// Gets/sets the resolution used by the reflection texture
		/// </summary>
		public int ReflectionResolution
		{
			get { return m_ReflectionResolution; }
			set { m_ReflectionResolution = value; }
		}

		/// <summary>
		/// Gets/sets the reflection flag
		/// </summary>
		public bool EnableReflections
		{
			get { return m_EnableReflections; }
			set { m_EnableReflections = value; }
		}


		/// <summary>
		/// Renders the scene
		/// </summary>
		/// <param name="solarSystem">The scene</param>
		/// <param name="camera">Scene camera</param>
		/// <param name="context">Rendering context</param>
		public override void Render( ISolarSystem solarSystem, IUniCamera camera, IRenderContext context )
		{
			Arguments.CheckNotNull( solarSystem, "solarSystem" );
			Arguments.CheckNotNull( camera, "camera" );

			PreRender( );

			UniRenderContext uniContext = new UniRenderContext( camera, context );
			uniContext.SetRenderTarget( UniRenderTargets.OceanReflections, m_OceanReflections );
			uniContext.Camera = camera;

			//	TODO: Render close geometry into stencil buffer or Z buffer to early-out far object rendering)
			//	(careful with alpha blending)
			//	Useful only really for very expensive atmosphere rendering - we only want to calculate
			//	scattering equations for visible pixels.
			//	Have separate far/close methods for rendering atmospheres?

			//	Render far away objects into the back buffer
			uniContext.CurrentPass = UniRenderPass.FarObjects;
			solarSystem.Render( uniContext );

			//	Clear the depth buffer before rendering close objects
			RbGraphics.Renderer.ClearDepth( 1.0f );

			//	Render close object reflection geometry into a render target
			if ( EnableReflections )
			{
				RenderReflections( uniContext, solarSystem );
			}

			//	Render close object shadow geometry into a render target
			//	TODO: ...

			uniContext.Camera.Begin( );
			//	Render close objects
			uniContext.CurrentPass = UniRenderPass.CloseObjects;
			solarSystem.Render( uniContext );
			uniContext.Camera.End( );

			//	TODO: Render fullscreen quad over back buffer, for post-process effects
		}

		#region Private Members

		private int m_ReflectionResolution = 512;
		private bool m_EnableReflections;
		private IRenderTarget m_OceanReflections;

		/// <summary>
		/// Finds the closest planet in the solar system
		/// </summary>
		private static IPlanet FindClosestPlanet( IUniCamera camera, ISolarSystem solarSystem, double maxDistance )
		{
			IPlanet closestPlanet = null;
			double closestDistance = maxDistance;
			foreach ( IUniObject uniObject in solarSystem.Components )
			{
				IPlanet planet = uniObject as IPlanet;
				if ( planet != null )
				{
					double distanceToCamera = planet.Transform.Position.DistanceTo( camera.Position );
					if ( distanceToCamera < closestDistance )
					{
						closestPlanet = planet;
						closestDistance = distanceToCamera;
					}
				}
			}
			return closestPlanet;
		}

		/// <summary>
		/// Renders reflections
		/// </summary>
		private void RenderReflections( UniRenderContext uniContext, ISolarSystem solarSystem )
		{
			IPlanet planet = FindClosestPlanet( uniContext.Camera, solarSystem, double.MaxValue );
			if ( planet == null )
			{
				//	No close planets == no reflections to render
				return;
			}
			IPlanetReflectiveOceanRenderer reflectionRenderer = planet.Renderer.GetRenderer<IPlanetReflectiveOceanRenderer>( );
			if ( reflectionRenderer == null )
			{
				//	Planet doesn't have a reflective ocean
				return;
			}

			IUniCamera currentCamera = uniContext.Camera;

			Point3 pointOnPlane;
			Plane3 plane = reflectionRenderer.GetTangentPlaneUnderPoint( uniContext.Camera.Position, out pointOnPlane );

			Point3 localCameraPosition = ( uniContext.Camera.Position - planet.Transform.Position ).ToRenderUnits( );
			Point3 rCamPos = pointOnPlane + plane.Normal * ( pointOnPlane.DistanceTo( localCameraPosition ) );

			InvariantMatrix44 reflectionMatrix = Matrix44.MakeReflectionMatrix( Point3.Origin, plane.Normal );

			long x = planet.Transform.Position.X + ( long )( rCamPos.X * Units.Convert.MulRenderToUni );
			long y = planet.Transform.Position.Y + ( long )( rCamPos.Y * Units.Convert.MulRenderToUni );
			long z = planet.Transform.Position.Z + ( long )( rCamPos.Z * Units.Convert.MulRenderToUni );

			UniCamera reflectedCamera = new UniCamera( );
			reflectedCamera.Frame = reflectionMatrix * currentCamera.Frame;
			reflectedCamera.Position = new UniPoint3( x, y, z );
			reflectedCamera.PerspectiveZNear = currentCamera.PerspectiveZNear;
			reflectedCamera.PerspectiveZFar = currentCamera.PerspectiveZFar;

			uniContext.Camera = reflectedCamera;

			//	Get the tangent space
			//Matrix44 tangentSpaceMatrix = new Matrix44( );
			//reflectionRenderer.Setup( tangentSpaceMatrix );

			//	TODO: Invert camera around reflection plane
			m_OceanReflections.Begin( );

			RbGraphics.Renderer.ClearDepth( 1.0f );
			RbGraphics.Renderer.ClearColour( Color.Black );

			reflectedCamera.Begin( );
		//	RbGraphics.Renderer.PushTransformPostModifier( TransformType.LocalToWorld, reflectionMatrix );

			uniContext.CurrentPass = UniRenderPass.CloseReflectedObjects;
			solarSystem.Render( uniContext );
			m_OceanReflections.End( );

		//	RbGraphics.Renderer.PopTransformPostModifier( TransformType.LocalToWorld );
			reflectedCamera.End( );

			uniContext.Camera = currentCamera;
		}

		/// <summary>
		/// Pre-render setup
		/// </summary>
		private void PreRender( )
		{
			//	Create render targets
			if ( EnableReflections )
			{
				if ( m_OceanReflections != null )
				{
					if ( m_OceanReflections.Width != ReflectionResolution )
					{
						m_OceanReflections.Dispose( );
						m_OceanReflections = null;
					}
				}
				if ( m_OceanReflections == null )
				{
					m_OceanReflections = RbGraphics.Factory.CreateRenderTarget( );
					m_OceanReflections.Create( "Ocean Reflections", ReflectionResolution, ReflectionResolution, TextureFormat.R8G8B8A8, 24, 0, false );
				}
			}
			else if ( m_OceanReflections != null )
			{
				m_OceanReflections.Dispose( );
				m_OceanReflections = null;
			}
		}

		#endregion
	}
}
