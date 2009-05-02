using System;
using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Astronomical.Planets.Renderers;
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
	/// Renders solar systems without any extra passes or effect
	/// </remarks>
	public class BaseSolarSystemRenderer : ISolarSystemRenderer
	{
		#region ISolarSystemRenderer Members

		/// <summary>
		/// Renders a solar system
		/// </summary>
		/// <param name="solarSystem">The scene</param>
		/// <param name="camera">Scene camera</param>
		/// <param name="context">Rendering context</param>
		public virtual void Render( ISolarSystem solarSystem, IUniCamera camera, IRenderContext context )
		{
			Arguments.CheckNotNull( solarSystem, "solarSystem" );
			Arguments.CheckNotNull( camera, "camera" );

			UniRenderContext uniContext = new UniRenderContext( camera, context );
			uniContext.Camera = camera;

			//	Render far objects
			uniContext.CurrentPass = UniRenderPass.FarObjects;
			solarSystem.Render( uniContext );

			Graphics.Renderer.ClearDepth( 1.0f );

			//	Render close objects
			uniContext.CurrentPass = UniRenderPass.CloseObjects;
			solarSystem.Render( uniContext );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Implementation of <see cref="IUniRenderContext"/>, with setters as well as getters
		/// </summary>
		protected class UniRenderContext : RenderContext, IUniRenderContext
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public UniRenderContext( IUniCamera camera, IRenderContext originalContext )
			{
				m_Camera = camera;
				RenderTime = originalContext.RenderTime;
				RenderFrameCounter = originalContext.RenderFrameCounter;
			}

			/// <summary>
			/// Gets the current camera
			/// </summary>
			public IUniCamera Camera
			{
				get { return m_Camera; }
				set { m_Camera = value; }
			}

			/// <summary>
			/// Returns true if far objects are being rendered according to the current pass
			/// </summary>
			public bool RenderFarObjects
			{
				get { return m_CurrentPass == UniRenderPass.FarObjects; }
			}

			/// <summary>
			/// Gets/sets the current render pass type
			/// </summary>
			public UniRenderPass CurrentPass
			{
				get { return m_CurrentPass; }
				set { m_CurrentPass = value; }
			}

			/// <summary>
			/// Gets the current planetary atmosphere renderer that the camera is inside. Returns null if the camera
			/// not inside any planetary atmospheres
			/// </summary>
			/// <remarks>
			/// Used to set up rendering effects for objects seen through atmospheres.
			/// </remarks>
			public IPlanetAtmosphereRenderer InAtmosphereRenderer
			{
				get { return m_InAtmosphereRenderer; }
				set { m_InAtmosphereRenderer = value; }
			}

			/// <summary>
			/// Sets a render target
			/// </summary>
			/// <param name="targetType">Render target type</param>
			/// <param name="target">Render target. Can be null</param>
			public void SetRenderTarget( UniRenderTargets targetType, IRenderTarget target )
			{
				m_Targets[ ( int )targetType ] = target;
			}


			/// <summary>
			/// Gets a specified render target
			/// </summary>
			/// <param name="targetType">Target to retrieve</param>
			/// <returns>Returns the specified render target. If it's not supported, null is returned.</returns>
			public IRenderTarget GetRenderTarget( UniRenderTargets targetType )
			{
				return m_Targets[ ( int )targetType ];
			}

			#region Private Members

			private UniRenderPass m_CurrentPass;
			private IUniCamera m_Camera;
			private IPlanetAtmosphereRenderer m_InAtmosphereRenderer;
			private readonly IRenderTarget[] m_Targets = new IRenderTarget[ Enum.GetValues( typeof( UniRenderTargets ) ).Length ]; 

			#endregion
		}


		#endregion
	}
}
