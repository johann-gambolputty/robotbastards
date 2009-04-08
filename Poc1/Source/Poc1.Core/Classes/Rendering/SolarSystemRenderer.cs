using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

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
		/// Gets/sets the resolution factor used by the reflection texture
		/// </summary>
		public float ReflectionResolution
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

			//	TODO: Get correct dimensions of fullscreen window
			int fsWidth = 1024;
			int fsHeight = 720;

			UniRenderContext uniContext = new UniRenderContext( camera, context );
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
			Graphics.Renderer.ClearDepth( 1.0f );

			//	Render close object reflection geometry into a render target
			if ( EnableReflections )
			{
				//	TODO: Invert camera around reflection plane
				UseRenderTarget( ( int )( fsWidth * ReflectionResolution ), ( int )( fsHeight * ReflectionResolution ), 24 );
				uniContext.CurrentPass = UniRenderPass.CloseReflectedObjects;
				solarSystem.Render( uniContext );
				//	TODO: 
			}

			//	Render close object shadow geometry into a render target
			//	TODO: ...

			//	Render close objects
			UseBackBuffer( );
			uniContext.CurrentPass = UniRenderPass.CloseObjects;
			solarSystem.Render( uniContext );

			//	TODO: Render fullscreen quad over back buffer, for post-process effects
		}

		#region Private Members

		private float m_ReflectionResolution;
		private bool m_EnableReflections;

		private void UseBackBuffer( )
		{
		}

		private void UseRenderTarget( int width, int height, int bpp )
		{
		}

		#endregion
	}
}
