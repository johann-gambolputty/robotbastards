using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.PlanarReflectionTest
{
	/// <summary>
	/// Reflections rendering context
	/// </summary>
	public class ReflectionsRenderContext : RenderContextDecorator
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="innerContext">Inner rendering context</param>
		/// <param name="reflectionsRenderTarget">Render target that the reflected scene is rendered to</param>
		public ReflectionsRenderContext( IRenderContext innerContext, IRenderTarget reflectionsRenderTarget ) :
			base( innerContext )
		{
			Arguments.CheckNotNull( reflectionsRenderTarget, "reflectionsRenderTarget" );
			m_ReflectionsRenderTarget = reflectionsRenderTarget;
		}

		/// <summary>
		/// Gets the render target containing the reflected scene
		/// </summary>
		public IRenderTarget ReflectionsRenderTarget
		{
			get { return m_ReflectionsRenderTarget; }
		}

		/// <summary>
		/// Returns true if the scene is being rendered into the reflections render target
		/// </summary>
		public bool RenderingReflections
		{
			get { return m_RenderingReflections; }
			set { m_RenderingReflections = value; }
		}

		#region Private Members

		private bool m_RenderingReflections;
		private readonly IRenderTarget m_ReflectionsRenderTarget;

		#endregion
	}
}
