using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Summary description for IParentObject.
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
