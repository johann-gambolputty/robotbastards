using System;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Delegate used for rendering stuff
	/// </summary>
	/// <param name="context">Rendering context</param>
	/// <remarks>
	/// A RenderDelegate is supported interchangeable alongside <see cref="IRenderable"/>.
	/// A RenderDelegate can be converted into an IRenderable object by using the
	/// <see cref="RenderableDelegate"/> wrapper class.
	/// </remarks>
	public delegate void RenderDelegate( IRenderContext context );

	//	TODO: Move RenderableDelegate to Rb.Rendering assembly

	/// <summary>
	/// A wrapper around the <see cref="RenderDelegate"/> delegate, that supports the IRenderable interface
	/// </summary>
	public class RenderableDelegate : IRenderable
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="render">The RenderDelegate to wrap</param>
		public RenderableDelegate( RenderDelegate render )
		{
			if ( render == null )
			{
				throw new ArgumentNullException( "render" );
			}

			m_Render = render;
		}

		#region IRenderable Members

		/// <summary>
		/// Calls the wrapped RenderDelegate
		/// </summary>
		/// <param name="context">Rendering context</param>
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
