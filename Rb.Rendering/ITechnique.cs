
namespace Rb.Rendering
{
    /// <summary>
    /// Techniques modify the rendering of an object in serial (unlike IPass, which modifies in parallel)
    /// </summary>
    public interface ITechnique
    {
        /// <summary>
        /// Applies this technique when rendering the specified object
        /// </summary>
        /// <param name="renderable"></param>
        void Apply( IRenderable renderable );


        /// <summary>
        /// Returns true if this technique is a reasonable substitute for the specified technique
        /// </summary>
        /// <param name="technique">Technique to substitute</param>
        /// <returns>true if this technique can substitute the specified technique</returns>
        /// <remarks>
        /// 
        /// </remarks>
        bool IsSubstituteFor( ITechnique technique );
    }
}