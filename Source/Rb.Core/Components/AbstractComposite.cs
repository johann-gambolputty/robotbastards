using System.Collections;

namespace Rb.Core.Components
{
	/// <summary>
	/// Abstract class implementing some parts of the <see cref="IComposite"/> interface
	/// </summary>
	/// <remarks>
	/// The main use of this class is to provide a base class for <see cref="Generic.Composite{T}"/>,
	/// allowing it access to the hidden members <see cref="IComposite.ComponentAdded"/>,
	/// <see cref="IComposite.ComponentRemoved"/> and <see cref="IComposite.Components"/>.
	/// </remarks>
	public abstract class AbstractComposite : IComposite
	{
		#region IComposite Members

		/// <summary>
		/// Event raised when a component is added to this composite
		/// </summary>
		public event OnComponentAddedDelegate ComponentAdded;

		/// <summary>
		/// Event raised when a component is removed from this composite
		/// </summary>
		public event OnComponentRemovedDelegate ComponentRemoved;

		/// <summary>
		/// Gets the components in this composite
		/// </summary>
		public ICollection Components
		{
			get { return GetComponents( ); }
		}

		/// <summary>
		/// Adds a component to this composite
		/// </summary>
		/// <param name="component">Component to add</param>
		public abstract void Add( object component );

		/// <summary>
		/// Removes a component from this composite
		/// </summary>
		/// <param name="component">Component to remove</param>
		public abstract void Remove( object component );

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the component collection. Required to solve hidden property problems in <see cref="Generic.Composite{T}"/>
		/// </summary>
		protected abstract ICollection GetComponents( );

		/// <summary>
		/// Calls the <see cref="ComponentAdded"/> event
		/// </summary>
		protected void OnComponentAdded( object component )
		{
			if ( ComponentAdded != null )
			{
				ComponentAdded( this, component );
			}
		}

		/// <summary>
		/// Calls the <see cref="ComponentRemoved"/> event
		/// </summary>
		protected void OnComponentRemoved( object component )
		{
			if ( ComponentRemoved != null )
			{
				ComponentRemoved( this, component );
			}
		}

		#endregion
	}
}
