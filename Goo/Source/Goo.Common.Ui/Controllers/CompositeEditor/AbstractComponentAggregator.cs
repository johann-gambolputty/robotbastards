using System;
using System.Collections.ObjectModel;
using Rb.Core.Utils;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	public abstract class AbstractComponentAggregator<TComponent> : IComponentAggregator<TComponent>
	{
		#region IComponentAggregator<TComponent> Members

		/// <summary>
		/// Adds a child object to a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		public abstract void Add( TComponent parent, object child );

		/// <summary>
		/// Removes a child object from a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		public abstract void Remove( TComponent parent, object child );

		/// <summary>
		/// Returns all child components in a composite
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <returns>Returns a list of child components</returns>
		public abstract ReadOnlyCollection<object> GetChildComponents( TComponent parent );

		#endregion

		#region IComponentAggregator Members

		/// <summary>
		/// Returns true if this aggregator can add components to a parent object
		/// </summary>
		/// <param name="parent">Parent object</param>
		/// <returns>True if Add, Remove and GetChildComponents can be called with the specified parent object</returns>
		public bool CanHandleParent( object parent )
		{
			return parent is TComponent;
		}

		/// <summary>
		/// Adds a child object to a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		public void Add( object parent, object child )
		{
			Arguments.CheckNotNull( parent, "parent" );
			Arguments.CheckNotNull( child, "child" );
			if ( parent is TComponent )
			{
				Add( ( TComponent )parent, child );
				return;
			}
			throw new NotSupportedException( "Doesn't handle parent composite objects of type " + parent.GetType( ) );
		}

		/// <summary>
		/// Removes a child object from a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		public void Remove( object parent, object child )
		{
			Arguments.CheckNotNull( parent, "parent" );
			Arguments.CheckNotNull( child, "child" );
			if ( parent is TComponent )
			{
				Remove( ( TComponent )parent, child );
				return;
			}
			throw new NotSupportedException( "Doesn't handle parent composite objects of type " + parent.GetType( ) );
		}

		/// <summary>
		/// Returns all child components in a composite
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <returns>Returns a list of child components</returns>
		public ReadOnlyCollection<object> GetChildComponents( object parent )
		{
			Arguments.CheckNotNull( parent, "parent" );
			if ( parent is TComponent )
			{
				return GetChildComponents( ( TComponent )parent );
			}
			throw new NotSupportedException( "Doesn't handle parent composite objects of type " + parent.GetType( ) );
		}

		#endregion
	}
}
