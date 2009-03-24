using System.Collections.Generic;
using Rb.Core.Utils;
using System.Collections;

namespace Rb.Core.Components.Generic
{
	/// <summary>
	/// Standard implementation of <see cref="IComposite{T}"/>
	/// </summary>
	public class Composite<T> : AbstractComposite, IComposite<T>
	{

		#region IComposite<T> Members

		/// <summary>
		/// Event raised when a component is added to this composite
		/// </summary>
		public new event OnComponentAddedDelegate<T> ComponentAdded;

		/// <summary>
		/// Event raised when a component is removed from this composite
		/// </summary>
		public new event OnComponentRemovedDelegate<T> ComponentRemoved;

		/// <summary>
		/// Gets all components in this composite
		/// </summary>
		/// <remarks>
		/// Returns a read only collection (throws if Add/Remove are called)
		/// </remarks>
		public new ICollection<T> Components
		{
			get { return m_Components.AsReadOnly( ); }
		}

		/// <summary>
		/// Adds a component to this composite
		/// </summary>
		/// <param name="component">Component to add</param>
		public virtual void Add( T component )
		{
			Arguments.CheckNotNull( component, "component" );
			if ( m_Components.Contains( component ) )
			{
				return;
			}
			m_Components.Add( component );
			if ( component is IComponent )
			{
				( ( IComponent )component ).Owner = this;
			}
			if ( ComponentAdded != null )
			{
				ComponentAdded( this, component );
			}
			OnComponentAdded( component );
		}

		/// <summary>
		/// Removes a component to this composite
		/// </summary>
		/// <param name="component">Component to remove</param>
		public virtual void Remove( T component )
		{
			Arguments.CheckNotNull( component, "component" );
			m_Components.Remove( component );
			if ( component is IComponent )
			{
				( ( IComponent )component ).Owner = null;
			}
			if ( ComponentRemoved != null )
			{
				ComponentRemoved( this, component );
			}
			OnComponentRemoved( component );
		}

		#endregion

		#region IComposite Members

		/// <summary>
		/// Adds a component
		/// </summary>
		/// <param name="component">Component to add</param>
		public override void Add( object component )
		{
			Add( ( T )component );
		}

		/// <summary>
		/// Removes a component
		/// </summary>
		/// <param name="component">Component to remove</param>
		public override void Remove( object component )
		{
			Remove( ( T )component );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the component collection. Required to solve hidden property problems in <see cref="Generic.Composite{T}"/>
		/// </summary>
		protected override IList GetComponents( )
		{
			return m_Components;
		}

		/// <summary>
		/// Gets the read-only component collection. Required to solve hidden property problems in <see cref="Generic.Composite{T}"/>
		/// </summary>
		protected override IList GetReadOnlyComponents( )
		{
			return m_Components.AsReadOnly( );
		}

		#endregion

		#region Private Members

		private readonly List<T> m_Components = new List<T>( );

		#endregion
	}
}
