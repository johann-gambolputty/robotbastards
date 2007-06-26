
namespace Rb.Core.Components
{
    /// <summary>
    /// Interface for objects that can create instances of themselves (a bit like ICloneable)
    /// </summary>
    public interface IInstanceBuilder
    {
        /// <summary>
        /// Creates an instance
        /// </summary>
        /// <param name="builder">Object builder</param>
        /// <returns>New instance</returns>
        object CreateInstance( IBuilder builder );
    }
}
