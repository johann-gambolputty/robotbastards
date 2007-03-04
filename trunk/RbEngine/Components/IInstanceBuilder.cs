using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Interface for classes that can create instances of themselves (like ICloneable, but implying data sharing)
	/// </summary>
	public interface IInstanceBuilder
	{
		/// <summary>
		/// Creates an instance of this object
		/// </summary>
		object CreateInstance( );
	}
}
