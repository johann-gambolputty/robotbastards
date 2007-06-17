using Rb.Core.Components;

namespace Rb.Rendering
{
    /// <summary>
    /// Techniques modify the rendering of an object in serial (unlike IPass, which modifies in parallel)
    /// </summary>
    public interface ITechnique : INamed
    {
		/// <summary>
		/// Gets the effect that this technique belongs to
		/// </summary>
		IShader Effect
		{
			get;
		}


        /// <summary>
        /// Applies this technique when rendering the specified object
        /// </summary>
        /// <param name="context">Rendering context</param>
        /// <param name="renderable">Object to render</param>
        void Apply( IRenderContext context, IRenderable renderable );

        /// <summary>
        /// Returns true if this technique is a reasonable substitute for the specified technique
        /// </summary>
        /// <param name="technique">Technique to substitute</param>
        /// <returns>true if this technique can substitute the specified technique</returns>
        bool IsSubstituteFor( ITechnique technique );
    }
}