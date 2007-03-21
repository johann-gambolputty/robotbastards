using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Component builder interface
	/// </summary>
	public interface IBuilder
	{
		/// <summary>
		/// Creates an object of the specified type
		/// </summary>
		object	Create( Type objectType );

		/// <summary>
		/// Builds an existing object
		/// </summary>
		object	Build( object obj );
	}
}
