using System.Collections.Generic;
using System.Collections;
using Rb.Core.Utils;

namespace Rb.Core.Components
{
	/// <summary>
	/// Standard implementation of the <see cref="IComposite"/> and <see cref="IComponent"/> interfaces
	/// </summary>
	public class Composite : Component, IComposite
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
		/// Gets all components in this composite
		/// </summary>
		public IList Components
		{
			get { return m_Components.AsReadOnly( ); }
		}

		/// <summary>
		/// Adds a component to this composite
		/// </summary>
		/// <param name="component">Composite to add</param>
		public virtual void Add( object component )
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
		}

		/// <summary>
		/// Removes a component to this composite
		/// </summary>
		/// <param name="component">Composite to remove</param>
		public virtual void Remove( object component )
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
		}

		/// <summary>
		/// Removes all components from this composite
		/// </summary>
		public virtual void Clear( )
		{
			while ( m_Components.Count > 0 )
			{
				Remove( m_Components[ 0 ] );
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the underlying component collection as an enumerable object (slightly faster that calling <see cref="Components"/>)
		/// </summary>
		protected IEnumerable EnumerableComponents
		{
			get { return m_Components; }
		}

		/// <summary>
		/// Removes a component at an index in this composite
		/// </summary>
		protected void RemoveAt( int index )
		{
			object component = m_Components[ 0 ];
			m_Components.RemoveAt( index );
			if ( ComponentRemoved != null )
			{
				ComponentRemoved( this, component );
			}
		}

		#endregion

		#region Private Members

		private readonly List<object> m_Components = new List<object>( );

		#endregion
	}
}
