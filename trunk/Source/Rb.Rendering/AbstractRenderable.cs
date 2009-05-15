using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Abstract implementation of <see cref="IRenderable{T}"/>
	/// </summary>
	public abstract class AbstractRenderable<TRenderContext> : IRenderable<TRenderContext>
		where TRenderContext : class, IRenderContext
	{
		#region IRenderable<TRenderContext> Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public abstract void Render( TRenderContext context );

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			TRenderContext typedContext = context as TRenderContext;
			if ( typedContext == null )
			{
				throw new ArgumentException( "Expected render context of type " + typeof( TRenderContext ), "context" );
			}
			Render( typedContext );
		}

		#endregion
	}
}
