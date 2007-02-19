using System;

namespace RbEngine.Components
{

	/// <summary>
	/// Delegate, used by IParentObject.VisitChildren()
	/// </summary>
	public delegate bool ChildVisitorDelegate( Object child );

	/// <summary>
	/// Interface for objects that can store child objects
	/// </summary>
	public interface IParentObject
	{
		/// <summary>
		/// Adds a child object to this object
		/// </summary>
		/// <param name="childObject"> New child object </param>
        void AddChild( Object childObject );

		/// <summary>
		/// Visits all children, calling visitor() for each
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		void VisitChildren( ChildVisitorDelegate visitor );
	}
}
