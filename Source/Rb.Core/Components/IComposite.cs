using System.Collections;

namespace Rb.Core.Components
{
	/// <summary>
	/// Component added delegate
	/// </summary>
    public delegate void OnComponentAddedDelegate( IComposite parent, object child );

	/// <summary>
	/// Component removed delegate
	/// </summary>
	public delegate void OnComponentRemovedDelegate( IComposite parent, object child );

    /// <summary>
    /// Interface for objects that can store a set of child components
    /// </summary>
    public interface IComposite
	{
		/// <summary>
		/// Event raised when a component is added to this composite
		/// </summary>
		event OnComponentAddedDelegate ComponentAdded;

		/// <summary>
		/// Event raised when a component is removed from this composite
		/// </summary>
		event OnComponentRemovedDelegate ComponentRemoved;

		/// <summary>
		/// Gets all components in this composite
		/// </summary>
		/// <remarks>
		/// Read only collection
		/// </remarks>
		ICollection Components
		{
			get;
		}

		/// <summary>
		/// Adds a component to this composite
		/// </summary>
		/// <param name="component">Composite to add</param>
		void Add( object component );

		/// <summary>
		/// Removes a component to this composite
		/// </summary>
		/// <param name="component">Composite to remove</param>
		void Remove( object component );
    }
}