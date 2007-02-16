using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Interface for objects that can create instances of themselves (not necessarily ICloneable)
	/// </summary>
	public interface IInstanceable
	{
		/// <summary>
		/// Creates an instance of the data stored in this model
		/// </summary>
		object CreateInstance( );
	}
}
