using System.Collections.ObjectModel;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Goo.Common.Ui.Controllers.CompositeEditor
{
	/// <summary>
	/// Component aggregator implementation
	/// </summary>
	public class CompositeAggregator : AbstractComponentAggregator<IComposite>
	{
		/// <summary>
		/// Adds a child object to a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		public override void Add( IComposite parent, object child )
		{
			Arguments.CheckNotNull( parent, "parent" );
			Arguments.CheckNotNull( child, "child" );
			
			parent.Add( child );
		}

		/// <summary>
		/// Removes a child object from a parent composite object
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <param name="child">Child component</param>
		public override void Remove( IComposite parent, object child )
		{
			Arguments.CheckNotNull( parent, "parent" );
			Arguments.CheckNotNull( child, "child" );

			parent.Remove( child );
		}

		/// <summary>
		/// Returns all child components in a composite
		/// </summary>
		/// <param name="parent">Parent composite object</param>
		/// <returns>Returns a list of child components</returns>
		public override ReadOnlyCollection<object> GetChildComponents( IComposite parent )
		{
			return EnumerableAdapter<object>.ToList( parent.Components ).AsReadOnly( );
		}
	}
}
