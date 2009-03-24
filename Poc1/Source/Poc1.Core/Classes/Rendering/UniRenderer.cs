using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Rendering;
using Poc1.Core.Interfaces.Rendering.Cameras;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Rendering
{
	/// <summary>
	/// Universe renderer
	/// </summary>
	public class UniRenderer : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public UniRenderer( ISolarSystem scene, IUniCamera camera, ISolarSystemRenderer sceneRenderer )
		{
			Arguments.CheckNotNull( scene, "scene" );
			Arguments.CheckNotNull( camera, "camera" );
			Arguments.CheckNotNull( sceneRenderer, "sceneRenderer" );

			m_Scene = scene;
			m_Camera = camera;
			m_Renderer = sceneRenderer;
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context (ignored and replaced by a IUniRenderContext)</param>
		public void Render( IRenderContext context )
		{
			m_Renderer.Render( m_Scene, m_Camera, context );
		}

		#endregion

		#region Private Members

		private ISolarSystem m_Scene;
		private IUniCamera m_Camera;
		private ISolarSystemRenderer m_Renderer;

		#endregion
	}
}
