using System;

namespace Rb.Core.Graphics
{
    public delegate void RenderDelegate( IRenderContext context );


    /// <summary>
    /// Rendering technique
    /// </summary>
    public interface ITechnique : INamed
    {
        /// <summary>
        /// Gets the type of this technique
        /// </summary>
        string TechniqueType
        {
            get;
        }

        /// <summary>
        /// Applies this technique
        /// </summary>
        void Apply( IRenderContext context, ITechniqueSelector selector, IRender obj );
    }

    /// <summary>
    /// Used to apply techniques to renderable object
    /// </summary>
    public interface ITechniqueSelector
    {
        /// <summary>
        /// Applies the currently selected technique, if there is one, to obj
        /// </summary>
        /// <param name="context">Rendering context</param>
        /// <param name="obj">Object to render</param>
        void Apply( IRenderContext context, IRender obj );

        /// <summary>
        /// Applies the currently selected technique, or technique, to obj
        /// </summary>
        /// <param name="context">Rendering context</param>
        /// <param name="obj">Object to render</param>
        /// <param name="technique">Technique to render obj with</param>
        void Apply( IRenderContext context, IRender obj, ITechnique technique );

    }

    /// <summary>
    /// Implementation of ITechniqueSelector
    /// </summary>
    public class TechniqueSelector : ITechniqueSelector
    {
        public TechniqueSelector( )
        {
        }

        public TechniqueSelector( ITechnique technique )
        {
            m_Technique = technique;
        }

        public void Apply( IRenderContext context, IRender obj )
        {
            if ( m_Technique != null )
            {
                m_Technique.Apply( context, obj );
            }
            else
            {
                obj.Render( context );
            }
        }

        public void Apply( IRenderContext context, IRender obj, ITechnique technique )
        {
            if ( m_Technique != null )
            {
                if ( technique.TechniqueType == m_Technique.TechniqueType )
                {
                    technique.Apply( context, obj );
                }
                else
                {
                    m_Technique.Apply( context, obj );
                }
            }
            else
            {
                technique.Apply( context, obj );
            }
        }

        private ITechnique m_Technique;
    }

    /// <summary>
    /// Rendering context
    /// </summary>
    public interface IRenderContext
    {
    }

    /// <summary>
    /// Interface for objects that can be rendered
    /// </summary>
    public interface IRender
    {
        /// <summary>
        /// Renders this object using a technique selector
        /// </summary>
        /// <param name="context">Rendering context (can be null)</param>
        /// <param name="selector">Technique selector</param>
        void Render( IRenderContext context, ITechniqueSelector selector );

        /// <summary>
        /// Renders this object
        /// </summary>
        /// <param name="context">Rendering context (can be null)</param>
        void Render( IRenderContext context );
    }
}