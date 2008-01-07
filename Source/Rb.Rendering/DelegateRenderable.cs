
namespace Rb.Rendering
{
    /// <summary>
    /// Rendering delegate
    /// </summary>
    /// <param name="context">Render context</param>
    public delegate void RenderDelegate( IRenderContext context );

    /// <summary>
    /// Calls a cached delegate to perform rendering
    /// </summary>
    public class DelegateRenderable : IRenderable
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="render">Delegate to call on <see cref="Render"/></param>
        public DelegateRenderable( RenderDelegate render )
        {
            m_Render = render;
        }

        #region IRenderable Members

        /// <summary>
        /// Calls the render delegate passed to the constructor
        /// </summary>
        /// <param name="context">Render context</param>
        public void Render( IRenderContext context )
        {
            m_Render( context );
        }

        #endregion

		#region Private members

		private readonly RenderDelegate m_Render;

		#endregion
	}
}
