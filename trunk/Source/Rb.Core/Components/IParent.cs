using System.Collections;

namespace Rb.Core.Components
{
    public delegate void OnChildAddedDelegate( object parent, object child );

    public delegate void OnChildRemovedDelegate( object parent, object child );

    /// <summary>
    /// Interface for objects that can store a set of child objects
    /// </summary>
    public interface IParent
    {
        /// <summary>
        /// Gets the child object collection
        /// </summary>
        ICollection Children
        {
            get;
        }

        /// <summary>
        /// Adds a child object
        /// </summary>
        /// <param name="obj">Child object</param>
        void AddChild( object obj );

        /// <summary>
        /// Removes a child object
        /// </summary>
        /// <param name="obj">Child object</param>
        void RemoveChild( object obj );

        /// <summary>
        /// Invoked by AddChild()
        /// </summary>
        event OnChildAddedDelegate OnChildAdded;

        /// <summary>
        /// Invoked by RemoveChild()
        /// </summary>
        event OnChildRemovedDelegate OnChildRemoved;
    }
}