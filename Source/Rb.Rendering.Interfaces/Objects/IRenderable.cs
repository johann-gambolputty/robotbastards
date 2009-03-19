
namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Renderable object interface
	/// </summary>
	public interface IRenderable
	{
		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		void Render( IRenderContext context );
	}

	/// <summary>
	/// Renderable with typed render context
	/// </summary>
	/// <typeparam name="TRenderContext"></typeparam>
	public interface IRenderable<TRenderContext> : IRenderable
		where TRenderContext : IRenderContext
	{
		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		void Render( TRenderContext context );
	}
}
