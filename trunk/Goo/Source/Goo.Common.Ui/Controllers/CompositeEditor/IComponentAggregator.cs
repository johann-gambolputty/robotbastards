using System.Collections.ObjectModel;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	/// <summary>
	/// Interface used for aggregating components together
	/// </summary>
	public interface IComponentAggregator
	{
		/// <summary>
		/// Returns true if this aggregator can add components to a parent object
		/// </summary>
		/// <param name="parent">Parent object</param>
		/// <returns>True if Add, Remove and GetChildComponents can be called with the specified parent object</returns>
		bool CanHandleParent( object parent );

		/// <summary>
		/// Adds a child object to a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		void Add( object parent, object child );

		/// <summary>
		/// Removes a child object from a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		void Remove( object parent, object child );

		/// <summary>
		/// Returns all child components in a composite
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <returns>Returns a list of child components</returns>
		ReadOnlyCollection<object> GetChildComponents( object parent );
	}

	/// <summary>
	/// Generic interface
	/// </summary>
	/// <typeparam name="TComponent">Component type</typeparam>
	public interface IComponentAggregator<TComponent> : IComponentAggregator
	{
		/// <summary>
		/// Adds a child object to a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		void Add( TComponent parent, object child );

		/// <summary>
		/// Removes a child object from a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		void Remove( TComponent parent, object child );

		/// <summary>
		/// Returns all child components in a composite
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <returns>Returns a list of child components</returns>
		ReadOnlyCollection<object> GetChildComponents( TComponent parent );		
	}
}
