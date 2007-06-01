
namespace Rb.Core.Components
{
    /// <summary>
    /// Child object interface
    /// </summary>
    public interface IChild
    {
        /// <summary>
        /// Called when this object is added to a parent object
        /// </summary>
        /// <param name="parent">Parent object</param>
        void AddedToParent( object parent );
    }
}
