
namespace Rb.Rendering
{
    /// <summary>
    /// Provides the context in which objects can be rendered
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Sets or gets the global technique
        /// </summary>
        /// <remarks>
        /// The global technique overrides locally applied techniques (passed to <see cref="RenderInContext"/>), unless
        /// the local technique is a reasonable substitute (<see cref="ITechnique.IsSubstituteFor"/>).
        /// </remarks>
        ITechnique GlobalTechnique
        {
            get;
            set;
        }

        /// <summary>
        /// Renders the specified renderable object
        /// </summary>
        /// <param name="technique">Technique to render with</param>
        /// <param name="renderable">Object to render</param>
        /// <remarks>
        /// If technique is a valid substitute to the <see cref="GlobalTechnique"/>, technique is used
        /// to render the object, otherwise, the global technique is used
        /// </remarks>
        void RenderInContext( ITechnique technique, IRenderable renderable );

    }
}