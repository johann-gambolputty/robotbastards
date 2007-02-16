using System;

namespace RbEngine.Components
{
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
	}
}
