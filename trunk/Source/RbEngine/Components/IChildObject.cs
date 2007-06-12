using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Child object
	/// </summary>
	public interface IChildObject
	{
		/// <summary>
		/// Called when this object is added to a parent object
		/// </summary>
		/// <param name="parentObject">Parent object</param>
		void AddedToParent( Object parentObject );
	}
}
