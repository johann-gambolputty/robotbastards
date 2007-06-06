
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
        /// <returns>New instance</returns>
        object CreateInstance( );
    }
}
