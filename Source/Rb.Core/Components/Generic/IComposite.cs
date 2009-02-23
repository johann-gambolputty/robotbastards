
using System.Collections.Generic;

namespace Rb.Core.Components.Generic
{
	/// <summary>
	/// Component added delegate
	/// </summary>
	public delegate void OnComponentAddedDelegate<T>( IComposite parent, T child );

	/// <summary>
	/// Component removed delegate
	/// </summary>
	public delegate void OnComponentRemovedDelegate<T>( IComposite parent, T child );

	/// <summary>
	/// Generic version of IComposite. Can only hold components of type T
	/// </summary>
	/// <typeparam name="T">Component type</typeparam>
	public interface IComposite<T> : IComposite
	{
		/// <summary>
		/// Event raised when a component is added to this composite
		/// </summary>
		new event OnComponentAddedDelegate<T> ComponentAdded;

		/// <summary>
		/// Event raised when a component is removed from this composite
		/// </summary>
		new event OnComponentRemovedDelegate<T> ComponentRemoved;

		/// <summary>
		/// Gets all components in this composite
		/// </summary>
		/// <remarks>
		/// Returns a read only collection (throws if Add/Remove are called)
		/// </remarks>
		new ICollection<T> Components
		{
			get;
		}

		/// <summary>
		/// Adds a component to this composite
		/// </summary>
		/// <param name="component">Component to add</param>
		void Add( T component );

		/// <summary>
		/// Removes a component to this composite
		/// </summary>
		/// <param name="component">Component to remove</param>
		void Remove( T component );
	}
}
