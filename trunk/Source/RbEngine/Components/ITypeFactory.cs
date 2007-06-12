using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Interface for factories that create objects from types
	/// </summary>
	public interface ITypeFactory
	{
		object Create( Type objectType );
	}
}
