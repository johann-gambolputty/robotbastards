using System;
using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
    /// <summary>
    /// Stores a list of renderable objects
    /// </summary>
    [Serializable]
    public class RenderableList : List< IRenderable >, IRenderable
    {
        #region IRenderable Members

        /// <summary>
        /// Renders all the objects in the list using the specified rendering context
        /// </summary>
        /// <param name="context">Rendering context</param>
        public void Render( IRenderContext context )
        {
            foreach ( IRenderable renderable in this )
            {
                renderable.Render( context );
            }
        }

        #endregion
    }


	[Serializable]
	public class RenderableList< T > : List< T >, IRenderable
		where T : IRenderable
	{
        #region IRenderable Members

        /// <summary>
        /// Renders all the objects in the list using the specified rendering context
        /// </summary>
        /// <param name="context">Rendering context</param>
        public void Render( IRenderContext context )
        {
            foreach ( IRenderable renderable in this )
            {
                renderable.Render( context );
            }
        }

        #endregion
	}
}