
namespace Rb.Interaction
{
    /// <summary>
    /// Factory class for Input objects
    /// </summary>
    public abstract class InputTemplate
    {
        /// <summary>
        /// Creates an Input object with a given context
        /// </summary>
        /// <param name="context">Input context</param>
        /// <returns>New Input object</returns>
        public abstract IInput CreateInput( InputContext context );
    }
}
