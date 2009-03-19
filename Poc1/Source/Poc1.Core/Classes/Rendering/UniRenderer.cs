using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Utils;
using Rb.Rendering;

namespace Poc1.Core.Classes.Rendering
{
	public class UniRenderer
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
		public void Render( ISolarSystem solarSystem )
		{
			Arguments.CheckNotNull( solarSystem, "solarSystem" );

			//	TODO: Get correct dimensions of fullscreen window
			int fsWidth = 1024;
			int fsHeight = 720;

			UniRenderContext context = new UniRenderContext( );

			//	TODO: Render close geometry into stencil buffer or Z buffer to early-out far object rendering)
			//	(careful with alpha blending)
			//	Useful only really for very expensive atmosphere rendering - we only want to calculate
			//	scattering equations for visible pixels.
			//	Have separate far/close methods for rendering atmospheres?

			//	Render far away objects into the back buffer
			context.CurrentPass = UniRenderPass.FarObjects;
			solarSystem.Render( context );

			//	TODO: Clear the z buffer

			//	Render close object reflection geometry into a render target
			if ( EnableReflections )
			{
				//	TODO: Invert camera around reflection plane
				UseRenderTarget( ( int )( fsWidth * ReflectionResolution ), ( int )( fsHeight * ReflectionResolution ), 24 );
				context.CurrentPass = UniRenderPass.CloseReflectedObjects;
				solarSystem.Render( context );
				//	TODO: 
			}

			//	Render close object shadow geometry into a render target
			//	TODO: ...

			//	Render close objects
			UseBackBuffer( );
			context.CurrentPass = UniRenderPass.CloseObjects;
			solarSystem.Render( context );

			//	TODO: Render fullscreen quad over back buffer, for post-process effects
		}

		#region Private Members

		/// <summary>
		/// Implementation of <see cref="IUniRenderContext"/>, with setters as well as getters
		/// </summary>
		private class UniRenderContext : RenderContext, IUniRenderContext
		{
			/// <summary>
			/// Gets the current camera
			/// </summary>
			public IUniCamera Camera
			{
				get { return m_Camera; }
				set { m_Camera = value; }
			}

			/// <summary>
			/// Gets/sets the current render pass type
			/// </summary>
			public UniRenderPass CurrentPass
			{
				get { return m_CurrentPass; }
				set { m_CurrentPass = value; }
			}

			private UniRenderPass m_CurrentPass;
			private IUniCamera m_Camera;
		}

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
